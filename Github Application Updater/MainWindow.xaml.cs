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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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

            AppImage.Source = Imaging.CreateBitmapSourceFromHIcon(
                                System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name).Handle,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());

            Test();

        }

        public void Test() {

            Applications.Clear();

            Applications.Add(new GithubApplication("https://api.github.com/DusterTheFirst/TestRepo"));

            README.DoNavigateToString(Applications[0].README);
        }



        #region WPF
        public void MainWindowClosing(object sender, CancelEventArgs args) {
            Console.Close();
            Config.Save();
        }

        private void Add_MouseEnter(object sender, MouseEventArgs e) {
            Brush hover = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            Add.Foreground = hover;
            AddPlus.Fill = hover;
        }
        private void Add_MouseLeave(object sender, MouseEventArgs e) {
            Brush hover = Brushes.White;
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

        private void HideREADME_Click(object sender, RoutedEventArgs e) {
            if (READMECol.Width.Value == 0) {
                READMECol.MinWidth = 350;
                READMECol.Width = new GridLength(400);
            } else {
                READMECol.MinWidth = 0;
                READMECol.Width = new GridLength(0);
            }
        }
        private void HideREADME_MouseEnter(object sender, MouseEventArgs e) {
            Brush hover = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            HideREADME.Foreground = hover;
        }
        private void HideREADME_MouseLeave(object sender, MouseEventArgs e) {
            Brush hover = Brushes.White;
            HideREADME.Foreground = hover;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private void CloseWindow(object sender, MouseButtonEventArgs e) {
            Close();
        }
        private void CloseButton_MouseEnter(object sender, MouseEventArgs e) {
            CloseButton.Background = Brushes.DarkRed;
        }
        private void CloseButton_MouseLeave(object sender, MouseEventArgs e) {
            CloseButton.Background = Brushes.Transparent;
        }

        private void MaxRestWindow(object sender, MouseButtonEventArgs e) {
            if (WindowState == WindowState.Maximized) {
                MaxRestButton.Text = "1";
                MaxRestButton.ToolTip = "Maximise";
                WindowState = WindowState.Normal;
            } else {
                MaxRestButton.Text = "2";
                MaxRestButton.ToolTip = "Restore Down";
                WindowState = WindowState.Maximized;
            }
        }
        private void MaxRestButton_MouseEnter(object sender, MouseEventArgs e) {
            MaxRestButton.Background = new SolidColorBrush(Color.FromArgb(75, 100, 100, 100));
        }
        private void MaxRestButton_MouseLeave(object sender, MouseEventArgs e) {
            MaxRestButton.Background = Brushes.Transparent;
        }

        private void MinWindow(object sender, MouseButtonEventArgs e) {
            WindowState = WindowState.Minimized;
        }
        private void MinButton_MouseEnter(object sender, MouseEventArgs e) {
            MinButton.Background = new SolidColorBrush(Color.FromArgb(75, 100, 100, 100));
        }
        private void MinButton_MouseLeave(object sender, MouseEventArgs e) {
            MinButton.Background = Brushes.Transparent;
        }

        private void Window_Deactivated(object sender, EventArgs e) {
            Foreground = Brushes.Gray;
        }
        private void Window_Activated(object sender, EventArgs e) {
            Foreground = Brushes.White;
        }
        #endregion

    }
}
