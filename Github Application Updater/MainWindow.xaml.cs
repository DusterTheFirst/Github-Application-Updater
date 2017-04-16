using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

        DebugConsole Console;

        public MainWindow() {
            InitializeComponent();

            Console = new DebugConsole();

            Closing += MainWindowClosing;

            #if DEBUG
                Console.Show();
            #endif

            Console.Error("ERROR");
            Console.Warn("WARNING");
            Console.Log("LOG");
        }

        public void MainWindowClosing(object sender, CancelEventArgs args) {
            Console.Close();
        }

        private void Add_MouseEnter(object sender, MouseEventArgs e) {
            Brush hover = new SolidColorBrush(Color.FromRgb(42, 42, 42));
            Add.Foreground = hover;
            AddPlus.Fill = hover;
        }

        private void Add_MouseLeave(object sender, MouseEventArgs e) {
            Brush hover = Brushes.Black;
            Add.Foreground = hover;
            AddPlus.Fill = hover;
        }
    }
}
