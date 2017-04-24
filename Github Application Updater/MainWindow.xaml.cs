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

        public List<GithubApplication> Applications;

        public MainWindow() {
            InitializeComponent();

            Console = new DebugConsole();

            Config = new ConfigManager(Console);

            Config.AfterConfigLoaded += (sender, ConfigFile) => {
                Applications = ConfigFile.Applications ?? new List<GithubApplication>();
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
            RateLimit RateLimit = new RateLimit(true);
            Console.Warn($"Only {RateLimit.Resources["core"].Remaining} API Calls Remaining Until {RateLimit.UnixTimeStampToDateTime(RateLimit.Resources["core"].Reset)}");

            AppImage.Source = Imaging.CreateBitmapSourceFromHIcon(
                                System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name).Handle,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());

            FillApps();

            ApplicationList.SelectedIndex = 0;
            ApplicationList.Focus();

            Test();

        }

        public void Test() {

            //Applications.Clear();

            //Only Update When Needed, Ratlimits exist
            //Applications.Add(new GithubApplication("https://api.github.com/DusterTheFirst/TestRepo"));
            //Applications.Add(new GithubApplication("https://api.github.com/DusterTheFirst/Github-Application-Updater"));
            //Applications.Add(new GithubApplication("https://api.github.com/DusterTheFirst/SMOL"));
            //Applications.Add(new GithubApplication("https://api.github.com/DusterTheFirst/dusterthefirst.github.io"));


            //README.DoNavigateToString(Applications[0].README);
        }



        #region WPF
        public void MainWindowClosing(object sender, CancelEventArgs args) {
            Console.Close();
            Config.Save();
        }

        private void Add_Mouse(object sender, MouseButtonEventArgs e) {
            if (AddURL.Visibility == Visibility.Collapsed) {
                AddURL.Visibility = Visibility.Visible;
                Keyboard.ClearFocus();
                AddPlus.LayoutTransform = new RotateTransform(45, .5, .5);
                Add.HorizontalAlignment = HorizontalAlignment.Right;
            } else {
                AddURL.Visibility = Visibility.Collapsed;
                Keyboard.ClearFocus();
                AddPlus.LayoutTransform = new RotateTransform(0, .5, .5);
                Add.HorizontalAlignment = HorizontalAlignment.Left;
            }
        }

        public void AddURLRemoveText(object sender, EventArgs e) {
            if (AddURL.Text == "Github URL") {
                AddURL.Text = "";
            }
            AddURL.Foreground = Brushes.Black;
        }
        public void AddURLAddText(object sender, EventArgs e) {
            if (String.IsNullOrWhiteSpace(AddURL.Text)) {
                AddURL.Text = "Github URL";
            }
            AddURL.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));
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
            FillApps();
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

        private void DragWindow(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed)
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

        bool ResizeInProcess = false;
        private void Resize_Init(object sender, MouseButtonEventArgs e) {
            if (sender is Rectangle senderRect) {
                ResizeInProcess = true;
                senderRect.CaptureMouse();
            }
        }
        private void Resize_End(object sender, MouseButtonEventArgs e) {
            if (sender is Rectangle senderRect) {
                ResizeInProcess = false; ;
                senderRect.ReleaseMouseCapture();
            }
        }
        private void Resizeing_Form(object sender, MouseEventArgs e) {
            if (ResizeInProcess) {
                Rectangle senderRect = sender as Rectangle;
                Window mainWindow = senderRect.Tag as Window;
                if (senderRect != null) {
                    double startwidth = e.GetPosition(mainWindow).X;
                    double startheight = e.GetPosition(mainWindow).Y;

                    double width = e.GetPosition(mainWindow).X;
                    double height = e.GetPosition(mainWindow).Y;
                    senderRect.CaptureMouse();
                    if (senderRect.Name.ToLower().Contains("right")) {
                        width += 1;
                        if (width > 1000) {
                            mainWindow.Width = width;
                        } else {
                            mainWindow.Width = width;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("left")) {
                        width -= 1;
                        double newwidth = mainWindow.Width - width;
                        if (newwidth > 1000) {
                            mainWindow.Left += width;
                            mainWindow.Width = newwidth;
                        } else {
                            //mainWindow.Left = startwidth;
                            mainWindow.Width = 1000;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("bottom")) {
                        height += 1;
                        if (height > 650) {
                            mainWindow.Height = height;
                        } else {
                            mainWindow.Height = 650;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("top")) {
                        height -= 1;
                        double newheight = mainWindow.Height - height;
                        if (newheight > 650) {
                            mainWindow.Top += height;
                            mainWindow.Height = newheight;
                        } else {
                            mainWindow.Height = 650;
                            //mainWindow.Top = startheight;
                        }
                    }
                }
            }
        }

        private void Refresh(object sender, RoutedEventArgs e) {
            Config.Save();
            Config.Load();

            FillApps(Search.Text.Replace("Filter Applications", "") ?? "");
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e) {
            if (Applications == null) return;

            FillApps(Search.Text.Replace("Filter Applications", "") ?? "");

            Search_X();
        }

        private void ApplicationList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            GithubApplication selected = Applications.FirstOrDefault(x => x.Repo.Name == (ApplicationList.SelectedItem as ListBoxItem).Content as string);
            if (selected == null) return;
            README.DoNavigateToString(selected.README);

            SelectedAuthor.Text = selected.Repo.Owner.Name;
            SelectedDescription.Text = selected.Repo.Description;
            SelectedLicense.NavigateUri = new Uri(selected.Repo.License?.URL ?? "https://www.google.com");
            SelectedLicense.Inlines.Clear();
            SelectedLicense.Inlines.Add(selected.Repo.License?.Name ?? "No License");
            SelectedUpdate.Text = selected.Repo.LastUpdated.ToString("D");
        }

        private void NoNav(object sender, NavigatingCancelEventArgs e) {
            if (Extentions.navigating) return;
            e.Cancel = true;
            try {
                System.Diagnostics.Process.Start(e.Uri.ToString());
            } catch {
                e.Cancel = false;
            }
        }

        private void Link_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start((sender as Hyperlink).NavigateUri.ToString());
        }
        #endregion

        private void FillApps(string query = "") {
            Applications = Applications.Distinct() as List<GithubApplication>;

            IEnumerable<GithubApplication> filtered = Applications.Where(x => x.Repo.Name.ToLower().Contains(query.ToLower()));

            ApplicationList.Items.Clear();
            if ((filtered.ToList()).Count == 0) {
                ApplicationList.Items.Add(new ListBoxItem() {
                    BorderThickness = new Thickness(0),
                    Height = 30,
                    FontSize = 15,
                    Padding = new Thickness(10, 0, 10, 0),
                    IsEnabled = false,
                    Content = "No Applications Found"
                });
            }
            foreach (GithubApplication a in filtered) {
                ApplicationList.Items.Add(new ListBoxItem() {
                    BorderThickness = new Thickness(0),
                    Height = 30,
                    FontSize = 15,
                    Padding = new Thickness(10, 0, 10, 0),
                    Content = a.Repo.Name
                });
            }
        }
    }
}
