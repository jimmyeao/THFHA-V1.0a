namespace THFHA_V1._0.Views
{
    partial class huesettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(huesettings));
            this.label1 = new System.Windows.Forms.Label();
            this.tb_hueip = new System.Windows.Forms.TextBox();
            this.btn_link = new System.Windows.Forms.Button();
            this.cb_huelights = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hue Bridge IP";
            // 
            // tb_hueip
            // 
            this.tb_hueip.Location = new System.Drawing.Point(149, 28);
            this.tb_hueip.Name = "tb_hueip";
            this.tb_hueip.Size = new System.Drawing.Size(236, 27);
            this.tb_hueip.TabIndex = 1;
            this.tb_hueip.TextChanged += new System.EventHandler(this.tb_hueip_TextChanged);
            // 
            // btn_link
            // 
            this.btn_link.Location = new System.Drawing.Point(416, 29);
            this.btn_link.Name = "btn_link";
            this.btn_link.Size = new System.Drawing.Size(94, 29);
            this.btn_link.TabIndex = 2;
            this.btn_link.Text = "Link";
            this.btn_link.UseVisualStyleBackColor = true;
            this.btn_link.Click += new System.EventHandler(this.btn_link_Click);
            // 
            // cb_huelights
            // 
            this.cb_huelights.FormattingEnabled = true;
            this.cb_huelights.Location = new System.Drawing.Point(149, 75);
            this.cb_huelights.Name = "cb_huelights";
            this.cb_huelights.Size = new System.Drawing.Size(236, 28);
            this.cb_huelights.TabIndex = 3;
            this.cb_huelights.SelectedIndexChanged += new System.EventHandler(this.cb_huelights_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Hue Light";
            // 
            // huesettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(537, 191);
            this.Controls.Add(this.cb_huelights);
            this.Controls.Add(this.btn_link);
            this.Controls.Add(this.tb_hueip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "huesettings";
            this.Text = "huesettings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.huesettings_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox tb_hueip;
        private Button btn_link;
        private ComboBox cb_huelights;
        private Label label2;
    }
}