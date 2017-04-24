using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Github_Application_Updater.Util {
    public static class Extentions {

        public static bool navigating = false;

        public static void DoNavigateToString(this WebBrowser browser, string url) {
            navigating = true;
            browser.NavigateToString(url);
            navigating = false;
        }
    }
}
