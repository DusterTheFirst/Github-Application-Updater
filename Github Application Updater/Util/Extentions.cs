using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Github_Application_Updater.Util {
    public static class Extentions {
        public static void DoNavigateToString(this WebBrowser browser, string url) {
            browser.Navigating += (sender, e) => { };
            browser.NavigateToString(url);
            browser.Navigating += (sender, e) => {
                e.Cancel = true;
                System.Diagnostics.Process.Start(e.Uri.ToString());
            };
        }
    }
}
