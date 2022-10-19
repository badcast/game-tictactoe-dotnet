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
    /// Логика взаимодействия для TrackBarH.xaml
    /// </summary>
    public partial class TrackBarH : UserControl, IStyle
    {
        private double ____v = 0;
        private double _minvalue = 0;
        private double _maxValue = 1.0;
        private bool isMouseDown = false;
        private System.Windows.Point startPoint;

        private double _value
        {
            get
            {
                return ____v;
            }

            set
            {
                ____v = value;
                ValueChanged?.Invoke(value);
            }
        }

        public double MinValue { get => _minvalue; set => _minvalue = value; }
        public double MaxValue { get => _maxValue; set => _maxValue = value; }
        public double Value { get => _value; set => SetValue(value); }
        public double Increment { get; set; }
        public event Action<double> ValueChanged;

        public TrackBarH()
        {
            Increment = 0.05;
            InitializeComponent();

            if (!GameEngine.Started)
                return;
            DesignManager.AddStyle(this);

        }

        private void SetValue(double value)
        {
            void normalaize(ref double val, double minval = -1)
            {
                if (minval == -1)
                    minval = MinValue;
                val = val < MinValue ? MinValue : val > MaxValue ? MaxValue : val;
            }


            //Нормализация value
            normalaize(ref value);
            double normalVal = value;

            var m = trkInd.Margin;
            double w = (this.Width - (bg.Margin.Left * 2))-trkInd.Width;
            m.Left = w * value;
            trkInd.Margin = m;
            value = normalVal;

            normalaize(ref value);
            _value = value;
        }

        public void SetStyle(DesignManager design)
        {
            if (design == null)
                return;
            bg.Background = design.GetImageBrushFromId("trakLine");
            leftA.Source = design.GetBitmapSourceFromId("trackLeftArrow");
            rightA.Source = design.GetBitmapSourceFromId("trackRightArrow");
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != bg)
                return;

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            startPoint = e.GetPosition(this);   
            isMouseDown = true;
            UserControl_PreviewMouseMove(sender, new MouseEventArgs(e.MouseDevice, e.Timestamp));
        }

        private void UserControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                var ms = e.GetPosition(bg);
                var p1 = bg.Margin;
                double w = trkInd.Width;
                double width = Width - (p1.Left*2)-w;
                System.Windows.Point bPoint = new System.Windows.Point(p1.Left + ms.X - w*2 + w/2, 0);

                double xVal = bPoint.X;
                xVal = xVal < 0 ? 0 : xVal > width ? width : xVal;
                double _value = bPoint.X / width;
                this._value = _value < MinValue ? MinValue : _value > MaxValue ? MaxValue : _value;
                var to = trkInd.Margin;
                to.Left = xVal;
                trkInd.Margin = to;
            }
        }

        private void UserControl_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {

            isMouseDown = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            leftA.PreviewMouseLeftButtonDown += (o, f) => Value -= Increment;
            rightA.PreviewMouseLeftButtonDown += (o, f) => Value += Increment;
        }
    }
}
