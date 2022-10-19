using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;

namespace CAZ
{
    public static class Audio
    {
        public static List<Stream> streams;
        public static List<Stream> soundStreams;
        public static List<Stream> musicStreams;
        
        public static void Initialize()
        {
            return;
            streams = new List<Stream>();
            soundStreams = new List<Stream>();
            musicStreams = new List<Stream>();
            var ff = DataBase.GetFilePaths("sound");
            for (int i = 0; i < ff.Length; i++)
            {
                Stream s = DataBase.GetFile(ff[i]);
                soundStreams.Add(s);
                streams.Add(s);
            }
            ff = DataBase.GetFilePaths("music");
            for (int i = 0; i < ff.Length; i++)
            {
                Stream s = DataBase.GetFile(ff[i]);
                musicStreams.Add(s);
                streams.Add(s);
            }
        }

        public static void playSound(string soundName)
        {

        }

        public static void playMusic(string musicName)
        {

        }
    }
}
