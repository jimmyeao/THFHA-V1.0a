namespace THFHA_V1._0.Views
{
    partial class wledsettings
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
            this.listbox_wledlights = new System.Windows.Forms.ComboBox();
            this.btn_discover = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listbox_wledlights
            // 
            this.listbox_wledlights.FormattingEnabled = true;
            this.listbox_wledlights.Location = new System.Drawing.Point(135, 13);
            this.listbox_wledlights.Name = "listbox_wledlights";
            this.listbox_wledlights.Size = new System.Drawing.Size(245, 28);
            this.listbox_wledlights.TabIndex = 0;
            this.listbox_wledlights.SelectedIndexChanged += new System.EventHandler(this.listbox_wledlights_SelectedIndexChanged);
            // 
            // btn_discover
            // 
            this.btn_discover.Location = new System.Drawing.Point(12, 12);
            this.btn_discover.Name = "btn_discover";
            this.btn_discover.Size = new System.Drawing.Size(94, 29);
            this.btn_discover.TabIndex = 1;
            this.btn_discover.Text = "Discover";
            this.btn_discover.UseVisualStyleBackColor = true;
            this.btn_discover.Click += new System.EventHandler(this.btn_discover_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(12, 47);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(94, 29);
            this.btn_stop.TabIndex = 1;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 112);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(402, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 16);
            // 
            // wledsettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 134);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_discover);
            this.Controls.Add(this.listbox_wledlights);
            this.Name = "wledsettings";
            this.Text = "wledsettings";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox listbox_wledlights;
        private Button btn_discover;
        private Button btn_stop;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}