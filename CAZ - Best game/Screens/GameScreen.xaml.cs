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
using System.Windows.Media.Animation;
using System.IO;

namespace CAZ
{
    /// <summary>
    /// Логика взаимодействия для GameScreen.xaml
    /// </summary>
    public partial class GameScreen : UserControl, IScreen
    {
        public const int STANDART_SIZE = 450;

        private struct SettingB
        {
            public double angle;
            public Point windowPoint;

            public SettingB(double angle, Point windowPoint)
            {
                this.angle = angle;
                this.windowPoint = windowPoint;
            }
        }

        static SettingB[] wndPoints = new SettingB[12]{
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero),
            new SettingB(0, Point.zero)};

        private class Throad
        {
            private bool _isSimulate;
            private RotateTransform rotation;
            private ScaleTransform scale;
            public FrameworkElement parent;
            public FrameworkElement element;
            public Point CurrentPoint { get { return new Point((int)Margin.Left, (int)Margin.Top); } set { Margin = new Thickness(value.X, value.Y, Margin.Right, Margin.Bottom); } }
            public double Angle { get { return rotation.Angle; } set { rotation.Angle = value; } }
            public int Speed { get; set; }
            public double SpeedRotation { get; set; }
            public double Size { get { return scale.ScaleX; } set { scale.ScaleX = value; scale.ScaleY = value; ; } }
            public bool IsScreenBorder { get { return ViewedZone(); } }
            public double Width { get { return element.Width; } set { element.Width = value; } }
            public double Height { get { return element.Height; } set { element.Height = value; } }
            public Point Minimum { get; set; }
            public Point Maximum { get; set; }
            public Thickness Margin { get { return element.Margin; } set { element.Margin = value; } }
            public TransformGroup transform { get { return (TransformGroup)element.RenderTransform; } }
            public Point direction { get; set; }
            public bool IsSimulate { get => _isSimulate; }
            public Throad(FrameworkElement parent, FrameworkElement element, double angle = 0, int speed = 2, double size = 1)
            {
                this.parent = parent;
                this.element = element;
                element.VerticalAlignment = VerticalAlignment.Top;
                element.HorizontalAlignment = HorizontalAlignment.Left;
                element.Width = 50;
                element.Height = 50;
                this.Speed = speed;
                this.SpeedRotation = 2;
                this.direction = GetDirection(PaddingCell.pDown);
                var rr = Resolution.CurrentResolution;
                this.Minimum = new Point((int)-Width, (int)-Height);
                this.Maximum = new Point((int)(rr.Width), (int)(rr.Height));
                this.CurrentPoint = GetScreenRandomPoint(out Point dir, (int)Width, (int)Height);
                this.direction = dir;
                this.element.RenderTransform = new TransformGroup();
                transform.Children.Add(new ScaleTransform(size, size));
                transform.Children.Add(new RotateTransform(angle));
                scale = (ScaleTransform)transform.Children[0];
                rotation = (RotateTransform)transform.Children[1];
                this.element.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                this.Size = size;
                this.Angle = angle;
                this._isSimulate = true;
                RndMgr.ReSeed();
            }

            private bool ViewedZone()
            {
                Point cc = CurrentPoint;

                return cc.X >= Minimum.X && cc.X <= Maximum.X && cc.Y >= Minimum.Y && cc.Y <= Maximum.Y;
            }

            private Point GetDirection(PaddingCell direction)
            {
                return Board.GetDirectionFromPadding(direction);
            }

            private Point RandomDirection()
            {
                return new Point(RndMgr.Range(0, 2), RndMgr.Range(0, 2));
            }

            public void Simulate()
            {
                if (!IsSimulate)
                    return;

                //Если наш Throad объект больше невидим для игрока, тогда пересоздаем его и задаем случайное направление
                if (!IsScreenBorder)
                {
                    // Size = RndMgr.Range(2);
                    CurrentPoint = GetScreenRandomPoint(out Point dir, (int)(Width), (int)(Height));
                    direction = dir;
                    SpeedRotation = RndMgr.Range(5);
                    Speed = RndMgr.Range(1, 5);
                }

                Angle += SpeedRotation;
                if (Angle >= 360)
                    Angle = 0;
                // Size += 0.001;
                CurrentPoint += direction * Speed;

            }
            public void StopSimulate()
            {
                _isSimulate = false;
            }

