using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public interface IGridObject
    {
        bool IsFree { get; }
        bool IsSelected { get; }
        Cell Entry { get; }
        int GetPlayerType();
        void Select();
        void Unselect();
        void SetPlayerType(int type);
        void SetIcon(BitmapSource icon);
        void ToPoint(Cell entry, IGridManager mgr, double x, double y, double gridWidth, double gridHeight, double mapWidth, double mapHeight);
        void Free();
    }
    public interface IGridManager
    {
        bool IsActiveMember { get; }
        int WidthMap { get; }
        int HeightMap { get; }
        int GridWidth { get; }
        int GridHeight { get; }
        int GridCount { get; }
        bool IsFree { get; }
        void SetActiveMember(bool stay);
        void Apply(IGridObject gridObject);
        BitmapSource PlayerSource(IGridObject gridObject);
        void UpdateGrid(IGridObject gridObject);
        Cell FromGrid(IGridObject gridObject);
        IGridObject FromCell(Cell cell);
        Point GetPoint(IGridObject gridObject);
        Point GetPoint(Cell cell);
        Point GetPointFromScreen(IGridObject gridObject);
        void Free();
    }

    /// <summary>
    /// Логика взаимодействия для GridPanel.xaml
    /// </summary>
    public partial class GridManager : UserControl, IGridManager
    {
        private List<IGridObject> _grids;
        private List<BitmapSource> _playerSources;
        private bool _free = true;
        private int _gridWidth, _gridHeight;

        public Action<IGridObject> GridActivated;

        /// <summary>
        /// Определяет активность поле
        /// </summary>
        public bool IsActiveMember { get => IsEnabled; }
        public bool IsFree { get => _free; }
        public int WidthMap { get { GenerateException(); return (int)this.Width; } }
        public int HeightMap { get { GenerateException(); return (int)this.Height; } }

        public int GridWidth { get { GenerateException(); return _gridWidth; } }
        public int GridHeight { get { GenerateException(); return _gridHeight; } }

        public int GridCount { get { GenerateException(); return (int)_grids.Count; } }
        public GridManager()
        {
            _grids = new List<IGridObject>();
            InitializeComponent();
        }

        private void SelectedGrid(IGridObject g)
        {
            GenerateException();
            if (!g.Entry.IsSpace)
                return;
            g.Select();
            GridActivated?.Invoke(g);
        }

        private IGridObject GetGrid()
        {
            GenerateException();
            if (_grids == null)
                _grids = new List<IGridObject>();

            IGridObject _result = null;

            for (int i = 0; i < _grids.Count; i++)
            {
                IGridObject g = _grids[i];

                if (g.IsFree)
                {
                    return g;
                }

            }

            var gg = new GridObject();
            _result = gg;
            _grids.Add(_result);
            return _result;
        }

        private void GenerateException()
        {
            if (_free)
                throw new Exception("Object is free");
        }
        public void CreateGrid(Board map, int horizontalCount, int verticalCount)
        {
            _free = false;
            SetActiveMember(true);
            if (_grids == null)
                _grids = new List<IGridObject>(horizontalCount * verticalCount);
            double w = this.WidthMap;
            double h = this.HeightMap;

            double gw = w / horizontalCount;
            double gh = h / verticalCount;

            _gridWidth = (int)gw;
            _gridHeight = (int)gh;

            for (int x = 0; x < horizontalCount; x++)
            {
                for (int y = 0; y < verticalCount; y++)
                {
                    IGridObject g = GetGrid();
                    g.ToPoint(map[x, y], this, x, y, gw, gh, w, h);
                    _content.Children.Add((GridObject)g);
                }
            }

        }
        public void Free()
        {
            _free = true;
            _content.Children.Clear();
            _grids.ForEach((f) => f.Free());
        }

        public void Apply(IGridObject gridObject)
        {
            GenerateException();
            SelectedGrid(gridObject);
            UpdateGrid(gridObject);
        }

        public void SetActiveMember(bool stay)
        {
            //IsEnabled = stay;
            this.IsHitTestVisible = stay;
        /*    for (int i = 0; i < _grids.Count; i++)
            {
                IGridObject ig = _grids[i];
                if (!stay)
                    ig.Select();
                else
                    ig.Unselect();
            }*/
        }

        public BitmapSource PlayerSource(IGridObject gridObject)
        {
            GenerateException();
            int t = gridObject.GetPlayerType();
            if (t == 0)
                return null;
            if (_playerSources == null)
            {
                _playerSources = new List<BitmapSource>();
                _playerSources.Add(DesignManager.current.GetBitmapSourceFromId("gameTypePlayer1"));
                _playerSources.Add(DesignManager.current.GetBitmapSourceFromId("gameTypePlayer2"));
            }
            return _playerSources[t - 1];
        }

        /// <summary>
        /// Обновляет сетку
        /// </summary>
        public void UpdateGrid(IGridObject gridObject)
        {
            GenerateException();

            BitmapSource gridIcon = PlayerSource(gridObject);
            gridObject.SetIcon(gridIcon);
        }

        public Cell FromGrid(IGridObject gridObject)
        {
            GenerateException();

            return gridObject.Entry;
        }

        public IGridObject FromCell(Cell cell)
        {
            GenerateException();


            for (int i = 0; i < _grids.Count; i++)
            {
                var g = _grids[i];
                if (g.Entry == cell)
                    return g;
            }

            return null;
        }

        public Point GetPoint(IGridObject gridObject)
        {
            return GetPoint(gridObject.Entry);
        }
        public Point GetPoint(Cell cell)
        {
            GenerateException();

            return new Point(cell.X * _gridWidth, cell.Y * GridHeight);
        }
        public Point GetPointFromScreen(IGridObject gridObject)
        {
            GridObject parse = (GridObject)gridObject;
            Point res = new Point((int) ((parse.Margin.Left / 2) + this.Margin.Left), (int) ((parse.Margin.Top / 2) + this.Margin.Top));

            return res;
        }
    }
}
