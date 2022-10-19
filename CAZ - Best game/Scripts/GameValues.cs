using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAZ
{
    public static class GameValues
    {
        public static int DraftLeftOffset, DraftRightOffset;
        public static double DraftOpacityLevel;
        public static bool DraftIsTile, DraftIsStretch;
        public static void Init()
        {
            LoadFromDesignManager(DesignManager.current);
        }
        public static void LoadFromDesignManager(DesignManager designManager)
        {
            var conf_Game = designManager.GetConfig("game");

            DraftLeftOffset = conf_Game.ReadInt32("draft", "leftOffset");
            DraftRightOffset = conf_Game.ReadInt32("draft", "rightOffset");
            DraftIsTile = conf_Game.ReadBoolean("draft", "tile");
            DraftIsStretch = conf_Game.ReadBoolean("draft", "stretch");
            DraftOpacityLevel = conf_Game.ReadDouble("draft", "opacityLevel");
        }
    }
}
