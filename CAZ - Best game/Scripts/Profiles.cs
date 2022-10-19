using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CAZ
{
    public class Profile
    {
        public string Name;
        public int AchivementScore;
        public int CurrentLevel;
        public string profileFileName;
        public List<bool> prizeApplied;
        public Profile()
        {
            prizeApplied = new List<bool>();
        }

        public bool hasPrize(int prizeId)
        {
            if (prizeId == -1)
                throw new ArgumentException();

            if (prizeId >= prizeApplied.Count)
                return false;

            return prizeApplied[prizeId];
        }

        public void applyPrize(int prizeId)
        {
            int levLeng = LevelManager.GetLevelLength();
            if (prizeId == -1 || prizeId >= levLeng)
                throw new ArgumentException();

            if (prizeId >= prizeApplied.Count)
            {
                int c = prizeApplied.Count - prizeId;
                for (int f = 0; f < c; f++)
                {
                    prizeApplied.Add(false);
                }
            }

            prizeApplied[prizeId] = true;
        }
    }


    public class Profiles
    {
        const double secureVersion = 1.0;
        const String unityFile = "profile_unityfile";
        const String profExt = ".prf";
        static int focusProfileIndex;
        static List<Profile> _profiles;


        public const int MaxProfileCount = 10;
        public static string ProfileDir { get => DataBase.AppDir + "\\profiles\\"; }
        public static bool HaveProfile { get => GetCurrentProfile() != null; }

        static Profiles()
        {
            _profiles = new List<Profile>();

        }

        private static void shellExec()
        {
            if (!Directory.Exists(ProfileDir))
                Directory.CreateDirectory(ProfileDir);
        }

        public static Profile GetCurrentProfile()
        {
            if (!HasProfile(focusProfileIndex))
                return null;
            return _profiles[focusProfileIndex];
        }


        public static void ReloadProfiles()
        {
            shellExec();
            string emptyFile = ProfileDir + unityFile;
            if (File.Exists(emptyFile))
            {
                string[] d = File.ReadAllLines(emptyFile);
                if (!int.TryParse(d[d.Length - 1], out focusProfileIndex))
                    focusProfileIndex = -1;
                else if (focusProfileIndex == -1)
                    focusProfileIndex = 0;
            }
            string[] profileFiles = Directory.GetFiles(ProfileDir, "*" + profExt);
            for (int i = 0; i < Math.Min(profileFiles.Length, MaxProfileCount); i++)
            {
                try
                {
                    string[] data = File.ReadAllLines(profileFiles[i]);
                    double v = double.Parse(data[0]);
                    if (v < secureVersion)
                        continue;
                    string profName = data[1];
                    int achvScore = int.Parse(data[2]);
                    int crLevel = int.Parse(data[3]);

                    Profile p = new Profile();
                    int sz = int.Parse(data[4]);
                    if (sz > 0)
                    {
                        p.prizeApplied = new List<bool>(sz); // for alloc mem
                        for (int x = 0; x < sz; x++)
                        {
                            p.prizeApplied[x] = (data[6 + x] == "+") ? true : false;
                        }
                    }
                    p.Name = (profName);
                    p.AchivementScore = (achvScore);
                    p.CurrentLevel = (crLevel);
                    p.profileFileName = Path.GetFileName(profileFiles[i]);
                    _profiles.Add(p);
                }
                catch { continue; }


            }
        }

        public static void SaveProfiles()
        {
            shellExec();

            string nl = Environment.NewLine;
            for (int i = 0; i < _profiles.Count; i++)
            {
                Profile p = _profiles[i];
                string fileData = secureVersion + nl;
                fileData += p.Name + nl;
                fileData += p.AchivementScore + nl;
                fileData += p.CurrentLevel + nl;
                //save prize
                fileData += p.prizeApplied.Count + nl + "Prizes:" +nl;
                for (int s = 0; s < p.prizeApplied.Count; s++)
                {
                    fileData += (p.prizeApplied[s] ? "+" : "-") + nl;
                }
                string fname = ProfileDir + (!string.IsNullOrEmpty(p.profileFileName) ? p.profileFileName : (i < 10 ? "0" + i : "" + i) + profExt);
                File.WriteAllText(fname, fileData);
            }

            string emptyFile = ProfileDir + unityFile;
            using (var fs = new StreamWriter(File.Create(emptyFile)))
            {
                fs.Write("unity file: version 1.0"+nl);
                fs.Write("fileVersion: " + secureVersion + nl+ "profile data count: " + _profiles.Count + nl);
                fs.Write("Data->FocusProfile"+nl);
                fs.Write(focusProfileIndex);
            }
        }

        public static bool HasProfile(int indexProfile)
        {
            return !(indexProfile > MaxProfileCount || indexProfile < 0 || indexProfile >= _profiles.Count);
        }
        public static Profile SelectProfile(int indexProfile)
        {
            if (!HasProfile(indexProfile))
                return null;

            return _profiles[focusProfileIndex = indexProfile];
        }

        public static int AddNewProfile(Profile prof)
        {
            if (_profiles.Count >= MaxProfileCount)
                return -1;

            _profiles.Add(prof);
            return _profiles.Count - 1;
        }

        public static bool DeleteProfile(int index)
        {
            if (!HasProfile(index))
                return false;
            Profile p = _profiles[(index)];
            if (index == focusProfileIndex)
                focusProfileIndex = -1;
            else if (index < focusProfileIndex)
                focusProfileIndex -= focusProfileIndex - index;
            if (File.Exists(ProfileDir + p.profileFileName))
                File.Delete(ProfileDir + p.profileFileName);
            _profiles.RemoveAt(index);
            return true;
        }

        public static int GetProfileCount()
        {
            return _profiles.Count;
        }
        public static Profile FromIndex(int index)
        {
            return _profiles[index];
        }
    };
}
