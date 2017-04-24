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
            http.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
            http.Accept = "application/vnd.github.drax-preview+json";
            WebResponse response = http.GetResponse();

            using (var stream = response.GetResponseStream()) {
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                return content;
            }
        }

        public static string Get(string url, Dictionary<string, string> parameters) {
            var http = (HttpWebRequest) WebRequest.Create($"{url}?{DictionaryToGet(parameters)}");
            http.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
            WebResponse response = http.GetResponse();

            using (var stream = response.GetResponseStream()) {
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                return content;
            }
        }

        public static string Post(string url, Dictionary<string, string> parameters) {
            var http = (HttpWebRequest) WebRequest.Create(url);
            const string contentType = "application/x-www-form-urlencoded";

            http.Method = "POST";
            http.ContentType = contentType;
            http.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
            http.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            StreamWriter requestWriter = new StreamWriter(http.GetRequestStream());
            requestWriter.Write(DictionaryToGet(parameters));
            requestWriter.Close();

            var response = http.GetResponse();

            using (var stream = response.GetResponseStream()) {
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();
                return content;
            }
        }

        public static string DictionaryToGet(Dictionary<string, string> dic) {
            string outp = "";
            foreach (KeyValuePair<string, string> p in dic) {
                outp += $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}&";
            }
            return outp;
        }
    }
}
