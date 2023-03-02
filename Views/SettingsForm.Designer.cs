namespace THFHA_V1._0.Views
{
    partial class SettingsForm
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
            this.cb_useha = new System.Windows.Forms.CheckBox();
            this.cb_usehue = new System.Windows.Forms.CheckBox();
            this.cb_usemqtt = new System.Windows.Forms.CheckBox();
            this.cb_usewled = new System.Windows.Forms.CheckBox();
            this.bt_hasettings = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.cb_runLogWatcherAtStart = new System.Windows.Forms.CheckBox();
            this.btn_hatchersettings = new System.Windows.Forms.Button();
            this.cb_hatchersettings = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cb_useha
            // 
            this.cb_useha.AutoSize = true;
            this.cb_useha.Location = new System.Drawing.Point(40, 49);
            this.cb_useha.Name = "cb_useha";
            this.cb_useha.Size = new System.Drawing.Size(169, 24);
            this.cb_useha.TabIndex = 0;
            this.cb_useha.Text = "Use Home Assistant?";
            this.cb_useha.UseVisualStyleBackColor = true;
            this.cb_useha.CheckedChanged += new System.EventHandler(this.cb_useha_CheckedChanged);
            // 
            // cb_usehue
            // 
            this.cb_usehue.AutoSize = true;
            this.cb_usehue.Location = new System.Drawing.Point(40, 81);
            this.cb_usehue.Name = "cb_usehue";
            this.cb_usehue.Size = new System.Drawing.Size(140, 24);
            this.cb_usehue.TabIndex = 1;
            this.cb_usehue.Text = "Use Philips Hue?";
            this.cb_usehue.UseVisualStyleBackColor = true;
            this.cb_usehue.CheckedChanged += new System.EventHandler(this.cb_usehue_CheckedChanged);
            // 
            // cb_usemqtt
            // 
            this.cb_usemqtt.AutoSize = true;
            this.cb_usemqtt.Location = new System.Drawing.Point(40, 113);
            this.cb_usemqtt.Name = "cb_usemqtt";
            this.cb_usemqtt.Size = new System.Drawing.Size(105, 24);
            this.cb_usemqtt.TabIndex = 2;
            this.cb_usemqtt.Text = "Use MQTT?";
            this.cb_usemqtt.UseVisualStyleBackColor = true;
            this.cb_usemqtt.CheckedChanged += new System.EventHandler(this.cb_usemqtt_CheckedChanged);
            // 
            // cb_usewled
            // 
            this.cb_usewled.AutoSize = true;
            this.cb_usewled.Location = new System.Drawing.Point(40, 145);
            this.cb_usewled.Name = "cb_usewled";
            this.cb_usewled.Size = new System.Drawing.Size(106, 24);
            this.cb_usewled.TabIndex = 3;
            this.cb_usewled.Text = "Use WLED?";
            this.cb_usewled.UseVisualStyleBackColor = true;
            this.cb_usewled.CheckedChanged += new System.EventHandler(this.cb_usewled_CheckedChanged);
            // 
            // bt_hasettings
            // 
            this.bt_hasettings.Location = new System.Drawing.Point(215, 44);
            this.bt_hasettings.Name = "bt_hasettings";
            this.bt_hasettings.Size = new System.Drawing.Size(102, 32);
            this.bt_hasettings.TabIndex = 4;
            this.bt_hasettings.Text = "Settings";
            this.bt_hasettings.UseVisualStyleBackColor = true;
            this.bt_hasettings.Click += new System.EventHandler(this.bt_hasettings_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(215, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 32);
            this.button1.TabIndex = 4;
            this.button1.Text = "Settings";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.bt_huesettings_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(215, 108);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 32);
            this.button2.TabIndex = 4;
            this.button2.Text = "Settings";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.bt_mqttsettings_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(215, 140);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(102, 32);
            this.button3.TabIndex = 4;
            this.button3.Text = "Settings";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.bt_usewled_Click);
            // 
            // cb_runLogWatcherAtStart
            // 
            this.cb_runLogWatcherAtStart.AutoSize = true;
            this.cb_runLogWatcherAtStart.Location = new System.Drawing.Point(40, 215);
            this.cb_runLogWatcherAtStart.Name = "cb_runLogWatcherAtStart";
            this.cb_runLogWatcherAtStart.Size = new System.Drawing.Size(136, 24);
            this.cb_runLogWatcherAtStart.TabIndex = 3;
            this.cb_runLogWatcherAtStart.Text = "Run At StartUp?";
            this.cb_runLogWatcherAtStart.UseVisualStyleBackColor = true;
            this.cb_runLogWatcherAtStart.CheckedChanged += new System.EventHandler(this.cb_runLogWatcherAtStart_CheckedChanged);
            // 
            // btn_hatchersettings
            // 
            this.btn_hatchersettings.Location = new System.Drawing.Point(215, 172);
            this.btn_hatchersettings.Name = "btn_hatchersettings";
            this.btn_hatchersettings.Size = new System.Drawing.Size(102, 32);
            this.btn_hatchersettings.TabIndex = 5;
            this.btn_hatchersettings.Text = "Settings";
            this.btn_hatchersettings.UseVisualStyleBackColor = true;
            this.btn_hatchersettings.Click += new System.EventHandler(this.btn_hatchersettings_Click);
            // 
            // cb_hatchersettings
            // 
            this.cb_hatchersettings.AutoSize = true;
            this.cb_hatchersettings.Location = new System.Drawing.Point(40, 177);
            this.cb_hatchersettings.Name = "cb_hatchersettings";
            this.cb_hatchersettings.Size = new System.Drawing.Size(118, 24);
            this.cb_hatchersettings.TabIndex = 6;
            this.cb_hatchersettings.Text = "Use Hatcher?";
            this.cb_hatchersettings.UseVisualStyleBackColor = true;
            this.cb_hatchersettings.CheckedChanged += new System.EventHandler(this.cb_hatchersettings_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(392, 251);
            this.Controls.Add(this.cb_hatchersettings);
            this.Controls.Add(this.btn_hatchersettings);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bt_hasettings);
            this.Controls.Add(this.cb_runLogWatcherAtStart);
            this.Controls.Add(this.cb_usewled);
            this.Controls.Add(this.cb_usemqtt);
            this.Controls.Add(this.cb_usehue);
            this.Controls.Add(this.cb_useha);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox cb_useha;
        private CheckBox cb_usehue;
        private CheckBox cb_usemqtt;
        private CheckBox cb_usewled;
        private Button bt_hasettings;
        private Button button1;
        private Button button2;
        private Button button3;
        private CheckBox cb_runLogWatcherAtStart;
        private Button btn_hatchersettings;
        private CheckBox cb_hatchersettings;
    }
}