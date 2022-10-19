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
    /// Логика взаимодействия для LevelButton.xaml
    /// </summary>
    public partial class LevelButton : UserControl, ILevelObject
    {
        static TripleData unlocked, locked;
        private TripleData curr;
        private int m_raitingLevel;
        private int m_level;
        private int m_state = 0;
        private ImageBrush m_brush;

        public int Raiting => m_raitingLevel;
        public int Level => m_level;
        public bool IsLocked { get { return curr == locked; } }

        public LevelButton()
        {
            this.InitializeComponent();
            if (!GameEngine.Started)
                return;

            this.icon.Background = m_brush = new ImageBrush();
            base.Loaded += Loaded;
            LoadSources();
        }

        private void setSource(TripleData source)
        {
            curr = source;

            setState(m_state);
        }

        /// <summary>
        /// states level 
        /// <para/>
        /// 0 normal 
        /// <para/>
        /// 1 hover
        /// <para/>
        /// 2 press
        /// </summary>
        private void setState(int state)
        {
            m_state = state;
            m_brush.ImageSource = curr.triples[state];
            this.icon.UpdateLayout();
        }

        private new void Loaded(object sender, EventArgs ee)
        {
            DesignManager dsg = DesignManager.current;
            LoadSources();

            this.MouseLeave += (o, e) =>
            {
                setState(0);
            };

            this.MouseEnter += (o, e) =>
            {
                setState(1);
            };

            this.MouseDown += (o, e) =>
            {
                setState(2);
            };

            this.MouseUp += (o, e) =>
            {
                setState(1);
            };

            setSource(curr);
        }

        private void UpdateCast()
        {
            setState(m_state);
        }

        public void LoadSources()
        {
            DesignManager dsg = DesignManager.current;
            if (unlocked == null || locked == null)
            {

                locked = dsg.GetTripleButton("LevelBoxButtonLocked");
                unlocked = dsg.GetTripleButton("LevelBoxButton");
            }
        }

        public void SetLock(bool locking)
        {
            setSource(locking ? locked : unlocked);
            _level.Opacity = locking ? 0 : 1;
            UpdateCast();
        }


        public void SetLevel(int newLevel)
        {
            m_level = newLevel;
        }

        public void SetRaiting(int newRaiting)
        {
            m_raitingLevel = newRaiting;
        }

        public void SetString(string text)
        {
            levelString.Content = text;
        }
    }
}
