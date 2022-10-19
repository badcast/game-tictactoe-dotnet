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
    public interface ILevelObject
    {
        int Level { get; }
        int Raiting { get; }
        bool IsLocked { get; }
        void SetLock(bool locking);
        void SetRaiting(int newRaiting);
        void SetLevel(int newLevel);
    }

    public interface ILevelBox
    {

        int LevelCount { get; }
        int CompletedLevelCount { get; }
        List<ILevelObject> Objects { get; }
        ILevelObject ObjectFromLevel(int level);
    }
    /// <summary>
    /// Логика взаимодействия для LevelBox.xaml
    /// </summary>
    public partial class LevelBox : UserControl
    {
        public List<ILevelObject> levels = new List<ILevelObject>();

        public LevelBox()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            levels.Clear();
            this.content.Items.Clear();
        }

        public void RefreshFromProfile(Profile targetProfile)
        {
            Clear();
            for (int i = 0; i < LevelManager.GetLevelLength(); i++)
            {
                LevelButton lvb = new LevelButton();
                content.Items.Add(lvb);
                lvb.SetLevel(i+1);
                bool isLocked = i > targetProfile.CurrentLevel;
                lvb.SetLock(isLocked);
                lvb.SetString("Уровень " + (i+1));
                levels.Add(lvb);
            }
        }
    }
}
