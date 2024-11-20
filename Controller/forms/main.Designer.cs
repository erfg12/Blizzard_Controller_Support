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
            label1 = new Label();
            cntrlStatus = new Label();
            label2 = new Label();
            ProcRunningLabel = new Label();
            IncCursorSpeed = new CheckBox();
            deadzoneBox = new TextBox();
            label3 = new Label();
            groupBox1 = new GroupBox();
            cursorSpeedBox = new TextBox();
            label4 = new Label();
            overlayBox = new CheckBox();
            groupBox2 = new GroupBox();
            HowToSetupTabControl = new TabControl();
            tabPage1 = new TabPage();
            label6 = new Label();
            label5 = new Label();
            restoreHotkeysBtn = new Button();
            gridForZBtn = new Button();
            gridForTPBtn = new Button();
            SC1SetupBox = new TextBox();
            tabPage2 = new TabPage();
            SC2SetupBox = new TextBox();
            tabPage5 = new TabPage();
            WC3SetupBox = new TextBox();
            startSC2 = new Button();
            ExitBtn = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            HowToSetupTabControl.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage5.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 15);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(65, 13);
            label1.TabIndex = 0;
            label1.Text = "Controller:";
            // 
            // cntrlStatus
            // 
            cntrlStatus.AutoSize = true;
            cntrlStatus.ForeColor = Color.Maroon;
            cntrlStatus.Location = new Point(85, 15);
            cntrlStatus.Margin = new Padding(4, 0, 4, 0);
            cntrlStatus.Name = "cntrlStatus";
            cntrlStatus.Size = new Size(79, 15);
            cntrlStatus.TabIndex = 1;
            cntrlStatus.Text = "Disconnected";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(197, 15);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(43, 13);
            label2.TabIndex = 2;
            label2.Text = "Game:";
            // 
            // ProcRunningLabel
            // 
            ProcRunningLabel.AutoSize = true;
            ProcRunningLabel.ForeColor = Color.Maroon;
            ProcRunningLabel.Location = new Point(245, 15);
            ProcRunningLabel.Margin = new Padding(4, 0, 4, 0);
            ProcRunningLabel.Name = "ProcRunningLabel";
            ProcRunningLabel.Size = new Size(75, 15);
            ProcRunningLabel.TabIndex = 3;
            ProcRunningLabel.Text = "Not Running";
            // 
            // IncCursorSpeed
            // 
            IncCursorSpeed.AutoSize = true;
            IncCursorSpeed.Checked = true;
            IncCursorSpeed.CheckState = CheckState.Checked;
            IncCursorSpeed.Location = new Point(13, 28);
            IncCursorSpeed.Margin = new Padding(4, 3, 4, 3);
            IncCursorSpeed.Name = "IncCursorSpeed";
            IncCursorSpeed.Size = new Size(140, 19);
            IncCursorSpeed.TabIndex = 5;
            IncCursorSpeed.Text = "Variable Cursor Speed";
            IncCursorSpeed.UseVisualStyleBackColor = true;
            IncCursorSpeed.CheckedChanged += IncCursorSpeed_CheckedChanged;
            // 
            // deadzoneBox
            // 
            deadzoneBox.Location = new Point(90, 54);
            deadzoneBox.Margin = new Padding(4, 3, 4, 3);
            deadzoneBox.Name = "deadzoneBox";
            deadzoneBox.Size = new Size(62, 23);
            deadzoneBox.TabIndex = 6;
            deadzoneBox.Text = "0.1";
            deadzoneBox.TextChanged += deadzoneBox_TextChanged;
            deadzoneBox.KeyPress += deadzoneBox_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(9, 58);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(68, 13);
            label3.TabIndex = 7;
            label3.Text = "Deadzone:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cursorSpeedBox);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(overlayBox);
            groupBox1.Controls.Add(deadzoneBox);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(IncCursorSpeed);
            groupBox1.Location = new Point(14, 44);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(388, 93);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "SETTINGS";
            // 
            // cursorSpeedBox
            // 
            cursorSpeedBox.Location = new Point(299, 54);
            cursorSpeedBox.Margin = new Padding(4, 3, 4, 3);
            cursorSpeedBox.Name = "cursorSpeedBox";
            cursorSpeedBox.Size = new Size(62, 23);
            cursorSpeedBox.TabIndex = 9;
            cursorSpeedBox.Text = "12";
            cursorSpeedBox.TextChanged += cursorSpeedBox_TextChanged;
            cursorSpeedBox.KeyPress += cursorSpeedBox_KeyPress;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(196, 58);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(87, 13);
            label4.TabIndex = 10;
            label4.Text = "Cursor Speed:";
            // 
            // overlayBox
            // 
            overlayBox.AutoSize = true;
            overlayBox.Checked = true;
            overlayBox.CheckState = CheckState.Checked;
            overlayBox.Location = new Point(200, 28);
            overlayBox.Margin = new Padding(4, 3, 4, 3);
            overlayBox.Name = "overlayBox";
            overlayBox.Size = new Size(66, 19);
            overlayBox.TabIndex = 8;
            overlayBox.Text = "Overlay";
            overlayBox.UseVisualStyleBackColor = true;
            overlayBox.CheckedChanged += OverlayBox_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(HowToSetupTabControl);
            groupBox2.Location = new Point(14, 144);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new Size(668, 322);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "HOW TO SETUP";
            // 
            // HowToSetupTabControl
            // 
            HowToSetupTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            HowToSetupTabControl.Controls.Add(tabPage1);
            HowToSetupTabControl.Controls.Add(tabPage2);
            HowToSetupTabControl.Controls.Add(tabPage5);
            HowToSetupTabControl.Location = new Point(8, 23);
            HowToSetupTabControl.Margin = new Padding(4, 3, 4, 3);
            HowToSetupTabControl.Name = "HowToSetupTabControl";
            HowToSetupTabControl.SelectedIndex = 0;
            HowToSetupTabControl.Size = new Size(653, 292);
            HowToSetupTabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label6);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(restoreHotkeysBtn);
            tabPage1.Controls.Add(gridForZBtn);
            tabPage1.Controls.Add(gridForTPBtn);
            tabPage1.Controls.Add(SC1SetupBox);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new Size(645, 264);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "StarCraft 1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(557, 174);
            label6.Name = "label6";
            label6.Size = new Size(82, 15);
            label6.TabIndex = 7;
            label6.Text = "---------------";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(578, 80);
            label5.Name = "label5";
            label5.Size = new Size(39, 15);
            label5.TabIndex = 6;
            label5.Text = "- OR -";
            // 
            // restoreHotkeysBtn
            // 
            restoreHotkeysBtn.Cursor = Cursors.Hand;
            restoreHotkeysBtn.Location = new Point(557, 202);
            restoreHotkeysBtn.Name = "restoreHotkeysBtn";
            restoreHotkeysBtn.Size = new Size(81, 53);
            restoreHotkeysBtn.TabIndex = 5;
            restoreHotkeysBtn.Text = "Restore Default Hotkeys";
            restoreHotkeysBtn.UseVisualStyleBackColor = true;
            restoreHotkeysBtn.Click += restoreHotkeysBtn_Click;
            // 
            // gridForZBtn
            // 
            gridForZBtn.BackColor = Color.Maroon;
            gridForZBtn.Cursor = Cursors.Hand;
            gridForZBtn.FlatStyle = FlatStyle.Popup;
            gridForZBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            gridForZBtn.ForeColor = SystemColors.Control;
            gridForZBtn.Location = new Point(557, 97);
            gridForZBtn.Name = "gridForZBtn";
            gridForZBtn.Size = new Size(81, 74);
            gridForZBtn.TabIndex = 4;
            gridForZBtn.Text = "Set Grid Layout for Zerg";
            gridForZBtn.UseVisualStyleBackColor = false;
            gridForZBtn.Click += gridForZBtn_Click;
            // 
            // gridForTPBtn
            // 
            gridForTPBtn.BackColor = Color.Teal;
            gridForTPBtn.Cursor = Cursors.Hand;
            gridForTPBtn.FlatStyle = FlatStyle.Popup;
            gridForTPBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            gridForTPBtn.ForeColor = SystemColors.Control;
            gridForTPBtn.Location = new Point(557, 6);
            gridForTPBtn.Name = "gridForTPBtn";
            gridForTPBtn.Size = new Size(81, 71);
            gridForTPBtn.TabIndex = 3;
            gridForTPBtn.Text = "Set Grid Layout for Terran and Protoss";
            gridForTPBtn.UseVisualStyleBackColor = false;
            gridForTPBtn.Click += gridForTPBtn_Click;
            // 
            // SC1SetupBox
            // 
            SC1SetupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SC1SetupBox.BackColor = Color.White;
            SC1SetupBox.BorderStyle = BorderStyle.None;
            SC1SetupBox.Location = new Point(7, 7);
            SC1SetupBox.Margin = new Padding(4, 3, 4, 3);
            SC1SetupBox.Multiline = true;
            SC1SetupBox.Name = "SC1SetupBox";
            SC1SetupBox.ReadOnly = true;
            SC1SetupBox.ScrollBars = ScrollBars.Both;
            SC1SetupBox.Size = new Size(543, 248);
            SC1SetupBox.TabIndex = 2;
            SC1SetupBox.Text = "N/A";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(SC2SetupBox);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4, 3, 4, 3);
            tabPage2.Size = new Size(645, 264);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "StarCraft II";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // SC2SetupBox
            // 
            SC2SetupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SC2SetupBox.BackColor = Color.White;
            SC2SetupBox.BorderStyle = BorderStyle.None;
            SC2SetupBox.Location = new Point(7, 7);
            SC2SetupBox.Margin = new Padding(4, 3, 4, 3);
            SC2SetupBox.Multiline = true;
            SC2SetupBox.Name = "SC2SetupBox";
            SC2SetupBox.ReadOnly = true;
            SC2SetupBox.Size = new Size(630, 248);
            SC2SetupBox.TabIndex = 2;
            SC2SetupBox.Text = "N/A";
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(WC3SetupBox);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Margin = new Padding(4, 3, 4, 3);
            tabPage5.Name = "tabPage5";
            tabPage5.Size = new Size(645, 264);
            tabPage5.TabIndex = 2;
            tabPage5.Text = "WarCraft III";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // WC3SetupBox
            // 
            WC3SetupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            WC3SetupBox.BackColor = Color.White;
            WC3SetupBox.BorderStyle = BorderStyle.None;
            WC3SetupBox.Location = new Point(7, 7);
            WC3SetupBox.Margin = new Padding(4, 3, 4, 3);
            WC3SetupBox.Multiline = true;
            WC3SetupBox.Name = "WC3SetupBox";
            WC3SetupBox.ReadOnly = true;
            WC3SetupBox.Size = new Size(634, 248);
            WC3SetupBox.TabIndex = 1;
            WC3SetupBox.Text = "N/A";
            // 
            // startSC2
            // 
            startSC2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            startSC2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            startSC2.ForeColor = Color.RoyalBlue;
            startSC2.Location = new Point(410, 72);
            startSC2.Margin = new Padding(4, 3, 4, 3);
            startSC2.Name = "startSC2";
            startSC2.Size = new Size(273, 66);
            startSC2.TabIndex = 10;
            startSC2.Text = "START BATTLE.NET";
            startSC2.UseVisualStyleBackColor = true;
            startSC2.Click += startSC2_Click;
            // 
            // ExitBtn
            // 
            ExitBtn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ExitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ExitBtn.ForeColor = Color.Maroon;
            ExitBtn.Location = new Point(410, 14);
            ExitBtn.Margin = new Padding(4, 3, 4, 3);
            ExitBtn.Name = "ExitBtn";
            ExitBtn.Size = new Size(273, 51);
            ExitBtn.TabIndex = 11;
            ExitBtn.Text = "EXIT APPLICATION";
            ExitBtn.UseVisualStyleBackColor = true;
            ExitBtn.Click += ExitBtn_Click;
            // 
            // mainform
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(696, 480);
            Controls.Add(ExitBtn);
            Controls.Add(startSC2);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(ProcRunningLabel);
            Controls.Add(label2);
            Controls.Add(cntrlStatus);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "mainform";
            ShowIcon = false;
            Text = "SC / SC2 / WC3 Controller";
            FormClosing += mainform_FormClosing;
            Load += Form1_Load;
            Shown += Mainform_Shown;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            HowToSetupTabControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label cntrlStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ProcRunningLabel;
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
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox WC3SetupBox;
        private System.Windows.Forms.TextBox SC1SetupBox;
        private System.Windows.Forms.TextBox SC2SetupBox;
        private System.Windows.Forms.Button gridForTPBtn;
        private System.Windows.Forms.Button gridForZBtn;
        private System.Windows.Forms.Button restoreHotkeysBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

