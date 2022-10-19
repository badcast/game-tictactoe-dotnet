using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotMapLevel
{
    public static class DefaultValues
    {
        public const int TYPE_STEP_DEFAULT = 0;
    }
    public struct Point
    {
        private int x, y;

        public int X { get => x; }
        public int Y { get => y; }
        public bool IsEmpty { get => x == 0 && y == 0; }
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("X=(0}, Y={1}", x, y);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.x + b.x, a.y + b.y);
        }
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.x - b.x, a.y - b.y);
        }
        public static Point operator *(Point a, Point b)
        {
            return new Point(a.x * b.x, a.y * b.y);
        }
        public static Point operator /(Point a, Point b)
        {
            return new Point(a.x / b.x, a.y / b.y);
        }
        public static Point operator ++(Point a)
        {
            return new Point(a.x + 1, a.y + 1);
        }
        public static Point operator --(Point a)
        {
            return new Point(a.x - 1, a.y - 1);
        }
        public static Point operator *(Point a, int b)
        {
            return new Point(a.x * b, a.y * b);
        }
        public static Point operator /(Point a, int b)
        {
            return new Point(a.x / b, a.y / b);
        }
    }

    public enum SideType
    {
        None,
        Crose,
        Zero
    }

    public class Side
    {
        public SideType sideType = SideType.None;
        public int cost;
    }
    public class Cell
    {
        private int x, y;
        private Side side;
        private Point point;
        public int Cost { get; set; }
        public int StepType { get; set; }
        public Map Map { get; }
        /// <summary>
        /// Определяет, свободное ли ячейка
        /// </summary>
        public bool IsSpace { get { return StepType == DefaultValues.TYPE_STEP_DEFAULT; } }
        public int X { get => x; }
        public int Y { get => y; }

        //Neighbour
        public Cell LeftNeighbour { get => Map[x - 1, y]; }
        public Cell RightNeighbour { get => Map[x + 1, y]; }
        public Cell UpNeighbour { get => Map[x, y - 1]; }
        public Cell DownNeighbour { get => Map[x, y + 1]; }
        public Cell AngularUpLeftNeighbour { get => Map[x - 1, y - 1]; }
        public Cell AngularUpRightNeighbour { get => Map[x + 1, y - 1]; }
        public Cell AngularDownLeftNeighbour { get => Map[x - 1, y + 1]; }
        public Cell AngularDownRightNeighbour { get => Map[x + 1, y + 1]; }

        //Padding
        public Cell PaddingLeft { get => Map.GetPaddingCell(this, Map.PaddingCell.pLeft); }
        public Cell PaddingRight { get => Map.GetPaddingCell(this, Map.PaddingCell.pRight); }
        public Cell PaddingUp { get => Map.GetPaddingCell(this, Map.PaddingCell.pUp); }
        public Cell PaddingDown { get => Map.GetPaddingCell(this, Map.PaddingCell.pDown); }
        public Cell PaddingAngularUpLeft { get => Map.GetPaddingCell(this, Map.PaddingCell.pAngularUpLeft); }
        public Cell PaddingAngularUpRight { get => Map.GetPaddingCell(this, Map.PaddingCell.pAngularUpRight); }
        public Cell PaddingAngularDownLeft { get => Map.GetPaddingCell(this, Map.PaddingCell.pAngularDownLeft); }
        public Cell PaddingAngularDownRight { get => Map.GetPaddingCell(this, Map.PaddingCell.pAngularDownRight); }


        /// <summary>
        /// Определяет, координаты X Y
        /// </summary>
        public Point Point { get => GetPoint(); }
        public Side Side { get => side; }

        public Cell(Map map, int x, int y)
        {
            this.Map = map;
            this.x = x;
            this.y = y;
            this.side = new Side();
            this.point = new Point(x, y);
        }

        /// <summary>
        /// Определяет, координаты X Y
        /// </summary>
        public Point GetPoint()
        {
            return point;
        }

        public override string ToString()
        {
            return string.Format("Cell {X={0}, Y={1}}", x, y);
        }

        public static implicit operator bool(Cell a)
        {
            return a != null;
        }

    }
    /// <summary>
    /// Карта, которое хранит в себе ячейку с информациями. Предоставляет легкое управление
    /// </summary>
    public class Map
    {
        public enum PaddingCell
        {
            pLeft,
            pRight,
            pUp,
            pDown,
            pAngularUpLeft,
            pAngularUpRight,
            pAngularDownLeft,
            pAngularDownRight,
        }
        private Cell[,] _cells;
        private int sizeX, sizeY;
        private int mapLength;
        private Dictionary<PaddingCell, Cell> _paddingCache;
        public int MapWidth { get => sizeX; }
        public int MapHeight { get => sizeY; }
        public int Count { get { if (mapLength == 0) mapLength = sizeX * sizeY; return mapLength; } }
        public Cell this[int x, int y] { get => FromPoint(x, y); }
        public Cell this[Point p] { get => FromPoint(p); }
        public Point this[Cell c] { get => c.Point; }
        public Map(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            this._cells = new Cell[sizeX, sizeY];
            this._paddingCache = new Dictionary<PaddingCell, Cell>();

            CreateDefaultMap();
        }

        private void CreateDefaultMap()
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Cell cell = _cells[x, y];
                    if (cell == null)
                    {
                        cell = _cells[x, y] = new Cell(this, x, y);
                    }
                }
            }
        }
        [System.Obsolete]
        public Cell GetFreePoint()
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Cell cur = FromPoint(x, y);

                    if (cur.IsSpace)
                    {
                        return cur;
                    }
                }
            }

            return null;
        }

        public Cell[] GetFreePoints()
        {
            List<Cell> cells = new List<Cell>();
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Cell cur = FromPoint(x, y);

                    if (cur.IsSpace)
                    {
                        cells.Add(cur);
                    }
                }
            }
            return cells.ToArray();
        }

        public bool IsPoint(Point p)
        {
            return IsPoint(p.X, p.Y);
        }
        public bool IsPoint(int x, int y)
        {
            return !(x < 0 || x > sizeX - 1 || y < 0 || y > sizeY - 1);
        }
        public bool HasFromPoint(Point p)
        {
            return HasFromPoint(p.X, p.Y);
        }
        public bool HasFromPoint(int x, int y)
        {
            return IsPoint(x, y) && !this[x, y].IsSpace;
        }

        public Cell FromPoint(int x, int y)
        {
            if (!IsPoint(x, y))
                return null;

            return _cells[x, y];
        }
        public Cell FromPoint(Point p)
        {
            return FromPoint(p.X, p.Y);
        }

        private Cell[] _tempCells;
        public Cell[] ToArray()
        {
            if (_tempCells == null)
                _tempCells = new Cell[sizeX * sizeY];

            int len = _tempCells.Length;
            for (int i = 0; i < sizeX; i++)
            {
                for (int f = 0; f < sizeY; f++)
                {
                    _tempCells[i * sizeY + f] = this[i, f];
                }
            }

            return _tempCells;
        }
        public void Reset()
        {

            for (int i = 0; i < sizeX; i++)
            {
                for (int f = 0; f < sizeY; f++)
                {
                    var c = _cells[i, f];
                    c.Cost = 0;
                    c.StepType = DefaultValues.TYPE_STEP_DEFAULT;
                    c.Side.sideType = SideType.None;
                }
            }
        }
        public Cell GetPaddingCell(Cell destination, PaddingCell padding)
        {

            if (destination == null)
                return null;

            /* Этот блок коментарий нужно дороботать
            if(false && _paddingCache.TryGetValue(padding, out Cell ret))
            {
                return ret;
            }
            */
            int x = 0;
            int y = 0;
            switch (padding)
            {
                case PaddingCell.pLeft:
                    x = -1;
                    break;
                case PaddingCell.pRight:
                    x = 1;
                    break;
                case PaddingCell.pUp:
                    y = -1;
                    break;
                case PaddingCell.pDown:
                    y = 1;
                    break;
                case PaddingCell.pAngularUpLeft:
                    x = -1;
                    y = -1;
                    break;
                case PaddingCell.pAngularUpRight:
                    x = 1;
                    y = -1;
                    break;
                case PaddingCell.pAngularDownLeft:
                    x = -1;
                    y = 1;
                    break;
                case PaddingCell.pAngularDownRight:
                    x = 1;
                    y = 1;
                    break;
                default:
                    break;
            }

            Cell c = destination;
            Cell s = null;
            do
            {
                s = c;
                c = this[c.X + x, c.Y + y];

            } while (c != null);

            // _paddingCache.Add(padding, s);

            return s;
        }

    }
}
