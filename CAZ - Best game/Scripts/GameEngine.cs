using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Input;

namespace CAZ
{
    public enum Screens
    {
        scrLogo,
        scrMainMenu,
        scrAbout,
        scrLoader,
        scrGame
    }

    public struct RemoveableObject
    {
        private IDisposable disposable;
        public RemoveableObject(IDisposable disposable)
        {
            this.disposable = disposable;
        }

        public void Destroy()
        {
            disposable.Dispose();
        }
    }

    public struct Resolution
    {
        public int Width { get; }
        public int Height { get; }
        public bool IsFullscreen { get; set; }
        public Resolution(int width, int height, bool isFullscreen)
        {
            this.Width = width;
            this.Height = height;
            this.IsFullscreen = isFullscreen;
        }
        public static Resolution CurrentResolution { get { return new Resolution((int)MainWindow.Current.Width, (int)MainWindow.Current.Height, false); } }
    }

    public class GameEngine
    {
        private DesignManager _designManager;
        private BackgroundManager _backgroundManager;
        private WindowManager _windowManager;
        private IDictionary<Screens, Func<IScreen>> _screens;
        private IDictionary<Screens, IScreen> _activeScreens;
        private Screens _currentScreenType = Screens.scrLoader;
        private bool isWindowShowed;
        private Border _screenContent;
        private MainWindow window;
        private BitmapSource _cursor;
        private IScreen _currentScreen;

        public IScreen CurrentScreen { get => _currentScreen; }

        /// <summary>
        /// Уровень громкости звука
        /// </summary>
        public float SoundVolume { get; set; }
        /// <summary>
        /// Уровень громкости музыки
        /// </summary>
        public float MusicVolume { get; set; }
        /// <summary>
        /// Указывает на режим отладки игры
        /// </summary>
        public bool IsDebug { get; set; }

        public bool ShowFPS { get; set; }

        public System.Windows.Point MousePosition
        {
            get
            {
                return Mouse.GetPosition(window);
            }
        }

        public Screens CurrentScreenType { get => _currentScreenType; }

        public GameEngine(MainWindow window, IntPtr handle)
        {



            try
            {

                Current = this;
                this.Handle = handle;
                this.window = window;
                this._screenContent = window._content;

                DataBase.DataBaseInitialize();

                Configs.ConfigInitialize();

                //Загрузка Профилей
                Profiles.ReloadProfiles();

                //Загружаем менеджер дизайна
                this._designManager = new DesignManager(DataBase.CombineWithSchemesPath("main.shm"));

                GameValues.Init();

                //Загружаем фоновые изображения
                this._backgroundManager = new BackgroundManager(DataBase.CombineWithSchemesPath("backgrounds.shm"));

                //Прочее загрузки
                this._windowManager = new WindowManager();

                CustomGUI.InitGraph(handle);

                //Загрузка данных из INI
                SoundVolume = Configs.ReadSingle("audio", "soundvolume");
                MusicVolume = Configs.ReadSingle("audio", "musicvolume");
                IsDebug = Configs.ReadBool("sys", "debug");
                ShowFPS = Configs.ReadBool("sys", "showfps");

                //loaded levels
                LevelManager.LoadLevels();
                //AudioManager
                Audio.Initialize();

                /* window.Cursor = Cursors.None;
                 window._curDraw.Source = this._cursor = _designManager.GetBitmapSourceFromId("cursor");
                 var curr = window._curDraw;
                 curr.VerticalAlignment = VerticalAlignment.Top;
                 curr.HorizontalAlignment = HorizontalAlignment.Left;
                 curr.Width = _cursor.PixelWidth;
                 curr.Height = _cursor.PixelHeight;
                 //Устанавливает для курсора центральную точку
                 curr.Margin = new Thickness(window.Width / 2, window.Height / 2, 0, 0);
                 curr.IsHitTestVisible = false;*/
                window._curDraw.Visibility = Visibility.Collapsed;
                //Произвольный рендеринг
                window.PreviewMouseMove += (o, e) => CustomRenderer(e);

                window.PreviewKeyDown += KeyDown;
                window.PreviewMouseDown += MouseDown;
                window.PreviewMouseUp += MouseUp;
                window.Closed += (obj, ev) => Close();

                //Инициализация макета окон
                _screens = new Dictionary<Screens, Func<IScreen>>() {
                { Screens.scrLogo, ()=>new LogoScreen()},
                { Screens.scrMainMenu, ()=>new MainMenuScreen() },
                { Screens.scrAbout, ()=>new AboutScreen()},
                { Screens.scrLoader, ()=>new LoaderScreen()},
                { Screens.scrGame, ()=>new GameScreen() }
            };

                window.ContentRendered += (o, e) =>
                {
                    if (_designManager != null)
                    //Обновляем стилизацию
                    DesignManager.RefreshStyle(_designManager);
                };

                //Начало времени игры
                Time.InitTime();

                DispatcherTimer tmrContent = new DispatcherTimer();
                tmrContent.Interval = TimeSpan.FromMilliseconds(1);
                tmrContent.Tick += (o, e) => WindowRenderer();
                tmrContent.Start();

                //Начальное окно
                ShowScreen(Screens.scrLogo);

                WindowUpdater();

            }
            catch
            {
                this.window.Content = null;
                this.window.Background = new SolidColorBrush(Color.FromArgb(1, 150, 150, 150));
                ShowError("Game crashed.");
                Close();
            }
        }



