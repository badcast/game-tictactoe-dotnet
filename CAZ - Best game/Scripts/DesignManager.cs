using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IniParser.Model;

namespace CAZ
{
    public class TripleData
    {
        public Size Size;
        public BitmapSource[] triples;
        public BitmapSource Normal { get { return triples[0]; } }
        public BitmapSource Hover { get { return triples[1]; } }
        public BitmapSource Pressed { get { return triples[2]; } }

    }

    public class DesignManager
    {

        public class classSchemeConfigs
        {
            private IniParser.FileIniDataParser parser;
            private IniData data;

            public classSchemeConfigs()
            {
                parser = new IniParser.FileIniDataParser();
            }

            public void Load(string ConfigFile)
            {
                if (File.Exists(ConfigFile))
                    data = parser.ReadFile(ConfigFile);
                else
                    data = new IniData();
            }

            private bool IsSpecialValue(Type valueType)
            {
                return valueType == typeof(Color);
            }

            private void WriteVal(string key, object value)
            {
                if (key == null || value == null)
                    return;

                Type t = value.GetType();
                string tpName = t.Name;
                if (data.Sections[tpName] == null)
                    data.Sections.AddSection(tpName);

                string writable = "";
                if (IsSpecialValue(t))
                {
                    if (t == typeof(Color))
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

            private T GetVal<T>(string key) where T : new()
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

                if (IsSpecialValue(type))
                {
                    if (type == typeof(Color))
                    {
                        object vald = null;
                        string[] vls = val.ToString().Trim().Split(',');
                        if (vls.Length == 4)
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
                    gettable = (T)Convert.ChangeType(val, type);
                }

                return (T)gettable;
            }

            public bool HasKey(string key)
            {
                return data.Sections.Any((f) => f.Keys.ContainsKey(key));
            }

            #region splits

            public bool GetBool(string key)
            {
                return GetVal<bool>(key);
            }
            public string GetString(string key)
            {
                return GetVal<object>(key).ToString();

            }

            public int GetInt(string key)
            {
                return GetVal<int>(key);

            }

            public float GetSingle(string key)
            {
                return GetVal<float>(key);

            }

            public double GetDouble(string key)
            {
                return GetVal<double>(key);
            }

            public Color GetColor(string key)
            {
                return GetVal<Color>(key);
            }

            public string ReadValue(string section, string key)
            {
                var sec = data.Sections[section];
                if (sec == null)
                    return null;
                var res = sec[key];
                return res;
            }

            public int ReadInt(string section, string key)
            {

                return int.TryParse(ReadValue(section, key), out int a) ? a : 0;
            }

            public float ReadSingle(string section, string key)
            {
                return float.TryParse(ReadValue(section, key), out float a) ? a : 0;
            }

            public string ReadString(string section, string key)
            {
                return ReadValue(section, key);
            }
            public long ReadInt64(string section, string key)
            {
                return long.TryParse(ReadValue(section, key), out long a) ? a : 0;
            }

            public double ReadDouble(string section, string key)
            {
                return double.TryParse(ReadValue(section, key), out double a) ? a : 0;
            }

            public bool ReadBool(string section, string key)
            {
                return bool.TryParse(ReadValue(section, key), out bool a) ? a : false;
            }

            #endregion

        }

        public class Config
        {
            classSchemeConfigs __sch;

            public Config(classSchemeConfigs cnf)
            {
                __sch = cnf;
            }

            public bool ReadBoolean(string Section, string Key)
            {
                return __sch.ReadBool(Section, Key);
            }
            public int ReadInt32(string Section, string Key)
            {
                return __sch.ReadInt(Section, Key);
            }

            public long ReadInt64(string Section, string Key)
            {
                return __sch.ReadInt64(Section, Key);
            }

            public string ReadString(string Section, string Key)
            {
                return __sch.ReadString(Section, Key);
            }

            public float ReadSingle(string Section, string Key)
            {
                return __sch.ReadSingle(Section, Key);
            }

            public double ReadDouble(string Section, string Key)
            {
                return __sch.ReadDouble(Section, Key);
            }
        }

        private static List<IStyle> styles;
        public static void AddStyle(IStyle style)
        {
            if (styles == null)
                styles = new List<IStyle>();
            styles.Add(style);
        }


        public static void RefreshStyle(DesignManager design)
        {
            if (styles == null)
                return;

            foreach (var item in styles)
            {
                item.SetStyle(design);
            }
        }

        public static DesignManager current { get; set; }
        Dictionary<string, BitmapSource> dictionary;
        Dictionary<BitmapSource, ImageBrush> bitmapCache;
        Dictionary<string, BitmapImage> bitmapStream;
        Dictionary<string, TripleData> tripleButtons;
        Dictionary<string, Config> shemeConfigs;

        public string Name { get; set; }
        public string Author { get; set; }
        public Color borderColor { get; set; }

        public DesignManager(string designFile)
        {
            current = this;
            dictionary = new Dictionary<string, BitmapSource>();
            bitmapCache = new Dictionary<BitmapSource, ImageBrush>();
            shemeConfigs = new Dictionary<string, Config>();
            LoadXmlData(designFile);
        }

        private void LoadConfig(string shemeConfig, string cnfName = null)
        {
            classSchemeConfigs cnf = new classSchemeConfigs();

            bool loadConfig = false;

            string findDir = Path.GetDirectoryName(shemeConfig) + "\\";
            string sfname = Path.GetFileNameWithoutExtension(shemeConfig);
            string ext = ".ini";
            string check = findDir + cnfName + ext;
            int lev = 0;
            do
            {
                if (!File.Exists(check))
                {
                    check = findDir + sfname + ext;
                }
                else
                {
                    loadConfig = true;
                    break;
                }
                lev++;
            } while (lev < 2);

            //Загружаем контент настроек
            if (loadConfig)
            {
                cnf.Load(check);
            }
            else return;

            shemeConfigs.Add(cnfName, new Config(cnf));
        }

        private void LoadPart(string partLoad)
        {
            Int32Rect getRect(string rectValue)
            {
                rectValue = rectValue.Trim();
                string[] vls = rectValue.Split(',');

                return new Int32Rect(
                            int.Parse(vls[0]),
                            int.Parse(vls[1]),
                            int.Parse(vls[2]),
                            int.Parse(vls[3]));
            }
            BitmapImage getBitmap(string absolutePath)
            {
                if (bitmapStream == null)
                    bitmapStream = new Dictionary<string, BitmapImage>();

                BitmapImage img = null;
                absolutePath = absolutePath.ToLower();
                if (!bitmapStream.ContainsKey(absolutePath))
                {
                    Stream file = DataBase.GetStreamFromBitmaps(absolutePath);

                    img = new BitmapImage();

                    img.BeginInit();
                    img.StreamSource = file;
                    img.EndInit();

                    //file.Dispose();

                    bitmapStream.Add(absolutePath, img);
                }
                else
                {
                    img = bitmapStream[absolutePath];
                }
                return img; /*new BitmapImage(new Uri("file://" + p));*/
            }

            XmlDocument doc = new XmlDocument();

            doc.Load(partLoad);
            XmlElement root = doc["design"];

            BitmapImage map = null;
            XmlElement source = root["source"];
            if (source != null && !string.IsNullOrEmpty(source.GetAttribute("src")))
            {
                map = getBitmap(source.GetAttribute("src"));
                map.CacheOption = BitmapCacheOption.OnLoad;
            }
            XmlElement info = root["info"];
            if (info != null)
            {
                this.Name = info.GetAttribute("name");
                this.Author = info.GetAttribute("author");
            }
            XmlElement rects = root["rects"];
            if (rects != null)
                for (int i = 0; i < rects.ChildNodes.Count; i++)
                {
                    XmlNode node = rects.ChildNodes[i];

                    if (node is XmlComment)
                        continue;

                    XmlElement r_data = (XmlElement)node;
                    string item_id = r_data.Name;

                    Int32Rect item_rect = Int32Rect.Empty;

                    BitmapImage dest = map;
                    bool mapLoaded = false;
                    if (r_data.HasAttribute("src"))
                    {
                        dest = getBitmap(r_data.GetAttribute("src"));
                        mapLoaded = true;
                    }

                    if (r_data.HasAttribute("rect"))
                    {
                        //Преобразование
                        item_rect = getRect(r_data.GetAttribute("rect"));
                    }
                    else
                    {
                        item_rect = new Int32Rect(0, 0, dest.PixelWidth, dest.PixelHeight);
                    }


                    CroppedBitmap cropped = new CroppedBitmap(dest, item_rect);
                    BitmapSource bmp = cropped;

                    dictionary.Add(item_id, bmp);

                    if (mapLoaded)
                    {
                        //dest.Relo
                    }
                }

            XmlElement tripElem = root["TripleButtons"];
            if (tripElem != null)
            {
                if (tripleButtons == null)
                    tripleButtons = new Dictionary<string, TripleData>();
                for (int i = 0; i < tripElem.ChildNodes.Count; i++)
                {
                    XmlElement tt = (XmlElement)tripElem.ChildNodes[i];

                    string key = tt.GetAttribute("key");
                    Int32Rect startRect = getRect(tt.GetAttribute("rect"));
                    string direction = tt.GetAttribute("direction").ToLower();
                    //Default
                    if (string.IsNullOrEmpty(direction))
                        direction = "Right";

                    int x = 0;
                    int y = 0;

                    if (direction == "Right".ToLower())
                    {
                        x = 1;
                    }
                    else if (direction == "Left".ToLower())
                    {
                        x = -1;
                    }
                    else if (direction == "Up".ToLower())
                    {
                        y = -1;
                    }
                    else if (direction == "Down".ToLower())
                    {
                        y = 1;
                    }

                    BitmapSource[] tbmp = new BitmapSource[3];
                    var tb = new TripleData() { Size = new Size(startRect.Width, startRect.Height), triples = tbmp };
                    int w = (int)tb.Size.Width;
                    int h = (int)tb.Size.Height;
                    for (int f = 0; f < 3; f++)
                    {
                        tbmp[f] = new CroppedBitmap(map, new Int32Rect(startRect.X + (x * f * w), startRect.Y + (y * f * h), w, h));
                    }

                    tripleButtons.Add(key, tb);
                }
            }

            LoadConfig(partLoad, root.GetAttribute("config"));
        }

        private void LoadXmlData(string designFile)
        {
            Int32Rect getRect(string rectValue)
            {
                rectValue = rectValue.Trim();
                string[] vls = rectValue.Split(',');

                return new Int32Rect(
                            int.Parse(vls[0]),
                            int.Parse(vls[1]),
                            int.Parse(vls[2]),
                            int.Parse(vls[3]));
            }
            BitmapImage getBitmap(string absolutePath)
            {
                if (bitmapStream == null)
                    bitmapStream = new Dictionary<string, BitmapImage>();

                BitmapImage img = null;
                absolutePath = absolutePath.ToLower();
                if (!bitmapStream.ContainsKey(absolutePath))
                {
                    Stream file = DataBase.GetStreamFromBitmaps(absolutePath);

                    img = new BitmapImage();

                    img.BeginInit();
                    img.StreamSource = file;
                    img.EndInit();

                    //file.Dispose();

                    bitmapStream.Add(absolutePath, img);
                }
                else
                {
                    img = bitmapStream[absolutePath];
                }
                return img; /*new BitmapImage(new Uri("file://" + p));*/
            }

            XmlDocument doc = new XmlDocument();

            doc.Load(designFile);
            XmlElement root = doc["design"];

            BitmapImage map = null;
            XmlElement source = root["source"];
            if (source != null && !string.IsNullOrEmpty(source.GetAttribute("src")))
            {
                map = getBitmap(source.GetAttribute("src"));
                map.CacheOption = BitmapCacheOption.OnLoad;
            }
            XmlElement info = root["info"];
            if (info != null)
            {
                this.Name = info.GetAttribute("name");
                this.Author = info.GetAttribute("author");
            }
            XmlElement rects = root["rects"];
            if (rects != null)
                for (int i = 0; i < rects.ChildNodes.Count; i++)
                {
                    XmlNode node = rects.ChildNodes[i];

                    if (node is XmlComment)
                        continue;

                    XmlElement r_data = (XmlElement)node;
                    string item_id = r_data.Name;

                    Int32Rect item_rect = Int32Rect.Empty;

                    BitmapImage dest = map;
                    bool mapLoaded = false;
                    if (r_data.HasAttribute("src"))
                    {
                        dest = getBitmap(r_data.GetAttribute("src"));
                        mapLoaded = true;
                    }

                    if (r_data.HasAttribute("rect"))
                    {
                        //Преобразование
                        item_rect = getRect(r_data.GetAttribute("rect"));
                    }
                    else
                    {
                        item_rect = new Int32Rect(0, 0, dest.PixelWidth, dest.PixelHeight);
                    }


                    CroppedBitmap cropped = new CroppedBitmap(dest, item_rect);
                    BitmapSource bmp = cropped;

                    dictionary.Add(item_id, bmp);

                    if (mapLoaded)
                    {
                        //dest.Relo
                    }
                }

            XmlElement tripElem = root["TripleButtons"];
            if (tripElem != null)
            {
                tripleButtons = new Dictionary<string, TripleData>();
                for (int i = 0; i < tripElem.ChildNodes.Count; i++)
                {
                    XmlElement tt = (XmlElement)tripElem.ChildNodes[i];

                    string key = tt.GetAttribute("key");
                    Int32Rect startRect = getRect(tt.GetAttribute("rect"));
                    string direction = tt.GetAttribute("direction").ToLower();
                    //Default
                    if (string.IsNullOrEmpty(direction))
                        direction = "Right";

                    int x = 0;
                    int y = 0;

                    if (direction == "Right".ToLower())
                    {
                        x = 1;
                    }
                    else if (direction == "Left".ToLower())
                    {
                        x = -1;
                    }
                    else if (direction == "Up".ToLower())
                    {
                        y = -1;
                    }
                    else if (direction == "Down".ToLower())
                    {
                        y = 1;
                    }

                    BitmapSource[] tbmp = new BitmapSource[3];
                    var tb = new TripleData() { Size = new Size(startRect.Width, startRect.Height), triples = tbmp };
                    int w = (int)tb.Size.Width;
                    int h = (int)tb.Size.Height;
                    for (int f = 0; f < 3; f++)
                    {
                        tbmp[f] = new CroppedBitmap(map, new Int32Rect(startRect.X + (x * f * w), startRect.Y + (y * f * h), w, h));
                    }

                    tripleButtons.Add(key, tb);
                }
            }

            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                if (!(root.ChildNodes[i] is XmlElement))
                    continue;

                XmlElement x = (XmlElement)root.ChildNodes[i];

                if (x.Name.ToLower() == "load")
                {
                    string fileName = x.GetAttribute("src");
                    if (string.IsNullOrEmpty(fileName))
                        continue;
                    fileName = DataBase.CombineWithSchemesPath(fileName);
                    LoadPart(fileName);
                }
            }

            //  map.Dispose();
        }

