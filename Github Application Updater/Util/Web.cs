using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Github_Application_Updater.Util {
    public static class Web {
        public static string DownloadString(string url) {
            var http = (HttpWebRequest) WebRequest.Create(url);
            http.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            WebResponse response = http.GetResponse();

            using (var stream = response.GetResponseStream()) {
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                return content;
            }
        }
    }
}
