using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AnimationEditor
{
    public static class Data
    {
        private static Image[] frameBackgrounds;

        public static Image[] GetFrameBackgrounds()
        {
            if (frameBackgrounds == null)
            {
                frameBackgrounds = new Image[3];
                frameBackgrounds[0] = Properties.Resources.transparent_background;
                Bitmap bmp = new Bitmap(1, 1);
                bmp.SetPixel(0, 0, Color.Black);
                frameBackgrounds[1] = bmp;
                bmp = new Bitmap(1, 1);
                bmp.SetPixel(0, 0, Color.White);
                frameBackgrounds[2] = bmp;
            }

            return frameBackgrounds;
        }
    }
}
