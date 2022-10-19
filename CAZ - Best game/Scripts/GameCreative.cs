using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CAZ
{
    public static class DefaultValues
    {
        public const byte TYPE_STEP_DEFAULT = 0;
        public const float TIME_STEP_WAIT = 1.0f;
    }
    public struct Point
    {
        private bool __data;
        
        public bool IsEmpty { get => !__data; }
        public readonly int X;
        public readonly int Y;
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.__data = true;
        }
        public override string ToString()
        {
            return string.Format("X={0}, Y={1}", X, Y);
        }

        public static Point zero { get => new Point(0, 0); }
        public static Point one { get => new Point(1, 1); }
        public static Point minusOne { get => new Point(-1, -1); }
        public static Point left { get => new Point(-1, 0); }
        public static Point right { get => new Point(1, 0); }
        public static Point up { get => new Point(0, -1); }
        public static Point down { get => new Point(0, 1); }
        public static Point angleUpLeft { get => new Point(-1, -1); }
        public static Point angleUpRight { get => new Point(1, -1); }
        public static Point angleDownLeft { get => new Point(-1, 1); }
        public static Point angleDownRight { get => new Point(1, 1); }

        public static readonly Point Empty = new Point();
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
        public static Point operator *(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }
        public static Point operator /(Point a, Point b)
        {
            return new Point(a.X / b.X, a.Y / b.Y);
        }
        public static Point operator ++(Point a)
        {
            return new Point(a.X + 1, a.Y + 1);
        }
        public static Point operator --(Point a)
        {
            return new Point(a.X - 1, a.Y - 1);
        }
        public static Point operator *(Point a, int b)
        {
            return new Point(a.X * b, a.Y * b);
        }
        public static Point operator /(Point a, int b)
        {
            return new Point(a.X / b, a.Y / b);
        }
        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a.X == b.X && a.Y == b.Y);
        }

        public override bool Equals(object obj)
        {
            // 
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


    }

    public class GameStep
    {
        public int StepType;
        public Cell cell;
    }

    public class GameResult
    {
        /// <summary>
        /// Игрок
        /// </summary>
        public IPlayer Player;
        public DirectionFind DirectionFind;
        public bool IsWin;
        public List<Cell> Result;
        public GameResult(bool isWin, DirectionFind directionFind, IPlayer player, List<Cell> winResult)
        {
            this.Result = winResult;
            this.Player = player;
            this.DirectionFind = directionFind;
            this.IsWin = isWin;
        }
    }

    public class StepResult
    {
        /// <summary>
        /// Игрок, которое выполнил ход
        /// </summary>
        public IPlayer Player { get; }
        /// <summary>
        /// Выбранная ячейка игрока
        /// </summary>
        public Cell Target { get; }

        public StepResult(IPlayer player, Cell target)
        {
            this.Player = player;
            this.Target = target;
        }
    }

    public enum DirectionFind
    {
        df_horizontal,
        df_vertical,
        df_diagonal
    }

    public class GameRules
    {
        public int MapSizeIndex;
        public bool PlayerVsBot;
        public int botLevel;
        public bool playerIsTic;
        public int winLineLength;

        public static GameRules current;

        public static GameRules Default()
        {
            if (current == null)
                current = new GameRules();
            current.botLevel = 3;
            current.MapSizeIndex = 0;
            current.playerIsTic = false;
            current.PlayerVsBot = true;
            current.winLineLength = GetWinLengths(GetSizeFromLevel(0))[0];
            return current;
        }

        public static int GetSizeFromLevel(int index)
        {
            return GameRegulations.MapSizes[index];
        }

        public static int GetGridSizeFromLevel(int index)
        {
            return 0;
        }

        public static int[] GetWinLengths(int size)
        {
            if (size < 3)
                throw new ArgumentException("argument size is min 3");
            int count = size / 2;
            int[] res = new int[count];
            int lastV = 1;
            for (int i = 0; i < count; i++)
                res[i] = lastV += 2;

            return res;
        }
    }
    /// <summary>
    /// Правила игры или класс которое управляем состоянием игры
    /// </summary>
    public class GameRegulations
    {
        sealed class PlayerRoom
        {
            private Cell _lastStep;
            public IPlayer Player { get; }
            public int PlayerStep { get; }
            public Cell PlayerLastStep { get => _lastStep; }
            public PlayerRoom(IPlayer player, int playerStep)
            {
                this.Player = player;
                this.PlayerStep = playerStep;
            }

            public void SetPlayerStep(Cell cell)
            {
                _lastStep = cell;
            }

            public void Default()
            {
                _lastStep = null;
            }


        }

        public static readonly int[] X_POINT = { -1, 1, 0, 0 };
        public static readonly int[] Y_POINT = { 0, 0, -1, 1 };
        public static readonly int[] M_HORIZONTAL_POINT = { -1, 1, 0, 0 };
        public static readonly int[] M_VERTICAL_POINT = { 0, 0, -1, 1 };
        public static readonly int[] M_CORNER_H_POINT = { -1, 0, 1, -1, 1, -1, 0, 1 };
        public static readonly int[] M_CORNER_V_POINT = { -1, -1, -1, 0, 0, 1, 1, 1 };
        public static readonly int[] M_CROSS_H_POINT = { -1, 1, -1, 1, -1, 1 };
        public static readonly int[] M_CROSS_V_POINT = { -1, -1, 0, 0, 1, 1 };
        public static readonly int[] MapSizes = { 3, 5 };

        private Board board_map;
        private int lengthWin;
        private List<GameStep> gameSteps;
        private bool isGameWinned = false;
        private bool isGameEnd = false;
        private bool isPlay = false;
        private byte _currentStepType;
        private PlayerRoom[] _players;


        /// <summary>
        /// Определяет текущего игрока чей должен быть ход
        /// </summary>
        public IPlayer CurrentPlayer { get => _players[_currentStepType - 1].Player; }
        /// <summary>
        /// Определяет, ход след. игрока
        /// </summary>
        public IPlayer NextPlayerStep { get => null; }
        /// <summary>
        /// Опредеялет, ход пред. игрока
        /// </summary>
        public IPlayer PreviewPlayerStep { get => null; }
        /// <summary>
        /// Опеределяет, состояние запуска игры
        /// </summary>
        public bool IsPlay { get => isPlay; }
        /// <summary>
        /// Определяет победу одного игрока
        /// </summary>
        public bool IsWinning { get => isGameWinned; }
        /// <summary>
        /// Определяет, состояние ничьи 
        /// </summary>
        public bool IsDrawGame { get => IsGameEnd && !isGameWinned; }
        /// <summary>
        /// Определяет, конец матча (раунда) или конец игры и результат выводится след. выражением <see cref="IsWinning"/>, <see cref="IsDrawGame"/>
        /// </summary>
        public bool IsGameEnd { get => isGameEnd; }
        /// <summary>
        /// Все подключенные игроки 
        /// </summary>
        public IPlayer[] Players { get => this._players.Select((f) => f.Player).ToArray(); }
        /// <summary>
        /// Определяет результат победы, может возвращать null
        /// </summary>
        public GameResult WinResult { get; private set; }

        /// <summary>
        /// Доска игры
        /// </summary>
        public Board Board { get => board_map; }
        /// <summary>
        /// Текущий на данный момент ход или воспользуитесь <see cref="CurrentPlayer"/>
        /// </summary>
        [Obsolete]
        public byte CurrentStep { get => _currentStepType; }

        public GameRegulations(int size, int lengthWin = 3)
        {
            this.SetSize(size, lengthWin);
            this.lengthWin = lengthWin;
            this.gameSteps = new List<GameStep>();
        }
        private GameResult Check(Board map, byte checkStep, Point checkPoint)
        {
            if (!map[checkPoint])
                return null;



            List<Cell> open = new List<Cell>();
            List<Cell> resultList = new List<Cell>();
            bool haveResult()
            {
                return resultList.Count >= lengthWin;
            }
            bool resultTest()
            {

                //Здесь оптимизирована
                if (open.Count < this.lengthWin)
                    return false;

                resultList.Clear();

                bool res = false;
                int count = open.Count;
                for (int i = 0; i <= open.Count; i++)
                {
                    //Здесь оптимизирована
                    if ((res = haveResult()) || count < lengthWin)
                        break;
                    Cell c = open[i];
                    //Проверка, если удовлетворяет условию, то можно продолжать, иначе сброс
                    if (assign(c))
                    {
                        resultList.Add(c);
                    }
                    //Сброс
                    else
                    {
                        resultList.Clear();
                        count--;
                    }
                }

                return res;

            }
            bool assign(Cell obj)
            {
                return obj && obj.StepType == checkStep;
            }
            void add(Cell obj)
            {
                open.Add(obj);
            }
            void clear()
            {
                open.Clear();
            }

            DirectionFind directionFind = DirectionFind.df_horizontal;
            Cell cur = null;
            bool isEnd = false;
            bool canFind = false;
            bool isBegin = false;
            bool result = false;
            bool angularRight = false;
            //Проверка, пока результат false (ложь) продолжает поиск
            while (!(result = resultTest()) && !isEnd)
            {
                //Перегрузка
                if (!canFind)
                {
                    clear();
                    cur = map[checkPoint];
                    canFind = true;
                    isBegin = true;
                }
                switch (directionFind)
                {
                    case DirectionFind.df_horizontal:

                        if (isBegin)
                        {
                            cur = cur.PaddingLeft;
                            add(cur);
                        }

                        cur = cur.RightNeighbour;

                        if (!cur)
                        {
                            directionFind = DirectionFind.df_vertical;
                            canFind = false;
                            break;
                        }

                        add(cur);

                        break;
                    case DirectionFind.df_vertical:

                        if (isBegin)
                        {
                            cur = cur.PaddingUp;
                            add(cur);
                        }

                        cur = cur.DownNeighbour;

                        if (!cur)
                        {
                            directionFind = DirectionFind.df_diagonal;
                            canFind = false;
                            break;
                        }

                        add(cur);
                        break;
                    case DirectionFind.df_diagonal:
                        if (isBegin)
                        {
                            cur = angularRight ? cur.PaddingAngularUpRight : cur.PaddingAngularUpLeft;
                            add(cur);
                        }
                        cur = angularRight ? cur.AngularDownLeftNeighbour : cur.AngularDownRightNeighbour;
                        if (!cur)
                        {

                            if (angularRight)
                            {
                                isEnd = true;
                            }

                            angularRight = true;

                            canFind = false;
                            break;
                        }

                        add(cur);
                        break;
                }

                isBegin = false;
            }
            if (cached_optimizemem == null)
            {
                cached_optimizemem = new GameResult(result, directionFind, this.Players[checkStep - 1], resultList);
            }
            else
            {
                cached_optimizemem.IsWin = result;
                cached_optimizemem.DirectionFind = directionFind;
                cached_optimizemem.Player = this.Players[checkStep - 1];
                cached_optimizemem.Result = resultList;
            }
            return cached_optimizemem;
        }
        private void GameCheck(Cell checkPoint)
        {
            GameResult result = Check(board_map, CurrentPlayer.StepType, checkPoint.Point);

            if (result == null)
                return;

            GetRoom(CurrentPlayer).SetPlayerStep(checkPoint);
            WinResult = result;
            isGameWinned = result.IsWin;
        }
        private int StepNext()
        {
            int cnt = _players.Length;

            _currentStepType++;

            if (_currentStepType > cnt)
                _currentStepType = 1;

            return _currentStepType;
        }
        /// <summary>
        /// Восстанавливает карту по умолчанию
        /// </summary>
        private void ResetMap()
        {
            for (int y = 0; y < board_map.Count; y++)
            {
                Cell c = board_map[y];
                //Устанавливает значение ячейки по умолчанию
                c.StepType = DefaultValues.TYPE_STEP_DEFAULT;
            }
        }
        private PlayerRoom GetRoom(IPlayer player)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                PlayerRoom r = _players[i];
                if (r.Player.StepType == player.StepType)
                {
                    return r;
                }
            }

            return null;
        }


        GameResult cached_optimizemem;
        public GameResult CustomCheckPoint(IPlayer player, Board map, Point startCheckPoint)
        {
            return Check(map, player.StepType, startCheckPoint);
        }
        public GameResult AutoCheckPoint(IPlayer player, Board map)
        {
            for (int i = 0; i < map.Count; i++)
            {
                Cell c = map[i];
                GameResult r = CustomCheckPoint(player, map, c.Point);
                if (r.IsWin)
                {
                    return r;
                }
            }

            return null;
        }
        public Point PlayerLastPoint(IPlayer player)
        {
            Point def = Point.one * -1;
            if (player != null)
            {
                PlayerRoom pr = GetRoom(player);
                if (pr != null)
                {
                    return pr.PlayerLastStep ? pr.PlayerLastStep.Point : def;
                }
            }

            return def;
        }
        /// <summary>
        /// Создает новую игру
        /// </summary>
        public void PlayNewGame(IPlayer[] playedPlayers)
        {
            if (playedPlayers == null)
                throw new ArgumentNullException();
            if (playedPlayers.Length <= 1)
                throw new ArgumentException("Минимум игроков 2");
            _players = new PlayerRoom[playedPlayers.Length];
            for (int i = 0; i < playedPlayers.Length; i++)
            {
                IPlayer player = playedPlayers[i];
                PlayerRoom plr = new PlayerRoom(player, player.StepType = (byte)(i + 1));
                plr.Player.Load(this);
                _players[i] = plr;
            }
            _currentStepType = 1;
        }
        /// <summary>
        /// Запускает игру
        /// </summary>
        public void StartGame()
        {
            isPlay = true;
        }
        /// <summary>
        /// Перезагружает игру
        /// </summary>
        public void ReplayGame()
        {
            _currentStepType = 1;
            //Очищает ходы игроков
            gameSteps.Clear();
            isGameEnd = false;
            isGameWinned = false;
            WinResult = null;

            ResetMap();

            StartGame();
        }
        /// <summary>
        /// Останавливает игру
        /// </summary>
        public void StopGame()
        {
            isPlay = false;
            isGameWinned = false;
            WinResult = null;
            isGameEnd = true;
        }

        public void SetSize(int size, int lengthWin)
        {
            if (board_map == null)
            {
                board_map = new Board(size);
            }

            board_map.SetSize(size);
            ResetMap();
            this.lengthWin = lengthWin;
        }

        /// <summary>
        /// Определяет, очередь (ход) игрока
        /// </summary>
        public int GetStepFromPlayer(IPlayer player)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                PlayerRoom p = _players[i];

                if (p.Player == player)
                {
                    return p.PlayerStep;
                }
            }

            return -1;
        }


        /// <summary>
        /// Возвращает ход от текущего игрока.
        /// </summary>
        public bool GetStep(out StepResult result)
        {
            result = null;
            if (isGameEnd || !isPlay)
                return false;

            if (CurrentPlayer.IsReady)
            {
                IPlayer playerStepped = CurrentPlayer;
                PlayerResult res = CurrentPlayer.GetStep(board_map);
                Cell cc = res.Cell;
                cc.StepType = CurrentStep;
                gameSteps.Add(new GameStep() { cell = res.Cell, StepType = CurrentStep });
                GameCheck(res.Cell);

                Cell drawed = board_map.GetFreePoint();

                //Проверка раунда
                isGameEnd = isGameWinned || !drawed;

                //Переходит к следующему ходу
                StepNext();

                result = new StepResult(playerStepped, res.Cell);
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Ячейка в карте
    /// </summary>
    public class Cell
    {
        private static int ids = 0;
        private int id;

        public byte StepType;
        /// <summary>
        /// Определяет, координаты X Y
        /// </summary>
        public readonly Point Point;
        public readonly byte Index;

        public readonly int X, Y;
        public readonly Board Board;
        /// <summary>
        /// Определяет, свободное ли ячейка
        /// </summary>
        public bool IsSpace { get { return StepType == DefaultValues.TYPE_STEP_DEFAULT; } }

        //Neighbour
        public Cell LeftNeighbour { get => Board[X - 1, Y]; }
        public Cell RightNeighbour { get => Board[X + 1, Y]; }
        public Cell UpNeighbour { get => Board[X, Y - 1]; }
        public Cell DownNeighbour { get => Board[X, Y + 1]; }
        public Cell AngularUpLeftNeighbour { get => Board[X - 1, Y - 1]; }
        public Cell AngularUpRightNeighbour { get => Board[X + 1, Y - 1]; }
        public Cell AngularDownLeftNeighbour { get => Board[X - 1, Y + 1]; }
        public Cell AngularDownRightNeighbour { get => Board[X + 1, Y + 1]; }

        //Padding
        public Cell PaddingLeft { get => Board.GetPaddingCell(this, PaddingCell.pLeft); }
        public Cell PaddingRight { get => Board.GetPaddingCell(this, PaddingCell.pRight); }
        public Cell PaddingUp { get => Board.GetPaddingCell(this, PaddingCell.pUp); }
        public Cell PaddingDown { get => Board.GetPaddingCell(this, PaddingCell.pDown); }
        public Cell PaddingAngularUpLeft { get => Board.GetPaddingCell(this, PaddingCell.pDiagonalUpLeft); }
        public Cell PaddingAngularUpRight { get => Board.GetPaddingCell(this, PaddingCell.pDiagonalUpRight); }
        public Cell PaddingAngularDownLeft { get => Board.GetPaddingCell(this, PaddingCell.pDiagonalDownLeft); }
        public Cell PaddingAngularDownRight { get => Board.GetPaddingCell(this, PaddingCell.pDiagonalDownRight); }



        //public Cell()
        //{
        //    this.Map = null;
        //    this.x = 0;
        //    this.y = 0;
        //    this.side = new Side();
        //    this.point = Point.Empty;
        //    this.StepType = DefaultValues.TYPE_STEP_DEFAULT;
        //    this._index = 0;
        //}

        public Cell(Board board, int x, int y)
        {
            this.id = ids++;
            this.Board = board;
            this.X = x;
            this.Y = y;
            this.Point = new Point(x, y);
            this.Index = (byte)board.IndexOf(this);
        }

        public override string ToString()
        {
            return string.Format("Cell (X={0}, Y={1})", X, Y);
        }

        public static implicit operator bool(Cell a)
        {
            return a != null;
        }
    }

    public enum PaddingCell
    {
        pLeft,
        pRight,
        pUp,
        pDown,
        pDiagonalUpLeft,
        pDiagonalUpRight,
        pDiagonalDownLeft,
        pDiagonalDownRight,

    }
    /// <summary>
    /// Карта, которое хранит в себе ячейку с информациями. Предоставляет легкое управление
    /// </summary>
    public class Board : ICloneable
    {
        //            case PaddingCell.pLeft:
        //                x = -1;
        //                break;
        //            case PaddingCell.pRight:
        //                x = 1;
        //                break;
        //            case PaddingCell.pUp:
        //                y = -1;
        //                break;
        //            case PaddingCell.pDown:
        //                y = 1;
        //                break;
        //            case PaddingCell.pDiagonalUpLeft:
        //                x = -1;
        //                y = -1;
        //                break;
        //            case PaddingCell.pDiagonalUpRight:
        //                x = 1;
        //                y = -1;
        //                break;
        //            case PaddingCell.pDiagonalDownLeft:
        //                x = -1;
        //                y = 1;
        //                break;
        //            case PaddingCell.pDiagonalDownRight:
        //                x = 1;
        //                y = 1;

        public static Point[] PADDING_MAP =
        {
            new Point(-1, 0),
            new Point(1, 0),
            new Point(0, -1),
            new Point(0, 1),
            new Point(-1, -1),
            new Point(1, -1),
            new Point(-1, 1),
            new Point(1,1)
        };

        private List<Cell> _cells;
        private int size;
        public int Size { get => size; }
        public int Count { get { return _cells.Count; } }
        public Cell this[int index] { get { return FromIndex(index); } }
        public Cell this[int x, int y] { get => FromPoint(x, y); }
        public Cell this[Point p] { get => FromPoint(p); }
        public Point this[Cell c] { get => c.Point; }
        /// <summary>
        /// Центральная ячейка
        /// </summary>
        public Cell CenterCell { get => FromPoint(size / 2, size / 2); }

        public Board(int size)
        {
            this._cells = new List<Cell>(size * size);
            this.SetSize(size);
            CreateDefaultMap();
        }

        private int TranslatePoint(Point p)
        {
            return p.X * size + p.Y;
        }

        private Point TranslateIndex(int index)
        {
            return new Point(index / size, index % size);
        }

        private void CreateDefaultMap()
        {
            for (int i = 0; i < Count; i++)
            {
                Cell cell = _cells[i];
                if (!cell)
                {
                    Point p = TranslateIndex(i);
                    cell = _cells[i] = new Cell(this, p.X, p.Y);
                }
            }
        }

        [System.Obsolete]
        public Cell GetFreePoint()
        {
            for (int x = 0; x < Count; x++)
            {
                Cell cur = _cells[x];

                if (cur.IsSpace)
                {
                    return cur;
                }
            }

            return null;
        }

        List<object> destructs = null;
        public void SetSize(int newSize)
        {
            if (size == newSize || newSize == 0)
                return;

            this.size = newSize;

            //var field = _cells.GetType().GetField("_items", System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.CreateInstance);
            //var vv = (Cell[])field.GetValue(_cells);

            int len = (int)Math.Pow(size, 2);
            _cells = new List<Cell>(len);
            for (int i = 0; i < len; i++)
                _cells.Add(null);

            CreateDefaultMap();
            if (libraryOptimize != null)
                libraryOptimize.Clear();

            //if (destructs == null)
            //    destructs = new List<object>(8);
            //else
            //{
            //    int count = destructs.Count;
            //    int destroyed = 0;
            //    for (int i = 0; i < count; i++)
            //    {
            //        int index = i - destroyed;
            //        object obj = destructs[index];
            //        destructs[index] = null;
            //        if(G_G.destroy(ref obj))
            //        {
            //            destructs.RemoveAt(index);
            //            destroyed++;
            //        }
            //        else
            //        {
            //            destructs[index] = obj;
            //        }
            //    }
            //}
            //object destruct = vv;
            //vv = null;
            //if (!G_G.destroy(ref destruct))
            //    destructs.Add(destruct);
        }

        public Cell[] GetFreePoints()
        {
            List<Cell> cells = new List<Cell>();
            for (int y = 0; y < Count; y++)
            {
                Cell cur = _cells[y];

                if (cur.IsSpace)
                {
                    cells.Add(cur);
                }
            }
            return cells.ToArray();
        }

        public bool isPoint(Point p)
        {
            return isPoint(p.X, p.Y);
        }
        public bool isPoint(int x, int y)
        {
            return !(x < 0 || x > size - 1 || y < 0 || y > size - 1);
        }
        public bool HasFromPoint(Point p)
        {
            return HasFromPoint(p.X, p.Y);
        }
        public bool HasFromPoint(int x, int y)
        {
            return isPoint(x, y) && !this[x, y].IsSpace;
        }

        public Cell FromPoint(Point p)
        {
            return FromPoint(p.X, p.Y);
        }

        public Cell FromPoint(int x, int y)
        {
            if (!isPoint(x, y))
                return null;
            int index = TranslatePoint(new Point(x, y));
            return _cells[index];
        }

        public Cell FromIndex(int index)
        {
            return _cells[index];
        }
        public int IndexOf(Cell cell)
        {
            return IndexOf(cell.Point);
        }

        public int IndexOf(Point p)
        {
            return TranslatePoint(p);
        }


        public void CopyTo(Board destination)
        {
            if (destination == null)
                throw new ArgumentNullException();

            if (destination.size != size)
                throw new ArgumentOutOfRangeException();

            for (int i = 0; i < Count; i++)
            {
                Cell tC = this[i];
                Cell c = destination._cells[i];
                if (!c)
                    c = destination._cells[i] = new Cell(destination, tC.X, tC.Y);
                c.StepType = tC.StepType;
            }
        }

        private Cell[] _tempCells;
        public Cell[] ToArray()
        {
            if (_tempCells == null || _tempCells.Length != Count)
                _tempCells = new Cell[Count];

            _cells.CopyTo(_tempCells);
            return _tempCells;
        }

        public void Reset()
        {
            CreateDefaultMap();
        }

        private static Dictionary<KeyValuePair<int, PaddingCell>, int> libraryOptimize;
        public Cell GetPaddingCell(Cell destination, PaddingCell padding)
        {

            if (!destination)
                return null;


            if (libraryOptimize == null)
                libraryOptimize = new Dictionary<KeyValuePair<int, PaddingCell>, int>();

            var kk = new KeyValuePair<int, PaddingCell>(destination.Index, padding);
            Cell from;
            if (libraryOptimize.TryGetValue(kk, out int r))
                return FromIndex(r);

            Point p = PADDING_MAP[(int)padding];
            from = destination;
            Cell res = destination;
            do
            {
                res = from;
                from = this[from.X + p.X, from.Y + p.Y];

            } while (from);
            libraryOptimize.Add(kk, res.Index);

            return res;
        }

        public object Clone()
        {
            Board clone = new Board(size);

            this.CopyTo(clone);

            return clone;
        }

        public static Point GetDirectionFromPadding(PaddingCell direction)
        {
            return PADDING_MAP[(int)direction];
        }
    }

    public static class BasicAlgorithm
    {
        /// <summary>
        /// Алгоритм бинарного поиска
        /// </summary>
        /// <param name="array">Массив элементов для поиска</param>
        /// <param name="value">Образец, которое нужно найти</param>
        /// <returns>Индекс из массива. Если возвратит -1 - это означает, что образец не был найден из массива</returns>
        public static int BinarySearch(int[] array, int value)
        {
            if (array.Length == 0)
                return -1;

            int low = 0;
            int len = array.Length - 1;

            while (low < len)
            {
                int m = (low + len) / 2;
                int val = array[m];
                if (val == value)
                    return val;

                if (val < value)
                    low = m - 1;
                else
                    len = m + 1;
            }

            return -1;
        }
        public static double DistancePhf(Point a, Point b)
        {
            int powLevel = 2;
            double minX = Math.Pow(a.X - b.X, powLevel);
            double minY = Math.Pow(a.Y - b.Y, powLevel);

            return Math.Abs(minX) + Math.Abs(minY);
        }

    }
}
