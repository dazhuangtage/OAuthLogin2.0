1:在configSections中添加节点:
   <section name="OAuthLogin2.0" type="OAuthLogin2.0.OAuthLoginConfig,OAuthLogin2.0"/>

2:在configSections节点下添加节点:
<OAuthLogin2.0>
    <etps>
      <etp name="QQ" authorizationUrl="https://graph.qq.com/oauth2.0/authorize" tokenUrl="https://graph.qq.com/oauth2.0/token" apiUrl="https://graph.qq.com/oauth2.0/me">
        <apps>
          <add appkey="" secret="" redirectUrl=""/>
        </apps>
      </etp>
    </etps>
    <AuthorizationProviders>
      <add etp="qq" type="OAuthLogin2.0.QQAuthorizationProvider" assembly="OAuthLogin2.0"/>
    </AuthorizationProviders>
	 <EtpExceptionBuilders>
      <add etp="qq" type="OAuthLogin2.0.QQExceptionBuilder" assembly="OAuthLogin2.0"/>
    </EtpExceptionBuilders>
  </OAuthLogin2.0>