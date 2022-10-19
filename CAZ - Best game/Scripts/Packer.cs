using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Linq;

namespace Farbox.Packer
{
    public struct sPos
    {
        public long startPos;
        public long endPos;

        public sPos(long startPos, long endPos)
        {
            this.startPos = startPos;
            this.endPos = endPos;
        }
    }


    public interface IPacker
    {
        Packer packer { get; }
    }
    public class PackFile : IPacker, IDisposable
    {
        ~PackFile()
        {
            Dispose(false);
        }

        private sPos _sPos;
        private bool isPossed = false;
        private bool _streamFlushing = false;
        private Stream _stream;

        public string Name { get; set; }
        public long Size { get { return stream.Length; } }
        public Stream stream
        {
            get
            {
                if (isPossed && !_streamFlushing && _stream == null)
                {
                    _stream = packer.GetStreamingPos(_sPos);
                }

                return _stream;
            }
        }
        public Packer packer { get; private set; }
        public PackFile(Packer packer, Stream stream, string Name)
        {
            this.packer = packer;
            this.Name = Name;
            byte[] bts = new byte[stream.Length];
            stream.Write(bts, 0, bts.Length);
            this._stream = new MemoryStream(bts);
        }

        public PackFile(Packer packer, string FileName, string Name)
        {
            this.packer = packer;
            this.Name = Name;
            this._stream = new MemoryStream(File.ReadAllBytes(FileName));
        }

        public PackFile(Packer packer, byte[] data, string Name)
        {
            this.packer = packer;
            this.Name = Name;
            this._stream = new MemoryStream(data);
        }

        public PackFile(Packer packer, sPos streamingPos, string Name)
        {
            if (!packer.IsFileLoaded)
                throw new FileLoadException("Система не удается открыть Packer поток, так как он был не загружен из файла");
            this.packer = packer;
            this._sPos = streamingPos;
            this.Name = Name;
            this.isPossed = true;
        }

