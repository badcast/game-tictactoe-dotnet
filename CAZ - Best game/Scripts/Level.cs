using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace CAZ
{
    public class LevelStats
    {
        public string levelName;
        public int levelPos;
        public uint score;
        public bool isFinish;
        public bool isDraw;
        public bool isPlayerWin;
        public bool isGameStart;
        public int raitingCount;
        ~LevelStats()
        {

        }

        public LevelStats()
        {

        }

        public void toNeutral()
        {
            this.levelName = "Simple game";
            this.levelPos = 0;
            isPlayerWin = false;
            isFinish = false;
            isGameStart = false;
            score = 0;
            isDraw = false;
            raitingCount = 0;
            GameEngine.Current.LoadScreen(Screens.scrGame);
        }

        public void toLevelFinish(string levelName, bool isFinished)
        {
            this.levelName = levelName;
            this.isFinish = isFinished;
            isGameStart = false;
            GameEngine.Current.LoadScreen(Screens.scrMainMenu);
        }

        public void toLevelStart(string levelName, int levPos)
        {
            this.levelName = levelName;
            this.levelPos = levPos;
            isPlayerWin = false;
            isFinish = false;
            isGameStart = true;
            score = 0;
            GameEngine.Current.LoadScreen(Screens.scrGame);
        }
    };
    public struct Level
    {
        public string levelName;
        public int levelID;
        public int levelPrize;
        public float levelRaitingTime;
        public int levelSize;
        public int botLevel;
        public float scorePower;
    }

    public class LevelManager
    {
        private static bool _leveldataLoaded;
        private static List<Level> levels;

        public static readonly LevelStats levelStat = new LevelStats();
        public static bool IsPlayerWin { get { return levelStat.isPlayerWin; } }
        public static bool IsLevelFinished { get { return levelStat.isFinish; } }
        public static Level CurrentLevel {  get { return getLevelStruct(levelStat.levelPos); } }
        public static int GetLevelLength()
        {
            return levels.Count;
        }
        public static Level getLevelStruct(int levelIndex)
        {
            return levels[levelIndex];
        }

        public static void LoadLevels()
        {
            if (_leveldataLoaded)
                return;

            string dataPath = DataBase.AppDir + "\\levels";

            string[] levelFiles = Directory.GetFiles(dataPath, "*.txt");
            levels = new List<Level>(levelFiles.Length);
            XmlDocument doc = new XmlDocument();
            string getStr(XmlNode nd, string name)
            {
                return nd[name].InnerText;
            }
            for (int i = 0; i < levelFiles.Length; i++)
            {
                //load
                doc.Load(levelFiles[i]);
                XmlElement r = doc["level"];
                Level lev = new Level();
                lev.levelName = getStr(r, "level_name");
                lev.scorePower = float.Parse(getStr(r, "level_score"));
                lev.levelID = int.Parse(getStr(r, "level_id"));
                lev.levelPrize = int.Parse(getStr(r, "level_prize"));
                lev.levelRaitingTime = float.Parse(getStr(r, "level_raiting"));
                lev.levelSize = int.Parse(getStr(r, "level_size"));
                lev.botLevel = int.Parse(getStr(r, "level_botDifficulty"));
                levels.Add(lev);
                //free
                doc.RemoveAll();
            }


            _leveldataLoaded = true;
        }
    };
};
