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
            this.SuspendLayout();
            // 
            // listbox_wledlights
            // 
            this.listbox_wledlights.FormattingEnabled = true;
            this.listbox_wledlights.Location = new System.Drawing.Point(163, 51);
            this.listbox_wledlights.Name = "listbox_wledlights";
            this.listbox_wledlights.Size = new System.Drawing.Size(245, 28);
            this.listbox_wledlights.TabIndex = 0;
            // 
            // btn_discover
            // 
            this.btn_discover.Location = new System.Drawing.Point(40, 50);
            this.btn_discover.Name = "btn_discover";
            this.btn_discover.Size = new System.Drawing.Size(94, 29);
            this.btn_discover.TabIndex = 1;
            this.btn_discover.Text = "Discover";
            this.btn_discover.UseVisualStyleBackColor = true;
            this.btn_discover.Click += new System.EventHandler(this.btn_discover_Click);
            // 
            // wledsettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 151);
            this.Controls.Add(this.btn_discover);
            this.Controls.Add(this.listbox_wledlights);
            this.Name = "wledsettings";
            this.Text = "wledsettings";
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox listbox_wledlights;
        private Button btn_discover;
    }
}