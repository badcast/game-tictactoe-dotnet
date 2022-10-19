using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CAZ
{
    public abstract class DrawObject : Brush                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    {
        
    }

    public abstract class Draw : DrawObject
    {

    }


    public class DrawAnimation : AnimationTimeline
    {
        public override Type TargetPropertyType => this.GetType();

        protected override Freezable CreateInstanceCore()
        {
            return this;
        }
    }


    public class Skin
    {
        public string Name;
        public int ver;
        public int id;
        public DrawObject Drawing;
        public Skin()
        {

        }
    }


    public class SkinManager
    {
    }
}
