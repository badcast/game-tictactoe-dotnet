namespace AnimationEditor
{
    partial class Editor
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.topPanel = new System.Windows.Forms.Panel();
            this.animStatePanel = new System.Windows.Forms.Panel();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.iMouseXY = new System.Windows.Forms.Label();
            this.iFps = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.TrackBar();
            this.peNameAnime = new System.Windows.Forms.TextBox();
            this.propertyContent = new System.Windows.Forms.Panel();
            this.peDurationAnime = new System.Windows.Forms.NumericUpDown();
            this.bResetChanges = new System.Windows.Forms.Button();
            this.bCancelChanges = new System.Windows.Forms.Button();
            this.bApplyChanges = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pCadrSizeVertical = new System.Windows.Forms.Label();
            this.pCadrSizeHorizontal = new System.Windows.Forms.Label();
            this.pName = new System.Windows.Forms.Label();
            this.iconList = new System.Windows.Forms.ImageList(this.components);
            this.animList = new System.Windows.Forms.ListBox();
            this.cm_baddMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mc_addEmpty = new System.Windows.Forms.ToolStripMenuItem();
            this.mc_addFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.cm_listMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.bToClone = new System.Windows.Forms.Button();
            this.blToDown = new System.Windows.Forms.Button();
            this.blToUp = new System.Windows.Forms.Button();
            this.blDelete = new System.Windows.Forms.Button();
            this.blAdd = new System.Windows.Forms.Button();
            this.screenPlay = new System.Windows.Forms.PictureBox();
            this.bStopAnimation = new System.Windows.Forms.Button();
            this.bPauseAnimation = new System.Windows.Forms.Button();
            this.bPlayAnimation = new System.Windows.Forms.Button();
            this.cm_toPointer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mmc_inbeg = new System.Windows.Forms.ToolStripMenuItem();
            this.mmc_inend = new System.Windows.Forms.ToolStripMenuItem();
            this.topPanel.SuspendLayout();
            this.animStatePanel.SuspendLayout();
            this.rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progress)).BeginInit();
            this.propertyContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.peDurationAnime)).BeginInit();
            this.cm_baddMenu.SuspendLayout();
            this.cm_listMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenPlay)).BeginInit();
            this.cm_toPointer.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.animStatePanel);
            this.topPanel.Controls.Add(this.rightPanel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(700, 30);
            this.topPanel.TabIndex = 0;
            // 
            // animStatePanel
            // 
            this.animStatePanel.Controls.Add(this.chkLoop);
            this.animStatePanel.Controls.Add(this.bStopAnimation);
            this.animStatePanel.Controls.Add(this.bPauseAnimation);
            this.animStatePanel.Controls.Add(this.bPlayAnimation);
            this.animStatePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.animStatePanel.Location = new System.Drawing.Point(0, 0);
            this.animStatePanel.Name = "animStatePanel";
            this.animStatePanel.Size = new System.Drawing.Size(199, 30);
            this.animStatePanel.TabIndex = 7;
            // 
            // chkLoop
            // 
            this.chkLoop.AutoSize = true;
            this.chkLoop.Checked = true;
            this.chkLoop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLoop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.chkLoop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Cyan;
            this.chkLoop.ForeColor = System.Drawing.Color.White;
            this.chkLoop.Location = new System.Drawing.Point(99, 8);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(63, 17);
            this.chkLoop.TabIndex = 7;
            this.chkLoop.Text = "Повтор";
            this.chkLoop.UseVisualStyleBackColor = true;
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.iMouseXY);
            this.rightPanel.Controls.Add(this.iFps);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Location = new System.Drawing.Point(509, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(191, 30);
            this.rightPanel.TabIndex = 5;
            // 
            // iMouseXY
            // 
            this.iMouseXY.BackColor = System.Drawing.Color.Transparent;
            this.iMouseXY.ForeColor = System.Drawing.Color.White;
            this.iMouseXY.Location = new System.Drawing.Point(3, 6);
            this.iMouseXY.Name = "iMouseXY";
            this.iMouseXY.Size = new System.Drawing.Size(87, 19);
            this.iMouseXY.TabIndex = 0;
            this.iMouseXY.Text = "Точка 400x400";
            this.iMouseXY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iFps
            // 
            this.iFps.BackColor = System.Drawing.Color.Transparent;
            this.iFps.ForeColor = System.Drawing.Color.White;
            this.iFps.Location = new System.Drawing.Point(136, 6);
            this.iFps.Name = "iFps";
            this.iFps.Size = new System.Drawing.Size(52, 19);
            this.iFps.TabIndex = 0;
            this.iFps.Text = "0 FPS";
            this.iFps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progress
            // 
            this.progress.Cursor = System.Windows.Forms.Cursors.Default;
            this.progress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progress.Location = new System.Drawing.Point(0, 405);
            this.progress.Maximum = 0;
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(700, 45);
            this.progress.TabIndex = 3;
            this.progress.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            // 
            // peNameAnime
            // 
            this.peNameAnime.Location = new System.Drawing.Point(15, 26);
            this.peNameAnime.Name = "peNameAnime";
            this.peNameAnime.Size = new System.Drawing.Size(158, 20);
            this.peNameAnime.TabIndex = 5;
            // 
            // propertyContent
            // 
            this.propertyContent.Controls.Add(this.peDurationAnime);
            this.propertyContent.Controls.Add(this.bResetChanges);
            this.propertyContent.Controls.Add(this.bCancelChanges);
            this.propertyContent.Controls.Add(this.bApplyChanges);
            this.propertyContent.Controls.Add(this.label1);
            this.propertyContent.Controls.Add(this.pCadrSizeVertical);
            this.propertyContent.Controls.Add(this.pCadrSizeHorizontal);
            this.propertyContent.Controls.Add(this.pName);
            this.propertyContent.Controls.Add(this.peNameAnime);
            this.propertyContent.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyContent.Location = new System.Drawing.Point(509, 30);
            this.propertyContent.Name = "propertyContent";
            this.propertyContent.Size = new System.Drawing.Size(191, 375);
            this.propertyContent.TabIndex = 7;
            // 
            // peDurationAnime
            // 
            this.peDurationAnime.DecimalPlaces = 2;
            this.peDurationAnime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.peDurationAnime.Location = new System.Drawing.Point(124, 102);
            this.peDurationAnime.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.peDurationAnime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.peDurationAnime.Name = "peDurationAnime";
            this.peDurationAnime.Size = new System.Drawing.Size(49, 20);
            this.peDurationAnime.TabIndex = 8;
            this.peDurationAnime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // bResetChanges
            // 
            this.bResetChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.bResetChanges.FlatAppearance.BorderSize = 0;
            this.bResetChanges.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.bResetChanges.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(32)))), ((int)(((byte)(55)))));
            this.bResetChanges.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.bResetChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bResetChanges.ForeColor = System.Drawing.Color.White;
            this.bResetChanges.Location = new System.Drawing.Point(15, 132);
            this.bResetChanges.Name = "bResetChanges";
            this.bResetChanges.Size = new System.Drawing.Size(160, 30);
            this.bResetChanges.TabIndex = 7;
            this.bResetChanges.Text = "Восстановить значение";
            this.bResetChanges.UseVisualStyleBackColor = false;
            // 
            // bCancelChanges
            // 
            this.bCancelChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.bCancelChanges.FlatAppearance.BorderSize = 0;
            this.bCancelChanges.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.bCancelChanges.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(32)))), ((int)(((byte)(55)))));
            this.bCancelChanges.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.bCancelChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancelChanges.ForeColor = System.Drawing.Color.White;
            this.bCancelChanges.Location = new System.Drawing.Point(95, 161);
            this.bCancelChanges.Name = "bCancelChanges";
            this.bCancelChanges.Size = new System.Drawing.Size(80, 30);
            this.bCancelChanges.TabIndex = 7;
            this.bCancelChanges.Text = "Отмена";
            this.bCancelChanges.UseVisualStyleBackColor = false;
            // 
            // bApplyChanges
            // 
            this.bApplyChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.bApplyChanges.FlatAppearance.BorderSize = 0;
            this.bApplyChanges.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.bApplyChanges.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(32)))), ((int)(((byte)(55)))));
            this.bApplyChanges.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.bApplyChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bApplyChanges.ForeColor = System.Drawing.Color.White;
            this.bApplyChanges.Location = new System.Drawing.Point(15, 161);
            this.bApplyChanges.Name = "bApplyChanges";
            this.bApplyChanges.Size = new System.Drawing.Size(80, 30);
            this.bApplyChanges.TabIndex = 7;
            this.bApplyChanges.Text = "Применить";
            this.bApplyChanges.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Продолжительность";
            // 
            // pCadrSizeVertical
            // 
            this.pCadrSizeVertical.AutoSize = true;
            this.pCadrSizeVertical.ForeColor = System.Drawing.Color.White;
            this.pCadrSizeVertical.Location = new System.Drawing.Point(12, 77);
            this.pCadrSizeVertical.Name = "pCadrSizeVertical";
            this.pCadrSizeVertical.Size = new System.Drawing.Size(136, 13);
            this.pCadrSizeVertical.TabIndex = 6;
            this.pCadrSizeVertical.Text = "Размер кадра Вертикали";
            // 
            // pCadrSizeHorizontal
            // 
            this.pCadrSizeHorizontal.AutoSize = true;
            this.pCadrSizeHorizontal.ForeColor = System.Drawing.Color.White;
            this.pCadrSizeHorizontal.Location = new System.Drawing.Point(12, 59);
            this.pCadrSizeHorizontal.Name = "pCadrSizeHorizontal";
            this.pCadrSizeHorizontal.Size = new System.Drawing.Size(92, 13);
            this.pCadrSizeHorizontal.TabIndex = 6;
            this.pCadrSizeHorizontal.Text = "Размер кадра X:";
            // 
            // pName
            // 
            this.pName.AutoSize = true;
            this.pName.ForeColor = System.Drawing.Color.White;
            this.pName.Location = new System.Drawing.Point(12, 10);
            this.pName.Name = "pName";
            this.pName.Size = new System.Drawing.Size(77, 13);
            this.pName.TabIndex = 6;
            this.pName.Text = "Имя объекта:";
            // 
            // iconList
            // 
            this.iconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconList.ImageStream")));
            this.iconList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconList.Images.SetKeyName(0, "file_icon.png");
            // 
            // animList
            // 
            this.animList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.animList.BackColor = System.Drawing.Color.Black;
            this.animList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.animList.Cursor = System.Windows.Forms.Cursors.Default;
            this.animList.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.animList.ForeColor = System.Drawing.Color.DarkGray;
            this.animList.FormattingEnabled = true;
            this.animList.IntegralHeight = false;
            this.animList.ItemHeight = 17;
            this.animList.Items.AddRange(new object[] {
            "[1]Elem01",
            "[2]Elem02",
            "[3]Elem03"});
            this.animList.Location = new System.Drawing.Point(0, 60);
            this.animList.Name = "animList";
            this.animList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.animList.Size = new System.Drawing.Size(199, 345);
            this.animList.TabIndex = 9;
            // 
            // cm_baddMenu
            // 
            this.cm_baddMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mc_addEmpty,
            this.mc_addFiles});
            this.cm_baddMenu.Name = "cm_baddMenu";
            this.cm_baddMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cm_baddMenu.Size = new System.Drawing.Size(192, 48);
            // 
            // mc_addEmpty
            // 
            this.mc_addEmpty.Name = "mc_addEmpty";
            this.mc_addEmpty.Size = new System.Drawing.Size(191, 22);
            this.mc_addEmpty.Text = "Создать пустой кадр";
            // 
            // mc_addFiles
            // 
            this.mc_addFiles.Name = "mc_addFiles";
            this.mc_addFiles.Size = new System.Drawing.Size(191, 22);
            this.mc_addFiles.Text = "Добавить файлы (*.*)";
            // 
            // cm_listMenu
            // 
            this.cm_listMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem3,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.cm_listMenu.Name = "cm_baddMenu";
            this.cm_listMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cm_listMenu.Size = new System.Drawing.Size(155, 76);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(154, 22);
            this.toolStripMenuItem4.Text = "Редактировать";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(151, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.toolStripMenuItem1.Text = "Редактировать";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(154, 22);
            this.toolStripMenuItem2.Text = "Изменить";
            // 
            // bToClone
            // 
            this.bToClone.FlatAppearance.BorderSize = 0;
            this.bToClone.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.bToClone.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.bToClone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bToClone.Font = new System.Drawing.Font("Wingdings 3", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.bToClone.Image = global::AnimationEditor.Properties.Resources.bt_clone;
            this.bToClone.Location = new System.Drawing.Point(120, 30);
            this.bToClone.Name = "bToClone";
            this.bToClone.Size = new System.Drawing.Size(30, 30);
            this.bToClone.TabIndex = 10;
            this.bToClone.UseVisualStyleBackColor = false;
            // 
            // blToDown
            // 
            this.blToDown.FlatAppearance.BorderSize = 0;
            this.blToDown.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.blToDown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.blToDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blToDown.Font = new System.Drawing.Font("Wingdings 3", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.blToDown.Image = global::AnimationEditor.Properties.Resources.bt_down;
            this.blToDown.Location = new System.Drawing.Point(90, 30);
            this.blToDown.Name = "blToDown";
            this.blToDown.Size = new System.Drawing.Size(30, 30);
            this.blToDown.TabIndex = 8;
            this.blToDown.UseVisualStyleBackColor = false;
            // 
            // blToUp
            // 
            this.blToUp.FlatAppearance.BorderSize = 0;
            this.blToUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.blToUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.blToUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blToUp.Font = new System.Drawing.Font("Wingdings 3", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.blToUp.Image = global::AnimationEditor.Properties.Resources.bt_up;
            this.blToUp.Location = new System.Drawing.Point(60, 30);
            this.blToUp.Name = "blToUp";
            this.blToUp.Size = new System.Drawing.Size(30, 30);
            this.blToUp.TabIndex = 8;
            this.blToUp.UseVisualStyleBackColor = false;
            // 
            // blDelete
            // 
            this.blDelete.FlatAppearance.BorderSize = 0;
            this.blDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.blDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.blDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blDelete.Image = ((System.Drawing.Image)(resources.GetObject("blDelete.Image")));
            this.blDelete.Location = new System.Drawing.Point(30, 30);
            this.blDelete.Name = "blDelete";
            this.blDelete.Size = new System.Drawing.Size(30, 30);
            this.blDelete.TabIndex = 8;
            this.blDelete.UseVisualStyleBackColor = false;
            // 
            // blAdd
            // 
            this.blAdd.FlatAppearance.BorderSize = 0;
            this.blAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.blAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.blAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.blAdd.Image = global::AnimationEditor.Properties.Resources.bt_add;
            this.blAdd.Location = new System.Drawing.Point(0, 30);
            this.blAdd.Name = "blAdd";
            this.blAdd.Size = new System.Drawing.Size(30, 30);
            this.blAdd.TabIndex = 8;
            this.blAdd.UseVisualStyleBackColor = false;
            // 
            // screenPlay
            // 
            this.screenPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screenPlay.BackgroundImage = global::AnimationEditor.Properties.Resources.transparent_background;
            this.screenPlay.Location = new System.Drawing.Point(199, 30);
            this.screenPlay.Name = "screenPlay";
            this.screenPlay.Size = new System.Drawing.Size(309, 375);
            this.screenPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.screenPlay.TabIndex = 1;
            this.screenPlay.TabStop = false;
            // 
            // bStopAnimation
            // 
            this.bStopAnimation.Dock = System.Windows.Forms.DockStyle.Left;
            this.bStopAnimation.FlatAppearance.BorderSize = 0;
            this.bStopAnimation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.bStopAnimation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.bStopAnimation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bStopAnimation.Image = global::AnimationEditor.Properties.Resources.bt_stop;
            this.bStopAnimation.Location = new System.Drawing.Point(60, 0);
            this.bStopAnimation.Name = "bStopAnimation";
            this.bStopAnimation.Size = new System.Drawing.Size(30, 30);
            this.bStopAnimation.TabIndex = 6;
            this.bStopAnimation.UseVisualStyleBackColor = true;
            // 
            // bPauseAnimation
            // 
            this.bPauseAnimation.Dock = System.Windows.Forms.DockStyle.Left;
            this.bPauseAnimation.FlatAppearance.BorderSize = 0;
            this.bPauseAnimation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.bPauseAnimation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.bPauseAnimation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bPauseAnimation.Image = global::AnimationEditor.Properties.Resources.bt_pause;
            this.bPauseAnimation.Location = new System.Drawing.Point(30, 0);
            this.bPauseAnimation.Name = "bPauseAnimation";
            this.bPauseAnimation.Size = new System.Drawing.Size(30, 30);
            this.bPauseAnimation.TabIndex = 8;
            this.bPauseAnimation.UseVisualStyleBackColor = true;
            // 
            // bPlayAnimation
            // 
            this.bPlayAnimation.Dock = System.Windows.Forms.DockStyle.Left;
            this.bPlayAnimation.FlatAppearance.BorderSize = 0;
            this.bPlayAnimation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.bPlayAnimation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.bPlayAnimation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bPlayAnimation.Image = global::AnimationEditor.Properties.Resources.bt_play;
            this.bPlayAnimation.Location = new System.Drawing.Point(0, 0);
            this.bPlayAnimation.Name = "bPlayAnimation";
            this.bPlayAnimation.Size = new System.Drawing.Size(30, 30);
            this.bPlayAnimation.TabIndex = 4;
            this.bPlayAnimation.UseVisualStyleBackColor = true;
            // 
            // cm_toPointer
            // 
            this.cm_toPointer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmc_inbeg,
            this.mmc_inend});
            this.cm_toPointer.Name = "cm_baddMenu";
            this.cm_toPointer.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cm_toPointer.Size = new System.Drawing.Size(125, 48);
            // 
            // mmc_inbeg
            // 
            this.mmc_inbeg.Name = "mmc_inbeg";
            this.mmc_inbeg.Size = new System.Drawing.Size(152, 22);
            this.mmc_inbeg.Text = "В начало";
            // 
            // mmc_inend
            // 
            this.mmc_inend.Name = "mmc_inend";
            this.mmc_inend.Size = new System.Drawing.Size(124, 22);
            this.mmc_inend.Text = "В конец";
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(32)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.bToClone);
            this.Controls.Add(this.blToDown);
            this.Controls.Add(this.blToUp);
            this.Controls.Add(this.blDelete);
            this.Controls.Add(this.blAdd);
            this.Controls.Add(this.propertyContent);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.screenPlay);
            this.Controls.Add(this.animList);
            this.Controls.Add(this.topPanel);
            this.MinimumSize = new System.Drawing.Size(700, 450);
            this.Name = "Editor";
            this.Size = new System.Drawing.Size(700, 450);
            this.Load += new System.EventHandler(this.Editor_Load);
            this.topPanel.ResumeLayout(false);
            this.animStatePanel.ResumeLayout(false);
            this.animStatePanel.PerformLayout();
            this.rightPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.progress)).EndInit();
            this.propertyContent.ResumeLayout(false);
            this.propertyContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.peDurationAnime)).EndInit();
            this.cm_baddMenu.ResumeLayout(false);
            this.cm_listMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.screenPlay)).EndInit();
            this.cm_toPointer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.PictureBox screenPlay;
        private System.Windows.Forms.TrackBar progress;
        private System.Windows.Forms.TextBox peNameAnime;
        private System.Windows.Forms.Panel propertyContent;
        private System.Windows.Forms.Label pName;
        private System.Windows.Forms.Button bPlayAnimation;
        private System.Windows.Forms.Label pCadrSizeHorizontal;
        private System.Windows.Forms.Button bApplyChanges;
        private System.Windows.Forms.Button bCancelChanges;
        private System.Windows.Forms.Label pCadrSizeVertical;
        private System.Windows.Forms.NumericUpDown peDurationAnime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bResetChanges;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Label iFps;
        private System.Windows.Forms.Label iMouseXY;
        private System.Windows.Forms.Button blAdd;
        private System.Windows.Forms.Button blDelete;
        private System.Windows.Forms.Button blToUp;
        private System.Windows.Forms.Button blToDown;
        private System.Windows.Forms.Button bStopAnimation;
        private System.Windows.Forms.Panel animStatePanel;
        private System.Windows.Forms.CheckBox chkLoop;
        private System.Windows.Forms.ImageList iconList;
        private System.Windows.Forms.ListBox animList;
        private System.Windows.Forms.Button bPauseAnimation;
        private System.Windows.Forms.ContextMenuStrip cm_baddMenu;
        private System.Windows.Forms.ToolStripMenuItem mc_addEmpty;
        private System.Windows.Forms.ToolStripMenuItem mc_addFiles;
        private System.Windows.Forms.ContextMenuStrip cm_listMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Button bToClone;
        private System.Windows.Forms.ContextMenuStrip cm_toPointer;
        private System.Windows.Forms.ToolStripMenuItem mmc_inbeg;
        private System.Windows.Forms.ToolStripMenuItem mmc_inend;
    }
}
