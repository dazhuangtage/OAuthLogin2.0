说下涉及到的几个名词：
平台:
　　定义一个数据结构，用于规范外部交易平台名称。名称是业务系统为Etp分配的唯一标识，由不限长度的字符组成。由于极易发生拼写错误，强烈建议在程序中采用如下规范写法：

　　（1）需要使用某一平台的名称字符串时，应使用其对应的静态字段获取实例，然后使用Name字段获取名称字符串；

　　（2）需要对一个名称字符串进行处理时，应先使用FromString方法获取其对应的实例，然后使用相应的实例方法进行处理；

　　（3）扩展机制：如果开发人员要使用未预定义的平台，可以定义自己的枚举器，使用静态字段存储Etp实例。

应用：
　　描述平台中的应用，存储该应用的基本接口信息。

　　应用是平台用来管理接口调用权限的机制。业务系统方要访问平台接口必须先申请一个应用，经平台方审核通过后才具有调用相应接口的权限。

 

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