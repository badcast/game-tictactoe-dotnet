namespace AnimationEditor
{
    partial class DlgSkin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.bCancel = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.cmbxBackFrameType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.preivewObject = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.preivewObject.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Controls.Add(this.bSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 222);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 40);
            this.panel1.TabIndex = 0;
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(199, 9);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 0;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(118, 9);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 23);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // cmbxBackFrameType
            // 
            this.cmbxBackFrameType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxBackFrameType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbxBackFrameType.FormattingEnabled = true;
            this.cmbxBackFrameType.Items.AddRange(new object[] {
            "Прозрачный",
            "Черный",
            "Белый"});
            this.cmbxBackFrameType.Location = new System.Drawing.Point(135, 27);
            this.cmbxBackFrameType.Name = "cmbxBackFrameType";
            this.cmbxBackFrameType.Size = new System.Drawing.Size(125, 21);
            this.cmbxBackFrameType.TabIndex = 1;
            this.cmbxBackFrameType.SelectedIndexChanged += new System.EventHandler(this.cmbxBackFrameType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Фоновый цвет рамки";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(37, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Образец";
            // 
            // preivewObject
            // 
            this.preivewObject.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.preivewObject.Controls.Add(this.label2);
            this.preivewObject.Location = new System.Drawing.Point(135, 54);
            this.preivewObject.Name = "preivewObject";
            this.preivewObject.Size = new System.Drawing.Size(125, 84);
            this.preivewObject.TabIndex = 3;
            // 
            // DlgSkin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbxBackFrameType);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.preivewObject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgSkin";
            this.Text = "Настройки вида";
            this.Load += new System.EventHandler(this.DlgSkin_Load);
            this.Shown += new System.EventHandler(this.DlgSkin_Shown);
            this.panel1.ResumeLayout(false);
            this.preivewObject.ResumeLayout(false);
            this.preivewObject.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.ComboBox cmbxBackFrameType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel preivewObject;
    }
}