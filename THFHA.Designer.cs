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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(THFHA));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.teamsLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbx_modules = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statuslabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.btn_start = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.lbl_status = new System.Windows.Forms.Label();
            this.lbl_activity = new System.Windows.Forms.Label();
            this.lbl_mute = new System.Windows.Forms.Label();
            this.lbl_camera = new System.Windows.Forms.Label();
            this.stateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pb_Status = new System.Windows.Forms.PictureBox();
            this.pb_Activity = new System.Windows.Forms.PictureBox();
            this.pb_mute = new System.Windows.Forms.PictureBox();
            this.lbl_blurred = new System.Windows.Forms.Label();
            this.lbl_recording = new System.Windows.Forms.Label();
            this.lbl_hand = new System.Windows.Forms.Label();
            this.lbl_muted = new System.Windows.Forms.Label();
            this.lbl_cam = new System.Windows.Forms.Label();
            this.lbl_meeting = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Activity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_mute)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(805, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(145, 26);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.applicationLogsToolStripMenuItem,
            this.teamsLogsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(204, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // applicationLogsToolStripMenuItem
            // 
            this.applicationLogsToolStripMenuItem.Name = "applicationLogsToolStripMenuItem";
            this.applicationLogsToolStripMenuItem.Size = new System.Drawing.Size(204, 26);
            this.applicationLogsToolStripMenuItem.Text = "Application Logs";
            this.applicationLogsToolStripMenuItem.Click += new System.EventHandler(this.applicationLogsToolStripMenuItem_Click);
            // 
            // teamsLogsToolStripMenuItem
            // 
            this.teamsLogsToolStripMenuItem.Name = "teamsLogsToolStripMenuItem";
            this.teamsLogsToolStripMenuItem.Size = new System.Drawing.Size(204, 26);
            this.teamsLogsToolStripMenuItem.Text = "Teams Logs";
            this.teamsLogsToolStripMenuItem.Click += new System.EventHandler(this.teamsLogsToolStripMenuItem_Click);
            // 
            // lbx_modules
            // 
            this.lbx_modules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbx_modules.ContextMenuStrip = this.contextMenuStrip1;
            this.lbx_modules.FormattingEnabled = true;
            this.lbx_modules.ItemHeight = 20;
            this.lbx_modules.Location = new System.Drawing.Point(602, 65);
            this.lbx_modules.Name = "lbx_modules";
            this.lbx_modules.Size = new System.Drawing.Size(191, 144);
            this.lbx_modules.TabIndex = 1;
            this.lbx_modules.DoubleClick += new System.EventHandler(this.lbx_modules_DoubleClick);
            this.lbx_modules.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbx_modules_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableToolStripMenuItem,
            this.disableToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(129, 52);
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(128, 24);
            this.enableToolStripMenuItem.Text = "Enable";
            this.enableToolStripMenuItem.Click += new System.EventHandler(this.enableModuleToolStripMenuItem_Click);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(128, 24);
            this.disableToolStripMenuItem.Text = "Disable";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableModuleToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(602, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Available Modules";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statuslabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 316);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(805, 26);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statuslabel
            // 
            this.statuslabel.Name = "statuslabel";
            this.statuslabel.Size = new System.Drawing.Size(50, 20);
            this.statuslabel.Text = "Ready";
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(234, 238);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(94, 29);
            this.btn_start.TabIndex = 4;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(234, 273);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(94, 29);
            this.btn_stop.TabIndex = 5;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Location = new System.Drawing.Point(40, 79);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(50, 20);
            this.lbl_status.TabIndex = 6;
            this.lbl_status.Text = "label2";
            // 
            // lbl_activity
            // 
            this.lbl_activity.AutoSize = true;
            this.lbl_activity.Location = new System.Drawing.Point(40, 116);
            this.lbl_activity.Name = "lbl_activity";
            this.lbl_activity.Size = new System.Drawing.Size(50, 20);
            this.lbl_activity.TabIndex = 7;
            this.lbl_activity.Text = "label3";
            // 
            // lbl_mute
            // 
            this.lbl_mute.AutoSize = true;
            this.lbl_mute.Location = new System.Drawing.Point(40, 153);
            this.lbl_mute.Name = "lbl_mute";
            this.lbl_mute.Size = new System.Drawing.Size(50, 20);
            this.lbl_mute.TabIndex = 8;
            this.lbl_mute.Text = "label4";
            // 
            // lbl_camera
            // 
            this.lbl_camera.AutoSize = true;
            this.lbl_camera.Location = new System.Drawing.Point(40, 190);
            this.lbl_camera.Name = "lbl_camera";
            this.lbl_camera.Size = new System.Drawing.Size(50, 20);
            this.lbl_camera.TabIndex = 9;
            this.lbl_camera.Text = "label5";
            // 
            // stateBindingSource
            // 
            this.stateBindingSource.DataSource = typeof(THFHA_V1._0.Model.State);
            // 
            // pb_Status
            // 
            this.pb_Status.BackgroundImage = global::THFHA_V1._0.Resource1.available;
            this.pb_Status.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb_Status.Location = new System.Drawing.Point(15, 238);
            this.pb_Status.Name = "pb_Status";
            this.pb_Status.Size = new System.Drawing.Size(50, 50);
            this.pb_Status.TabIndex = 25;
            this.pb_Status.TabStop = false;
            // 
            // pb_Activity
            // 
            this.pb_Activity.BackgroundImage = global::THFHA_V1._0.Resource1.notinacall;
            this.pb_Activity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb_Activity.Location = new System.Drawing.Point(85, 238);
            this.pb_Activity.Name = "pb_Activity";
            this.pb_Activity.Size = new System.Drawing.Size(50, 50);
            this.pb_Activity.TabIndex = 26;
            this.pb_Activity.TabStop = false;
            // 
            // pb_mute
            // 
            this.pb_mute.BackgroundImage = global::THFHA_V1._0.Resource1.mic_icon;
            this.pb_mute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb_mute.Location = new System.Drawing.Point(156, 238);
            this.pb_mute.Name = "pb_mute";
            this.pb_mute.Size = new System.Drawing.Size(50, 50);
            this.pb_mute.TabIndex = 28;
            this.pb_mute.TabStop = false;
            // 
            // lbl_blurred
            // 
            this.lbl_blurred.AutoSize = true;
            this.lbl_blurred.Location = new System.Drawing.Point(359, 79);
            this.lbl_blurred.Name = "lbl_blurred";
            this.lbl_blurred.Size = new System.Drawing.Size(50, 20);
            this.lbl_blurred.TabIndex = 30;
            this.lbl_blurred.Text = "label2";

            // 
            // lbl_recording
            // 
            this.lbl_recording.AutoSize = true;
            this.lbl_recording.Location = new System.Drawing.Point(359, 116);
            this.lbl_recording.Name = "lbl_recording";
            this.lbl_recording.Size = new System.Drawing.Size(50, 20);
            this.lbl_recording.TabIndex = 31;
            this.lbl_recording.Text = "label2";
            // 
            // lbl_hand
            // 
            this.lbl_hand.AutoSize = true;
            this.lbl_hand.Location = new System.Drawing.Point(359, 153);
            this.lbl_hand.Name = "lbl_hand";
            this.lbl_hand.Size = new System.Drawing.Size(50, 20);
            this.lbl_hand.TabIndex = 32;
            this.lbl_hand.Text = "label3";
            // 
            // lbl_muted
            // 
            this.lbl_muted.AutoSize = true;
            this.lbl_muted.Location = new System.Drawing.Point(359, 190);
            this.lbl_muted.Name = "lbl_muted";
            this.lbl_muted.Size = new System.Drawing.Size(50, 20);
            this.lbl_muted.TabIndex = 33;
            this.lbl_muted.Text = "label4";
            // 
            // lbl_cam
            // 
            this.lbl_cam.AutoSize = true;
            this.lbl_cam.Location = new System.Drawing.Point(359, 227);
            this.lbl_cam.Name = "lbl_cam";
            this.lbl_cam.Size = new System.Drawing.Size(50, 20);
            this.lbl_cam.TabIndex = 34;
            this.lbl_cam.Text = "label5";
            // 
            // lbl_meeting
            // 
            this.lbl_meeting.AutoSize = true;
            this.lbl_meeting.Location = new System.Drawing.Point(359, 268);
            this.lbl_meeting.Name = "lbl_meeting";
            this.lbl_meeting.Size = new System.Drawing.Size(50, 20);
            this.lbl_meeting.TabIndex = 35;
            this.lbl_meeting.Text = "label6";
            // 
            // THFHA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(805, 342);
            this.Controls.Add(this.lbl_meeting);
            this.Controls.Add(this.lbl_cam);
            this.Controls.Add(this.lbl_muted);
            this.Controls.Add(this.lbl_hand);
            this.Controls.Add(this.lbl_recording);
            this.Controls.Add(this.lbl_blurred);
            this.Controls.Add(this.pb_mute);
            this.Controls.Add(this.pb_Activity);
            this.Controls.Add(this.pb_Status);
            this.Controls.Add(this.lbl_camera);
            this.Controls.Add(this.lbl_mute);
            this.Controls.Add(this.lbl_activity);
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbx_modules);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "THFHA";
            this.Text = "THFHA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.THFHA_FormClosing);
            this.Shown += new System.EventHandler(this.THFHA_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Activity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_mute)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}