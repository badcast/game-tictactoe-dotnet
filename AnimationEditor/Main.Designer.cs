namespace AnimationEditor
{
    partial class Main
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.mainmenu = new System.Windows.Forms.MenuStrip();
            this.mmFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.импортToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mmExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.mmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mmChangeView = new System.Windows.Forms.ToolStripMenuItem();
            this.макетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьРазмерToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.анимацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mm_animPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.mm_animPause = new System.Windows.Forms.ToolStripMenuItem();
            this.mm_animStop = new System.Windows.Forms.ToolStripMenuItem();
            this.editor = new AnimationEditor.Editor();
            this.mainmenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainmenu
            // 
            this.mainmenu.BackColor = System.Drawing.SystemColors.ControlLight;
            this.mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmFile,
            this.видToolStripMenuItem,
            this.макетToolStripMenuItem,
            this.анимацияToolStripMenuItem,
            this.помощьToolStripMenuItem});
            this.mainmenu.Location = new System.Drawing.Point(0, 0);
            this.mainmenu.Name = "mainmenu";
            this.mainmenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainmenu.Size = new System.Drawing.Size(701, 24);
            this.mainmenu.TabIndex = 0;
            this.mainmenu.Text = "menuStrip1";
            // 
            // mmFile
            // 
            this.mmFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem1,
            this.toolStripMenuItem3,
            this.toolStripMenuItem6,
            this.открытьToolStripMenuItem,
            this.toolStripMenuItem2,
            this.импортToolStripMenuItem,
            this.mmExport,
            this.toolStripMenuItem5,
            this.mmExit});
            this.mmFile.Name = "mmFile";
            this.mmFile.Size = new System.Drawing.Size(48, 20);
            this.mmFile.Text = "Файл";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.toolStripMenuItem4.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItem4.Text = "Создать проект";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.toolStripMenuItem1.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItem1.Text = "Открыть...";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.ShortcutKeyDisplayString = "";
            this.toolStripMenuItem3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItem3.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItem3.Text = "Сохранить";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItem6.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItem6.Text = "Сохранить как...";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.открытьToolStripMenuItem.Text = "Закрыть проект";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(231, 6);
            // 
            // импортToolStripMenuItem
            // 
            this.импортToolStripMenuItem.Name = "импортToolStripMenuItem";
            this.импортToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.импортToolStripMenuItem.Text = "Импорт";
            // 
            // mmExport
            // 
            this.mmExport.Name = "mmExport";
            this.mmExport.Size = new System.Drawing.Size(234, 22);
            this.mmExport.Text = "Экспорт";
            this.mmExport.Click += new System.EventHandler(this.mmExport_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(231, 6);
            // 
            // mmExit
            // 
            this.mmExit.Name = "mmExit";
            this.mmExit.Size = new System.Drawing.Size(234, 22);
            this.mmExit.Text = "Выход";
            this.mmExit.Click += new System.EventHandler(this.mmExit_Click);
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmChangeView});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // mmChangeView
            // 
            this.mmChangeView.Name = "mmChangeView";
            this.mmChangeView.Size = new System.Drawing.Size(150, 22);
            this.mmChangeView.Text = "Изменить вид";
            this.mmChangeView.Click += new System.EventHandler(this.mmChangeView_Click);
            // 
            // макетToolStripMenuItem
            // 
            this.макетToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.изменитьРазмерToolStripMenuItem});
            this.макетToolStripMenuItem.Name = "макетToolStripMenuItem";
            this.макетToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.макетToolStripMenuItem.Text = "Макет";
            // 
            // изменитьРазмерToolStripMenuItem
            // 
            this.изменитьРазмерToolStripMenuItem.Name = "изменитьРазмерToolStripMenuItem";
            this.изменитьРазмерToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.изменитьРазмерToolStripMenuItem.Text = "Изменить размер";
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.помощьToolStripMenuItem.Text = "Помощь";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Enabled = false;
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Enabled = false;
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // анимацияToolStripMenuItem
            // 
            this.анимацияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mm_animPlay,
            this.mm_animPause,
            this.mm_animStop});
            this.анимацияToolStripMenuItem.Name = "анимацияToolStripMenuItem";
            this.анимацияToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.анимацияToolStripMenuItem.Text = "Анимация";
            // 
            // mm_animPlay
            // 
            this.mm_animPlay.Name = "mm_animPlay";
            this.mm_animPlay.Size = new System.Drawing.Size(221, 22);
            this.mm_animPlay.Text = "Воспроизвести анимацию";
            this.mm_animPlay.Click += new System.EventHandler(this.mm_animPlay_Click);
            // 
            // mm_animPause
            // 
            this.mm_animPause.Name = "mm_animPause";
            this.mm_animPause.Size = new System.Drawing.Size(221, 22);
            this.mm_animPause.Text = "Приостановить анимацию";
            this.mm_animPause.Click += new System.EventHandler(this.mm_animPause_Click);
            // 
            // mm_animStop
            // 
            this.mm_animStop.Name = "mm_animStop";
            this.mm_animStop.Size = new System.Drawing.Size(221, 22);
            this.mm_animStop.Text = "Остановить анимацию";
            this.mm_animStop.Click += new System.EventHandler(this.mm_animStop_Click);
            // 
            // editor
            // 
            this.editor.AnimationPosition = 0;
            this.editor.AnimationState = AnimationEditor.AnimationState.Stop;
            this.editor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(32)))), ((int)(((byte)(51)))));
            this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editor.FrameBackground = ((System.Drawing.Image)(resources.GetObject("editor.FrameBackground")));
            this.editor.IsLoop = false;
            this.editor.Location = new System.Drawing.Point(0, 24);
            this.editor.MinimumSize = new System.Drawing.Size(500, 300);
            this.editor.Name = "editor";
            this.editor.SelectedIndex = 0;
            this.editor.Size = new System.Drawing.Size(701, 379);
            this.editor.TabIndex = 1;
            this.editor.Load += new System.EventHandler(this.editor_Load);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(32)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(701, 403);
            this.Controls.Add(this.editor);
            this.Controls.Add(this.mainmenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainmenu;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактор Анимаций";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.mainmenu.ResumeLayout(false);
            this.mainmenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainmenu;
        private System.Windows.Forms.ToolStripMenuItem mmFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private Editor editor;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem импортToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mmExport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem mmExit;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mmChangeView;
        private System.Windows.Forms.ToolStripMenuItem макетToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменитьРазмерToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem анимацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mm_animPlay;
        private System.Windows.Forms.ToolStripMenuItem mm_animPause;
        private System.Windows.Forms.ToolStripMenuItem mm_animStop;
    }
}

