using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CAZ
{
    public struct xRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public xRect(int x, int y, int w, int h)
        {
            this.left = x;
            this.top = y;
            this.right = w;
            this.bottom = h;
        }
    }


    public delegate void StreamEvent(byte currentByte);
    public delegate void StreamBlockEvent(byte[] buffered);
    public delegate void StreamProgressEvent(double progress);

    /// <summary>
    /// Представляет собой класс Потока управлением, которое занимает асинхронное чтение и запись
    /// </summary>
    public class AsyncStream : Stream, IDisposable
    {
        private Stream data;

        public override bool CanRead => data.CanRead;

        public override bool CanSeek => data.CanSeek;

        public override bool CanWrite => data.CanWrite;

        public override long Length => data.Length;

        public override long Position { get => data.Position; set => data.Position = value; }

        public event StreamEvent OnStreamChanged;
        public event StreamEvent OnStreamRead;
        public event StreamBlockEvent OnBlockRead;
        public event StreamProgressEvent OnProgress;

        public AsyncStream()
        {
            data = new MemoryStream();
        }

        public AsyncStream(Stream stream)
        {
            data = stream;
        }

        public override void Flush()
        {
            data.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int res = data.Read(buffer, offset, count);
            OnBlockRead?.Invoke(buffer);
            OnProgress?.Invoke(1D * Position / Length);
            return res;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return data.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            data.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Position = offset;
            for (; offset < count; offset++)
            {
                byte b = buffer[offset];
                data.WriteByte(b);
                OnStreamChanged?.Invoke(b);
            }
        }

        public override void WriteByte(byte value)
        {
            data.WriteByte(value);
        }

        public override int ReadByte()
        {
            int b = data.ReadByte();

            if (b != -1)
                OnStreamRead?.Invoke((byte)b);

            return b;
        }

        public override void Close()
        {
            data.Close();
        }

        public new void Dispose()
        {
            data.Dispose();
        }
    }

    public static class G_G
    {

        public static bool destroy(ref StringBuilder target)
        {
            object t = target;
            target = null;
            bool des = destroy(ref t);
            target = t as StringBuilder;
            return des;
        }
        public static bool destroy(ref object target)
        {
            if (target == null)
                return false;

            WeakReference wef = new WeakReference(target, true);
            target = null;

            GC.Collect(GC.GetGeneration(wef.Target), GCCollectionMode.Forced);
            var r = GC.WaitForFullGCComplete();
            System.GC.WaitForPendingFinalizers();
            return (target = wef.Target) == null;
        }
    }

    public static class CustomGUI
    {
        const string user32 = "user32.dll";
        const string gdi32 = "gdi32.dll";
        const string kernel32 = "kernel32.dll";

        [DllImport(user32)]
        extern static bool InvalidateRect(IntPtr hwnd, ref xRect lpRect, bool erase);
        [DllImport(user32)]
        extern static IntPtr GetDC(IntPtr hwnd);
        [DllImport(user32)]
        extern static int ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [DllImport(user32)]
        extern static IntPtr GetDesktopWindow();
        [DllImport(user32)]
        extern static IntPtr SetActiveWindow(IntPtr hwndWindow);
        [DllImport(user32)]
        extern static bool IsWindow(IntPtr hWnd);
        [DllImport(user32)]
        extern static bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport(user32)]
        extern static bool CloseWindow(IntPtr hWnd);
        [DllImport(user32)]
        extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport(gdi32)]
        extern static bool DeleteDC(IntPtr hdc);
        [DllImport(gdi32)]
        extern static int GetDeviceCaps(IntPtr hdc, int nIndex);
        [DllImport(gdi32)]
        extern static int SaveDC(IntPtr hdc);
        [DllImport(gdi32, EntryPoint = "CreateDCA")]
        extern static IntPtr CreateDC(string lpszDriver, IntPtr lpszDevice, IntPtr lpszOutput, IntPtr lpdvmInit);
        [DllImport(kernel32)]
        extern static ushort GetMaximumProcessorGroupCount();
        [DllImport(kernel32)]
        extern static uint GetMaximumProcessorCount(ushort GroupNumber);

        private static IntPtr hwnd;
        private static IntPtr hdc;
        private static Graphics graph;
        private static Font defaultFont = new Font("Arial", 11);
        private static SolidBrush defaultSolidBrush = new SolidBrush(Color.Black);
        public static void InitGraph(IntPtr handleGraphics)
        {
            hwnd = handleGraphics;
            graph = Graphics.FromHwnd(handleGraphics);
        }

        public static void DrawText(string text, int x, int y, Color color)
        {
            var f = new xRect(x, y, (int)(text.Length * defaultFont.Size), defaultFont.Height);
            defaultSolidBrush.Color = Color.Black;
            graph.FillRectangle(defaultSolidBrush, x, y, f.right, f.top);
            defaultSolidBrush.Color = color;
            graph.DrawString(text, defaultFont, defaultSolidBrush, x, y);
        }
    }

    public static class RndMgr
    {
        private static Random rr = new Random();
        public static void ReSeed()
        {
            ReSeed((int)Time.time);
        }
        public static void ReSeed(int seed)
        {
            rr = new Random(seed);
        }
        public static int Range(int min, int max)
        {
            return rr.Next(min, max);
        }
        public static double Range(double max)
        {
            return rr.NextDouble() * max;
        }
    }
}
