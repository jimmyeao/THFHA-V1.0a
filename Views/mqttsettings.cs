using System.Text.RegularExpressions;
using System.Windows.Forms;
using THFHA_V1._0.Model;

namespace THFHA_V1._0.Views
{
    public partial class mqttsettings : Form
    {
        private Settings settings;
        public mqttsettings()
        {
            InitializeComponent();
            this.settings = Settings.Instance;
            tb_mqttip.Text = settings.Mqttip;
            tb_mqttuser.Text = settings.Mqttusername;
            tb_mqttpass.Text = settings.Mqttpassword;
            tb_mqtttopic.Text = settings.Mqtttopic;
        }

        private void buttontest_Click(object sender, EventArgs e)
        {
            // lets validate its at least a valid IP...
            if (Regex.IsMatch(tb_mqttip.Text, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
            {
                //valid ip continue
                
            }
            else
            {
                var title = "IP Error";
                var message = "Please Enter a valid ip address";
                MessageBox.Show(message, title);
            }
        }

        private void mqttip_TextChanged(object sender, EventArgs e)
        {

            settings.Mqttip = tb_mqttip.Text;
            settings.Save();
        }

        private void mqttuser_TextChanged(object sender, EventArgs e)
        {
            settings.Mqttusername = tb_mqttuser.Text;
            settings.Save();
        }

        private void mqttpass_TextChanged(object sender, EventArgs e)
        {
            settings.Mqttpassword = tb_mqttpass.Text;
            settings.Save();
        }

        private void mqtttopic_TextChanged(object sender, EventArgs e)
        {
            settings.Mqtttopic= tb_mqtttopic.Text;
            settings.Save();
        }
    }
}
