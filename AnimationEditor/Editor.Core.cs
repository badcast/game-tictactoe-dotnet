using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace AnimationEditor
{
    public delegate void EventIndexChanged(object obj, int newIndex, int oldIndex);

    public enum AnimationState
    {
        Idle,
        Playing,
        Paused,
        Stop
    }





    public interface IPictureProvider
    {
        string Name { get; set; }
        TimeSpan Duration { get; set; }
        Bitmap GetBitmap();
    }

    public class Picture : IPictureProvider, IDisposable
    {
        private Bitmap _cached;
        private bool _isSync;
        private string _name;
        private TimeSpan _duration;

        public bool IsSync { get { RaiseExcep(); return _isSync; } }
        public bool IsDisposed { get; private set; }
        public int Width { get { return _cached.Width; } }
        public int Height { get { return _cached.Height; } }
        public AnimationEditor.PixelFormat PixelFormat { get; set; }
        public string Name { get { RaiseExcep(); return _name; } set { RaiseExcep(); _name = value; } }
        public TimeSpan Duration { get { RaiseExcep(); return _duration; } set { RaiseExcep(); _duration = value; } }
        public Picture(int Width, int Height)
        {
            this._cached = new Bitmap(Width, Height);
            this.Name = DefaultName;
            this.Duration = DefaultDuration;
            this.PixelFormat = PixelFormat.ARGB;
            this._isSync = false;
        }

        public Picture(string name, TimeSpan duration, int width, int height, PixelFormat pixelFormat)
        {
            this.Name = name;
            this.Duration = duration;
            this.PixelFormat = pixelFormat;
            this._cached = new Bitmap(width, height);
            this._isSync = false;
        }

        public Picture(Bitmap cache, bool isSync, PixelFormat pixelFormat = PixelFormat.ARGB)
        {
            this._cached = cache;
            this.Name = DefaultName;
            this.Duration = DefaultDuration;
            this.PixelFormat = pixelFormat;
            this._isSync = isSync;
        }

        public Picture(string name, TimeSpan duration, Bitmap cache, PixelFormat pixelFormat)
        {
            this.Name = name;
            this.Duration = duration;
            this.PixelFormat = pixelFormat;
            this._cached = cache;
            this._isSync = true;
        }

        private void RaiseExcep()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("Picture");
            }
        }

        public Color GetPixel(int x, int y)
        {
            return GetBitmap().GetPixel(x, y);
        }

        public void SetPixel(int x, int y, Color pixelColor)
        {
            GetBitmap().SetPixel(x, y, pixelColor);
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;
            GC.SuppressFinalize(this);
            if (!_isSync)
                _cached?.Dispose();
        }

        public Bitmap GetBitmap()
        {
            RaiseExcep();
            return _cached;
        }

        public static readonly string DefaultName = "Кадр";
        public static readonly TimeSpan DefaultDuration = new TimeSpan(0, 0, 1);

        public static Picture CreateFromFile(string FileName, bool isSync)
        {
            using (FileStream fs = File.OpenRead(FileName))
                return CreateFromStream(fs, isSync);
        }

        public static Picture CreateFromStream(Stream stream, bool isSync)
        {
            return new Picture(new Bitmap(stream), isSync);
        }
    }

    public class TimeLine
    {
        private List<IPictureProvider> animations;
        private int ________index;

        public IPictureProvider this[int index] { get => FromIndex(index); }

        public int Count { get => animations.Count; }
        public bool IsEmpty { get => Count == 0; }
        public bool IsLoop { get; set; }
        public int SelectedIndex { get => GetIndex(); set => SetIndex(value); }
        public IPictureProvider SelectedObject { get => FromIndex(GetIndex()); }
        public bool HasNextIndex { get => NextIndex != -1; }
        public bool HasPreviewIndex { get => PreviewIndex != -1; }
        public int NextIndex { get => GetNextIndex(); }
        public IPictureProvider NextObject { get => GetNextObject(); }
        public int PreviewIndex { get => GetPreviewIndex(); }
        public IPictureProvider PreviewObject { get => GetPreviewObject(); }
        public TimeSpan NextDuration { get => NextObject.Duration; }
        public TimeSpan PreviewDuration { get => PreviewObject.Duration; }
        public event EventIndexChanged OnSeletedIndexChange;

        public TimeLine()
        {
            this.animations = new List<IPictureProvider>();
            ________index = -1;

        }

        public IPictureProvider Add(Bitmap bitmap, bool isSync)
        {
            Picture p = new Picture(bitmap, isSync);
            Add(p);
            return p;
        }

        public IPictureProvider Add(int width, int height)
        {
            Picture p = new Picture(width, height);
            Add(p);
            return p;
        }

        public void Add(IPictureProvider provider)
        {
            animations.Add(provider);
            SetIndex(0);
        }

        public void Remove(IPictureProvider provider)
        {
            animations.Remove(provider);
        }

        public bool RemoveAt(int index)
        {
            if (!IsRange(index))
                return false;

            animations.RemoveAt(index);
            return true;
        }

        public bool Move(int from, int to)
        {
            if (!IsRange(from) || !IsRange(to) || from == to)
                return false;

            //&TODO: Реализовать алгоритм изменения по индексу

            //Смещения между числами
            int offset = from - to;
            //Направление, назад или в верх 
            //Если в верх, то direction-у присваевается -1 в вниз 1
            int direction = offset / Math.Abs(offset) * -1;
            offset = Math.Abs(offset);
            //Длина
            int length = offset+1;
            IPictureProvider[] changes = new IPictureProvider[length];
            //Готовит массив с направлением 
            for (int i = from; i != to + direction; i += direction)
            {
                changes[Math.Abs(i-from)] = animations[i];
                //  animations[i] = null;
            }
            //Выводит направленный массив
            for (int i = 0; i < changes.Length; i ++)
            {
                animations[Math.Abs(from + Math.Abs(i-offset) * direction)] = changes[i];
            }

            return true;
        }

        public bool Move(IPictureProvider from, IPictureProvider to)
        {
            if (from == to)
                return false;
            return Move(IndexOf(from), IndexOf(to));
        }

        public int IndexOf(IPictureProvider provider)
        {
            return animations.IndexOf(provider);
        }

        public bool IsRange(int index)
        {
            return !(index >= Count || index < 0);
        }

        public IPictureProvider FromIndex(int index)
        {
            if (!IsRange(index))
                return null;

            return animations[index];
        }

        public bool SetIndex(int index)
        {
            if (!IsRange(index))
                return false;

            int lastIndex = GetIndex();
            ________index = index;

            OnSeletedIndexChange?.Invoke(this, index, lastIndex);

            return true;
        }

        public int GetIndex()
        {
            if (Count == 0)
                ________index = -1;
            else
            if (________index >= Count)
                ________index = Count - 1;

            return ________index;
        }

        public int GetNextIndex()
        {
            int havent = GetIndex() + 1;

            if (!IsRange(havent))
                if (IsLoop)
                    return 0;
                else
                    return -1;

            return havent;
        }

        public IPictureProvider GetNextObject()
        {
            return FromIndex(NextIndex);
        }

        public int GetPreviewIndex()
        {
            int havent = GetIndex() - 1;

            if (!IsRange(havent))
                if (IsLoop)
                    return Count;
                else
                    return -1;

            return havent;
        }

        public IPictureProvider GetPreviewObject()
        {
            return FromIndex(PreviewIndex);
        }
    }
}