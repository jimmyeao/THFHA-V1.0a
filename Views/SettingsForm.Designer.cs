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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            cb_useha = new CheckBox();
            cb_usehue = new CheckBox();
            cb_usemqtt = new CheckBox();
            cb_usewled = new CheckBox();
            bt_hasettings = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            cb_runLogWatcherAtStart = new CheckBox();
            btn_hatchersettings = new Button();
            cb_hatchersettings = new CheckBox();
            textBox1 = new TextBox();
            label1 = new Label();
            cb_min = new CheckBox();
            SuspendLayout();
            // 
            // cb_useha
            // 
            cb_useha.AutoSize = true;
            cb_useha.Location = new Point(40, 49);
            cb_useha.Name = "cb_useha";
            cb_useha.Size = new Size(169, 24);
            cb_useha.TabIndex = 0;
            cb_useha.Text = "Use Home Assistant?";
            cb_useha.UseVisualStyleBackColor = true;
            cb_useha.CheckedChanged += cb_useha_CheckedChanged;
            // 
            // cb_usehue
            // 
            cb_usehue.AutoSize = true;
            cb_usehue.Location = new Point(40, 81);
            cb_usehue.Name = "cb_usehue";
            cb_usehue.Size = new Size(140, 24);
            cb_usehue.TabIndex = 1;
            cb_usehue.Text = "Use Philips Hue?";
            cb_usehue.UseVisualStyleBackColor = true;
            cb_usehue.CheckedChanged += cb_usehue_CheckedChanged;
            // 
            // cb_usemqtt
            // 
            cb_usemqtt.AutoSize = true;
            cb_usemqtt.Location = new Point(40, 113);
            cb_usemqtt.Name = "cb_usemqtt";
            cb_usemqtt.Size = new Size(105, 24);
            cb_usemqtt.TabIndex = 2;
            cb_usemqtt.Text = "Use MQTT?";
            cb_usemqtt.UseVisualStyleBackColor = true;
            cb_usemqtt.CheckedChanged += cb_usemqtt_CheckedChanged;
            // 
            // cb_usewled
            // 
            cb_usewled.AutoSize = true;
            cb_usewled.Location = new Point(40, 145);
            cb_usewled.Name = "cb_usewled";
            cb_usewled.Size = new Size(106, 24);
            cb_usewled.TabIndex = 3;
            cb_usewled.Text = "Use WLED?";
            cb_usewled.UseVisualStyleBackColor = true;
            cb_usewled.CheckedChanged += cb_usewled_CheckedChanged;
            // 
            // bt_hasettings
            // 
            bt_hasettings.Location = new Point(215, 44);
            bt_hasettings.Name = "bt_hasettings";
            bt_hasettings.Size = new Size(102, 32);
            bt_hasettings.TabIndex = 4;
            bt_hasettings.Text = "Settings";
            bt_hasettings.UseVisualStyleBackColor = true;
            bt_hasettings.Click += bt_hasettings_Click;
            // 
            // button1
            // 
            button1.Location = new Point(215, 76);
            button1.Name = "button1";
            button1.Size = new Size(102, 32);
            button1.TabIndex = 4;
            button1.Text = "Settings";
            button1.UseVisualStyleBackColor = true;
            button1.Click += bt_huesettings_Click;
            // 
            // button2
            // 
            button2.Location = new Point(215, 108);
            button2.Name = "button2";
            button2.Size = new Size(102, 32);
            button2.TabIndex = 4;
            button2.Text = "Settings";
            button2.UseVisualStyleBackColor = true;
            button2.Click += bt_mqttsettings_Click;
            // 
            // button3
            // 
            button3.Location = new Point(215, 140);
            button3.Name = "button3";
            button3.Size = new Size(102, 32);
            button3.TabIndex = 4;
            button3.Text = "Settings";
            button3.UseVisualStyleBackColor = true;
            button3.Click += bt_usewled_Click;
            // 
            // cb_runLogWatcherAtStart
            // 
            cb_runLogWatcherAtStart.AutoSize = true;
            cb_runLogWatcherAtStart.Location = new Point(40, 260);
            cb_runLogWatcherAtStart.Name = "cb_runLogWatcherAtStart";
            cb_runLogWatcherAtStart.Size = new Size(136, 24);
            cb_runLogWatcherAtStart.TabIndex = 3;
            cb_runLogWatcherAtStart.Text = "Run At StartUp?";
            cb_runLogWatcherAtStart.UseVisualStyleBackColor = true;
            cb_runLogWatcherAtStart.CheckedChanged += cb_runLogWatcherAtStart_CheckedChanged;
            // 
            // btn_hatchersettings
            // 
            btn_hatchersettings.Location = new Point(215, 172);
            btn_hatchersettings.Name = "btn_hatchersettings";
            btn_hatchersettings.Size = new Size(102, 32);
            btn_hatchersettings.TabIndex = 5;
            btn_hatchersettings.Text = "Settings";
            btn_hatchersettings.UseVisualStyleBackColor = true;
            btn_hatchersettings.Click += btn_hatchersettings_Click;
            // 
            // cb_hatchersettings
            // 
            cb_hatchersettings.AutoSize = true;
            cb_hatchersettings.Location = new Point(40, 177);
            cb_hatchersettings.Name = "cb_hatchersettings";
            cb_hatchersettings.Size = new Size(118, 24);
            cb_hatchersettings.TabIndex = 6;
            cb_hatchersettings.Text = "Use Hatcher?";
            cb_hatchersettings.UseVisualStyleBackColor = true;
            cb_hatchersettings.CheckedChanged += cb_hatchersettings_CheckedChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(149, 222);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(223, 27);
            textBox1.TabIndex = 7;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(40, 225);
            label1.Name = "label1";
            label1.Size = new Size(77, 20);
            label1.TabIndex = 8;
            label1.Text = "Teams API";
            // 
            // cb_min
            // 
            cb_min.AutoSize = true;
            cb_min.Location = new Point(184, 260);
            cb_min.Name = "cb_min";
            cb_min.Size = new Size(137, 24);
            cb_min.TabIndex = 9;
            cb_min.Text = "Run Minimized?";
            cb_min.UseVisualStyleBackColor = true;
            cb_min.CheckedChanged += cb_min_CheckedChanged;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(392, 296);
            Controls.Add(cb_min);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(cb_hatchersettings);
            Controls.Add(btn_hatchersettings);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(bt_hasettings);
            Controls.Add(cb_runLogWatcherAtStart);
            Controls.Add(cb_usewled);
            Controls.Add(cb_usemqtt);
            Controls.Add(cb_usehue);
            Controls.Add(cb_useha);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SettingsForm";
            Text = "SettingsForm";
            Load += SettingsForm_Load;
            ResumeLayout(false);
            PerformLayout();
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
        private TextBox textBox1;
        private Label label1;
        private CheckBox cb_min;
    }
}