            private static int[] p = new int[2];
            /// <summary>
            /// Определяет точку где объект Throad должен пересоздаться и задать для нее направление
            /// </summary>
            public static Point GetScreenRandomPoint(out Point toDirection, int widthOffset, int heightOffset)
            {
                Resolution rr = Resolution.CurrentResolution;

                int w = rr.Width;
                int h = rr.Height;

                int rnd()
                {
                    return RndMgr.Range(0, 2);
                }

                p[0] = w;
                p[1] = h;
                int index = rnd();
                p[index] = RndMgr.Range(0, p[index]);
                p[0] *= rnd();
                p[1] *= rnd();

                Point result = new Point(p[0], p[1]);

                p[0] = p[0] > 0 ? -1 : 1;
                p[1] = p[1] > 0 ? -1 : 1;


                toDirection = new Point(p[0], p[1]);

                result += new Point(widthOffset * toDirection.X * -1, heightOffset * toDirection.Y * -1);

                return result;
            }
        }

        private GameRegulations game;
        private Window menu;
        private Window winWindow;
        private double startTime;
        private Player player1, player2;
        private ImageSource starEnabled, starDisabled;
        private Image[] stars;
        private bool isStoped;
        private bool _progressAnimateBar;
        private IGridManager manager { get => ggPanel; }
        private double CurrentTime { get => Time.time - startTime; }
        public bool IsPlayerStep { get { return game.CurrentPlayer is Player; } }
        public bool IsProgressAnimate { get { return _progressAnimateBar; } set { _progressAnimateBar = value; progressBotBar.IsIndeterminate = value; } }
        public GameScreen()
        {
            InitializeComponent();
        }

        private void HideDraftLine()
        {
            draftLine.Visibility = Visibility.Hidden;
        }
        private void ShowDraftLine(IGridObject a, IGridObject b)
        {
            int w = (int)_gridElements.Margin.Left;
            int h = (int)_gridElements.Margin.Top;
            var def = new Point(w, h);
            Point offset = (new Point(manager.GridWidth, manager.GridHeight)) + def;

            Point aPoint = manager.GetPoint(a) + offset,
                  bPoint = manager.GetPoint(b) + offset;

            ShowDraftLine(aPoint, bPoint);
        }
        private void ShowDraftLine(Point a, Point b)
        {
            draftLine.Width = STANDART_SIZE;
            draftLine.SetPosition(a, b);
            // draftLine.Visibility = Visibility.Visible;
        }

        private string ToTotalTime(bool toHour = true)
        {
            int cur = (int)CurrentTime;
            int minute = cur / 60;
            int sec = cur % 60;

            string r = "";

            if (toHour)
            {
                int hour = cur / 3600;

                if (hour < 10)
                    r += "0";
                r += hour + ":";
            }

            if (minute < 10)
                r += "0";
            r += minute + ":";
            if (sec < 10)
                r += "0";
            r += sec;

            return r;
        }

        private void CallGame()
        {
            if (GameInput.Key == Key.Escape)
                if (menu.IsShown)
                    menu.Close();
                else
                    menu.ShowMain();

            if (isStoped)
                return;

            if (game.IsGameEnd)
            {
                CallPlayEnd();
            }
            else if (game.IsPlay)
            {
                timer.Content = ToTotalTime(false);

                stepType.Content = string.Format("Ход: {0}", game.CurrentPlayer.Name);

                //Вызывает метод обновления и состояния
                CallPlayStep();
            }
        }
        private bool isChange = true;
        private float beginStepTime = 0;
        private void CallPlayStep()
        {
            if (!IsPlayerStep)
            {
                IPlayer bot = game.CurrentPlayer;
                bool result = !(!bot.IsReady);

                if (isChange)
                {
                    isChange = false;
                    beginStepTime = Time.time;
                    manager.SetActiveMember(false);
                    IsProgressAnimate = true;
                }

                if (Time.time - beginStepTime <= DefaultValues.TIME_STEP_WAIT)
                    return;

                manager.SetActiveMember(result);
                IsProgressAnimate = !result;
            }
            else
                IsProgressAnimate = false;

            //Планируем ход 
            if (game.GetStep(out StepResult res))
            {
                IGridObject gridObj = manager.FromCell(res.Target);

                gridObj.SetPlayerType(res.Player.StepType);
                manager.Apply(gridObj);
                isChange = true;
            }
        }
        private void CallPlayEnd()
        {
            isStoped = true;
            double endTime = CurrentTime;
            //stat to
            var stat = LevelManager.levelStat;
            var currentLevel = LevelManager.CurrentLevel;
            stat.score = 0;
            stat.raitingCount = 0;
            stat.isPlayerWin = false;
            stat.isDraw = false;

            var result = game.WinResult;
            //Проверка на выйграш
            if (game.IsWinning)
            {
                stat.score = (uint)((endTime / Math.Max(currentLevel.levelRaitingTime, 0.01f)) * currentLevel.scorePower);
                if (stat.isPlayerWin = result.Player == player1)
                    stat.raitingCount = (int)Math.Min((currentLevel.levelRaitingTime * 3 / endTime), 3);
                CallWin();
            }
            else if (game.IsDrawGame)
            {
                stat.isDraw = true;
                CallDraw();
            }
            //Show window
            ShowWinWindow();
            game.StopGame();
            manager.SetActiveMember(false);
        }
        private void CallDraw()
        {
            stepType.Content = "Ничья.";
        }
        private void CallWin()
        {
            var win = game.WinResult;
            stepType.Content = "Конец игры.";

            IGridObject startPos = manager.FromCell(win.Result[0]),
                endPos = manager.FromCell(win.Result[win.Result.Count - 1]);

            for (int i = 0; i < win.Result.Count; i++)
                manager.FromCell(win.Result[i]).Select();

            ShowDraftLine(startPos, endPos);


        }