        /// <summary>
        /// Удаляет автоматический ненужный поток и возвращает результат выполнения
        /// </summary>
        /// <returns></returns>
        public bool AutoDestruct()
        {
            try
            {
                _streamFlushing = true;
                if (!isPossed || _stream == null)
                    return false;

                MemoryStream ms = (MemoryStream)_stream;

                System.Reflection.FieldInfo f = typeof(MemoryStream).GetField("_buffer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                object buff = null;
                if (f != null)
                {
                    buff = f.GetValue(ms);
                }

                ms.Dispose();
                _stream = null;
                ms = null;
                f = null;

                return CAZ.G_G.destroy(ref buff);
            }
            catch
            {
                _streamFlushing = false;
            }

            return _streamFlushing;
        }

        /// <summary>
        /// Очищает текущий <see cref="PackFile"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (stream != null)
                    stream.Dispose();

                packer = null;
                Name = null;
            }
            else
            {
            }
        }
    }
    public class PackDirectory : IPacker, IDisposable
    {
        ~PackDirectory()
        {
            Dispose(false);
        }
        private string dirName;
        /// <summary>
        /// Имя папки
        /// </summary>
        public string Name { get { return isRoot ? "ROOT" : dirName; } set { dirName = value; } }
        /// <summary>
        /// Главный каталог ?
        /// </summary>
        public bool isRoot { get; }

        /// <summary>
        /// Каталоги в этом каталоге
        /// </summary>
        public Dictionary<string, PackDirectory> directories;
        /// <summary>
        /// Файлы в этом каталоге
        /// </summary>
        public Dictionary<string, PackFile> files;

        /// <summary>
        /// Верхний уровень каталога
        /// </summary>
        public PackDirectory upDirectory { get; }

        public Packer packer { get; }

        public PackDirectory()
        {

        }

        public PackDirectory(Packer packer, PackDirectory upDirectory, string Name, bool root = false)
        {
            this.packer = packer;
            this.isRoot = root;
            this.Name = Name;
            this.upDirectory = upDirectory;
            this.directories = new Dictionary<string, PackDirectory>();
            this.files = new Dictionary<string, PackFile>();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveAll();
                files = null;
                directories = null;
                Name = null;
            }
            else
            {
            }
        }

        /// <summary>
        /// Удаляет подкаталог текущего каталога
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        public bool RemoveDirectory(string directoryName)
        {
            foreach (var dir in directories)
            {
                string pos = dir.Key;
                if (pos.ToLower() == directoryName.ToLower())
                {
                    dir.Value.Dispose();
                    directories.Remove(pos);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Удаляет файл текущего каталога
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public bool RemoveFile(string FileName)
        {
            foreach (var file in files)
            {
                string pos = file.Key;
                if (pos.ToLower() == FileName.ToLower())
                {
                    file.Value.Dispose();
                    files.Remove(pos);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Удаляет все файлы, каталоги и подкаталоги текущего пути
        /// </summary>
        public void RemoveAll()
        {
            foreach (var f in files)
            {
                f.Value.Dispose();
            }

            files.Clear();

            foreach (var d in directories)
            {
                d.Value.Dispose();
            }
            directories.Clear();
        }

        private bool HasFile(string fileName)
        {
            foreach (var file in files.Keys)
            {
                if (file.ToLower() == fileName.ToLower())
                    return true;
            }
            return false;
        }

        private bool HasDirectory(string dirName)
        {
            return GetDirectory(dirName) != null;
        }

        /// <summary>
        /// Выводит список файлов текущего каталога
        /// </summary>
        /// <returns></returns>
        public PackFile[] GetFiles()
        {
            return files.Values.ToArray();
        }

        /// <summary>
        /// Выводит список подкаталогов текущего каталога
        /// </summary>
        public PackDirectory[] GetDirectoryes()
        {
            return directories.Values.ToArray();
        }

        public PackDirectory GetDirectory(string DirectoryName)
        {
            foreach (var item in directories)
            {
                if (item.Key.ToLower() == DirectoryName.ToLower())
                    return item.Value;
            }

            return null;
        }

        public PackFile GetFile(string FileName)
        {
            foreach (var item in files)
            {
                if (item.Key.ToLower() == FileName.ToLower())
                    return item.Value;
            }

            return null;
        }

        /// <summary>
        /// Добавляет подкаталог 
        /// </summary>
        public PackDirectory AddDirectory(string DirectoryName)
        {
            if (HasDirectory(DirectoryName))
                return GetDirectory(DirectoryName);
            PackDirectory newDir = new PackDirectory(packer, this, DirectoryName);
            directories.Add(DirectoryName, newDir);
            return newDir;
        }

        public PackDirectory AddDirectory(PackDirectory Directory)
        {
            if (HasDirectory(Directory.Name))
                return GetDirectory(Directory.Name);
            directories.Add(Directory.Name, Directory);

            return Directory;
        }

        public PackFile AddFile(string FileName, Stream stream)
        {
            if (HasFile(FileName))
                throw new Exception("Файл существует!");

            PackFile pf = new PackFile(packer, stream, FileName);
            files.Add(FileName, pf);
            return pf;
        }

        public PackFile AddFile(PackFile file)
        {
            if (HasFile(file.Name))
                throw new Exception("Файл существует!");
            files.Add(file.Name, file);

            return file;
        }


        /// <summary>
        /// Выводит имя папки
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
    public class PackInfo : IPacker
    {
        private enum FindType
        {
            Directory,
            File
        }
        public string Packname { get; set; }
        /// <summary>
        /// Главный каталог
        /// </summary>
        public PackDirectory Root { get; private set; }
        /// <summary>
        /// Версия
        /// </summary>
        public int Version { get { return packer.Version; } }
        public Packer packer { get; }
        public PackInfo(Packer pack)
        {
            this.packer = pack;
            this.Root = new PackDirectory(pack, null, "", true);
        }


        /// <summary>
        /// Существует ли файл, по определенному адресу 
        /// </summary>
        public bool FileExists(string pPath)
        {
            return GetOfPath(pPath, FindType.File) != null;
        }

        /// <summary>
        /// Существует ли такой каталог 
        /// </summary>
        public bool DirectoryExists(string pPath)
        {
            return GetOfPath(pPath, FindType.Directory) != null;
        }

        /// <summary>
        /// Создает каталог или подкаталоги 
        /// </summary>
        /// <param name="pPath"></param>
        public PackDirectory DirectoryCreate(string pPath)
        {
            pPath = pPath.StartsWith("\\\\") ? pPath.Remove(1, 1) : pPath;
            string[] splits = pPath.Split('\\');

            string pos = "";
            PackDirectory last = Root;
            PackDirectory dir = null;
            bool reCreated = false;
            foreach (var spl in splits)
            {
                reCreated = false;
                refs:
                dir = GetDirectoryFromPath(pos);
                if (dir == null)
                {
                    dir = last.AddDirectory(spl);
                }
                if (!reCreated)
                {
                    bool isEnd = !pos.EndsWith("\\");
                    pos += (pos.StartsWith("\\") ? isEnd ? "\\" : "" : "\\") + spl;
                    if (isEnd)
                        last = dir;
                }

                if (!DirectoryExists(pos))
                {
                    reCreated = true;
                    goto refs;
                }

                last = dir;
            }

            return dir;
        }

        /// <summary>
        /// Файл на указанном адресе
        /// </summary>
        public PackFile GetFileFromPath(string pPath)
        {
            return (PackFile)GetOfPath(pPath, FindType.File);
        }

        /// <summary>
        /// Каталог по указанному адресу
        /// </summary>
        public PackDirectory GetDirectoryFromPath(string pPath)
        {
            return (PackDirectory)GetOfPath(pPath, FindType.Directory);
        }

        /// <summary>
        /// Удаляет каталог по указанному пути
        /// </summary>
        public bool DeleteDirectory(string pPath)
        {
            string extractName = System.IO.Path.GetFileName(pPath);
            PackDirectory pack = GetDirectoryFromPath(pPath);

            if (pack == null)
                return false;

            PackDirectory upDir = pack.upDirectory;

            return upDir.RemoveDirectory(extractName); ;
        }
        /// <summary>
        /// Удаляет файл по указанному пути
        /// </summary>
        public bool DeleteFile(string pPath)
        {
            string extractName = System.IO.Path.GetFileName(pPath);
            string extractDirectory = System.IO.Path.GetDirectoryName(pPath);
            PackDirectory pack = GetDirectoryFromPath(extractDirectory);

            if (pack == null)
                return false;

            return pack.RemoveFile(extractName);
        }

        /// <summary>
        /// Вставляет в <see cref="Packer"/> глобальную папку
        /// </summary>
        public void Compact(string directoryPath)
        {
            List<string> dirsA = new List<string>();
            List<string> filesA = new List<string>();

            Action<string> getFiles = (dirForFind) =>
            {
                string[] files = Directory.GetFiles(dirForFind);
                filesA.AddRange(files);
            };

            Action<string> getDirs = null;
            getDirs = (findDirs) =>
            {
                string[] dirs = Directory.GetDirectories(findDirs);
                for (int i = 0; i < dirs.Length; i++)
                {
                    string dir = dirs[i];
                    dirsA.Add(dir);
                    getFiles(dir);
                    getDirs(dir);
                }
            };

            getFiles(directoryPath);

            getDirs(directoryPath);

            if (dirsA.Count == 0)
                return;

            int index = 0;
            int length = directoryPath.Length;
            foreach (var p in dirsA)
            {
                string changed = p.Remove(index, length);

                PackDirectory created = DirectoryCreate("\\" + changed);


            }

            foreach (var f in filesA)
            {
                string changed = f.Remove(index, length);
                string dir = System.IO.Path.GetDirectoryName(changed);

                PackDirectory dr = GetDirectoryFromPath(dir);
                if (dr == null)
                {
                    dr = DirectoryCreate(dir);
                }
                dr.AddFile(new PackFile(this.packer, System.IO.File.ReadAllBytes(f), System.IO.Path.GetFileName(f)));
            }
        }

        /// <summary>
        /// Вывод в список файлов, каталогов и подкаталогов 
        /// </summary>
        public string GetListFiles()
        {
            return GetPathsOfDir("", Root);
        }

        /// <summary>
        /// Выводит Object c указанным адресом
        /// </summary>
        private object GetOfPath(string pPath, FindType findType)
        {
            if (pPath == "\\" || pPath == "")
                return Root;

            string[] split = pPath.Split('\\');

            PackDirectory[] findDirs = new PackDirectory[] { Root };
            for (int i = 0; i < split.Length; i++)
            {
                string level = split[i].ToLower();
                if (string.IsNullOrEmpty(level))
                    continue;
                for (int l = 0; l < findDirs.Length; l++)
                {
                    Refresh:
                    if (l >= findDirs.Length)
                        return null;
                    PackDirectory item = findDirs[l];
                    if (findType == FindType.File && item.isRoot)
                    {
                        PackDirectory d = GetDirectoryFromPath(System.IO.Path.GetDirectoryName(pPath));
                        if (d == null)
                            return null;

                        item = d;
                        level = d.Name.ToLower();
                        string fileName = System.IO.Path.GetFileName(pPath).ToLower();

                        var files = item.GetFiles();

                        foreach (var file in files)
                        {
                            if (file.Name.ToLower() == fileName)
                            {
                                return file;
                            }
                        }

                        return null;
                    }
                    else
                    if (findType == FindType.Directory && item.isRoot)
                    {
                        findDirs = item.GetDirectoryes();
                        goto Refresh;
                    }

                    if (findType == FindType.Directory && (item.Name.ToLower() == level))
                    {
                        findDirs = item.GetDirectoryes();
                        if (i == split.Length - 1)
                        {
                            return item;
                        }
                        break;
                    }
                    else if (l == findDirs.Length - 1)
                        return null;


                }

            }
            return null;
        }
        private string GetPathsOfDir(string startDir, PackDirectory directory)
        {
            string collectPath = "";
            var mainDirs = directory.GetDirectoryes();
            var mainFiles = directory.GetFiles();

            bool isRoot = directory.isRoot;
            string mainDirName = isRoot ? "" : directory.Name;

            string startPath = collectPath = (directory.isRoot ? "" : startDir + "\\") + mainDirName;

            for (int i = 0; i < mainDirs.Length; i++)
            {
                PackDirectory selectDir = mainDirs[i];
                string findIn = "\n" + startPath + selectDir.Name;
                string finds = GetPathsOfDir("\n" + startPath.TrimStart('\n'), selectDir);
                collectPath += finds.TrimEnd('\n');
            }
            bool start = false;
            for (int i = 0; i < mainFiles.Length; i++)
            {
                var selectFile = mainFiles[i];
                collectPath += (!start ? "\n" : "") + startPath.TrimStart('\n') + "\\" + selectFile.Name + string.Format(" ({0}KB)", selectFile.Size / 1024) + "\n";
                start = true;
            }

            return collectPath;
        }

    }
    public class Packer : IDisposable
    {


        private const string Head = "p";
        /// <summary>
        /// Версия <see cref="Packer"/>-a
        /// </summary>
        public const int PackerVersion = 1;

        ~Packer()
        {
            Dispose(false);
        }

        private FileStream _fileStream;

        /// <summary>
        /// Версия <see cref="Packer"/>-а из файла
        /// </summary>
        public int Version { get; private set; }
        public string FileName { get; private set; }

        /// <summary>
        /// Указывает на состояние загрузки из файла
        /// </summary>
        public bool IsFileLoaded { get; private set; }

        /// <summary>
        /// Файлы, каталоги и дополнительная информация о <see cref="Packer"/>-a
        /// </summary>
        public PackInfo PackInfo { get; }

        public Packer()
        {
            this.Version = PackerVersion;
            this.PackInfo = new PackInfo(this);
        }

        /// <summary>
        /// Загрузка
        /// </summary>
        public void PackLoad(string PackFileName)
        {
            this.FileName = PackFileName;
            PackRead(PackFileName);
        }
        /// <summary>
        /// Сохранение
        /// </summary>
        public void PackSave(string PackFileName)
        {
            PackWrite(PackFileName);
        }

        /// <summary> 
        /// Очистка 
        /// </summary>
        public void ClearPacker()
        {
            PackInfo.Root.RemoveAll();

            if (IsFileLoaded)
            {
                _fileStream?.Dispose();
            }

            GC.Collect();
        }

        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }

        public Stream GetStreamingPos(sPos sPos)
        {
            if (!IsFileLoaded)
                return null;

            byte[] bts = new byte[sPos.endPos - sPos.startPos];

            _fileStream.Position = sPos.startPos;

            _fileStream.Read(bts, 0, bts.Length);

            return new MemoryStream(bts); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearPacker();
            }
            else
            {
            }
        }
        #region Private member
        private void PackRead(string FileName)
        {
            BinaryReader br = new BinaryReader(_fileStream = File.OpenRead(FileName), System.Text.Encoding.UTF8);
            br.ReadChars(Head.Length);

            Version = br.ReadInt32();

            this.IsFileLoaded = true;

            //Чтение файлов и каталогов
            ReadDirectoryOfByte(br, PackInfo.Root);
        }
        private void PackWrite(string FileName)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Create(FileName), System.Text.Encoding.UTF8))
            {
                //Запись ТЭГА
                bw.Write(Head.ToCharArray());

                //Версия
                bw.Write(PackerVersion);

                //Запись файлов и каталогов
                PackDirectory root = PackInfo.Root;

                WriteDirectoryToByte(bw, root);

            }
        }
        private void WriteDirectoryToByte(BinaryWriter bin, PackDirectory directory)
        {
            string dirName = directory.Name;
            PackDirectory[] dirs = directory.GetDirectoryes();
            PackFile[] directoryFiles = directory.GetFiles();

            bin.Write(dirName);
            int fileCount = directoryFiles.Length;
            bin.Write(fileCount);
            for (int i = 0; i < fileCount; i++)
            {
                WriteFileToByte(bin, directoryFiles[i]);
            }
            int dirCount = dirs.Length;
            bin.Write(dirCount);
            for (int i = 0; i < dirCount; i++)
            {
                WriteDirectoryToByte(bin, dirs[i]);
            }


        }
        private void ReadDirectoryOfByte(BinaryReader bin, PackDirectory directory)
        {
            directory.Name = bin.ReadString();
            int fileCount = bin.ReadInt32();
            for (int i = 0; i < fileCount; i++)
            {
                PackFile file = ReadFileOfByte(bin);
                directory.AddFile(file);
            }
            int dirCount = bin.ReadInt32();
            for (int i = 0; i < dirCount; i++)
            {
                PackDirectory dir = new PackDirectory(this, directory, "");
                ReadDirectoryOfByte(bin, dir);
                directory.AddDirectory(dir);
            }
        }
        private void WriteFileToByte(BinaryWriter bin, PackFile file)
        {
            Stream fls = file.stream;
            bin.Write(file.Name);
            bin.Write(file.Size);
            byte[] vbs = new byte[fls.Length];
            fls.Read(vbs, 0, vbs.Length);
            bin.Write(vbs);

            if (IsFileLoaded)
                fls.Dispose();
        }
        private PackFile ReadFileOfByte(BinaryReader bin)
        {
            string FileName = bin.ReadString();
            long length = bin.ReadInt64();
            long position = bin.BaseStream.Position;
            sPos sPos = new sPos(position, position + length);
            bin.BaseStream.Position += length;
            return new PackFile(this, sPos, FileName);
        }
        #endregion
    }
}
