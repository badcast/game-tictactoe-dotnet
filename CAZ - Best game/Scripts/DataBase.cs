#define PACKRELOAD_

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farbox.Packer;
using System.IO;

namespace CAZ
{
    public static class DataBase
    {
        private const string DATA_BASE_DIR = "data";
        private const string DIR_SCHEMES = "schemes";
        private const string DIR_BITMAPS = "bitmaps";

        /// <summary>
        /// Полный путь к папке программы
        /// </summary>
        public static string AppDir { get { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); } }
        /// <summary>
        /// Полный путь к папке Базы данных
        /// </summary>
        public static string DataBaseDirFullPath { get { return AppDir + "\\" + DataBaseDir; } }
        /// <summary>
        /// Абсолютный путь к папке базы данных
        /// </summary>
        public static string DataBaseDir { get { return DATA_BASE_DIR; } }
        /// <summary>
        /// Каталочный класс объекта
        /// </summary>
        public static Packer Packer { get; private set; }
        public static void DataBaseInitialize()
        {
            if (Packer != null)
                return;

            string rtDir = AppDir;


            Packer = new Packer();
            string file = rtDir + "\\" + DATA_BASE_DIR + ".pack";
#if PACKRELOAD
            Packer.PackInfo.Compact(DataBaseDirFullPath);
            Packer.PackSave(file);
#else
            Packer.PackLoad(file);
#endif

        }

        public static Stream GetFile(string path)
        {
            string isDir = DataBaseDirFullPath.ToLower();
            if (!path.ToLower().StartsWith(isDir))
                throw new FileNotFoundException("File not found " + path);
            if (File.Exists(path))
            {
                byte[] data = File.ReadAllBytes(path);
                return new MemoryStream(data);
            }
            
            path = path.Remove(0, isDir.Length);
            if (!Packer.PackInfo.FileExists(path))
                return null;

            return Packer.PackInfo.GetFileFromPath(path).stream;
        }

        public static Stream GetStreamFromDir(string absolutePath)
        {
            absolutePath = DataBaseDirFullPath + "\\" + absolutePath.Replace('/', '\\');

            if (!File.Exists(absolutePath))
                return null;

            return File.Open(absolutePath, FileMode.Open);
        }

        public static Stream GetStreamFromShemes(string absolutePath)
        {
            string fileName = CombineWithSchemesPath(absolutePath);
            if (!File.Exists(fileName))
                return null;
            return File.OpenRead(fileName);
        }
        public static Stream GetStreamFromBitmaps(string absolutePath)
        {
            string fileName = CombineWithBitmapsPath(absolutePath);
            if (!File.Exists(fileName))
                return null;
            return File.OpenRead(fileName);
        }

        public static string[] GetFilePaths(string absoluteDir)
        {
            string relativePath = (DataBaseDirFullPath + "\\" + absoluteDir);
            string[] ff = null;
            if (Directory.Exists(relativePath))
                ff = Directory.GetFiles(relativePath);
            else
                if (Packer.PackInfo.DirectoryExists(absoluteDir))
                ff = Packer.PackInfo.GetDirectoryFromPath(absoluteDir).GetFiles().Select((f) => absoluteDir + f.Name).ToArray();
            return ff;
        }

        public static string[] GetDirectories(string absoluteDir)
        {
            string relativePath = Path.GetDirectoryName(DataBaseDirFullPath + "\\" + absoluteDir);
            string[] ff = null;
            if (Directory.Exists(relativePath))
                ff = Directory.GetDirectories(relativePath);
            else
                if (Packer.PackInfo.DirectoryExists(absoluteDir))
                ff = Packer.PackInfo.GetDirectoryFromPath(absoluteDir).GetDirectoryes().Select((f) => absoluteDir + f.Name).ToArray();
            return ff;
        }

        public static string CombinePath(string absoluteDataPath)
        {
            return DataBaseDirFullPath + "\\" + absoluteDataPath.Replace('/', '\\');
        }

        public static string CombineWithSchemesPath(string fileName)
        {
            return CombinePath(DIR_SCHEMES) + "\\" + fileName;
        }

        public static string CombineWithBitmapsPath(string fileName)
        {
            return CombinePath(DIR_BITMAPS) + "\\" + fileName;
        }
    }
}
