namespace AutoUploadChangesToFtp
{
	partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			this.notifyIconTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStripNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkForchangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autouploadChangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.addMoreLinkedDirectoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonCheckForChanges = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.labelStatusMessage = new System.Windows.Forms.Label();
			this.checkBoxTopmost = new System.Windows.Forms.CheckBox();
			this.timerRoutineChecking = new System.Windows.Forms.Timer(this.components);
			this.buttonChangeLinkedFolders = new System.Windows.Forms.Button();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDownIntervalForChecking = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonRabaseMetadata = new System.Windows.Forms.Button();
			this.labelAbout = new System.Windows.Forms.Label();
			this.buttonAutoUploadChanged = new System.Windows.Forms.Button();
			this.contextMenuStripNotifyIcon.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervalForChecking)).BeginInit();
			this.SuspendLayout();
			// 
			// notifyIconTrayIcon
			// 
			this.notifyIconTrayIcon.ContextMenuStrip = this.contextMenuStripNotifyIcon;
			this.notifyIconTrayIcon.Text = "Monitoring local changes";
			this.notifyIconTrayIcon.Visible = true;
			this.notifyIconTrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
			// 
			// contextMenuStripNotifyIcon
			// 
			this.contextMenuStripNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.checkForchangesToolStripMenuItem,
            this.autouploadChangesToolStripMenuItem,
            this.toolStripSeparator1,
            this.addMoreLinkedDirectoriesToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.contextMenuStripNotifyIcon.Name = "contextMenuStripNotifyIcon";
			this.contextMenuStripNotifyIcon.Size = new System.Drawing.Size(221, 148);
			// 
			// showToolStripMenuItem
			// 
			this.showToolStripMenuItem.Name = "showToolStripMenuItem";
			this.showToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.showToolStripMenuItem.Text = "Sh&ow";
			this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
			// 
			// checkForchangesToolStripMenuItem
			// 
			this.checkForchangesToolStripMenuItem.Name = "checkForchangesToolStripMenuItem";
			this.checkForchangesToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.checkForchangesToolStripMenuItem.Text = "Check for &changes";
			this.checkForchangesToolStripMenuItem.Click += new System.EventHandler(this.checkForchangesToolStripMenuItem_Click);
			// 
			// autouploadChangesToolStripMenuItem
			// 
			this.autouploadChangesToolStripMenuItem.Name = "autouploadChangesToolStripMenuItem";
			this.autouploadChangesToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.autouploadChangesToolStripMenuItem.Text = "Auto &upload changes";
			this.autouploadChangesToolStripMenuItem.Click += new System.EventHandler(this.autouploadChangesToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(217, 6);
			// 
			// addMoreLinkedDirectoriesToolStripMenuItem
			// 
			this.addMoreLinkedDirectoriesToolStripMenuItem.Name = "addMoreLinkedDirectoriesToolStripMenuItem";
			this.addMoreLinkedDirectoriesToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.addMoreLinkedDirectoriesToolStripMenuItem.Text = "&Add more linked directories";
			this.addMoreLinkedDirectoriesToolStripMenuItem.Click += new System.EventHandler(this.addMoreLinkedDirectoriesToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(217, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// buttonCheckForChanges
			// 
			this.buttonCheckForChanges.Location = new System.Drawing.Point(13, 13);
			this.buttonCheckForChanges.Name = "buttonCheckForChanges";
			this.buttonCheckForChanges.Size = new System.Drawing.Size(120, 23);
			this.buttonCheckForChanges.TabIndex = 0;
			this.buttonCheckForChanges.Text = "&Check for changes";
			this.buttonCheckForChanges.UseVisualStyleBackColor = true;
			this.buttonCheckForChanges.Click += new System.EventHandler(this.buttonCheckForChanges_Click);
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(12, 70);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(579, 225);
			this.textBox1.TabIndex = 1;
			this.textBox1.WordWrap = false;
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(12, 324);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(527, 17);
			this.progressBar1.TabIndex = 2;
			// 
			// labelStatusMessage
			// 
			this.labelStatusMessage.AutoSize = true;
			this.labelStatusMessage.Location = new System.Drawing.Point(12, 302);
			this.labelStatusMessage.Name = "labelStatusMessage";
			this.labelStatusMessage.Size = new System.Drawing.Size(35, 13);
			this.labelStatusMessage.TabIndex = 3;
			this.labelStatusMessage.Text = "label1";
			// 
			// checkBoxTopmost
			// 
			this.checkBoxTopmost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxTopmost.AutoSize = true;
			this.checkBoxTopmost.Location = new System.Drawing.Point(524, 17);
			this.checkBoxTopmost.Name = "checkBoxTopmost";
			this.checkBoxTopmost.Size = new System.Drawing.Size(67, 17);
			this.checkBoxTopmost.TabIndex = 4;
			this.checkBoxTopmost.Text = "&Topmost";
			this.checkBoxTopmost.UseVisualStyleBackColor = true;
			this.checkBoxTopmost.CheckedChanged += new System.EventHandler(this.checkBoxTopmost_CheckedChanged);
			// 
			// timerRoutineChecking
			// 
			this.timerRoutineChecking.Interval = 60000;
			this.timerRoutineChecking.Tick += new System.EventHandler(this.timerRoutineChecking_Tick);
			// 
			// buttonChangeLinkedFolders
			// 
			this.buttonChangeLinkedFolders.Location = new System.Drawing.Point(262, 13);
			this.buttonChangeLinkedFolders.Name = "buttonChangeLinkedFolders";
			this.buttonChangeLinkedFolders.Size = new System.Drawing.Size(124, 23);
			this.buttonChangeLinkedFolders.TabIndex = 5;
			this.buttonChangeLinkedFolders.Text = "Change &linked folders";
			this.buttonChangeLinkedFolders.UseVisualStyleBackColor = true;
			this.buttonChangeLinkedFolders.Click += new System.EventHandler(this.buttonChangeLinkedFolders_Click);
			// 
			// linkLabel1
			// 
			this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(510, 54);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(81, 13);
			this.linkLabel1.TabIndex = 6;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Clear messages";
			this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 49);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Check every";
			// 
			// numericUpDownIntervalForChecking
			// 
			this.numericUpDownIntervalForChecking.Location = new System.Drawing.Point(82, 47);
			this.numericUpDownIntervalForChecking.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
			this.numericUpDownIntervalForChecking.Name = "numericUpDownIntervalForChecking";
			this.numericUpDownIntervalForChecking.Size = new System.Drawing.Size(42, 20);
			this.numericUpDownIntervalForChecking.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(130, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "minutes";
			// 
			// buttonRabaseMetadata
			// 
			this.buttonRabaseMetadata.Location = new System.Drawing.Point(392, 15);
			this.buttonRabaseMetadata.Name = "buttonRabaseMetadata";
			this.buttonRabaseMetadata.Size = new System.Drawing.Size(110, 21);
			this.buttonRabaseMetadata.TabIndex = 10;
			this.buttonRabaseMetadata.Text = "Rebase metadata";
			this.buttonRabaseMetadata.UseVisualStyleBackColor = true;
			this.buttonRabaseMetadata.Click += new System.EventHandler(this.buttonRabaseMetadata_Click);
			// 
			// labelAbout
			// 
			this.labelAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelAbout.AutoSize = true;
			this.labelAbout.Cursor = System.Windows.Forms.Cursors.Hand;
			this.labelAbout.Location = new System.Drawing.Point(546, 324);
			this.labelAbout.Name = "labelAbout";
			this.labelAbout.Size = new System.Drawing.Size(35, 13);
			this.labelAbout.TabIndex = 11;
			this.labelAbout.Text = "A&bout";
			this.labelAbout.Click += new System.EventHandler(this.labelAbout_Click);
			// 
			// buttonAutoUploadChanged
			// 
			this.buttonAutoUploadChanged.Location = new System.Drawing.Point(133, 13);
			this.buttonAutoUploadChanged.Name = "buttonAutoUploadChanged";
			this.buttonAutoUploadChanged.Size = new System.Drawing.Size(95, 23);
			this.buttonAutoUploadChanged.TabIndex = 12;
			this.buttonAutoUploadChanged.Text = "&Upload changes";
			this.buttonAutoUploadChanged.UseVisualStyleBackColor = true;
			this.buttonAutoUploadChanged.Click += new System.EventHandler(this.buttonAutoUploadChanged_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(603, 353);
			this.Controls.Add(this.buttonAutoUploadChanged);
			this.Controls.Add(this.labelAbout);
			this.Controls.Add(this.buttonRabaseMetadata);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.numericUpDownIntervalForChecking);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.buttonChangeLinkedFolders);
			this.Controls.Add(this.checkBoxTopmost);
			this.Controls.Add(this.labelStatusMessage);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.buttonCheckForChanges);
			this.DoubleBuffered = true;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Upload changes to FTP";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Shown += new System.EventHandler(this.Form1_Shown);
			this.contextMenuStripNotifyIcon.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntervalForChecking)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIconTrayIcon;
		private System.Windows.Forms.Button buttonCheckForChanges;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label labelStatusMessage;
		private System.Windows.Forms.CheckBox checkBoxTopmost;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripNotifyIcon;
		private System.Windows.Forms.ToolStripMenuItem checkForchangesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addMoreLinkedDirectoriesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.Timer timerRoutineChecking;
		private System.Windows.Forms.Button buttonChangeLinkedFolders;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDownIntervalForChecking;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonRabaseMetadata;
		private System.Windows.Forms.Label labelAbout;
		private System.Windows.Forms.Button buttonAutoUploadChanged;
		private System.Windows.Forms.ToolStripMenuItem autouploadChangesToolStripMenuItem;
	}
}