        public bool HasId(string id)
        {
            return dictionary.ContainsKey(id);
        }

        public ImageBrush GetImageBrushFromId(string id)
        {
            BitmapSource src = GetBitmapSourceFromId(id);
            if (src == null)
                return null;

            ImageBrush _brsh;
            if (bitmapCache.TryGetValue(src, out _brsh))
            {
                return _brsh;
            }

            bitmapCache.Add(src, _brsh = new ImageBrush(src));

            return (ImageBrush)_brsh;

        }
        public TripleData GetTripleButton(string id)
        {
            if (tripleButtons.TryGetValue(id, out TripleData t))
            {
                return t;
            }

            return null;
        }
        public Config GetConfig(string id)
        {
            if (shemeConfigs.TryGetValue(id, out Config c))
                return c;

            return null;
        }
        public void RefreshStyle()
        {
            RefreshStyle(this);
        }

        public BitmapSource GetBitmapSourceFromId(string id)
        {
            if (!HasId(id))
                return null;
            return dictionary[id];
        }
    }

    public class BackgroundManager
    {
        private Dictionary<string, BitmapImage> bitmapStream;
        private Dictionary<string, BitmapImage> resultData;
        public BackgroundManager(string backgroundSchemeFile)
        {
            Current = this;

            BitmapImage getBitmap(string absolutePath)
            {
                if (bitmapStream == null)
                    bitmapStream = new Dictionary<string, BitmapImage>();

                BitmapImage img = null;
                absolutePath = absolutePath.ToLower();
                if (!bitmapStream.ContainsKey(absolutePath))
                {
                    Stream file = DataBase.GetStreamFromBitmaps(absolutePath);

                    img = new BitmapImage();

                    img.BeginInit();
                    img.StreamSource = file;
                    img.EndInit();

                    bitmapStream.Add(absolutePath, img);
                }
                else
                {
                    img = bitmapStream[absolutePath];
                }
                return img; /*new BitmapImage(new Uri("file://" + p));*/
            }

            bitmapStream = new Dictionary<string, BitmapImage>();
            resultData = new Dictionary<string, BitmapImage>();

            XmlDocument doc = new XmlDocument();
            doc.Load(backgroundSchemeFile);

            XmlElement backgrounds = doc["backgrounds"];
            if (backgrounds != null)
            {
                for (int i = 0; i < backgrounds.ChildNodes.Count; i++)
                {
                    if (!(backgrounds.ChildNodes[i] is XmlElement))
                        continue;

                    XmlElement x = (XmlElement)backgrounds.ChildNodes[i];

                    string n = x.GetAttribute("name");
                    string filename = x.GetAttribute("filename");

                    BitmapImage b = getBitmap(filename);

                    resultData.Add(n, b);
                }
            }

        }
        public BitmapImage GetBackground(string backgroundName)
        {
            if (resultData.TryGetValue(backgroundName, out BitmapImage b))
            {
                return b;
            }
            return null;
        }

        private static Dictionary<string, ImageBrush> _cacheBrush;
        public static BackgroundManager Current { get; private set; }
        public static BitmapImage BackgroundFromName(string backgroundName)
        {
            return Current.GetBackground(backgroundName);
        }

        public static ImageBrush BackgroundGetBrush(string backgroundName)
        {
            if (_cacheBrush == null)
                _cacheBrush = new Dictionary<string, ImageBrush>();

            if (_cacheBrush.TryGetValue(backgroundName, out ImageBrush res))
                return res;

            BitmapImage v = BackgroundFromName(backgroundName);

            if (v == null)
                return null;

            res = new ImageBrush(v);
            _cacheBrush.Add(backgroundName, res);
            return res;
        }
    }



}
