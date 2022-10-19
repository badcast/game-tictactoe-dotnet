using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IniParser;
using IniParser.Model;
using System.Drawing;

namespace AnimationEditor
{
    public static class Configs
    {
        private const string FILE_CONFIG_NAME = "Config.ini";
        private static string path;
        private static IniParser.FileIniDataParser parser;
        private static IniData data;
        public static void ConfigInitialize()
        {
            path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/" + FILE_CONFIG_NAME;
            parser = new FileIniDataParser();
            if (File.Exists(path))
                data = parser.ReadFile(path);
            else
                data = new IniData();
        }
        
        private static bool IsSpecialValue(Type valueType)
        {
            return valueType == typeof(Color);
        }

        private static void WriteVal(string key, object value)
        {
            if (key == null || value == null)
                return;

            Type t = value.GetType();
            string tpName = t.Name;
            if (data.Sections[tpName] == null)
                data.Sections.AddSection(tpName);

            string writable = "";
            if(IsSpecialValue(t))
            {
                if(t == typeof(Color))
                {
                    Color vl = (Color)value;
                    writable = string.Format("{0}, {1}, {2}, {3}", vl.A, vl.R, vl.G, vl.B);
                }
            }
            else
            {
                writable = value.ToString();
            }
            var sec = data.Sections[tpName];
            if (!sec.AddKey(key, writable))
            {
                sec[key] = writable;
            }
        }

        private static T GetVal<T>(string key) where T : new()
        {
            Type type = typeof(T);

            SectionData sec = null;
            if (!data.Sections.Any((f) =>
            {
                sec = f;
                return f.SectionName == type.Name;
            }))
                return default(T);

            if (!sec.Keys.ContainsKey(key))
                    return default(T);

            object val = sec.Keys[key];

            T gettable = default(T);

            if(IsSpecialValue(type))
            {
                if(type == typeof(Color))
                {
                    object vald = null;
                    string[] vls = val.ToString().Trim().Split(',');
                    if(vls.Length == 4)
                    {
                        vald = Color.FromArgb(byte.Parse(vls[0]), byte.Parse(vls[1]), byte.Parse(vls[2]), byte.Parse(vls[3]));
                    }
                    else
                    {
                        vald = Color.FromArgb(255, byte.Parse(vls[0]), byte.Parse(vls[1]), byte.Parse(vls[2]));
                    }
                    gettable = (T)vald;
                }
            }
            else
            {
                gettable = (T)System.Convert.ChangeType(val, type);
            }

            return (T) gettable; 
        }
        public static void Save()
        {
            if (data == null)
                return;

            parser.WriteFile(path, data);
        }

        public static bool HasKey(string key)
        {
            return data.Sections.Any((f) => f.Keys.ContainsKey(key));
        }

        public static bool GetBool(string key)
        {
            return GetVal<bool>(key);
        }
        public static string GetString(string key)
        {
            return GetVal<object>(key).ToString();

        }

        public static int GetInt(string key)
        {
            return GetVal<int>(key);

        }

        public static float GetSingle(string key)
        {
            return GetVal<float>(key);

        }

        public static double GetDouble(string key)
        {
            return GetVal<double>(key);
        }

        public static Color GetColor(string key)
        {
            return GetVal<Color>(key);
        }

        public static void WriteString(string key, string value)
        {

            WriteVal(key, value);
        }

        public static void WriteBool(string key, bool value)
        {

            WriteVal(key, value);
        }

        public static void WriteInt(string key, int value)
        {

            WriteVal(key, value);
        }

        public static void WriteSingle(string key, float value)
        {

            WriteVal(key, value);
        }

        public static void WriteDouble(string key, double value)
        {
            WriteVal(key, value);
        }

        public static void WriteColor(string key, Color value)
        {
            WriteVal(key, value);
        }

        public static string ReadValue(string section, string key)
        {
            var sec = data.Sections[section];
            if (sec == null)
                return null;
            var res = sec[key];
            return res;
        }
        
        public static void WriteValue(string section, string key, object value)
        {
            var sec = data.Sections[section];
            if (sec == null)
            {
                data.Sections.Add(new SectionData(section));
                sec = data[section];
                sec.AddKey(key);
            }

            sec[key] = value.ToString();
        }

        public static int ReadInt(string section, string key)
        {

            return int.TryParse(ReadValue(section, key), out int a) ? a : 0;
        }

        public static void WriteInt(string section, string key, int value)
        {
            WriteValue(section, key, value);
        }

        public static float ReadSingle (string section, string key)
        {
            return float.TryParse(ReadValue(section, key), out float a) ? a : 0;
        }

        public static void WriteSingle(string section, string key, float value)
        {
            WriteValue(section, key, value);
        }

        public static long ReadInt64(string section, string key)
        {
            return long.TryParse(ReadValue(section, key), out long a) ? a : 0;
        }

        public static void WriteInt64(string section, string key, long value)
        {
            WriteValue(section, key, value);
        }

        public static double ReadDouble(string section, string key)
        {
            return double.TryParse(ReadValue(section, key), out double a) ? a : 0;
        }

        public static void WriteDouble(string section, string key, double value)
        {
            WriteValue(section, key, value);
        }

        public static bool ReadBool(string section, string key)
        {
            return bool.TryParse(ReadValue(section, key), out bool a) ? a : false;
        }

        public static void WriteBool(string section, string key, bool value)
        {
            WriteValue(section, key, value);
        }
    }
}
