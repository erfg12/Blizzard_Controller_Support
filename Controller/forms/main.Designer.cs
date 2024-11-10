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
            buttonPressesBGW = new System.ComponentModel.BackgroundWorker();
            label1 = new System.Windows.Forms.Label();
            cntrlStatus = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ProcRunningLabel = new System.Windows.Forms.Label();
            joysticksBGW = new System.ComponentModel.BackgroundWorker();
            IncCursorSpeed = new System.Windows.Forms.CheckBox();
            deadzoneBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            cursorSpeedBox = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            overlayBox = new System.Windows.Forms.CheckBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            HowToSetupTabControl = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            SC1SetupBox = new System.Windows.Forms.TextBox();
            tabPage2 = new System.Windows.Forms.TabPage();
            SC2SetupBox = new System.Windows.Forms.TextBox();
            tabPage3 = new System.Windows.Forms.TabPage();
            WC3SetupBox = new System.Windows.Forms.TextBox();
            startSC2 = new System.Windows.Forms.Button();
            ExitBtn = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            HowToSetupTabControl.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // buttonPressesBGW
            // 
            buttonPressesBGW.WorkerSupportsCancellation = true;
            buttonPressesBGW.DoWork += buttonPresses_DoWork;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(12, 15);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(65, 13);
            label1.TabIndex = 0;
            label1.Text = "Controller:";
            // 
            // cntrlStatus
            // 
            cntrlStatus.AutoSize = true;
            cntrlStatus.ForeColor = System.Drawing.Color.Maroon;
            cntrlStatus.Location = new System.Drawing.Point(85, 15);
            cntrlStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            cntrlStatus.Name = "cntrlStatus";
            cntrlStatus.Size = new System.Drawing.Size(79, 15);
            cntrlStatus.TabIndex = 1;
            cntrlStatus.Text = "Disconnected";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(197, 15);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(43, 13);
            label2.TabIndex = 2;
            label2.Text = "Game:";
            // 
            // ProcRunningLabel
            // 
            ProcRunningLabel.AutoSize = true;
            ProcRunningLabel.ForeColor = System.Drawing.Color.Maroon;
            ProcRunningLabel.Location = new System.Drawing.Point(245, 15);
            ProcRunningLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ProcRunningLabel.Name = "ProcRunningLabel";
            ProcRunningLabel.Size = new System.Drawing.Size(75, 15);
            ProcRunningLabel.TabIndex = 3;
            ProcRunningLabel.Text = "Not Running";
            // 
            // joysticksBGW
            // 
            joysticksBGW.WorkerSupportsCancellation = true;
            joysticksBGW.DoWork += joysticks_DoWork;
            // 
            // IncCursorSpeed
            // 
            IncCursorSpeed.AutoSize = true;
            IncCursorSpeed.Checked = true;
            IncCursorSpeed.CheckState = System.Windows.Forms.CheckState.Checked;
            IncCursorSpeed.Location = new System.Drawing.Point(13, 28);
            IncCursorSpeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            IncCursorSpeed.Name = "IncCursorSpeed";
            IncCursorSpeed.Size = new System.Drawing.Size(140, 19);
            IncCursorSpeed.TabIndex = 5;
            IncCursorSpeed.Text = "Variable Cursor Speed";
            IncCursorSpeed.UseVisualStyleBackColor = true;
            IncCursorSpeed.CheckedChanged += IncCursorSpeed_CheckedChanged;
            // 
            // deadzoneBox
            // 
            deadzoneBox.Location = new System.Drawing.Point(90, 54);
            deadzoneBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            deadzoneBox.Name = "deadzoneBox";
            deadzoneBox.Size = new System.Drawing.Size(62, 23);
            deadzoneBox.TabIndex = 6;
            deadzoneBox.Text = "0.1";
            deadzoneBox.TextChanged += deadzoneBox_TextChanged;
            deadzoneBox.KeyPress += deadzoneBox_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(9, 58);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(68, 13);
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
            groupBox1.Location = new System.Drawing.Point(14, 44);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(388, 93);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "SETTINGS";
            // 
            // cursorSpeedBox
            // 
            cursorSpeedBox.Location = new System.Drawing.Point(299, 54);
            cursorSpeedBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cursorSpeedBox.Name = "cursorSpeedBox";
            cursorSpeedBox.Size = new System.Drawing.Size(62, 23);
            cursorSpeedBox.TabIndex = 9;
            cursorSpeedBox.Text = "12";
            cursorSpeedBox.TextChanged += cursorSpeedBox_TextChanged;
            cursorSpeedBox.KeyPress += cursorSpeedBox_KeyPress;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label4.Location = new System.Drawing.Point(196, 58);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(87, 13);
            label4.TabIndex = 10;
            label4.Text = "Cursor Speed:";
            // 
            // overlayBox
            // 
            overlayBox.AutoSize = true;
            overlayBox.Checked = true;
            overlayBox.CheckState = System.Windows.Forms.CheckState.Checked;
            overlayBox.Location = new System.Drawing.Point(200, 28);
            overlayBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            overlayBox.Name = "overlayBox";
            overlayBox.Size = new System.Drawing.Size(66, 19);
            overlayBox.TabIndex = 8;
            overlayBox.Text = "Overlay";
            overlayBox.UseVisualStyleBackColor = true;
            overlayBox.CheckedChanged += OverlayBox_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox2.Controls.Add(HowToSetupTabControl);
            groupBox2.Location = new System.Drawing.Point(14, 144);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(668, 322);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "HOW TO SETUP";
            // 
            // HowToSetupTabControl
            // 
            HowToSetupTabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            HowToSetupTabControl.Controls.Add(tabPage1);
            HowToSetupTabControl.Controls.Add(tabPage2);
            HowToSetupTabControl.Controls.Add(tabPage3);
            HowToSetupTabControl.Location = new System.Drawing.Point(8, 23);
            HowToSetupTabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            HowToSetupTabControl.Name = "HowToSetupTabControl";
            HowToSetupTabControl.SelectedIndex = 0;
            HowToSetupTabControl.Size = new System.Drawing.Size(653, 292);
            HowToSetupTabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(SC1SetupBox);
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage1.Size = new System.Drawing.Size(645, 264);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "StarCraft 1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // SC1SetupBox
            // 
            SC1SetupBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SC1SetupBox.BackColor = System.Drawing.Color.White;
            SC1SetupBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            SC1SetupBox.Location = new System.Drawing.Point(7, 7);
            SC1SetupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SC1SetupBox.Multiline = true;
            SC1SetupBox.Name = "SC1SetupBox";
            SC1SetupBox.ReadOnly = true;
            SC1SetupBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            SC1SetupBox.Size = new System.Drawing.Size(630, 248);
            SC1SetupBox.TabIndex = 2;
            SC1SetupBox.Text = "N/A";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(SC2SetupBox);
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage2.Size = new System.Drawing.Size(645, 264);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "StarCraft II";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // SC2SetupBox
            // 
            SC2SetupBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SC2SetupBox.BackColor = System.Drawing.Color.White;
            SC2SetupBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            SC2SetupBox.Location = new System.Drawing.Point(7, 7);
            SC2SetupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SC2SetupBox.Multiline = true;
            SC2SetupBox.Name = "SC2SetupBox";
            SC2SetupBox.ReadOnly = true;
            SC2SetupBox.Size = new System.Drawing.Size(630, 248);
            SC2SetupBox.TabIndex = 2;
            SC2SetupBox.Text = "N/A";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(WC3SetupBox);
            tabPage3.Location = new System.Drawing.Point(4, 24);
            tabPage3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new System.Drawing.Size(645, 264);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "WarCraft III";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // WC3SetupBox
            // 
            WC3SetupBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            WC3SetupBox.BackColor = System.Drawing.Color.White;
            WC3SetupBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            WC3SetupBox.Location = new System.Drawing.Point(7, 7);
            WC3SetupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            WC3SetupBox.Multiline = true;
            WC3SetupBox.Name = "WC3SetupBox";
            WC3SetupBox.ReadOnly = true;
            WC3SetupBox.Size = new System.Drawing.Size(539, 248);
            WC3SetupBox.TabIndex = 1;
            WC3SetupBox.Text = "N/A";
            // 
            // startSC2
            // 
            startSC2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            startSC2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            startSC2.ForeColor = System.Drawing.Color.RoyalBlue;
            startSC2.Location = new System.Drawing.Point(410, 72);
            startSC2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            startSC2.Name = "startSC2";
            startSC2.Size = new System.Drawing.Size(273, 66);
            startSC2.TabIndex = 10;
            startSC2.Text = "START BATTLE.NET";
            startSC2.UseVisualStyleBackColor = true;
            startSC2.Click += startSC2_Click;
            // 
            // ExitBtn
            // 
            ExitBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ExitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ExitBtn.ForeColor = System.Drawing.Color.Maroon;
            ExitBtn.Location = new System.Drawing.Point(410, 14);
            ExitBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ExitBtn.Name = "ExitBtn";
            ExitBtn.Size = new System.Drawing.Size(273, 51);
            ExitBtn.TabIndex = 11;
            ExitBtn.Text = "EXIT APPLICATION";
            ExitBtn.UseVisualStyleBackColor = true;
            ExitBtn.Click += ExitBtn_Click;
            // 
            // mainform
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(696, 480);
            Controls.Add(ExitBtn);
            Controls.Add(startSC2);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(ProcRunningLabel);
            Controls.Add(label2);
            Controls.Add(cntrlStatus);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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

