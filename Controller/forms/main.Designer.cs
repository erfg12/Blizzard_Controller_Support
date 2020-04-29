namespace Blizzard_Controller
{
    partial class mainform
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
            this.buttonPressesBGW = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.cntrlStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ProcRunningLabel = new System.Windows.Forms.Label();
            this.joysticksBGW = new System.ComponentModel.BackgroundWorker();
            this.IncCursorSpeed = new System.Windows.Forms.CheckBox();
            this.deadzoneBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cursorSpeedBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.overlayBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.HowToSetupTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.WC3SetupBox = new System.Windows.Forms.TextBox();
            this.startSC2 = new System.Windows.Forms.Button();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.SC2SetupBox = new System.Windows.Forms.TextBox();
            this.SC1SetupBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.HowToSetupTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPressesBGW
            // 
            this.buttonPressesBGW.WorkerSupportsCancellation = true;
            this.buttonPressesBGW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.buttonPresses_DoWork);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Controller:";
            // 
            // cntrlStatus
            // 
            this.cntrlStatus.AutoSize = true;
            this.cntrlStatus.ForeColor = System.Drawing.Color.Maroon;
            this.cntrlStatus.Location = new System.Drawing.Point(73, 13);
            this.cntrlStatus.Name = "cntrlStatus";
            this.cntrlStatus.Size = new System.Drawing.Size(73, 13);
            this.cntrlStatus.TabIndex = 1;
            this.cntrlStatus.Text = "Disconnected";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(169, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Game:";
            // 
            // ProcRunningLabel
            // 
            this.ProcRunningLabel.AutoSize = true;
            this.ProcRunningLabel.ForeColor = System.Drawing.Color.Maroon;
            this.ProcRunningLabel.Location = new System.Drawing.Point(210, 13);
            this.ProcRunningLabel.Name = "ProcRunningLabel";
            this.ProcRunningLabel.Size = new System.Drawing.Size(67, 13);
            this.ProcRunningLabel.TabIndex = 3;
            this.ProcRunningLabel.Text = "Not Running";
            // 
            // joysticksBGW
            // 
            this.joysticksBGW.WorkerSupportsCancellation = true;
            this.joysticksBGW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.joysticks_DoWork);
            // 
            // IncCursorSpeed
            // 
            this.IncCursorSpeed.AutoSize = true;
            this.IncCursorSpeed.Checked = true;
            this.IncCursorSpeed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IncCursorSpeed.Location = new System.Drawing.Point(11, 24);
            this.IncCursorSpeed.Name = "IncCursorSpeed";
            this.IncCursorSpeed.Size = new System.Drawing.Size(131, 17);
            this.IncCursorSpeed.TabIndex = 5;
            this.IncCursorSpeed.Text = "Variable Cursor Speed";
            this.IncCursorSpeed.UseVisualStyleBackColor = true;
            this.IncCursorSpeed.CheckedChanged += new System.EventHandler(this.IncCursorSpeed_CheckedChanged);
            // 
            // deadzoneBox
            // 
            this.deadzoneBox.Location = new System.Drawing.Point(77, 47);
            this.deadzoneBox.Name = "deadzoneBox";
            this.deadzoneBox.Size = new System.Drawing.Size(54, 20);
            this.deadzoneBox.TabIndex = 6;
            this.deadzoneBox.Text = "6000";
            this.deadzoneBox.TextChanged += new System.EventHandler(this.deadzoneBox_TextChanged);
            this.deadzoneBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.deadzoneBox_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Deadzone:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cursorSpeedBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.overlayBox);
            this.groupBox1.Controls.Add(this.deadzoneBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.IncCursorSpeed);
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 81);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SETTINGS";
            // 
            // cursorSpeedBox
            // 
            this.cursorSpeedBox.Location = new System.Drawing.Point(256, 47);
            this.cursorSpeedBox.Name = "cursorSpeedBox";
            this.cursorSpeedBox.Size = new System.Drawing.Size(54, 20);
            this.cursorSpeedBox.TabIndex = 9;
            this.cursorSpeedBox.Text = "12";
            this.cursorSpeedBox.TextChanged += new System.EventHandler(this.cursorSpeedBox_TextChanged);
            this.cursorSpeedBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cursorSpeedBox_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(168, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Cursor Speed:";
            // 
            // overlayBox
            // 
            this.overlayBox.AutoSize = true;
            this.overlayBox.Checked = true;
            this.overlayBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.overlayBox.Location = new System.Drawing.Point(171, 24);
            this.overlayBox.Name = "overlayBox";
            this.overlayBox.Size = new System.Drawing.Size(62, 17);
            this.overlayBox.TabIndex = 8;
            this.overlayBox.Text = "Overlay";
            this.overlayBox.UseVisualStyleBackColor = true;
            this.overlayBox.CheckedChanged += new System.EventHandler(this.OverlayBox_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.HowToSetupTabControl);
            this.groupBox2.Location = new System.Drawing.Point(12, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(573, 279);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "HOW TO SETUP";
            // 
            // HowToSetupTabControl
            // 
            this.HowToSetupTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HowToSetupTabControl.Controls.Add(this.tabPage1);
            this.HowToSetupTabControl.Controls.Add(this.tabPage2);
            this.HowToSetupTabControl.Controls.Add(this.tabPage3);
            this.HowToSetupTabControl.Location = new System.Drawing.Point(7, 20);
            this.HowToSetupTabControl.Name = "HowToSetupTabControl";
            this.HowToSetupTabControl.SelectedIndex = 0;
            this.HowToSetupTabControl.Size = new System.Drawing.Size(560, 253);
            this.HowToSetupTabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SC1SetupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(552, 227);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "StarCraft 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.SC2SetupBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(552, 227);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "StarCraft II";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.WC3SetupBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(552, 227);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "WarCraft III";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // WC3SetupBox
            // 
            this.WC3SetupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WC3SetupBox.BackColor = System.Drawing.Color.White;
            this.WC3SetupBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.WC3SetupBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.WC3SetupBox.Location = new System.Drawing.Point(6, 6);
            this.WC3SetupBox.Multiline = true;
            this.WC3SetupBox.Name = "WC3SetupBox";
            this.WC3SetupBox.ReadOnly = true;
            this.WC3SetupBox.Size = new System.Drawing.Size(462, 215);
            this.WC3SetupBox.TabIndex = 1;
            this.WC3SetupBox.Text = "N/A";
            // 
            // startSC2
            // 
            this.startSC2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startSC2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startSC2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.startSC2.Location = new System.Drawing.Point(351, 62);
            this.startSC2.Name = "startSC2";
            this.startSC2.Size = new System.Drawing.Size(234, 57);
            this.startSC2.TabIndex = 10;
            this.startSC2.Text = "START BATTLE.NET";
            this.startSC2.UseVisualStyleBackColor = true;
            this.startSC2.Click += new System.EventHandler(this.startSC2_Click);
            // 
            // ExitBtn
            // 
            this.ExitBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitBtn.ForeColor = System.Drawing.Color.Maroon;
            this.ExitBtn.Location = new System.Drawing.Point(351, 12);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(234, 44);
            this.ExitBtn.TabIndex = 11;
            this.ExitBtn.Text = "EXIT APPLICATION";
            this.ExitBtn.UseVisualStyleBackColor = true;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // SC2SetupBox
            // 
            this.SC2SetupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SC2SetupBox.BackColor = System.Drawing.Color.White;
            this.SC2SetupBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SC2SetupBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.SC2SetupBox.Location = new System.Drawing.Point(6, 6);
            this.SC2SetupBox.Multiline = true;
            this.SC2SetupBox.Name = "SC2SetupBox";
            this.SC2SetupBox.ReadOnly = true;
            this.SC2SetupBox.Size = new System.Drawing.Size(540, 215);
            this.SC2SetupBox.TabIndex = 2;
            this.SC2SetupBox.Text = "N/A";
            // 
            // SC1SetupBox
            // 
            this.SC1SetupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SC1SetupBox.BackColor = System.Drawing.Color.White;
            this.SC1SetupBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SC1SetupBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.SC1SetupBox.Location = new System.Drawing.Point(6, 6);
            this.SC1SetupBox.Multiline = true;
            this.SC1SetupBox.Name = "SC1SetupBox";
            this.SC1SetupBox.ReadOnly = true;
            this.SC1SetupBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.SC1SetupBox.Size = new System.Drawing.Size(540, 215);
            this.SC1SetupBox.TabIndex = 2;
            this.SC1SetupBox.Text = "N/A";
            // 
            // mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 416);
            this.Controls.Add(this.ExitBtn);
            this.Controls.Add(this.startSC2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ProcRunningLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cntrlStatus);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "mainform";
            this.ShowIcon = false;
            this.Text = "SC / SC2 / WC3 Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainform_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Mainform_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.HowToSetupTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker buttonPressesBGW;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label cntrlStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ProcRunningLabel;
        private System.ComponentModel.BackgroundWorker joysticksBGW;
        private System.Windows.Forms.CheckBox IncCursorSpeed;
        private System.Windows.Forms.TextBox deadzoneBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox overlayBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox cursorSpeedBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button startSC2;
        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.TabControl HowToSetupTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox WC3SetupBox;
        private System.Windows.Forms.TextBox SC1SetupBox;
        private System.Windows.Forms.TextBox SC2SetupBox;
    }
}

