using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CAZ
{
    public delegate void Event();

    public class Window
    {
        private static WindowManager __windowr;

        private UIElement __wind;
        private Screens _screenType;
        /// <summary>
        /// Определяет, окно сейчас в режиме видимости
        /// </summary>
        public bool IsShown { get => IsWindowExist && __wind.Visibility == Visibility.Visible; }
        /// <summary>
        /// Название окна
        /// </summary>
        public string WindowName { get; }
        /// <summary>
        /// Уникальный адрес окна
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Определяет, существует ли окно в данной Сцене
        /// </summary>
        public bool IsWindowExist { get => GameEngine.Current.CurrentScreenType == _screenType; }

        public event Event Shown;

        public event Event Closed;

        /// <summary>
        /// Инициализатор
        /// </summary>
        public Window(WindowManager baseEd, int id, string windowName, UIElement windowPanel)
        {
            if (__windowr == null)
                __windowr = baseEd;
            this.WindowName = windowName;
            this.__wind = windowPanel;
            this.Id = id;
            this._screenType = GameEngine.Current.CurrentScreenType;
            __wind.Visibility = Visibility.Hidden;
        }

        public void Show()
        {
            if (!IsWindowExist)
                throw new Exception("Окно не найдена");
            if (IsShown)
                return;
            __wind.Visibility = Visibility.Visible;
            __windowr.AddHistory(Id);
            Shown?.Invoke();
        }

        public void ShowMain()
        {
            if (!IsWindowExist)
                throw new Exception("Окно не найдена");

            __windowr.ShowAsMain(Id);
        }

        public void Close()
        {
            if (!IsWindowExist)
                return;// throw new Exception("Окно не найдена");

            if (!IsShown)
                return;
            __wind.Visibility = Visibility.Hidden;
            __windowr.RemoveHistory(Id);
            Closed?.Invoke();
        }


    }

    public class WindowManager
    {
        public static WindowManager Current { get; set; }
        private Dictionary<int, Window> __windows;
        private List<int> _history;
        private Random r = new Random(int.MaxValue / 2);

        public bool IsHistory { get => _history.Count > 0; }
        public WindowManager()
        {
            Current = this;

            __windows = new Dictionary<int, Window>();
            _history = new List<int>();
        }

        private int GetSpaceIdent()
        {
            int get()
            {
                return r.Next(int.MinValue, int.MaxValue);
            }
            int id = 0;
            uint checkCount = 0;
            do
            {
                if (checkCount == uint.MaxValue)
                {
                    return -1;
                }

                id = get();
                checkCount++;
            } while (HasWindow(id) || id == 0 || id == -1);

            return id;
        }

        /// <summary>
        /// Создает новое окно и добавляет в колекцию других окон
        /// </summary>
        public Window AddWindow(string windowName, UIElement windowElement)
        {
            int id = GetSpaceIdent();
            //Значение определяет, что нет больше мест в макете окон
            if (id == -1)
                return null;
            Window w = new Window(this, id, windowName, windowElement);
            __windows.Add(id, w);
            return w;
        }

        /// <summary>
        /// Возвращает окно из уникального адреса
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public Window GetWindow(int window)
        {
            if (__windows.TryGetValue(window, out Window ww))
                return ww;
            return null;
        }

        /// <summary>
        /// Показывает новое окно, но закрывает предыдущий и возвращает результат выполнения
        /// </summary>
        /// <param name="previewWindow">Предыдущая окно</param>
        /// <param name="newWindow">Новое окно</param>
        /// <returns>Результат</returns>
        public bool ShowWindow(int previewWindow, int newWindow)
        {
            Window last = GetWindow(previewWindow), newWind = GetWindow(newWindow);

            if (last == null || newWind == null || !last.IsWindowExist || !last.IsWindowExist)
                return false;

            last.Close();
            newWind.Show();

            return true;
        }

        /// <summary>
        /// Показывает новое окно, но закрывает предыдущий и возвращает результат выполнения
        /// </summary>
        /// <param name="preview">Предыдущая окно</param>
        /// <param name="newWindow">Новое окно</param>
        /// <returns>Результат</returns>
        public bool ShowWindow(Window preview, Window newWindow)
        {
            return ShowWindow(preview.Id, newWindow.Id);
        }
        /// <summary>
        /// Показывает окно и задает приоритет высокий. Другие окна при этом закрываются
        /// </summary>
        public bool ShowAsMain(int window)
        {
            var f = __windows.Values;
            bool result = false;
            for (int i = 0; i < f.Count; i++)
            {
                var e = f.ElementAt(i);
                if (!e.IsWindowExist)
                    continue;

                if (e.Id == window)
                {
                    e.Show();
                    result = true;
                }
                else
                    e.Close();
            }

            return result;
        }

        /// <summary>
        /// Показывает окно и задает приоритет высокий. Другие окна при этом закрываются
        /// </summary>
        public bool ShowAsMain(Window window)
        {
            return ShowAsMain(window.Id);
        }

        public bool HasWindow(int window)
        {
            return GetWindow(window) != null;
        }

        public void AddHistory(int window)
        {
            if (_history.Contains(window))
                return;

            _history.Add(window);
        }

        public bool RemoveHistory(int window)
        {
            return _history.Remove(window);
        }

        public Window HistoryGetBack()
        {
            if (_history.Count > 1)
                return null;

            return GetWindow(_history[_history.Count - 2]);
        }

        public Window HistoryGetGo()
        {
            if (!IsHistory)
                return null;
            return GetWindow(_history[_history.Count - 1]);
        }

        public void HistoryClear()
        {
            _history.Clear();
        }

    }

}
