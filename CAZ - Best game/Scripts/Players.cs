#define BAKE_ALGORITHM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CAZ
{

    #region Class And Additional interfaces
    public interface IPlayer
    {
        /// <summary>
        /// Имя игрока
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Игрок готов к ходу
        /// </summary>
        bool IsReady { get; }
        /// <summary>
        /// Тип шага
        /// </summary>
        byte StepType { set; get; }
        /// <summary>
        /// Коллекция ходов
        /// </summary>
        MotionCollection MotionCollection { get; }
        /// <summary>
        /// Выделяет и загружает для игрока игровую систему
        /// </summary>
        void Load(GameRegulations game);
        /// <summary>
        /// Метод для восстановления игры
        /// </summary>
        void Reset();
        /// <summary>
        /// Метод для завершение игры
        /// </summary>
        void Unload();
        /// <summary>
        /// Метод для определения хода
        /// </summary>
        PlayerResult GetStep(Board map);
    }

    public enum AILevel
    {
        Easy,
        Middle,
        Hard,
        VertyHard
    }
    public class MotionCollection
    {

        private List<Cell> _list;

        public Cell this[int index] { get => _list[index]; }

        public int Count { get => _list.Count; }

        public MotionCollection()
        {
            this._list = new List<Cell>();
        }

        public void Add(Cell point)
        {
            if (_list.Contains(point))
                return;

            _list.Add(point);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public void Remove(Cell value)
        {
            _list.Remove(value);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public int IndexOf(Cell value)
        {
            return _list.IndexOf(value);
        }

    }
    public struct PlayerResult
    {
        public IPlayer PlayerUse { get; }
        public Cell Cell { get; }
        public PlayerResult(IPlayer player, Cell move)
        {
            this.PlayerUse = player;
            this.Cell = move;
        }

        public static readonly PlayerResult Empty;
    }


    public class PlayerBase
    {
        public int id { get; }
    }
    #endregion

    #region Base players

    /*Строк кода для алгоритма бота*/

    /// <summary>
    /// Базовый класс для бота
    /// </summary>
    public class BotBase : PlayerBase, IPlayer
    {
        protected MotionCollection mc;
        protected GameRegulations _game;

        public virtual string Name { get; }

        public virtual bool IsReady => false;
        public virtual byte StepType { get; set; }

        public MotionCollection MotionCollection { get { if (mc == null) mc = new MotionCollection(); return mc; } }

        public virtual void Load(GameRegulations game)
        {
            _game = game;

        }

        public virtual void Reset()
        {

        }
        public virtual void Unload()
        {

        }

        public virtual PlayerResult GetStep(Board map)
        {
            return PlayerResult.Empty;
        }
    }
    #endregion

    #region AI

    public class AI : BotBase
    {
        sealed class Tree
        {
            private int m_totalCount = -1;
            public List<Tree> childs;
            public string treeName;
            public int indexOnBoard;
            public Tree parent;
            public TreeData data;
            /// <summary>
            /// Количество дочерных иерархий 
            /// </summary>
            public int totals { get { if (m_totalCount == -1) { m_totalCount = 0; for (int i = 0; i < childs.Count; i++) m_totalCount += childs[i].totals + 1; } return m_totalCount; } }
            public Tree(string treeName, Tree parent)
            {
                this.treeName = treeName;
                this.childs = new List<Tree>();
                this.parent = parent;
            }

            public void pushBack(Tree child)
            {
                m_totalCount = -1;
                childs.Add(child);
            }

            public void popBack()
            {
                if (childCount() < 1)
                    return;
                m_totalCount = -1;
                childs.RemoveAt(childCount() - 1);
            }

            public int childCount()
            {
                return childs.Count;
            }

            public Tree getChild(int index)
            {
                return childs[index];
            }

            public override string ToString()
            {
                return string.Format("{0}, parent: {1}, childs: {2}, stepType: {3}, score: {4}, totals: {5}", treeName, parent == null ? "null" : parent.treeName, childCount(), data.StepType, data.Score, totals);
            }
        }

        struct TreeData
        {
            public Point Point;
            public byte StepType;
            public sbyte Score;

            public TreeData(Point Point, sbyte Score, byte StepType)
            {
                this.Point = Point;
                this.Score = Score;
                this.StepType = StepType;
            }

            public override string ToString()
            {
                return "Score: " + Score;
            }
        }

        private static readonly int[] MATRIX_H = { 0, 1, 0, 1 }, MATRIX_V = { 0, 0, 1, 1 };
        public static readonly string CacheFile = "ai_cache";
        private static readonly char[] label = "AI_CACHE_BACKED".ToCharArray();
        private static bool _cacheIsLoaded = false;
        private static Tree main;

        private IPlayer[] __players;
        private IPlayer enemy;
        private object _lock;
        private bool _isInited = false;
        private bool isGameReset = false;
        private bool isReady = false;
        private List<Cell>[] tempCells = new List<Cell>[4];
        private bool isThreadBroken = true;
        /// <summary>
        /// Определяет, оптимизированный интелект загружен с кэша
        /// </summary>
        public virtual bool IntelectFromCache { get { return _cacheIsLoaded; } }
        /// <summary>
        /// Определяет, готовность интелекта к ходу
        /// </summary>
        public override bool IsReady
        {
            get
            {
                if (_game == null)
                    return false;

                if (isAsync && !isReady)
                    return false;

                if (!isAsync && _botCallback == null)
                {
                    GetEnemyLastPoint();
                    _botCallback = AsyncOperation.CreateAsync(__EnumBotCall(_game.Board), System.Windows.Threading.DispatcherPriority.Background);
                    _botCallback.Start();
                    isReady = false;
                    isAsync = true;
                }

                return isReady;
            }
        }
        /// <summary>
        /// Имя Искуственного Интелекта
        /// </summary>
        public override string Name => "AI";
        /// <summary>
        /// Очередь-шаг 
        /// </summary>
        public override byte StepType => base.StepType;
        /// <summary>
        /// Уровень сложности ИИ, чем выше уровень, тем долговременное ожидания, но высокий результат решения
        /// </summary>
        public virtual AILevel ArtificalIntelectLevel { get; }
        public AI(AILevel aiLevel)
        {
            _lock = new object();
            this.ArtificalIntelectLevel = aiLevel;
        }


#if !BAKE_ALGORITHM
        /// <summary>
        /// Загружает файл интелекта, способствует быстродействию 
        /// </summary>
        private void LoadCache(Board board)
        {
            if (_cacheIsLoaded)
                return;

            string fullPath = DataBase.AppDir + "\\logic\\" + CacheFile;

            if (!File.Exists(fullPath))
            {
                return;
            }

            try
            {
                using (BinaryReader br = new BinaryReader(File.OpenRead(fullPath)))
                {
                    br.BaseStream.Position = label.Length;
                    bool isOptimizableData = br.ReadBoolean();
                    int mapSize = br.ReadByte();
                    byte stepAiTypeOn = br.ReadByte();
                    main = new Tree("MainHierarcy", null);
                    GetRead(board, br, main);
                }
            }
            catch
            {

                return;
            }

            _cacheIsLoaded = true;
        }

        private void GetRead(Board board, BinaryReader br, Tree tree)
        {
            //read child count
            byte chidCount = (byte)br.ReadByte();
            //read index
            tree.indexOnBoard = br.ReadByte();
            tree.data.Point = board.FromIndex(tree.indexOnBoard).Point;
            //read score
            tree.data.Score = br.ReadSByte();

            //read childs
            tree.childs = new List<Tree>(chidCount);
            for (int i = 0; i < chidCount; i++)
            {
                Tree child = new Tree("Child", tree);
                GetRead(board, br, child);
                tree.pushBack(child);
            }
        }
#endif
#if BAKE_ALGORITHM
        /// <summary>
        /// Сохраняет интелект в файл
        /// </summary>
        private void SaveCache(Board map, int iteratedCount, Tree data, bool isOptimizableData)
        {
            string fullPath = DataBase.AppDir + "\\logic\\" + CacheFile;
            using (BinaryWriter bw = new BinaryWriter(File.Create(fullPath)))
            {
                bw.Write(label);
                bw.Write(isOptimizableData);
                bw.Write((byte)map.Size); // write first size
                bw.Write((byte)this.StepType); // write step type is ai

                string log = "";
                //write data
                ToWrite(map, ref log, bw, data, data);
                long strSize = bw.BaseStream.Length;
                log = string.Format("Древо созданное в AI Backed\n" +
                                           "Размер карты: {0}x{0}\n" +
                                           "Полная итерация: {1}(итераций)\n" +
                                           "Оптимизировано до: {2}(итераций)\n" +
                                           "С экономлено: {3}(итераций)\n" +
                                           "Объетов записана: {4}\n" +
                                           "Размер данных: {5}(килобайт)" +
                                           "\n=======================Tree view=======================\n",
                                           map.Size, iteratedCount, data.totals, iteratedCount - data.totals, data.totals, (float)Math.Round(strSize / 1000f, 3)) + log;
                string logFile = fullPath + "_backed.log";
                File.WriteAllText(logFile, log);
                // System.Diagnostics.Process.Start(logFile);
            }
            System.GC.Collect();
        }

        private void ToWrite(Board board, ref string outLog, BinaryWriter writer, Tree target, Tree main)
        {
            //write data
            byte childCount;
            writer.Write(childCount = (byte)target.childCount());           // write a child count
            writer.Write((byte)board[target.data.Point].Index);    // write a index cell
            writer.Write((sbyte)target.data.Score);   // write score
            //start write data (childs)
            //logs
            outLog += string.Format(">[ID-{0}] name: {1}, childs: {2}, totals: {3}, score: {4}, Point: {5}\n", main.totals - target.totals, target.treeName, childCount, target.totals, target.data.Score, board[target.indexOnBoard].Point);
            for (byte i = 0; i < childCount; i++)
            {
                try
                {

                    Tree child = target.getChild(i);
                    string childData = "";
                    Tree tt = child.parent;
                    while (tt != null)
                    {
                        childData += "--";
                        tt = tt.parent;
                    }
                    ToWrite(board, ref childData, writer, child, main);
                    outLog += childData;
                }
                catch (Exception e)
                {

                }

            }
        }

#endif
        #region voids
        private Cell RandomBound(Board board, Cell relative)
        {
            if (board.CenterCell == relative)
                return relative;

            int sz = board.Size - 1;
            int rndIndex = RndMgr.Range(0, 4);
            int x = relative.X + (MATRIX_H[rndIndex] * sz);
            int y = relative.Y + (MATRIX_V[rndIndex] * sz);
            return board[x, y];
        }
        private void MakeCategorize(ICollection<Cell> cells, out List<Cell> enemyCells, out List<Cell> myCells, out List<Cell> freeCells)
        {
            if (!_isInited)
            {
                if (tempCells == null)
                {
                    tempCells = new List<Cell>[4];
                }
                for (int f = 0; f < tempCells.Length; f++)
                {
                    if (tempCells[f] == null)
                    {
                        tempCells[f] = new List<Cell>(cells.Count);
                    }

                }
            }

            for (int i = 0; i < tempCells.Length; i++)
            {
                tempCells[i].Clear();
            }

            enemyCells = tempCells[0];
            myCells = tempCells[1];
            freeCells = tempCells[2];
            foreach (var c in cells)
            {
                if (c.IsSpace)
                    freeCells.Add(c);
                else if (c.StepType == this.StepType)
                    myCells.Add(c);
                else enemyCells.Add(c);
            }
        }
        private bool IsWinPoint(Board board, Point p)
        {
            return IsWinPlayerPoint(board, p, this);
        }
        private bool IsEnemyWinPoint(Board map, Point p)
        {
            if (__players == null)
                __players = _game.Players;

            if (enemy == null)
            {
                for (int i = 0; i < __players.Length; i++)
                {
                    IPlayer plr = __players[i];
                    if (plr != this)
                    {
                        enemy = plr;
                        break;
                    }
                }
                if (enemy == null)
                    return false;
            }

            return IsWinPlayerPoint(map, p, enemy);
        }
        private bool IsWinPlayerPoint(Board map, Point p, IPlayer pl)
        {
            return _game.CustomCheckPoint(pl, map, p).IsWin;
        }
        private Point GetEnemyLastPoint()
        {
            if (enemy == null)
                IsEnemyWinPoint(_game.Board, Point.zero);

            return _game.PlayerLastPoint(enemy);
        }
        private bool isSolo = false;
        private List<Cell> Avail(ICollection<Cell> undefineList)
        {
            IEnumerable<Cell> rr = null;

            Point lastPoint = Point.zero;
            if (!isSolo)
                rr = undefineList.Where((f) => f.IsSpace);
            else
                rr = undefineList.Where((f) =>
                {
                    Point p = f.Point;
                    if (f.IsSpace && (p.X > lastPoint.X || p.Y > lastPoint.Y))
                    {
                        return true;
                    }

                    return false;
                });

            return rr.ToList();
        }
        private bool Win(Board map, IPlayer check, ref Point checkPoint)
        {
            //AutoCheker
            GameResult r = _game.CustomCheckPoint(check, map, checkPoint);
            return r != null && r.IsWin;
        }
        private bool GetBestResult(ref TreeData reference, ref TreeData newReference, bool isMin)
        {
            if ((isMin && newReference.Score > reference.Score) || (!isMin && newReference.Score < reference.Score))
            {
                reference = newReference;
                return true;
            }
            return false;
        }
        #endregion

        private int iter = 0;
        private const sbyte aiWin = 1;
        private const sbyte positiveBig = 2;
        private const sbyte negativeBig = positiveBig * -1;
        private const sbyte opponentWin = aiWin * -1;
        private Tree recursiveTree;
#if !BAKE_ALGORITHM
        private Tree saved_on_cache = null;
#endif
        private TreeData Minimax(Board board, IPlayer player, ref Point check, List<Cell> unfilterList, byte listCount)
        {
            // get the alpha beta values
            //int min = int.MinValue;
            //int max = int.MaxValue;
            //return AlphaBeta(board, player, ref check, unfilterList, unfilterList.Count, min, max);

            /*
             * SCORE ==  0 - Значение указывающая на ничью
             * SCORE ==  1 - Значение указывающая на победу ИИ
             * SCORE == -1 - Значение указывающая на победу ГГ или противника
             */

            int curIter = ++iter;

            TreeData result;

#if BAKE_ALGORITHM

            //Все ходы в текущей рекурсии
            List<TreeData> moves = null;
            Tree thisTree;
            //bake on
            //BEGIN RECURSIVE
            if (curIter == 1)
            {
                isSolo = board.Size > 3;
                moves = new List<TreeData>();

                //recursiveTree = thisTree = main = new Tree("Main-Tree", null);
            }
            else
                //recursiveTree = thisTree = new Tree("tree" + curIter, recursiveTree);


            if (Win(board, this, ref check))
            {
                return new TreeData(check, aiWin, board[check].StepType);
            }
            else if (Win(board, enemy, ref check))
            {
                return new TreeData(check, opponentWin, board[check].StepType);
            }
            else//Если достигнута конца
            if (listCount == 0)
            {
                return new TreeData(check, 0, DefaultValues.TYPE_STEP_DEFAULT);
            }


            bool isAi = player == this;

            result = new TreeData(Point.minusOne, (sbyte)(isAi ? negativeBig : positiveBig), DefaultValues.TYPE_STEP_DEFAULT);
            //Index of the best move
            IPlayer nextPlayer = isAi ? enemy : this;
            sbyte bestResult = (sbyte)(isAi ? aiWin : opponentWin);
            Point refPoint;
            listCount--;
            int len = unfilterList.Count;
            TreeData data;
            for (int i = 0; i < len; i++)
            {
                //Cell self
                Cell c = unfilterList[i];
                if (!c.IsSpace)
                    continue;
               // recursiveTree = thisTree;
                //player
                c.StepType = player.StepType;
                //Входим в глубину(Depth)... (Рекурсия) 
                refPoint = c.Point;
                TreeData res = Minimax(board, nextPlayer, ref refPoint, unfilterList, listCount);

                data = new TreeData(c.Point, res.Score, c.StepType);
                data.Point = c.Point;
                data.StepType = player.StepType;
                c.StepType = DefaultValues.TYPE_STEP_DEFAULT;
                if (GetBestResult(ref result, ref data, isAi))
                {
                    if (curIter != 1)
                    {
                        if (result.Score == bestResult)
                        {
                            result.StepType = player.StepType;
                            return result;
                        }
                    }
                    else
                        moves.Add(data);
                }
                //recursiveTree.data = data;
                //recursiveTree.indexOnBoard = c.Index;

               // thisTree.pushBack(recursiveTree);
            }
          //  recursiveTree = thisTree;
#else
            bool isAI = player == this;
            result = new TreeData(Point.minusOne, (sbyte)(isAI ? negativeBig : positiveBig), DefaultValues.TYPE_STEP_DEFAULT);
            //Index of the best move
            Tree currentHierarchy = saved_on_cache != null ? saved_on_cache : main;
            int getBestIndex(Board filterBoard, Tree treeFind, out int depth)
            {
                depth = 0;
                if (treeFind == null)
                    return -1;

                int depth_in(Tree tree)
                {
                    if (tree.data.Score == 0)
                        return 1;
                    else
                    if (tree.data.StepType == this.StepType && tree.data.Score == 1)
                    {
                        return 0;
                    }

                    int bestValue = int.MinValue;
                    for (int i = 0; i < tree.childCount(); i++)
                    {
                        int res = depth_in(tree.getChild(i));
                        if (res < bestValue)
                        {
                            bestValue = res;
                        }
                    }

                    return bestValue;
                }

                int bestIndex = -1;
                for (int i = 0; i < treeFind.childCount(); i++)
                {
                    if (filterBoard[i].IsSpace)
                    {
                        int depthAt = depth_in(treeFind.getChild(i));
                        if (depthAt > depth)
                        {

                            depth = depthAt;
                            bestIndex = i;
                        }
                    }
                }

                return bestIndex;
            }

            Tree depthOn(Tree select, bool autoFindOn = true)
            {
                cncTocken.ThrowIfCancellationRequested();

                int haveBest = -1;
                bool isCriticalState = false;
                for (int f = 0; f < select.childCount(); f++)
                {
                    Tree child = select.getChild(f);
                    Cell c = board[child.indexOnBoard];
                    if (c.IsSpace && child.data.Score >= 0)
                    {
                        c.StepType = player.StepType;
                        Point p = c.Point;
                        if (Win(board, this, ref p))
                        {
                            c.StepType = DefaultValues.TYPE_STEP_DEFAULT;
                            return child;
                        }

                        c.StepType = enemy.StepType;
                        if (Win(board, enemy, ref p))
                        {
                            isCriticalState = true;
                            haveBest = f;
                        }
                        else
                        if (!isCriticalState)
                        {
                            haveBest = f;
                        }
                        c.StepType = DefaultValues.TYPE_STEP_DEFAULT;
                    }
                }
                //Все ходы были просчитаны и не найдена лучшая оценца, значит пробуем альтернативный ход
                //Ход оглубленно ищет лучшую оценку возможно и худщую 
                if (haveBest == -1)
                {
                    haveBest = getBestIndex(board, select, out int depth);
                    return select.getChild(haveBest);
                }
                else
                {
                    //Используем произвольный выбор лучших результатов
                    return select.getChild(haveBest);
                }

                if (autoFindOn) // переменная на самостоятельного поиска
                {
                    if (currentHierarchy != select && currentHierarchy != select.parent)
                    {
                        return null;
                    }

                    Tree parent = null;
                    for (int x = 0; x < select.childCount(); x++)
                    {
                        Tree curTree = select.getChild(x);
                        parent = depthOn(select.getChild(x));
                        if (parent != null)
                            return parent;
                    }
                }

                return null;
            }


            //Устанавливаем кэш
            saved_on_cache = depthOn(currentHierarchy);

            result.Point = board[saved_on_cache.indexOnBoard].Point;
            result.StepType = this.StepType;


#endif

#if BAKE_ALGORITHM
            if (curIter == 1)
            {
                //END RECURSIVE
                //Условность на то, если текущая рекурсия полностью закончила свою работу
                cncTocken.ThrowIfCancellationRequested();
               // System.Diagnostics.Debug.Print("Logged! Iterated count: " + iter);
               // SaveCache(board, iter, main, true);
                //Установка общей итераций на ноль
                iter = 0;
                //Проверка, ИИ выбрал выйгришный путь ? 
                if (!Win(board, player, ref result.Point))
                {
                    bool isWin = false;
                    bool isEnemyWin = false;
                    TreeData enemyCheckRes = result;
                    Point p;
                    for (int f = 0; f < moves.Count; f++)
                    {
                        Cell cc = board[moves[f].Point];
                        cc.StepType = this.StepType;
                        p = cc.Point;
                        if (Win(board, this, ref p))
                        {
                            result = moves[f];
                            cc.StepType = DefaultValues.TYPE_STEP_DEFAULT;
                            isWin = true;
                            break;
                        }
                        else
                        {
                            cc.StepType = enemy.StepType;
                            if (Win(board, enemy, ref p))
                            {
                                enemyCheckRes = moves[f];
                                isEnemyWin = true;
                            }
                        }
                        cc.StepType = DefaultValues.TYPE_STEP_DEFAULT;
                    }

                    //Когда нет выхода к победе, то принимаем последние меры
                    if (!isWin && isEnemyWin)
                        result = enemyCheckRes;

                }

            }
#endif
            return result;
        }

        private TreeData AlphaBeta(Board board, IPlayer player, ref Point check, List<Cell> unfilterList, int listCount, int alpha, int beta)
        {
            int curIter = iter++;

            if (Win(board, this, ref check)) // ai win
            {
                return new TreeData(check, aiWin, board[check].StepType);
            }
            else if (Win(board, enemy, ref check))  // is player2 win
            {
                return new TreeData(check, opponentWin, board[check].StepType);
            }
            else//Если достигнута конца
            if (listCount == 0)
            {
                return new TreeData(check, 0, DefaultValues.TYPE_STEP_DEFAULT);
            }

            bool isAI = player == this;

            IPlayer nextplayer = isAI ? enemy : this;
            TreeData res = new TreeData(check, (sbyte)alpha, DefaultValues.TYPE_STEP_DEFAULT);
            listCount--;
            for (int i = 0; i < unfilterList.Count; i++)
            {
                Cell c = unfilterList[i];
                if (!c.IsSpace)
                    continue;
                c.StepType = player.StepType;
                Point p = c.Point;
                TreeData rr = AlphaBeta(board, nextplayer, ref p, unfilterList, listCount, -beta, -alpha);
                c.StepType = DefaultValues.TYPE_STEP_DEFAULT;

                if (-rr.Score > alpha)
                {
                    alpha = -rr.Score;
                    res = new TreeData(rr.Point, (sbyte)alpha, nextplayer.StepType);
                }
                if (alpha >= beta)
                {
                    return res;
                }
            }


            return res;
        }

        private bool isAsync;
        private AsyncOperation _botCallback = null;
        private PlayerResult playerResult;
        private System.Threading.CancellationToken cncTocken;
        private System.Threading.CancellationTokenSource tocken;
        private System.Threading.Tasks.Task task;
        private Board clasterMap;
        private System.Collections.IEnumerator __EnumBotCall(Board map)
        {
            System.Diagnostics.Debug.Print(message: "New thread created!");
            isReady = false;

            isGameReset = false;
            tocken = new System.Threading.CancellationTokenSource();
            cncTocken = tocken.Token;
            isThreadBroken = false;
            task = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    if (clasterMap == null)
                        clasterMap = (Board)map.Clone();
                    else if (clasterMap.Size != map.Size)
                        clasterMap.SetSize(map.Size);

                    map.CopyTo(clasterMap);
                    Point p = Point.one * -1;
                    TreeData rr = Minimax( clasterMap, this, ref p, clasterMap.ToArray().ToList(), (byte)clasterMap.Count);
                    playerResult = new PlayerResult(this, map[rr.Point]);
                }
                catch
                {
                    isThreadBroken = true;
                    cncTocken.ThrowIfCancellationRequested();
                }
            }, tocken.Token);
            while (true)
            {
                if (task.IsCompleted)
                {
                    if (task.IsCanceled || task.IsFaulted)
                    {
                        System.Diagnostics.Debug.Print("Thread is aborted!");
                        ResetCallBack();
                        task.Dispose();
                    }
                    break;
                }
                yield return 0;
            }
            iter = 0;
            if (!isGameReset)
                isReady = true;

            isAsync = false;
            isThreadBroken = true;
        }
        private void ResetCallBack()
        {
            isGameReset = true;
            if (_botCallback != null)
            {
                if (_botCallback.IsAsync)
                    _botCallback.Invoke();
                _botCallback.Dispose();
                _botCallback = null;
            }
            tocken?.Cancel();

            if (task != null && task.IsCompleted)
                task.Dispose();
            isAsync = false;
            isGameReset = false;
        }
        public override void Load(GameRegulations game)
        {
            base.Load(game);
#if !BAKE_ALGORITHM
            LoadCache(game.Board);
#endif
        }

        public override void Reset()
        {
            if (isGameReset || tocken == null)
                return;
            tocken.Cancel();
            isGameReset = true;
#if !BAKE_ALGORITHM
            saved_on_cache = null;
#endif
        }

        public override void Unload()
        {
#if !BAKE_ALGORITHM
            saved_on_cache = null;
#endif
            ResetCallBack();
        }

        public override PlayerResult GetStep(Board map)
        {

            if (!isReady)
                return PlayerResult.Empty;

            MakeCategorize(map.ToArray(), out List<Cell> enemyCells, out List<Cell> myCells, out List<Cell> freeCells);

            //Определяет, первый ход должен быть у меня
            bool isMyBeginStep = enemyCells.Count == 0 && myCells.Count == 0;

            Point sel = Point.one * -1;


            //Для начало проверяем, существуют ли свободные ячейки
            if (freeCells.Count != 0)
            {
                //Если первый ход, то выбераем лучшую позицию.
                if (false && isMyBeginStep)
                {
                    Cell[] r = new Cell[2];
                    r[0] = RandomBound(map, freeCells[0]);
                    r[1] = map.CenterCell;

                    sel = r[RndMgr.Range(0, 2)].Point;
                }
                //Если ход требует моего следующего определения. 
                else
                {
                    sel = playerResult.Cell.Point;

                }
            }

            _botCallback?.Dispose();
            _botCallback = null;

            return new PlayerResult(this, map[sel]);
        }

    }

    #region Low level
    /// <summary>
    /// Самый легкий по уровню сложности
    /// </summary>
    public class BotLower : BotBase
    {
        private Random random = new Random();

        public override string Name => "Bot Low Level";
        public override bool IsReady => true;
        public override byte StepType => base.StepType;
        public override void Load(GameRegulations game)
        {
            base.Load(game);
        }

        public override PlayerResult GetStep(Board map)
        {
            Cell[] cc = map.GetFreePoints();

            if (cc == null || cc.Length == 0)
                return PlayerResult.Empty;

            return new PlayerResult(this, cc[random.Next(0, cc.Length)]);
        }
    }
    #endregion
    #region Middle level
    public class BotMiddle : AI
    {
        public override string Name => "Bot Middle Level";

        public BotMiddle() : base(AILevel.Middle)
        {

        }


    }
    #endregion
    #region High level
    public class BotHigh : AI
    {
        public override string Name => "Bot High Level";

        public BotHigh() : base(AILevel.Hard)
        {

        }
    }
    #endregion
    #endregion

    #region Player
    public class Player : PlayerBase, IPlayer
    {
        private bool _isReady = false;
        private Cell _result = null;
        private GameRegulations _game;

        public string Name => "Игрок";

        public bool IsReady => _isReady;
        public byte StepType { get; set; }

        public MotionCollection MotionCollection => throw new NotImplementedException();

        public void Load(GameRegulations game)
        {
            _game = game;
        }
        public void Reset()
        {

        }
        public void Unload()
        {

        }
        public PlayerResult GetStep(Board map)
        {
            _isReady = false;
            return new PlayerResult(this, _result);
        }

        public void ToStep(Point to)
        {
            _isReady = false;
            Board map = _game.Board;

            _result = map[to];

            if (!_result || !_result.IsSpace)
                return;

            _isReady = true;
        }
    }
    #endregion

}