        Point GetRandomPoint()
        {
            return new Point(RndMgr.Range(0, (int)(ActualWidth)), RndMgr.Range(0, (int)(ActualHeight)));
        }
        ImageSource[] icons;
        private System.Collections.IEnumerator StartAnimationRandomThroader()
        {

            if (icons == null)
            {
                icons = new ImageSource[10]
                {
                    DesignManager.current.GetBitmapSourceFromId("gameTypePlayer1"),
                    DesignManager.current.GetBitmapSourceFromId("gameTypePlayer2"),
                    DesignManager.current.GetBitmapSourceFromId("gameIcon01"),
                    DesignManager.current.GetBitmapSourceFromId("gameIcon02"),
                    DesignManager.current.GetBitmapSourceFromId("gameIcon03"),
                    DesignManager.current.GetBitmapSourceFromId("gameIcon04"),
                    DesignManager.current.GetBitmapSourceFromId("gameIcon05"),
                    DesignManager.current.GetBitmapSourceFromId("gameIcon06"),
                    DesignManager.current.GetBitmapSourceFromId("gameIcon07"),
                    DesignManager.current.GetBitmapSourceFromId("gameIcon08"),
                };
            }
            int count = icons.Length;
            Throad[] throads = new Throad[count];
            throadParent.Children.Clear();
            int iconSel = 0;
            for (int i = 0; i < count; i++)
            {
                Image src = new Image();

                if (iconSel == icons.Length)
                    iconSel = 0;
                src.Source = icons[iconSel];
                iconSel++;


                var thr = throads[i] = new Throad(throadParent, src);
                Point newPoint = GetRandomPoint();
                thr.CurrentPoint = newPoint;
                throadParent.Children.Add(src);
            }

            float lastTime = 0;
            while (true)
            {
                if (Time.time > lastTime)
                {
                    for (int index = 0; index < count; index++)
                    {
                        Throad cur = throads[index];
                        if (GameEngine.IsDebugGame)
                            CustomGUI.DrawText("ID " + (index + 1), cur.CurrentPoint.X, cur.CurrentPoint.Y, System.Drawing.Color.Red);
                        //Симуляция выбранного объекта 
                        cur.Simulate();
                    }

                    lastTime = Time.time + 0.01f;

                }
                yield return null;
            }
        }

        private System.Collections.IEnumerator StartAnimationScreen()
        {
            float startTime = Time.time + 0.2f;
            float maxTime = 1;

            isStoped = true;
            gameTable.Opacity = 0;
            _gridElements.Opacity = 0;
            IsProgressAnimate = true;
            HideDraftLine();
            game.StopGame();
            manager.Free();
            stepType.Content = "Запуск игры...";
            timer.Content = "--:--";
            _gridElements.Opacity = 0;
            while (true)
            {
                float curTime = Time.time - startTime;
                float progress = curTime / maxTime;
                gameTable.Opacity = progress;
                //isEndprogress
                if (curTime > maxTime)
                    break;
                yield return null;
            }
            _gridElements.Opacity = 1;
            gameTable.Opacity = 1;

            ResetGameStatistics();
        }

        IPlayer[] players;
        IPlayer[] botLevels;
        IPlayer bot;
        private void ResetGameStatistics()
        {


            isStoped = false;
            startTime = Time.time;
            menu.Close();
            GameRules rr = GameRules.current;
            if (rr == null)
            {
                rr = GameRules.Default();
                rr.botLevel = 2;
                rr.MapSizeIndex = 0;// GameSystem.MapSizes.Length-1;
                rr.playerIsTic = false;
                rr.PlayerVsBot = true;
            }
            int sz = GameRegulations.MapSizes[rr.MapSizeIndex];

            if (game == null)
                game = new GameRegulations(sz, sz);

            bool isReset = true;
            if (players == null)
            {
                players = new IPlayer[2];
                botLevels = new IPlayer[3];
                botLevels[0] = new BotLower();
                botLevels[1] = new BotMiddle();
                botLevels[2] = new BotHigh();
                player1 = new Player();
                player2 = new Player();
                isReset = false;
            }

            bot = botLevels[rr.botLevel];

            if (rr.playerIsTic)
            {
                players[0] = player1;
                players[1] = rr.PlayerVsBot ? bot : player2;
            }
            else
            {
                players[0] = rr.PlayerVsBot ? bot : player2;
                players[1] = player1;
            }

            if (isReset)
                foreach (var p in players)
                    p.Reset();

            game.SetSize(sz, rr.winLineLength);
            game.PlayNewGame(players);
            game.ReplayGame();
            manager.Free();
            game.Board.Reset();
            ggPanel.CreateGrid(game.Board, sz, sz);
            HideDraftLine();
            //ShowDraftLine(manager.FromCell(_game.Map[0, 0]), manager.FromCell(_game.Map[2, 0]));
        }
        private AsyncOperation resetAsync;
        private void ResetGame()
        {
            if (resetAsync != null && !resetAsync.IsDisposed)
            {
                resetAsync.Dispose();
                resetAsync = null;
            }
            resetAsync = AsyncOperation.CreateAsync(StartAnimationScreen());
            resetAsync.Start();
        }

