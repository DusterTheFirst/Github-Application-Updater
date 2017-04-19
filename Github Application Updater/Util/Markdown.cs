using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Github_Application_Updater.Util {
    class Markdown {
        static public string RenderGithubMarkdown(string markdown) {
            WebClient webClient = new WebClient();

            webClient.Headers.Add("User-Agent", "ghmd-renderer");
            webClient.Headers.Add("Content-Type", "text/x-markdown");

            return webClient.UploadString("https://api.github.com/markdown/raw", "POST", markdown);
        }
    }
}
