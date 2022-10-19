using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;

namespace CAZ
{
    /// <summary>
    /// Логика взаимодействия для MainMenuScreen.xaml
    /// </summary>
    public partial class MainMenuScreen : UserControl, IScreen
    {
        List<ILevelObject> _levels = new List<ILevelObject>();
        SoundPlayer musicPlayer = new SoundPlayer();
        Window w_addNewProfile,
                w_editProfile,
                w_mainWindow,
                w_playWindow,
                w_optionWindow,
                w_singlePlayerCampaignWindow,
                w_AchievementsWindow,
                w_singlePlayerSLWindow;
        private WindowManager wmr { get => WindowManager.Current; }
        public MainMenuScreen()
        {
            InitializeComponent();
        }

        public void Update()
        {
        }

        public void Closed()
        {
            musicPlayer.Stop();

        }

        public void Created()
        {
            w_mainWindow = wmr.AddWindow("wMainMenu", wMainMenu);
            w_playWindow = wmr.AddWindow("wPlay", wPlay);
            w_optionWindow = wmr.AddWindow("wOptions", wMainOptions);
            w_singlePlayerCampaignWindow = wmr.AddWindow("wSinglePlayerCampaignWindow", wMainCampaign);
            w_singlePlayerSLWindow = wmr.AddWindow("wSinglePlayerSelectLevelWindow", wSpsWindow);
            w_AchievementsWindow = wmr.AddWindow("wAchievementsWindow", wMainCampaignAchievements);
            w_addNewProfile = wmr.AddWindow("wAddNewProfile", wAddNewProfile);
            w_editProfile = wmr.AddWindow("wEditProf", wSelectProfile);

            background.Source = BackgroundManager.BackgroundFromName("mainmenu_background");
            LogoBanner.Source = BackgroundManager.BackgroundFromName("gameLogo");
            _trackBarAudioV.ValueChanged += (v) => GameEngine.Current.SoundVolume = (float)v;
            _trackBarMusicV.ValueChanged += (v) => GameEngine.Current.MusicVolume = (float)v;

            bAddNewProf.Click += delegate
            {
                if (Profiles.GetProfileCount() >= Profiles.MaxProfileCount)
                {
                    MessageBox.Show("Профиль заполнен. Удалите один или несколько, чтобы добавить новый.");
                    return;
                }
                w_addNewProfile.ShowMain();
            };
            bAddNewProfInsec.Click += delegate
            {
                if (Profiles.GetProfileCount() >= Profiles.MaxProfileCount)
                {
                    MessageBox.Show("Профиль заполнен. Удалите один или несколько, чтобы добавить новый.");
                    return;
                }
                var f = new Profile();
                f.Name = (tProfNewName.Text);
                tProfNewName.Text = "Имя профиля";
                bool have = Profiles.HaveProfile;
                Profiles.SelectProfile(Profiles.AddNewProfile(f));
                if (have)
                    w_editProfile.ShowMain();
            };


            bProfCancelBack.Click += delegate
            {
                if (Profiles.HaveProfile)
                    w_editProfile.ShowMain();

            };

            bSelectProf.Click += delegate
            {
                int index = cmbxProfileList.SelectedIndex;
                if (Profiles.HasProfile(index))
                {
                    Profiles.SelectProfile(index);
                    w_mainWindow.ShowMain();
                }
            };

            bEditProfiles.Click += delegate
            {
                if (!Profiles.HaveProfile)
                    return;

                w_editProfile.ShowMain();
            };

            void refreshProfileList()
            {
                cmbxProfileList.Items.Clear();
                int index = -1;
                for (int i = 0; i < System.Math.Min(Profiles.GetProfileCount(), Profiles.MaxProfileCount); i++)
                {
                    Profile p = Profiles.FromIndex(i);
                    if (index == -1 && p == Profiles.GetCurrentProfile())
                        index = i;

                    cmbxProfileList.Items.Add(p.Name);
                }
                cmbxProfileList.SelectedIndex = index;
                if (Profiles.HaveProfile)
                    _levelBox.RefreshFromProfile(Profiles.GetCurrentProfile());
            }

            w_addNewProfile.Shown += delegate
            {
                if (Profiles.MaxProfileCount == Profiles.GetProfileCount())
                {
                    w_addNewProfile.Close();
                }

            };

            bDeleteProf.Click += delegate
            {
                if (Profiles.GetProfileCount() == 1)
                {
                    MessageBox.Show("Минимальный профиль должен быть 1");
                    return;
                }
                else if (cmbxProfileList.SelectedIndex == -1)
                {
                    MessageBox.Show("Профиль не выбран!");
                    return;
                }
                Profile p = Profiles.FromIndex(cmbxProfileList.SelectedIndex);
                if (MessageBox.Show($"Удалить профиль \"{p.Name}\"\nУровень: {p.CurrentLevel.ToString()}", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Profiles.DeleteProfile(cmbxProfileList.SelectedIndex);
                    refreshProfileList();
                }

            };

            w_editProfile.Shown += delegate
            {
                refreshProfileList();
            };


            cmbxMapSize.Items.Clear();
            foreach (var sz in GameRegulations.MapSizes)
            {
                cmbxMapSize.Items.Add(string.Format("{0}x{0}", sz));
            }

            cmbxMapSize.SelectionChanged += (o, ee) =>
            {
                int size = GameRegulations.MapSizes[cmbxMapSize.SelectedIndex];
                int[] szs = GameRules.GetWinLengths(size);
                cmbxWinLine.Items.Clear();
                for (int i = 0; i < szs.Length; i++)
                {
                    cmbxWinLine.Items.Add(szs[i]);
                }
                cmbxWinLine.SelectedIndex = szs.Length - 1;
            };

            cmbxVS.SelectionChanged += (o, ee) =>
            {
                bool vs = (cmbxVS.SelectedIndex != 1);
                cmbxBLevel.IsEnabled = vs;
                cmbxBLevel.Opacity = vs ? 100 : 0;
            };


            cmbxBLevel.SelectedIndex = 0;
            cmbxMapSize.SelectedIndex = 0;
            cmbxPType.SelectedIndex = 0;
            cmbxVS.SelectedIndex = 0;

            musicPlayer.SoundLocation = DataBase.DataBaseDirFullPath + "\\Music\\music.wav";
        }

        public void Showed()
        {
            //musicPlayer.PlayLooping();
            if (!Profiles.HaveProfile)
            {
                if (Profiles.GetProfileCount() > 0)
                    w_editProfile.ShowMain();
                else
                    w_addNewProfile.ShowMain();
            }
            else
                w_mainWindow.ShowMain();
            _trackBarAudioV.Value = GameEngine.Current.SoundVolume;
            _trackBarMusicV.Value = GameEngine.Current.MusicVolume;
            //Levelbox change
            if (Profiles.HaveProfile)
                _levelBox.RefreshFromProfile(Profiles.GetCurrentProfile());
        }

        public UIElement GetElement()
        {
            return this;
        }

        public void SetStyle(DesignManager design)
        {
        }

        private void bShowPlay_Click(object sender, RoutedEventArgs e)
        {
            w_playWindow.ShowMain();
        }

        private void bShowOption_Click(object sender, RoutedEventArgs e)
        {
            w_optionWindow.ShowMain();
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            w_mainWindow.ShowMain();
        }

        private void bBack_toCampignWindow_Click(object sender, RoutedEventArgs e)
        {
            w_singlePlayerCampaignWindow.ShowMain();
        }

        private void bPlayGameOn_Click(object sender, RoutedEventArgs e)
        {
            var prof = Profiles.GetCurrentProfile();
            int lev = prof.CurrentLevel;
            LevelManager.levelStat.toLevelStart("Уровень " + lev+1, lev);
        }

        private void bShowAchievementsWindow_Click(object sender, RoutedEventArgs e)
        {
            w_AchievementsWindow.ShowMain();
        }

        private void bPlayGame_Click(object sender, RoutedEventArgs e)
        {
            w_singlePlayerSLWindow.ShowMain();
        }

        private void bStartGame_Click(object sender, RoutedEventArgs e)
        {
            GameRules ss = GameRules.current;
            if (ss == null)
                GameRules.current = ss = new GameRules();

            ss.botLevel = cmbxBLevel.SelectedIndex;
            ss.MapSizeIndex = cmbxMapSize.SelectedIndex;
            ss.playerIsTic = cmbxPType.SelectedIndex == 0;
            ss.PlayerVsBot = cmbxVS.SelectedIndex == 0;
            ss.winLineLength = (int)cmbxWinLine.SelectedValue;
            GameEngine.Current.LoadScreen(Screens.scrGame);
        }

        private void bPlayCampaign_Click(object sender, RoutedEventArgs e)
        {
            w_singlePlayerCampaignWindow.ShowMain();
        }

        private void cmbxMapSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void bShowAbout_Click(object sender, RoutedEventArgs e)
        {
            GameEngine.Current.ShowScreen(Screens.scrAbout);
        }



        private void TripleButton_Click(object sender, RoutedEventArgs e)
        {
            GameEngine.CloseEngine();
        }
    }
}