        public void ShowWinWindow()
        {
            var stat = LevelManager.levelStat;
            wintext.Content = stat.isDraw ? "Ничья!" : stat.isPlayerWin ? "Поздравляем!" : "Вы проиграли!";

            bNext.Visibility = (stat.isDraw || !stat.isPlayerWin) ? Visibility.Hidden : Visibility.Visible;

            //set star cell
            for (int i = 0; i < 3; i++)
            {
                stars[i].Source = (LevelManager.levelStat.raitingCount >= i + 1) ? starEnabled : starDisabled;
            }

            winWindow.ShowMain();
        }
        AsyncOperation throaderAnim;
        private void StartAnimationThrdr()
        {
            StopAnimationThrdr();
            throaderAnim = AsyncOperation.CreateAsync(StartAnimationRandomThroader(), System.Windows.Threading.DispatcherPriority.Background);
            throaderAnim.Start();
        }
        private void StopAnimationThrdr()
        {
            throaderAnim?.Dispose();
        }
        Point[] points = new Point[2];
        public void Update()
        {
            if ((GameInput.IsMouseLeftDown || GameInput.IsMouseRightDown))
            {
                Point p = GameInput.MousePosition;
                // var offset = this._canvas.PointToScreen(new System.Windows.Point(p.X, p.Y));
                // p += new Point((int)offset.X, (int)offset.Y);
                int index = GameInput.IsMouseLeftDown ? 0 : 1;

                points[index] = p;
                ShowDraftLine(points[0], points[1]);

            }

            CallGame();
        }

        public void Closed()
        {
            //Отправляет команду игрокам, что игра выгружается
            if (players != null)
                for (int i = 0; i < players.Length; i++)
                    players[i].Unload();

            ggPanel.Free();
            StopAnimationThrdr();
            winWindow.Close();
        }

        public void Created()
        {
            game = new GameRegulations(GameRegulations.MapSizes[0]);
            menu = WindowManager.Current.AddWindow("gameMenu", _menu);
            winWindow = WindowManager.Current.AddWindow("gameFinishWinWindow", windowWinresult);

            ggPanel.GridActivated += GridSelected;
            draftLine.Visibility = Visibility.Hidden;
            IsProgressAnimate = false;
            HideDraftLine();
            _clockImage.Source = DesignManager.current.GetBitmapSourceFromId("clock");

            ImageBrush imgBrsh = null;
            gameTable.Background = imgBrsh = BackgroundManager.BackgroundGetBrush("backgroundStars");

            bFinish.Click += delegate
            {
                LevelManager.levelStat.toLevelFinish("Игра закончена", true);
            };
            bTryAgain.Click += delegate
            {
                winWindow.Close();
                var stat = LevelManager.levelStat;
                ResetGame();
            };
            bNext.Click += delegate
            {

            };

            starEnabled = DesignManager.current.GetBitmapSourceFromId("star_enabled");
            starDisabled = DesignManager.current.GetBitmapSourceFromId("star_disabled");

            stars = new Image[3];
            stars[0] = star1; // left 
            stars[1] = star3; // right
            stars[2] = star2; // center
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
            menu.Close();
            winWindow.Close();
            ResetGame();
            StartAnimationThrdr();
        }

        private void bContinue_Click(object sender, RoutedEventArgs e)
        {
            menu.Close();
        }

        private void bReturnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            GameEngine.Current.LoadScreen(Screens.scrMainMenu);
        }

        private void GridSelected(IGridObject g)
        {
            if (!IsPlayerStep || !g.Entry.IsSpace)
                return;

            Player p = ((Player)game.CurrentPlayer);
            p.ToStep(g.Entry.Point);
        }

        private void bRestart_Click(object sender, RoutedEventArgs e)
        {
            GameEngine.Current.LoadScreen(Screens.scrGame);
        }

        private void b_resetGame_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }
    }
}
