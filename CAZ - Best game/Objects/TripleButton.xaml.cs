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
    /// Логика взаимодействия для TripleButton.xaml
    /// </summary>
    public partial class TripleButton : Button, IStyle
    {
        public const string DEFAULT_TAG = "button";
        private TripleData _source;
        private ImageBrush _brsh;

        public TripleData Source { get => _source; set => SetSource(value); }

        public TripleButton()
        {
            InitializeComponent();
        }

        public void SetSource(TripleData source)
        {
            if (_brsh == null)
            {
                _brsh = new ImageBrush();
                _brsh.Stretch = Stretch.Fill;
                this.Background = _brsh;
            }

            _source = source;

            ToState(0);
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            ToState(1);
        }

        private void ToState(int index)
        {
            if (_source == null || _source.triples.Length == 0 || index > _source.triples.Length || index < 0)
            {
                return;
            }

            BitmapSource b;
            _brsh.ImageSource = b = _source.triples[index];
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            ToState(0);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToState(2);
        }

        private void Button_Initialized(object sender, EventArgs e)
        {
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignManager.current != null)
                SetStyle(DesignManager.current);
        }

        public void SetStyle(DesignManager design)
        {
            Source = design.GetTripleButton(Tag?.ToString() ?? DEFAULT_TAG);
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ToState(1);
        }

        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ToState(1);
        }
    }
}
