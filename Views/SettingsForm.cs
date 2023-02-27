using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using THFHA_V1._0.Model;

namespace THFHA_V1._0.Views
{
    public partial class SettingsForm : Form
    {
        private List<IModule> modules;
        private Settings settings;
        public SettingsForm(List<IModule> modules)
        {
           

            InitializeComponent();
            this.settings = Settings.Instance;
            this.modules = modules;
            cb_runLogWatcherAtStart.Checked =  settings.RunLogWatcherAtStart;
            cb_usehue.Checked = settings.UseHue;
            cb_useha.Checked = settings.UseHA;
            cb_usemqtt.Checked = settings.UseMQTT;
            cb_usewled.Checked = settings.UseWLED;

            // Add code to initialize the settings form with the list of modules
        }

        private void bt_hasettings_Click(object sender, EventArgs e)
        {

        }

        private void bt_usehueClick(object sender, EventArgs e)
        {

        }

        private void bt_usemqtt_Click(object sender, EventArgs e)
        {

        }

        private void bt_usewled_Click(object sender, EventArgs e)
        {
           
        }

        private void cb_useha_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseHA = cb_useha.Checked;
            settings.Save();
        }

        private void cb_usehue_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseHue = cb_usehue.Checked;
            settings.Save();
        }

        private void cb_usemqtt_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseMQTT = cb_usemqtt.Checked;
            settings.Save();
        }

        private void cb_usewled_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseWLED = cb_usewled.Checked;
            settings.Save();
        }

        private void cb_runLogWatcherAtStart_CheckedChanged(object sender, EventArgs e)
        {

            settings.RunLogWatcherAtStart = cb_runLogWatcherAtStart.Checked;
            settings.Save();
        }

    }
}
