using System.Configuration;

namespace OAuthLogin
{
    public class OAuthLoginConfig : ConfigurationSection
    {
        #region etps

        [ConfigurationProperty("etps", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(EtpElementCollection), AddItemName = "etp")]
        public EtpElementCollection Etps
        {
            get { return (EtpElementCollection)base["etps"]; }
            set { base["etps"] = value; }
        }

        #endregion

        #region AuthorizationProviders
        [ConfigurationProperty("AuthorizationProviders", IsDefaultCollection = false)]
        public AuthorizationProviderElemetCollection AuthorizationProvider
        {
            get { return (AuthorizationProviderElemetCollection)base["AuthorizationProviders"]; }
            set { base["AuthorizationProviders"] = value; }
        }

        #endregion

        #region EtpExceptionBuilders

        [ConfigurationProperty("EtpExceptionBuilders", IsDefaultCollection = false)]
        public AuthorizationProviderElemetCollection EtpExceptionBuilder
        {
            get { return (AuthorizationProviderElemetCollection)base["EtpExceptionBuilders"]; }
            set { base["EtpExceptionBuilders"] = value; }
        }

        #endregion
    }

    #region ElementCollection

    public class EtpElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EtpElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EtpElement)element).Name;
        }

        protected override string ElementName
        {
            get { return "etp"; }
        }

        public new EtpElement this[string name]
        {
            get { return BaseGet(key: name) as EtpElement; }
        }

        public EtpElement this[int index]
        {
            get { return BaseGet(index: index) as EtpElement; }
        }
    }

    public class AuthorizationProviderElemetCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AuthorizationProviderElemet();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AuthorizationProviderElemet)element).Etp;
        }

        public new AuthorizationProviderElemet this[string name]
        {
            get { return BaseGet(key: name) as AuthorizationProviderElemet; }
        }

        public AuthorizationProviderElemet this[int index]
        {
            get { return BaseGet(index: index) as AuthorizationProviderElemet; }
        }
    }

    public class EtpExceptionBuilderElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EtpExceptionBuilderElemet();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EtpExceptionBuilderElemet)element).Etp;
        }

        public new EtpExceptionBuilderElemet this[string name]
        {
            get { return BaseGet(key: name) as EtpExceptionBuilderElemet; }
        }

        public EtpExceptionBuilderElemet this[int index]
        {
            get { return BaseGet(index: index) as EtpExceptionBuilderElemet; }
        }
    }

    public class AppElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AppElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AppElement)element).Appkey;
        }

        public new AppElement this[string name]
        {
            get { return BaseGet(key: name) as AppElement; }
        }

        public AppElement this[int index]
        {
            get { return BaseGet(index: index) as AppElement; }
        }
    }

    #endregion

    #region Element

    public class EtpElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("authorizationUrl")]
        public string AuthorizationUrl
        {
            get { return (string)base["authorizationUrl"]; }
            set { base["authorizationUrl"] = value; }
        }

        [ConfigurationProperty("tokenUrl")]
        public string TokenUrl
        {
            get { return (string)base["tokenUrl"]; }
            set { base["tokenUrl"] = value; }
        }

        [ConfigurationProperty("apiUrl")]
        public string ApiUrl
        {
            get { return (string)base["apiUrl"]; }
            set { base["apiUrl"] = value; }
        }

        [ConfigurationProperty("apps", IsDefaultCollection = false)]
        public AppElementCollection Apps
        {
            get { return (AppElementCollection)base["apps"]; }
            set { base["apps"] = value; }
        }

    }

    public class AppElement : ConfigurationElement
    {
        [ConfigurationProperty("appkey", IsKey = true, IsRequired = true)]
        public string Appkey
        {
            get { return (string)base["appkey"]; }
            set { base["appkey"] = value; }
        }

        [ConfigurationProperty("secret")]
        public string Secret
        {
            get { return (string)base["secret"]; }
            set { base["secret"] = value; }
        }

        [ConfigurationProperty("redirectUrl")]
        public string RedirectUrl
        {
            get { return (string)base["redirectUrl"]; }
            set { base["redirectUrl"] = value; }
        }
    }

    public class AuthorizationProviderElemet : ConfigurationElement
    {
        [ConfigurationProperty("etp", IsKey = true, IsRequired = true)]
        public string Etp
        {
            get { return (string)base["etp"]; }
            set { base["etp"] = value; }
        }

        [ConfigurationProperty("type")]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("assembly")]
        public string Assembly
        {
            get { return (string)base["assembly"]; }
            set { base["assembly"] = value; }
        }
    }

    public class EtpExceptionBuilderElemet : ConfigurationElement
    {
        [ConfigurationProperty("etp", IsKey = true, IsRequired = true)]
        public string Etp
        {
            get { return (string)base["etp"]; }
            set { base["etp"] = value; }
        }

        [ConfigurationProperty("type")]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("assembly")]
        public string Assembly
        {
            get { return (string)base["assembly"]; }
            set { base["assembly"] = value; }
        }
    }

    #endregion




}
