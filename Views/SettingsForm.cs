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
            cb_runLogWatcherAtStart.Checked = settings.RunLogWatcherAtStart;
            cb_usehue.Checked = settings.UseHue;
            cb_useha.Checked = settings.UseHA;
            cb_usemqtt.Checked = settings.UseMQTT;
            cb_usewled.Checked = settings.UseWLED;

            // Add code to initialize the settings form with the list of modules
        }

        private void bt_hasettings_Click(object sender, EventArgs e)
        {
            hasettings hasettings = new hasettings();

            // Show the SettingsForm
            hasettings.ShowDialog();
        }

        private void bt_huesettings_Click(object sender, EventArgs e)
        {
            huesettings huesettings = new huesettings();

            // Show the SettingsForm
            huesettings.ShowDialog();
        }

        private void bt_mqttsettings_Click(object sender, EventArgs e)
        {
            mqttsettings mqttsettings = new mqttsettings();

            // Show the SettingsForm
            mqttsettings.ShowDialog();
        }

        private void bt_usewled_Click(object sender, EventArgs e)
        {
            wledsettings wledsettings = new wledsettings();

            // Show the SettingsForm
            wledsettings.ShowDialog();
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
