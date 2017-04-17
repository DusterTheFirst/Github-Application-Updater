using Github_Application_Updater.Config;
using Github_Application_Updater.Objects;
using Github_Application_Updater.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Github_Application_Updater {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public static DebugConsole Console;
        ConfigManager Config;

        public GithubApplications Applications;

        public MainWindow() {
            InitializeComponent();

            Console = new DebugConsole();

            Config = new ConfigManager(Console);
            
            Config.AfterConfigLoaded += (sender, ConfigFile) => {
                Applications = ConfigFile.Applications ?? new GithubApplications();
            };

            Config.BeforeConfigSaved += (sender, ConfigFile) => {
                ConfigFile.Applications = Applications;
            };

            Config.Load();

            Closing += MainWindowClosing;

#if DEBUG
            Console.Show();
#endif

            Console.Error("ERROR");
            Console.Warn("WARNING");
            Console.Log("LOG");

            Test();

        }

        public void Test() {

            Applications.Clear();

            Applications.Add(new GithubApplication("https://api.github.com/DusterTheFirst/AASS"));
        }



        #region WPF
        public void MainWindowClosing(object sender, CancelEventArgs args) {
            Console.Close();
            Config.Save();
        }

        private void Add_MouseEnter(object sender, MouseEventArgs e) {
            Brush hover = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            Add.Foreground = hover;
            AddPlus.Fill = hover;
        }
        private void Add_MouseLeave(object sender, MouseEventArgs e) {
            Brush hover = Brushes.Black;
            Add.Foreground = hover;
            AddPlus.Fill = hover;
        }
        private void Add_MouseDown(object sender, MouseButtonEventArgs e) {
            Keyboard.ClearFocus();
        }

        public void SearchRemoveText(object sender, EventArgs e) {
            if (Search.Text == "Filter Applications") {
                Search.Text = "";
            }
            Search.Foreground = Brushes.Black;

            Search_X();
        }
        public void SearchAddText(object sender, EventArgs e) {
            if (String.IsNullOrWhiteSpace(Search.Text)) {
                Search.Text = "Filter Applications";
            }
            Search.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));

            Search_X();
        }
        private void Search_X(object sender = null, EventArgs e = null) {
            if (String.IsNullOrWhiteSpace(Search.Text) || Search.Text == "Filter Applications") {
                ClearSearch.Content = "";
                ClearSearch.IsEnabled = false;
            } else {
                ClearSearch.Content = "r";
                ClearSearch.IsEnabled = true;
            }

            if ((Search.IsFocused && Search.IsKeyboardFocused) || (Search.IsMouseOver)) {
                if (Search.IsFocused && Search.IsKeyboardFocused) {
                    ClearSearch.BorderBrush = new SolidColorBrush(Color.FromRgb(51, 153, 255));
                } else {
                    ClearSearch.BorderBrush = new SolidColorBrush(Color.FromRgb(126, 180, 234));
                }
                ClearSearch.Foreground = Brushes.Black;
                ClearSearch.IsEnabled = true;
            } else {
                ClearSearch.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
                ClearSearch.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));
            }
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e) {
            Search.Text = "";
            SearchAddText(null, null);
        }

        private void GridSplitter_SizeChanged(object sender, SizeChangedEventArgs e) {

        }
        #endregion
    }
}
