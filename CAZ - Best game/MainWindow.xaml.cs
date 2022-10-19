using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CAZ
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private GameEngine engine;
        private System.Windows.Interop.HwndSource hwnd;
        public System.Windows.Interop.HwndSource HandleSource { get => hwnd; }
        public MainWindow()
        {
            current = this;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.hwnd = PresentationSource.FromVisual(this) as System.Windows.Interop.HwndSource;
            hwnd.AddHook(WndProc);
            this.engine = GameEngine.GameEngineInit();
        }

        static MainWindow current;
        public static MainWindow Current { get => current; }
        public static event Action<int> MessageHandled;
        public static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MessageHandled?.Invoke(msg);
            return IntPtr.Zero;
        }
    }
}
