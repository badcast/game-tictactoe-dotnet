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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CAZ;

namespace CAZ
{
    /// <summary>
    /// Логика взаимодействия для GridObject.xaml
    /// </summary>
    public partial class GridObject : Button, IStyle, IGridObject
    {
        public const string DEFAULT_TAG = "czgrid";
        private static readonly SolidColorBrush __black = new SolidColorBrush(new Color());
        private static readonly RoutedEventArgs __last = new RoutedEventArgs();

        private TripleData _source;
        private ImageBrush _brsh;
        private int _playerType;
        private IGridManager _mgr;
        private bool _free = true;
        private Cell _entry;
        private bool onlySelect;

        bool IGridObject.IsFree { get => _free; }
        bool IGridObject.IsSelected { get => !IsEnabled; }
        Cell IGridObject.Entry { get { return _entry; } }

        public TripleData Source { get => _source; set => SetSource(value); }
        public event RoutedEventHandler OnPlayerChanged;

        public GridObject()
        {
            InitializeComponent();
            if (!GameEngine.Started)
                return;

            SetStyle(DesignManager.current);
        }

        void IGridObject.Select()
        {
            onlySelect = true;
            SelectOn();
        }

        void IGridObject.Unselect()
        {
            Unselect();

        }


        void IGridObject.SetIcon(BitmapSource icon)
        {
            SetPlayerIcon(icon);
        }


        void IGridObject.Free()
        {
            Unselect();
            _playerType = DefaultValues.TYPE_STEP_DEFAULT;
            _mgr = null;
            this._entry = null;
            _free = true;
            SetPlayerIcon(null);
        }

        int IGridObject.GetPlayerType()
        {
            return _playerType;
        }

        void IGridObject.SetPlayerType(int type)
        {
            _playerType = type;
            OnPlayerChanged?.Invoke(this, __last);
        }

        void IGridObject.ToPoint(Cell entry, IGridManager mgr, double x, double y, double gridWidth, double gridHeight, double mapWidth, double mapHeight)
        {
            if ((_mgr = mgr) == null)
                return;
            IsEnabled = true;
            _entry = entry;

            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            this.Width = gridHeight;
            this.Height = gridHeight;
            this.Margin = new Thickness(x * gridHeight % mapWidth, y * gridHeight % mapHeight, 0, 0);
            this._free = false;
            this.SetPlayerIcon(null);
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

        private void Unselect()
        {
            IsEnabled = true;
            onlySelect = false;
            ToState(0);
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            ToState(1);
        }


        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            ToState(0);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToState(2);
        }

        private void ToState(int index)
        {
            if (index == -1)
            {
                this.Background = __black;
                return;
            }


            this.Background = _brsh;
            if (_source == null || _source.triples.Length == 0 || index > _source.triples.Length || index < 0)
            {
                return;
            }

            BitmapSource b;
            _brsh.ImageSource = b = _source.triples[index];
        }

        private void SelectOn()
        {
            IsEnabled = false;
            ToState(2);
        }

        public void SetStyle(DesignManager design)
        {
            Source = design.GetTripleButton(Tag?.ToString() ?? DEFAULT_TAG);
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ToState(1);
        }

        private void Button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ToState((bool)e.NewValue ? 0 : 2);
        }

        public void SetPlayerIcon(BitmapSource icon)
        {
            gridImage.Source = icon;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mgr.Apply(this);
        }


    }
}
