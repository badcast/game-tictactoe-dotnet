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
using System.Windows.Threading;

namespace CAZ
{
    /// <summary>
    /// Логика взаимодействия для LogoScreen.xaml
    /// </summary>
    public partial class LogoScreen : UserControl, IScreen
    {
        private DispatcherTimer tt;
        private DateTime start;
        private double durationTime = 5d;
        private bool _start, _showLogo, _middleTime;
        private double opacityLevel = 0;

        public LogoScreen()
        {
            InitializeComponent();
        }

        public void Update()
        {
        }

        public void Closed()
        {
            GameEngine.Current.SetCursorVisible(true);
        }
        public void Showed()
        {
            _start = true;
            start = DateTime.Now + TimeSpan.FromSeconds(durationTime);
            _logo.Opacity = 0;
            tt.Start();
            GameEngine.Current.SetCursorVisible(false);
        }

        public void Created()
        {
            _logo.Source = DesignManager.current.GetBitmapSourceFromId("logo");
            tt = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };

            tt.Tick += (o, e) =>
            {
                if(_start)
                {
                    opacityLevel = 0;
                    _showLogo = true;
                    _start = false;
                }
                DateTime now = DateTime.Now;
                TimeSpan cur = (start - now);
                opacityLevel = cur.TotalSeconds / durationTime;
                _middleTime = opacityLevel >= 0.5;

                _logo.Opacity = _middleTime ? 1 - opacityLevel : opacityLevel;

                //Конец
                if (now > start)
                {
                    GameEngine.Current.LoadScreen(Screens.scrMainMenu);
                    tt.Stop();
                }
            };
        }

        public UIElement GetElement()
        {
            return this;
        }

        public void SetStyle(DesignManager design)
        {
        }


    }
}
