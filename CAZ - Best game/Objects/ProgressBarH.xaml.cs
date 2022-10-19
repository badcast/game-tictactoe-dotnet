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
    /// Логика взаимодействия для ProgressBarH.xaml
    /// </summary>
    public partial class ProgressBarH : UserControl, IStyle
    {
        private double ____v = 0;
        private double _minvalue = 0;
        private double _maxValue = 1.0;

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
        public event Action<double> ValueChanged;

        public ProgressBarH()
        {
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

            double w = (this.Width);
            var m = backOver.Margin;

            m.Right = w - w * value;

            backOver.Margin = m;
            normalaize(ref value);
            _value = value;
        }

        public void SetStyle(DesignManager design)
        {
            if (design == null)
                return;
            background.Background = design.GetImageBrushFromId("progressBackground");
            backOver.Source = design.GetBitmapSourceFromId("progressOver");
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
