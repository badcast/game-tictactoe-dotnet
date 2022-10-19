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
    public struct DlgSkinField
    {
        public bool isEmpty;
        public bool isChanged;
        public int backgroundFrameType;
        public DlgSkinField(int backgroundFrameType, bool isChanged, bool isEmpty)
        {
            this.isEmpty = isEmpty;
            this.backgroundFrameType = backgroundFrameType;
            this.isChanged = isChanged;
        }
    }

    public partial class DlgSkin : Form
    {
        public DlgSkin()
        {
            InitializeComponent();
        }

        private void DlgSkin_Load(object sender, EventArgs e)
        {
        }

        private void SetFrom(DlgSkinField value)
        {
            cmbxBackFrameType.SelectedIndex = value.backgroundFrameType;
        }

        private DlgSkinField GetFrom()
        {
            return new DlgSkinField(cmbxBackFrameType.SelectedIndex, true, false);
        }

        public DlgSkinField ShowDialog(DlgSkinField reload)
        {
            SetFrom(reload);
            DialogResult = DialogResult.None;
            if (ShowDialog() != DialogResult.OK)
                return new DlgSkinField(-1, false, true);
            return GetFrom();
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
            if (cmbxBackFrameType.SelectedIndex == -1)
                cmbxBackFrameType.SelectedIndex = 0;
        }

        private void cmbxBackFrameType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbxBackFrameType.SelectedIndex == -1)
                return;

            preivewObject.BackgroundImage = Data.GetFrameBackgrounds()[cmbxBackFrameType.SelectedIndex];
        }
    }
}
