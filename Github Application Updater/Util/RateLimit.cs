using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Github_Application_Updater.Util {
    public class RateLimit {

        [JsonProperty("resources")]
        public Dictionary<string, LimitInfo> Resources { get; set; }
        [JsonProperty("rate")]
        public LimitInfo Rate { get; set; }

        public class LimitInfo {
            [JsonProperty("limit")]
            public uint Limit { get; set; }
            [JsonProperty("remaining")]
            public uint Remaining { get; set; }
            [JsonProperty("reset")]
            public double Reset { get; set; }
        }

        public RateLimit(bool download = false) {
            if (download) {
                RateLimit RATELIMIT = JsonConvert.DeserializeObject<RateLimit>(Web.DownloadString("https://api.github.com/rate_limit"));
                Resources = RATELIMIT.Resources;
                Rate = RATELIMIT.Rate;
            }
        }

        public RateLimit Update() {
            RateLimit RATELIMIT = JsonConvert.DeserializeObject<RateLimit>(Web.DownloadString("https://api.github.com/rate_limit"));
            Resources = RATELIMIT.Resources;
            Rate = RATELIMIT.Rate;

            return this;
        }

        public override string ToString() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