        private void ShowError(string msg)
        {
            MessageBox.Show(msg);
        }
        private void WindowUpdater()
        {
            void update(AsyncOperation.Retrace retrace)
            {
                if (CurrentScreen != null)
                    CurrentScreen.Update();

                //Устанавливает все значения по умолчанию
                ToDefault();

            }

            AsyncOperation.CreateAsyncNotDependency(update).Start();


        }
        private void ToDefault()
        {
            GameInput.ToDef();
        }
        private void CustomRenderer(MouseEventArgs e)
        {


        }
        private int lastFPSValue = 0;
        private float lastFPSTimeStep;
        private void WindowRenderer()
        {
            if (ShowFPS)
            {
                if (Time.time > lastFPSTimeStep)
                {
                    lastFPSTimeStep = Time.time + 1;
                    lastFPSValue = (int)(Time.fps);
                }
                Time.CalculateFPS();
                CustomGUI.DrawText(lastFPSValue + " FPS", 0, Resolution.CurrentResolution.Height - 50, System.Drawing.Color.White);
            }

            if (window._curDraw.Visibility != Visibility.Visible)
                return;

            Thickness m = window._curDraw.Margin;
            var p = MousePosition;
            m.Left = p.X;
            m.Top = p.Y;

            window._curDraw.Margin = m;
        }
        private void KeyDown(object sender, KeyEventArgs e)
        {
            GameInput.Key = e.Key;
        }
        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            GameInput.MouseLeftButtonState = e.LeftButton;
            GameInput.MouseRightButtonState = e.RightButton;
            GameInput.MouseMiddleButtonState = e.MiddleButton;
        }
        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            GameInput.MouseLeftButtonState = e.LeftButton;
            GameInput.MouseRightButtonState = e.RightButton;
            GameInput.MouseMiddleButtonState = e.MiddleButton;
        }

        private List<RemoveableObject> _handlers;
        private void CallRemoveHandler()
        {
            if (_handlers == null)
                return;

            _handlers.ForEach((f) => { f.Destroy(); });
            _handlers.Clear();
        }

        public void AddRemoveHandler(IDisposable register)
        {
            if (_handlers == null)
                _handlers = new List<RemoveableObject>();

            _handlers.Add(new RemoveableObject(register));
        }

        /// <summary>
        /// Показывает окно с помощью загрузщика
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        public IScreen LoadScreen(Screens screen)
        {
            if (screen == Screens.scrLoader)
                return null;

            IScreen res = GetScreen(screen);

            LoaderScreen scr = (LoaderScreen)GetScreen(Screens.scrLoader);
            ShowScreen(Screens.scrLoader);
            scr.StartLoadScreen(screen);

            return res;
        }
        public IScreen ShowScreen(Screens screen)
        {
            if (isWindowShowed && screen == _currentScreenType)
                return null;

            if (isWindowShowed)
            {
                IScreen lastWindow = GetScreen(_currentScreenType);
                lastWindow.Closed();
            }
            //Вызывает пакет удаления асинхронных и инных операций, которые выполнялись или выполняются в последнем скрине
            CallRemoveHandler();

            IScreen w = _currentScreen = GetScreen(screen);
            _currentScreenType = screen;
            _screenContent.Child = (w.GetElement());
            w.Showed();

            isWindowShowed = true;

            return w;
        }

        public IScreen GetScreen(Screens screen)
        {
            if (_activeScreens == null)
            {
                _activeScreens = new Dictionary<Screens, IScreen>();
            }

            if (_activeScreens.TryGetValue(screen, out IScreen o))
                return o;
            else
            {
                IScreen get = _screens[screen]?.Invoke();
                _activeScreens.Add(screen, get);
                Screens last = _currentScreenType;
                _currentScreenType = screen;
                try
                {
                    get.Created();
                    get.SetStyle(_designManager);
                    DesignManager.RefreshStyle(_designManager);
                }
                catch
                {
                    ShowError("Init failure.");
                    Close();
                }
                _currentScreenType = last;
                return get;
            }
        }

        public void SetCursorVisible(bool visible)
        {
            window._curDraw.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }

        public IntPtr Handle { get; }

        public void Close()
        {
            this.window.Close();

            Configs.WriteValue("audio", "soundvolume", SoundVolume);
            Configs.WriteValue("audio", "musicvolume", MusicVolume);
            Configs.WriteValue("sys", "showfps", ShowFPS);
            Configs.Save();

            Profiles.SaveProfiles();
        }

        public static GameEngine Current { get; set; }
        public static bool Started { get; private set; }
        public static bool IsDebugGame { get { return Current.IsDebug; } }
        public static GameEngine GameEngineInit()
        {
            Started = true;
            return new GameEngine(MainWindow.Current, MainWindow.Current.HandleSource.Handle);
        }

        public static void CloseEngine()
        {
            Current?.Close();
        }

    }

    public static class GameInput
    {
        private static Dictionary<int, bool> _mouseButtonStates;
        public static Point MousePosition { get { return new Point((int)GameEngine.Current.MousePosition.X, (int)GameEngine.Current.MousePosition.Y); } }
        public static Key Key { get; internal set; }
        internal static MouseButtonState MouseLeftButtonState { get; set; }
        internal static MouseButtonState MouseRightButtonState { get; set; }
        internal static MouseButtonState MouseMiddleButtonState { get; set; }
        internal static MouseAction MouseAction { get; set; }
        public static bool IsMouseFire { get => MouseButton(0); }
        public static bool IsMouseLeftDown { get { return MouseButton(0); } }
        public static bool IsMouseRightDown { get { return MouseButton(1); } }
        public static bool IsMouseMiddleDown { get { return MouseButton(2); } }
        public static bool IsMouseLeftClick
        {
            get
            {
                return MouseButtonClicked(0);
            }
        }
        public static bool IsMouseRightClick
        {
            get
            {
                return MouseButtonClicked(1);
            }
        }
        public static bool IsMiddleLeftClick
        {
            get { return MouseButtonClicked(2); }
        }
        public static bool MouseButtonClicked(int button)
        {
            if (_mouseButtonStates == null)
            {
                _mouseButtonStates = new Dictionary<int, bool>()
                {
                    { 0, false},
                    { 1, false},
                    { 2, false}
                };
            }

            bool _res = MouseButton(button);

            if (_mouseButtonStates.ContainsKey(button))
            {
                bool msRes = _mouseButtonStates[button];
                if (!_res)
                {
                    _mouseButtonStates[button] = false;
                }
                else
                if (msRes == _res)
                {
                    return false;
                }
                else if (_res)
                {
                    _mouseButtonStates[button] = true;
                    return true;
                }
            }
            return false;
        }

        public static bool MouseButton(int button)
        {
            MouseButtonState state = MouseButtonState.Released;

            switch (button)
            {
                case 0:
                    state = MouseLeftButtonState;
                    break;
                case 1:
                    state = MouseRightButtonState;
                    break;
                case 2:
                    state = MouseMiddleButtonState;
                    break;

            }

            return state == MouseButtonState.Pressed;
        }

        internal static void ToDef()
        {
            Key = Key.None;
            MouseAction = MouseAction.None;
            //  MouseLeftButtonState = MouseButtonState.Released;
            //  MouseRightButtonState = MouseButtonState.Released;
            //  MouseMiddleButtonState = MouseButtonState.Released;
        }
    }

    public static class Time
    {
        private const float fpsMeasurePeriod = 1f;
        private static float fpsNextPeriod;
        private static bool _start;
        private static DateTime _startTime;
        private static float _time;
        private static float _deltaTime;
        private static int _frameCount;
        private static float _fps;
        private static float _lastTime;
        /// <summary>
        /// Текущее время 
        /// </summary>
        public static float time
        {
            get
            {
                return _time;
            }
        }

        public static float realTimeOnSince
        {
            get
            {
                return (float)(DateTime.Now - _startTime).TotalSeconds;
            }
        }

        public static float fps { get { return _fps; } }

        /// <summary>
        /// Прошедшее время в кадрах
        /// </summary>
        public static float deltaTime { get => _deltaTime; }

        /// <summary>
        /// Количество показанных кадров
        /// </summary>
        public static int frameCount { get => _frameCount; }
        public static void InitTime()
        {
            if (_start)
                return;
            _start = true;
            _startTime = DateTime.Now;
            _time = 0;
            _lastTime = 0;
            fpsNextPeriod = realTimeOnSince + fpsMeasurePeriod;
            CompositionTarget.Rendering += (o, e) =>
            {
                Calculate();
            };
        }

        /// <summary>
        /// Вычисляет время 
        /// </summary>
        public static void Calculate()
        {
            float _newTime = (float)(DateTime.Now - _startTime).TotalSeconds;

            float elapsed = time - _lastTime;



            _deltaTime = elapsed;
            if (elapsed >= 1f)
            {
                _lastTime = time;
            }
            _time = _newTime;



        }
        public static void CalculateFPS()
        {
            _frameCount++;
            if (realTimeOnSince > fpsNextPeriod)
            {
                _fps = (float)Math.Round(_frameCount / fpsMeasurePeriod, 1);
                _frameCount = 0;
                fpsNextPeriod += fpsMeasurePeriod;
            }
        }
    }
}
