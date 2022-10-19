using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using PInvoke;

namespace AnimationEditor
{
    public partial class DlgExporter : Form
    {
        unsafe public DlgExporter()
        {
            InitializeComponent();
        }

        private void DlgSkin_Load(object sender, EventArgs e)
        {
            string p = @"D:\Games\Alawar.ru\Alawar Rise of the Machines\Keygen\Gifs\00078.gif";
            Image gif = Image.FromFile(p);
            ImageAnimator.Animate(gif, _eventAnimate);
            Image last = gif;
            pictureBox1.Image = gif;
        }

        bool isStart = false;
        float startTime = 0;
        float updateTime;
        List<float> timer = new List<float>();
        private void _eventAnimate(object o, EventArgs e)
        {
            if(!isStart)
            {
                startTime = Time.time;
                isStart = true;
            }
            float lastTime = Time.time - updateTime;
            updateTime = Time.time;
            timer.Add(lastTime);
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            Close();
        }

        private void DlgSkin_Shown(object sender, EventArgs e)
        {
        }

        private void cmbxBackFrameType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
