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
    /// Логика взаимодействия для LoaderScreen.xaml
    /// </summary>
    public partial class LoaderScreen : UserControl, IScreen
    {
        private AsyncOperation _async;
        private Screens __screen;
        private bool isLoad = false;
        private DateTime start;
        private double durationTime = 1.5;

        public bool IsLoadScreen { get => isLoad; }

        public LoaderScreen()
        {
            InitializeComponent();
        }

        public void Update()
        {
        }

        private System.Collections.IEnumerator AsyncCallBack()
        {
            while (true)
            {
                if (!isLoad)
                {
                    isLoad = true;
                }
                DateTime now = DateTime.Now;
                TimeSpan cur = (start - now);
                double pos = cur.TotalSeconds / durationTime;

                progress.Value = 1 - pos;

                //Конец
                if (now > start)
                {
                    GameEngine.Current.ShowScreen(__screen);
                    isLoad = false;
                    break;
                }

                yield return 0;
            }
            _async.Dispose();
        }

        public void Closed()
        {
            progress.Value = 0;
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
        }

        public void Showed()
        {
        }

        public void StartLoadScreen(Screens loadScreen)
        {
            if (loadScreen == Screens.scrLoader || isLoad)
                return;
            __screen = loadScreen;
            start = DateTime.Now + TimeSpan.FromSeconds(durationTime);
            _async = AsyncOperation.CreateAsync(AsyncCallBack());
            _async.Start();
        }
    }
}
