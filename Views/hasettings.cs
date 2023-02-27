using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THFHA_V1._0.Views
{
    public partial class hasettings : Form
    {
        public hasettings()
        {
            InitializeComponent();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                richTextBox1.Text += (string)Clipboard.GetData("Text");
                e.Handled = true;
            }
        }
    }
}
