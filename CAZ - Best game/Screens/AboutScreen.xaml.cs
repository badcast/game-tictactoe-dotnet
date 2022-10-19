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
using CAZ;

namespace CAZ
{
    /// <summary>
    /// Логика взаимодействия для AboutScreen.xaml
    /// </summary>
    public partial class AboutScreen : UserControl, IScreen
    {
        public AboutScreen()
        {
            InitializeComponent();
        }

        public void Update()
        {
        }

        public void Closed()
        {
        }

        public void Created()
        {
        }

        public UIElement GetElement()
        {
            return this;
        }

        public void SetStyle(DesignManager design)
        {
            Background = new ImageBrush(BackgroundManager.Current.GetBackground("aboutbackground"));
        }

        public void Showed()
        {
        }



        private void TripleButton_Click(object sender, RoutedEventArgs e)
        {
            GameEngine.Current.ShowScreen(Screens.scrMainMenu);
        }
    }
}
