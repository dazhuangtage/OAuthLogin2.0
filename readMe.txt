博客地址:http://www.cnblogs.com/dazhuangtage/p/6306133.html

使用教程:
　　Nuget安装:Install-Package OAuthLogin2.0
 

配置文件修改如下：
1:在configSections中添加节点:

<configSections>
    <section name="OAuthLogin" type="OAuthLogin.OAuthLoginConfig,OAuthLogin2.0" />
  </configSections>
 

2:在configSections节点下添加节点:
<OAuthLogin>
  <etps>
    <etp name="qq" authorizationUrl="https://graph.qq.com/oauth2.0/authorize" tokenUrl="https://graph.qq.com/oauth2.0/token" apiUrl="https://graph.qq.com/oauth2.0/me">
      <apps>
        <add appkey="" secret="" redirectUrl="" />
      </apps>
    </etp>
  </etps>
  <AuthorizationProviders>
    <add etp="qq" type="OAuthLogin.QQAuthorizationProvider" assembly="OAuthLogin2.0" />
  </AuthorizationProviders>
  <EtpExceptionBuilders>
    <add etp="qq" type="OAuthLogin.QQExceptionBuilder" assembly="OAuthLogin2.0" />
  </EtpExceptionBuilders>
</OAuthLogin>
　　

 

跳转到授权URL:

public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var platform = Platform.Find(EtpName.QQ);
            return Redirect(platform.Applications[0].GenerateAuthorizationUrl("Test"));
        }
    }



获取回调结果:
 public  ActionResult Index()
        {
            var palteFrom = Platform.Find(EtpName.QQ);
            var token=palteFrom.Applications[0].GetToken(System.Web.HttpContext.Current.Request);//GetToken支持异步获取,异步方法为GetTokenAsync
            return Content(token.ToString());
        }