using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
namespace OAuthLogin
{
    public class HttpHelp
    {
        public static async Task<string> GetStrAsync(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    return await httpClient.GetStringAsync(url).ConfigureAwait(false);
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
                catch (Exception) {
                    throw;
                }
            }

        }

        public static string GetStr(string url)
        {
            try
            {
                return GetStrAsync(url).Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
