using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Github_Application_Updater {
    /// <summary>
    /// Interaction logic for Debug_Console.xaml
    /// </summary>
    public partial class DebugConsole : Window {
        public DebugConsole() {
            InitializeComponent();
        }

        public void Error(string text) => Add($"[{DateTime.Now.ToString("G")}] {text}", Brushes.Red);

        public void Warn(string text) => Add($"[{DateTime.Now.ToString("G")}] {text}", Brushes.Goldenrod);

        public void Log(string text) => Add($"[{DateTime.Now.ToString("G")}] {text}", Brushes.Black);

        private void Add(string message, Brush color) { 
            Console.Items.Add(new TextBlock {
                    Text = message,
                    Foreground = color
            });
        }
    }
}
