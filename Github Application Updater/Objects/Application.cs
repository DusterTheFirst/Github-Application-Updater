using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Github_Application_Updater.Util;
using Github_Application_Updater.Properties;

namespace Github_Application_Updater.Objects {
    public class GithubApplication {

        public Repository Repo { get; set; }
        public string README { get; set; }

        public GithubApplication(string URL) {
            UriBuilder URI = new UriBuilder(URL) {
                Host = "api.github.com"
            };
            URI.Path = "repos" + URI.Path;

            MainWindow.Console.Log(URI.Uri.ToString());
            Repo = JsonConvert.DeserializeObject<Repository>(Web.DownloadString(URI.Uri.ToString()));
            try {
                README = Web.DownloadString($"https://raw.githubusercontent.com/{Repo.Owner.Name}/{Repo.Name}/master/README.md");
            } catch {
                README = "# No README Provided";
            }

            //Convert README To HTML
            README = Markdown.RenderGithubMarkdown(README);
            README = $"<style>{Resources.github_markdown}</style><body oncontextmenu='return false'>{README}</body>";
        }

        public GithubApplication() { }

    }

    public class Repository {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("html_url")]
        public string URL { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("open_issues_count")]
        public ulong Issues { get; set; }
        [JsonProperty("updated_at")]
        public DateTime LastUpdated { get; set; }
        [JsonProperty("owner")]
        public GithubUser Owner { get; set; }
    }

    public class GithubUser {
        [JsonProperty("html_url")]
        public string URL { get; set; }
        [JsonProperty("login")]
        public string Name { get; set; }
        [JsonProperty("avatar_url")]
        public string Avatar_URL { get; set; }
    }

    public class GithubApplications : Collection<GithubApplication> { }
}
