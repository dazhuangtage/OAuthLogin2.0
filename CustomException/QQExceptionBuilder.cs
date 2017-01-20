using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthLogin
{
    public class QQExceptionBuilder : IEtpExceptionBuilder
    {
        private static Dictionary<string, string> _codes;

        static QQExceptionBuilder()
        {
            _codes = new Dictionary<string, string>();
            _codes.Add("100000", "缺少参数response_type或response_type非法。");
            _codes.Add("100001", "缺少参数client_id。");
            _codes.Add("100002", "缺少参数client_secret。");
            _codes.Add("100003", "http head中缺少Authorization。");
            _codes.Add("100004", "缺少参数grant_type或grant_type非法。");
            _codes.Add("100005", "缺少参数code。");
            _codes.Add("100006", "缺少refresh token。");
            _codes.Add("100007", "缺少access token。");
            _codes.Add("100008", "该appid不存在。");
            _codes.Add("100009", "client_secret（即appkey）非法。");
            _codes.Add("100010", "回调地址不合法。");
            _codes.Add("100011", "APP不处于上线状态。");
            _codes.Add("100012", "HTTP请求非post方式。");
            _codes.Add("100013", "access token非法。");
            _codes.Add("100014", "access token过期。 token过期时间为3个月。");
            _codes.Add("100015", "access token废除。 token被回收，或者被用户删除。");
            _codes.Add("100016", "access token验证失败。");
            _codes.Add("100017", "获取appid失败。");
            _codes.Add("100018", "获取code值失败。");
            _codes.Add("100019", "用code换取access token值失败。");
            _codes.Add("100020", "code被重复使用。");
            _codes.Add("100021", "获取access token值失败。");
            _codes.Add("100022", "获取refresh token值失败。");
            _codes.Add("100023", "获取app具有的权限列表失败。");
            _codes.Add("100024", "获取某OpenID对某appid的权限列表失败。");
            _codes.Add("100025", "获取全量api信息、全量分组信息。");
            _codes.Add("100026", "设置用户对某app授权api列表失败。");
            _codes.Add("100027", "设置用户对某app授权时间失败。");
            _codes.Add("100028", "缺少参数which。");
            _codes.Add("100029", "错误的http请求。");
            _codes.Add("100030", "用户没有对该api进行授权，或用户在腾讯侧删除了该api的权限。请用户重新走登录、授权流程，对该api进行授权。");
            _codes.Add("100031", "第三方应用没有对该api操作的权限。");
        }
        public EtpException Create(string code, string description, string subCode = "", string subDescription = "")
        {
            if (_codes[code] != null)
                description = _codes[code];
            var etpException = EtpException.CreateApplicationException();
            etpException.SetError(code, description);
            return etpException;
        }
    }
}
