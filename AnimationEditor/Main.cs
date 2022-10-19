using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnimationEditor
{
    public partial class Main : Form
    {
        private DlgSkin dialogChangeSkin;
        private DlgExporter dialogExport;
        private int backgroundStyleIndex;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

            loadConfig();
            refreshParams();
        }

        private void mmExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mmChangeView_Click(object sender, EventArgs e)
        {
            if (dialogChangeSkin == null)
                dialogChangeSkin = new DlgSkin();

            var res = dialogChangeSkin.ShowDialog(new DlgSkinField(backgroundStyleIndex, false, false));
            if (!res.isEmpty)
            {
                backgroundStyleIndex = res.backgroundFrameType;
                refreshParams();
            }
        }

        private void loadConfig()
        {
            backgroundStyleIndex = Configs.ReadInt("view", "frameBackground");

            if (backgroundStyleIndex < 0)
                backgroundStyleIndex = 0;
            else if (backgroundStyleIndex >= Data.GetFrameBackgrounds().Length)
                backgroundStyleIndex = 0;
        }

        private void saveConfig()
        {
            Configs.WriteInt("view", "frameBackground", backgroundStyleIndex);
        }

        private void refreshParams()
        {
            editor.FrameBackground = Data.GetFrameBackgrounds()[backgroundStyleIndex];
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveConfig();
        }

        private void editor_Load(object sender, EventArgs e)
        {

        }

        private void mmExport_Click(object sender, EventArgs e)
        {
            if (dialogExport == null)
                dialogExport = new DlgExporter();

            dialogExport.ShowDialog();
        }

        private void mm_animPlay_Click(object sender, EventArgs e)
        {
            editor.PlayAnimation();
        }

        private void mm_animPause_Click(object sender, EventArgs e)
        {
            editor.PauseAnimation();
        }

        private void mm_animStop_Click(object sender, EventArgs e)
        {
            editor.StopAnimation();
        }
    }
}
