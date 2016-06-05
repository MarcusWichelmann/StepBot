namespace StepBotViewer
{
	partial class MainWindow
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
			if(disposing && (components != null))
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
			this.connectButton = new System.Windows.Forms.Button();
			this.connectHostTextBox = new System.Windows.Forms.TextBox();
			this.horizontalSplitContainer = new System.Windows.Forms.SplitContainer();
			this.verticalSplitContainer = new System.Windows.Forms.SplitContainer();
			this.robotStatusStrip = new System.Windows.Forms.StatusStrip();
			this.speedLeftBar = new System.Windows.Forms.ToolStripProgressBar();
			this.speedLeftLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStripAlignmentLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.speedRightLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.speedRightBar = new System.Windows.Forms.ToolStripProgressBar();
			this.debugLogTextBox = new System.Windows.Forms.TextBox();
			this.connectPortNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.mapView = new StepBotViewer.MapView();
			((System.ComponentModel.ISupportInitialize)(this.horizontalSplitContainer)).BeginInit();
			this.horizontalSplitContainer.Panel1.SuspendLayout();
			this.horizontalSplitContainer.Panel2.SuspendLayout();
			this.horizontalSplitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.verticalSplitContainer)).BeginInit();
			this.verticalSplitContainer.Panel1.SuspendLayout();
			this.verticalSplitContainer.SuspendLayout();
			this.robotStatusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.connectPortNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// connectButton
			// 
			this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.connectButton.Location = new System.Drawing.Point(812, 8);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(75, 23);
			this.connectButton.TabIndex = 0;
			this.connectButton.Text = "Verbinden";
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
			// 
			// connectHostTextBox
			// 
			this.connectHostTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.connectHostTextBox.Location = new System.Drawing.Point(8, 10);
			this.connectHostTextBox.Name = "connectHostTextBox";
			this.connectHostTextBox.Size = new System.Drawing.Size(711, 20);
			this.connectHostTextBox.TabIndex = 1;
			this.connectHostTextBox.Text = "192.168.50.1";
			// 
			// horizontalSplitContainer
			// 
			this.horizontalSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.horizontalSplitContainer.Location = new System.Drawing.Point(0, 40);
			this.horizontalSplitContainer.Name = "horizontalSplitContainer";
			this.horizontalSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// horizontalSplitContainer.Panel1
			// 
			this.horizontalSplitContainer.Panel1.Controls.Add(this.verticalSplitContainer);
			this.horizontalSplitContainer.Panel1.Controls.Add(this.robotStatusStrip);
			// 
			// horizontalSplitContainer.Panel2
			// 
			this.horizontalSplitContainer.Panel2.Controls.Add(this.debugLogTextBox);
			this.horizontalSplitContainer.Size = new System.Drawing.Size(895, 606);
			this.horizontalSplitContainer.SplitterDistance = 329;
			this.horizontalSplitContainer.TabIndex = 2;
			// 
			// verticalSplitContainer
			// 
			this.verticalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.verticalSplitContainer.Location = new System.Drawing.Point(0, 0);
			this.verticalSplitContainer.Name = "verticalSplitContainer";
			// 
			// verticalSplitContainer.Panel1
			// 
			this.verticalSplitContainer.Panel1.Controls.Add(this.mapView);
			this.verticalSplitContainer.Size = new System.Drawing.Size(895, 307);
			this.verticalSplitContainer.SplitterDistance = 396;
			this.verticalSplitContainer.TabIndex = 1;
			// 
			// robotStatusStrip
			// 
			this.robotStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.speedLeftBar,
            this.speedLeftLabel,
            this.statusStripAlignmentLabel,
            this.speedRightLabel,
            this.speedRightBar});
			this.robotStatusStrip.Location = new System.Drawing.Point(0, 307);
			this.robotStatusStrip.Name = "robotStatusStrip";
			this.robotStatusStrip.Size = new System.Drawing.Size(895, 22);
			this.robotStatusStrip.SizingGrip = false;
			this.robotStatusStrip.TabIndex = 0;
			this.robotStatusStrip.Text = "statusStrip1";
			// 
			// speedLeftBar
			// 
			this.speedLeftBar.Maximum = 400;
			this.speedLeftBar.Name = "speedLeftBar";
			this.speedLeftBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.speedLeftBar.RightToLeftLayout = true;
			this.speedLeftBar.Size = new System.Drawing.Size(100, 16);
			// 
			// speedLeftLabel
			// 
			this.speedLeftLabel.Name = "speedLeftLabel";
			this.speedLeftLabel.Size = new System.Drawing.Size(38, 17);
			this.speedLeftLabel.Text = "0 rpm";
			// 
			// statusStripAlignmentLabel
			// 
			this.statusStripAlignmentLabel.Name = "statusStripAlignmentLabel";
			this.statusStripAlignmentLabel.Size = new System.Drawing.Size(600, 17);
			this.statusStripAlignmentLabel.Spring = true;
			// 
			// speedRightLabel
			// 
			this.speedRightLabel.Name = "speedRightLabel";
			this.speedRightLabel.Size = new System.Drawing.Size(38, 17);
			this.speedRightLabel.Text = "0 rpm";
			// 
			// speedRightBar
			// 
			this.speedRightBar.Maximum = 400;
			this.speedRightBar.Name = "speedRightBar";
			this.speedRightBar.Size = new System.Drawing.Size(100, 16);
			// 
			// debugLogTextBox
			// 
			this.debugLogTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.debugLogTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.debugLogTextBox.Cursor = System.Windows.Forms.Cursors.Default;
			this.debugLogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.debugLogTextBox.Location = new System.Drawing.Point(0, 0);
			this.debugLogTextBox.Multiline = true;
			this.debugLogTextBox.Name = "debugLogTextBox";
			this.debugLogTextBox.ReadOnly = true;
			this.debugLogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.debugLogTextBox.Size = new System.Drawing.Size(895, 273);
			this.debugLogTextBox.TabIndex = 0;
			// 
			// connectPortNumericUpDown
			// 
			this.connectPortNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.connectPortNumericUpDown.Location = new System.Drawing.Point(725, 10);
			this.connectPortNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.connectPortNumericUpDown.Name = "connectPortNumericUpDown";
			this.connectPortNumericUpDown.Size = new System.Drawing.Size(81, 20);
			this.connectPortNumericUpDown.TabIndex = 3;
			this.connectPortNumericUpDown.Value = new decimal(new int[] {
            8888,
            0,
            0,
            0});
			// 
			// mapView
			// 
			this.mapView.BackColor = System.Drawing.SystemColors.Window;
			this.mapView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapView.Location = new System.Drawing.Point(0, 0);
			this.mapView.Name = "mapView";
			this.mapView.Size = new System.Drawing.Size(396, 307);
			this.mapView.TabIndex = 0;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(895, 646);
			this.Controls.Add(this.connectPortNumericUpDown);
			this.Controls.Add(this.horizontalSplitContainer);
			this.Controls.Add(this.connectHostTextBox);
			this.Controls.Add(this.connectButton);
			this.Name = "MainWindow";
			this.Text = "StepBot - Viewer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
			this.horizontalSplitContainer.Panel1.ResumeLayout(false);
			this.horizontalSplitContainer.Panel1.PerformLayout();
			this.horizontalSplitContainer.Panel2.ResumeLayout(false);
			this.horizontalSplitContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.horizontalSplitContainer)).EndInit();
			this.horizontalSplitContainer.ResumeLayout(false);
			this.verticalSplitContainer.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.verticalSplitContainer)).EndInit();
			this.verticalSplitContainer.ResumeLayout(false);
			this.robotStatusStrip.ResumeLayout(false);
			this.robotStatusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.connectPortNumericUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.TextBox connectHostTextBox;
		private System.Windows.Forms.SplitContainer horizontalSplitContainer;
		private System.Windows.Forms.TextBox debugLogTextBox;
		private System.Windows.Forms.NumericUpDown connectPortNumericUpDown;
		private System.Windows.Forms.StatusStrip robotStatusStrip;
		private System.Windows.Forms.ToolStripStatusLabel speedLeftLabel;
		private System.Windows.Forms.ToolStripStatusLabel speedRightLabel;
		private System.Windows.Forms.ToolStripProgressBar speedLeftBar;
		private System.Windows.Forms.ToolStripProgressBar speedRightBar;
		private System.Windows.Forms.ToolStripStatusLabel statusStripAlignmentLabel;
		private System.Windows.Forms.SplitContainer verticalSplitContainer;
		private MapView mapView;
	}
}