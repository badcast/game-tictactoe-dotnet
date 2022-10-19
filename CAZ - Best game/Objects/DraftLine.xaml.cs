using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CAZ;

namespace CAZ
{
    /// <summary>
    /// Логика взаимодействия для DraftLine.xaml
    /// </summary>
    public partial class DraftLine : UserControl, IStyle
    {
        int MinSizeX = 50;
        int MinSizeY = 50;
        private RotateTransform rotate;

        public double Angle { get { return rotate.Angle; } set { rotate.Angle = value; } }
        public DraftLine()
        {
            InitializeComponent();

            if (!GameEngine.Started)
                return;
            SetStyle(DesignManager.current);

            
            foreach (var child in (RenderTransform as TransformGroup).Children)
            {
                if(child is RotateTransform)
                {
                    rotate = (RotateTransform)child;
                    break;
                }
            }
        }
        private double GetAngle(Point pointA, Point pointB)
        {
            double YEnd = pointB.Y;
            double YStart = pointA.Y;
            double XEnd = pointB.X;
            double XStart = pointA.X;

            var a = Math.Atan2(YEnd - YStart, XEnd - XStart);
            if (a < 0) a += 2 * Math.PI; //angle is now in radians

            a -= (Math.PI / 2); //shift by 90deg
                                //restore value in range 0-2pi instead of -pi/2-3pi/2
            if (a < 0) a += 2 * Math.PI;
            if (a < 0) a += 2 * Math.PI;
            a = Math.Abs((Math.PI * 2) - a); //invert rotation
            a = a * 180 / Math.PI; //convert to deg

            return a;
        }
        public void SetStyle(DesignManager design)
        {
            BitmapSource src = design.GetBitmapSourceFromId("gameDraft");

            CroppedBitmap crop(int x, int y, int w, int h)
            {
                return new CroppedBitmap(src, new Int32Rect(x, y, w, h));
            }

            int srcWidth = MinSizeX = src.PixelWidth, srcHeight = MinSizeY = src.PixelHeight;
            int leftOffset = GameValues.DraftLeftOffset, rightOffset = GameValues.DraftRightOffset;
            bool isTile = GameValues.DraftIsTile,
                 isStretch = GameValues.DraftIsStretch;
            double opacity = GameValues.DraftOpacityLevel;

            this.Opacity = opacity;

            //Левая сторона
            CroppedBitmap c = crop(0, 0, leftOffset, srcHeight);
            lf.Source = c;

            //Центральная сторона
            int dh = rightOffset - leftOffset;
            c = crop(leftOffset, 0, dh, srcHeight);
            var iBrsh = (cr.Background as ImageBrush);
            iBrsh.ImageSource = c;
            iBrsh.Viewport = new Rect(0, 0, dh, Height);
            iBrsh.TileMode = isTile ? TileMode.Tile : TileMode.None;
            iBrsh.Stretch = isStretch ? Stretch.Fill : Stretch.None;

            //Правая сторона
            c = crop(rightOffset, 0, srcWidth - rightOffset, srcHeight);
            rf.Source = c;
        }
        public void SetPosition(Point a, Point b)
        {
            VerticalAlignment = VerticalAlignment.Top;
            HorizontalAlignment = HorizontalAlignment.Left;

            //this.Width = BasicAlgorithm.DistancePhf(a, b);
            Angle = -GetAngle(a, b)-180;
            var t = Margin;
            a -= Point.one * ((int)MinSizeY/2);
            t.Left = a.X;
            t.Top = a.Y;
            Margin = t;
        }
    }
}
