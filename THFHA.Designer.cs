namespace THFHA_V1._0
{
    partial class THFHA
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(THFHA));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            applicationLogsToolStripMenuItem = new ToolStripMenuItem();
            teamsLogsToolStripMenuItem = new ToolStripMenuItem();
            lbx_modules = new ListBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            enableToolStripMenuItem = new ToolStripMenuItem();
            disableToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            statusStrip1 = new StatusStrip();
            statuslabel = new ToolStripStatusLabel();
            btn_start = new Button();
            btn_stop = new Button();
            lbl_status = new Label();
            lbl_activity = new Label();
            lbl_mute = new Label();
            lbl_camera = new Label();
            stateBindingSource = new BindingSource(components);
            pb_Status = new PictureBox();
            pb_Activity = new PictureBox();
            pb_mute = new PictureBox();
            lbl_blurred = new Label();
            lbl_recording = new Label();
            lbl_hand = new Label();
            lbl_muted = new Label();
            lbl_cam = new Label();
            lbl_meeting = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            notifyIcon1 = new NotifyIcon(components);
            menuStrip1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)stateBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_Status).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_Activity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_mute).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(607, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { settingsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(145, 26);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem, applicationLogsToolStripMenuItem, teamsLogsToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(204, 26);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // applicationLogsToolStripMenuItem
            // 
            applicationLogsToolStripMenuItem.Name = "applicationLogsToolStripMenuItem";
            applicationLogsToolStripMenuItem.Size = new Size(204, 26);
            applicationLogsToolStripMenuItem.Text = "Application Logs";
            applicationLogsToolStripMenuItem.Click += applicationLogsToolStripMenuItem_Click;
            // 
            // teamsLogsToolStripMenuItem
            // 
            teamsLogsToolStripMenuItem.Name = "teamsLogsToolStripMenuItem";
            teamsLogsToolStripMenuItem.Size = new Size(204, 26);
            teamsLogsToolStripMenuItem.Text = "Teams Logs";
            teamsLogsToolStripMenuItem.Click += teamsLogsToolStripMenuItem_Click;
            // 
            // lbx_modules
            // 
            lbx_modules.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbx_modules.ContextMenuStrip = contextMenuStrip1;
            lbx_modules.FormattingEnabled = true;
            lbx_modules.ItemHeight = 20;
            lbx_modules.Location = new Point(404, 59);
            lbx_modules.Name = "lbx_modules";
            lbx_modules.Size = new Size(191, 144);
            lbx_modules.TabIndex = 1;
            lbx_modules.DoubleClick += lbx_modules_DoubleClick;
            lbx_modules.MouseDown += lbx_modules_MouseDown;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { enableToolStripMenuItem, disableToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(129, 52);
            // 
            // enableToolStripMenuItem
            // 
            enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            enableToolStripMenuItem.Size = new Size(128, 24);
            enableToolStripMenuItem.Text = "Enable";
            enableToolStripMenuItem.Click += enableModuleToolStripMenuItem_Click;
            // 
            // disableToolStripMenuItem
            // 
            disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            disableToolStripMenuItem.Size = new Size(128, 24);
            disableToolStripMenuItem.Text = "Disable";
            disableToolStripMenuItem.Click += disableModuleToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(404, 36);
            label1.Name = "label1";
            label1.Size = new Size(132, 20);
            label1.TabIndex = 2;
            label1.Text = "Available Modules";
            label1.Click += label1_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { statuslabel });
            statusStrip1.Location = new Point(0, 352);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(607, 26);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // statuslabel
            // 
            statuslabel.Name = "statuslabel";
            statuslabel.Size = new Size(50, 20);
            statuslabel.Text = "Ready";
            // 
            // btn_start
            // 
            btn_start.Location = new Point(228, 276);
            btn_start.Name = "btn_start";
            btn_start.Size = new Size(94, 29);
            btn_start.TabIndex = 4;
            btn_start.Text = "Start";
            btn_start.UseVisualStyleBackColor = true;
            btn_start.Click += btn_start_Click;
            // 
            // btn_stop
            // 
            btn_stop.Location = new Point(228, 311);
            btn_stop.Name = "btn_stop";
            btn_stop.Size = new Size(94, 29);
            btn_stop.TabIndex = 5;
            btn_stop.Text = "Stop";
            btn_stop.UseVisualStyleBackColor = true;
            btn_stop.Click += btn_stop_Click;
            // 
            // lbl_status
            // 
            lbl_status.AutoSize = true;
            lbl_status.Location = new Point(99, 42);
            lbl_status.Name = "lbl_status";
            lbl_status.Size = new Size(60, 20);
            lbl_status.TabIndex = 6;
            lbl_status.Text = "Waiting";
            // 
            // lbl_activity
            // 
            lbl_activity.AutoSize = true;
            lbl_activity.Location = new Point(99, 79);
            lbl_activity.Name = "lbl_activity";
            lbl_activity.Size = new Size(60, 20);
            lbl_activity.TabIndex = 7;
            lbl_activity.Text = "Waiting";
            // 
            // lbl_mute
            // 
            lbl_mute.AutoSize = true;
            lbl_mute.Location = new Point(99, 117);
            lbl_mute.Name = "lbl_mute";
            lbl_mute.Size = new Size(60, 20);
            lbl_mute.TabIndex = 8;
            lbl_mute.Text = "Waiting";
            // 
            // lbl_camera
            // 
            lbl_camera.AutoSize = true;
            lbl_camera.Location = new Point(99, 154);
            lbl_camera.Name = "lbl_camera";
            lbl_camera.Size = new Size(60, 20);
            lbl_camera.TabIndex = 9;
            lbl_camera.Text = "Waiting";
            // 
            // stateBindingSource
            // 
            stateBindingSource.DataSource = typeof(Model.State);
            // 
            // pb_Status
            // 
            pb_Status.BackgroundImage = Resource1.available;
            pb_Status.BackgroundImageLayout = ImageLayout.None;
            pb_Status.Location = new Point(9, 276);
            pb_Status.Name = "pb_Status";
            pb_Status.Size = new Size(50, 50);
            pb_Status.TabIndex = 25;
            pb_Status.TabStop = false;
            // 
            // pb_Activity
            // 
            pb_Activity.BackgroundImage = Resource1.notinacall;
            pb_Activity.BackgroundImageLayout = ImageLayout.None;
            pb_Activity.Location = new Point(79, 276);
            pb_Activity.Name = "pb_Activity";
            pb_Activity.Size = new Size(50, 50);
            pb_Activity.TabIndex = 26;
            pb_Activity.TabStop = false;
            // 
            // pb_mute
            // 
            pb_mute.BackgroundImage = Resource1.mic_icon;
            pb_mute.BackgroundImageLayout = ImageLayout.None;
            pb_mute.Location = new Point(150, 276);
            pb_mute.Name = "pb_mute";
            pb_mute.Size = new Size(50, 50);
            pb_mute.TabIndex = 28;
            pb_mute.TabStop = false;
            // 
            // lbl_blurred
            // 
            lbl_blurred.AutoSize = true;
            lbl_blurred.Location = new Point(198, 42);
            lbl_blurred.Name = "lbl_blurred";
            lbl_blurred.Size = new Size(60, 20);
            lbl_blurred.TabIndex = 30;
            lbl_blurred.Text = "Waiting";
            // 
            // lbl_recording
            // 
            lbl_recording.AutoSize = true;
            lbl_recording.Location = new Point(198, 80);
            lbl_recording.Name = "lbl_recording";
            lbl_recording.Size = new Size(60, 20);
            lbl_recording.TabIndex = 31;
            lbl_recording.Text = "Waiting";
            // 
            // lbl_hand
            // 
            lbl_hand.AutoSize = true;
            lbl_hand.Location = new Point(198, 118);
            lbl_hand.Name = "lbl_hand";
            lbl_hand.Size = new Size(60, 20);
            lbl_hand.TabIndex = 32;
            lbl_hand.Text = "Waiting";
            // 
            // lbl_muted
            // 
            lbl_muted.AutoSize = true;
            lbl_muted.Location = new Point(198, 156);
            lbl_muted.Name = "lbl_muted";
            lbl_muted.Size = new Size(60, 20);
            lbl_muted.TabIndex = 33;
            lbl_muted.Text = "Waiting";
            // 
            // lbl_cam
            // 
            lbl_cam.AutoSize = true;
            lbl_cam.Location = new Point(198, 194);
            lbl_cam.Name = "lbl_cam";
            lbl_cam.Size = new Size(60, 20);
            lbl_cam.TabIndex = 34;
            lbl_cam.Text = "Waiting";
            // 
            // lbl_meeting
            // 
            lbl_meeting.AutoSize = true;
            lbl_meeting.Location = new Point(198, 232);
            lbl_meeting.Name = "lbl_meeting";
            lbl_meeting.Size = new Size(60, 20);
            lbl_meeting.TabIndex = 35;
            lbl_meeting.Text = "Waiting";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 154);
            label2.Name = "label2";
            label2.Size = new Size(60, 20);
            label2.TabIndex = 36;
            label2.Text = "Camera";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 117);
            label3.Name = "label3";
            label3.Size = new Size(43, 20);
            label3.TabIndex = 37;
            label3.Text = "Mute";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 79);
            label4.Name = "label4";
            label4.Size = new Size(58, 20);
            label4.TabIndex = 38;
            label4.Text = "Activity";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 42);
            label5.Name = "label5";
            label5.Size = new Size(49, 20);
            label5.TabIndex = 39;
            label5.Text = "Status";
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Teams Notifier";
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // THFHA
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(607, 378);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lbl_meeting);
            Controls.Add(lbl_cam);
            Controls.Add(lbl_muted);
            Controls.Add(lbl_hand);
            Controls.Add(lbl_recording);
            Controls.Add(lbl_blurred);
            Controls.Add(pb_mute);
            Controls.Add(pb_Activity);
            Controls.Add(pb_Status);
            Controls.Add(lbl_camera);
            Controls.Add(lbl_mute);
            Controls.Add(lbl_activity);
            Controls.Add(lbl_status);
            Controls.Add(btn_stop);
            Controls.Add(btn_start);
            Controls.Add(statusStrip1);
            Controls.Add(label1);
            Controls.Add(lbx_modules);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "THFHA";
            Text = "THFHA";
            FormClosing += THFHA_FormClosing;
            Shown += THFHA_Shown;
            Resize += Form1_Resize;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)stateBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_Status).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_Activity).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_mute).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ListBox lbx_modules;
        private Label label1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem enableToolStripMenuItem;
        private ToolStripMenuItem disableToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statuslabel;
        private Button btn_start;
        private Button btn_stop;
        private Label lbl_status;
        private Label lbl_activity;
        private Label lbl_mute;
        private Label lbl_camera;
        private BindingSource stateBindingSource;
        private ToolStripMenuItem applicationLogsToolStripMenuItem;
        private ToolStripMenuItem teamsLogsToolStripMenuItem;
        private PictureBox pb_Status;
        private PictureBox pb_Activity;
        private PictureBox pb_mute;
        private Label lbl_blurred;
        private Label lbl_recording;
        private Label lbl_hand;
        private Label lbl_muted;
        private Label lbl_cam;
        private Label lbl_meeting;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private NotifyIcon notifyIcon1;
    }
}