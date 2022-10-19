using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для ProgressBarStyle.xaml
    /// </summary>
    public partial class ProgressBarStyle : UserControl
    {
        public static double[] LineBeizers = new double[]
        {
            .0,
            .01,
            .02,
            .11,
            .15,
            .17,
            .20,
            .1,
            .2,
            .25,
            .27,
            .28,
            .29,
            .3,
            .51,
            .58,
            .7,
            .71,
            .72,
            .75,
            .76,
            .78,
            .78,
            .79,
            .793,
            .89,
            .97,
            .99,
            1.0,
        };
        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(ProgressBarStyle));

        private bool _isIndeterminate;
        private RotateTransform rtr;
        public double angle { get { return rtr.Angle; } set { rtr.Angle = value; } }
        public bool IsIndeterminate { get { return _isIndeterminate; } set { _isIndeterminate = value; Call(_isIndeterminate); } }
        public ProgressBarStyle()
        {
            InitializeComponent();

            if (!GameEngine.Started)
                return;
            var cc = (animator.RenderTransform as TransformGroup).Children;
            for (int i = 0; i < cc.Count; i++)
            {
                if (cc[i].GetType() == typeof(RotateTransform))
                {
                    rtr = (RotateTransform)cc[i];
                    break;
                }
            }
            ((ImageBrush)animator.Background).ImageSource = DesignManager.current.GetBitmapSourceFromId("wheel");
        }
        private System.Collections.IEnumerator rator()
        {
            double speed = 1;
            double radiusMax = 360 + 1;
            double radiusMin = 0;
            double radius = radiusMin;
            double opacityTime = 1;
            this.Opacity = 0;
            int leng = LineBeizers.Length;
            float startTime = 0;
            float lastTime = 0;
            float steptime = 0.01f;
            float time = 0;
            int state = 0;

            this.Opacity = 1;
            bool ending = false;
            startTime = Time.time;
            while (true)
            {
                switch (state)
                {
                    case 0:
                        time = Time.time - startTime;
                        if(time > opacityTime)
                        {
                            state++;
                        }
                        this.Opacity = (time / opacityTime);
                        break;
                }
                if (Time.time > lastTime)
                {
                    radius += (speed) % radiusMax;
                    angle = radius;
                    lastTime = Time.time + steptime;
                }
                //Physics called
                yield return null;

            }
            this.Opacity = 0;
        }

        AsyncOperation asc;
        private void Call(bool state)
        {
            if (!state)
            {
                if (asc != null && !asc.IsDisposed)
                    asc.Dispose();
                Visibility = Visibility.Hidden;
                return;
            }
            else
                if (asc != null && !asc.IsDisposed)
                return;

            asc = new AsyncOperation(rator(), System.Windows.Threading.DispatcherPriority.Background, true);
            asc.Start();
            Visibility = Visibility.Visible;
        }
    }
}
