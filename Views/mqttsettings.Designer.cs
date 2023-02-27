namespace THFHA_V1._0.Views
{
    partial class mqttsettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mqttsettings));
            this.label1 = new System.Windows.Forms.Label();
            this.tb_mqttip = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_mqttuser = new System.Windows.Forms.TextBox();
            this.tb_mqttpass = new System.Windows.Forms.TextBox();
            this.tb_mqtttopic = new System.Windows.Forms.TextBox();
            this.btn_test = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "MQTT Broker IP";
            // 
            // tb_mqttip
            // 
            this.tb_mqttip.Location = new System.Drawing.Point(149, 22);
            this.tb_mqttip.Name = "tb_mqttip";
            this.tb_mqttip.Size = new System.Drawing.Size(209, 27);
            this.tb_mqttip.TabIndex = 1;
            this.tb_mqttip.TextChanged += new System.EventHandler(this.mqttip_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "MQTT User Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "MQTT Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "MQTT Topic";
            // 
            // tb_mqttuser
            // 
            this.tb_mqttuser.Location = new System.Drawing.Point(149, 56);
            this.tb_mqttuser.Name = "tb_mqttuser";
            this.tb_mqttuser.Size = new System.Drawing.Size(209, 27);
            this.tb_mqttuser.TabIndex = 1;
            this.tb_mqttuser.TextChanged += new System.EventHandler(this.mqttuser_TextChanged);
            // 
            // tb_mqttpass
            // 
            this.tb_mqttpass.Location = new System.Drawing.Point(149, 90);
            this.tb_mqttpass.Name = "tb_mqttpass";
            this.tb_mqttpass.Size = new System.Drawing.Size(209, 27);
            this.tb_mqttpass.TabIndex = 1;
            this.tb_mqttpass.TextChanged += new System.EventHandler(this.mqttpass_TextChanged);
            // 
            // tb_mqtttopic
            // 
            this.tb_mqtttopic.Location = new System.Drawing.Point(149, 124);
            this.tb_mqtttopic.Name = "tb_mqtttopic";
            this.tb_mqtttopic.Size = new System.Drawing.Size(209, 27);
            this.tb_mqtttopic.TabIndex = 1;
            this.tb_mqtttopic.TextChanged += new System.EventHandler(this.mqtttopic_TextChanged);
            // 
            // btn_test
            // 
            this.btn_test.Location = new System.Drawing.Point(264, 171);
            this.btn_test.Name = "btn_test";
            this.btn_test.Size = new System.Drawing.Size(94, 29);
            this.btn_test.TabIndex = 2;
            this.btn_test.Text = "Test";
            this.btn_test.UseVisualStyleBackColor = true;
            this.btn_test.Click += new System.EventHandler(this.buttontest_Click);
            // 
            // mqttsettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 221);
            this.Controls.Add(this.btn_test);
            this.Controls.Add(this.tb_mqtttopic);
            this.Controls.Add(this.tb_mqttpass);
            this.Controls.Add(this.tb_mqttuser);
            this.Controls.Add(this.tb_mqttip);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mqttsettings";
            this.Text = "mqttsettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox tb_mqttip;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox tb_mqttuser;
        private TextBox tb_mqttpass;
        private TextBox tb_mqtttopic;
        private Button btn_test;
    }
}