using System.Reflection;
using THFHA_V1._0.apis;
using THFHA_V1._0.Model;
using static THFHA_V1._0.Model.Settings;

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
            cb_hatchersettings.Checked = settings.UseHatcher;
            cb_usewled.Checked = settings.UseWLED;
            if (settings.Haurl == "" || settings.Hatoken == "")
            {
                cb_useha.Checked = false;
                cb_useha.Enabled = false;
            }
            else
            {
                cb_useha.Enabled = true;
            }

            // Subscribe to the SettingChanged event
            Settings.SettingChanged += SettingsForm_Settings_SettingChanged;
        }
        private void SettingsForm_Settings_SettingChanged(object sender, SettingChangedEventArgs e)
        {
            // Update the module status based on the changed setting
            HueModule hueModule = modules.Find(module => module.Name == "hue") as HueModule;
            if (hueModule != null && e.SettingName == nameof(Settings.UseHue))
            {
                hueModule.IsEnabled = settings.UseHue;
            }

            MqttModule mqttModule = modules.Find(module => module.Name == "mqtt") as MqttModule;
            if (mqttModule != null && e.SettingName == nameof(Settings.UseMQTT))
            {
                mqttModule.IsEnabled = settings.UseMQTT;
            }

            WledModule wledModule = modules.Find(module => module.Name == "wled") as WledModule;
            if (wledModule != null && e.SettingName == nameof(Settings.UseWLED))
            {
                wledModule.IsEnabled = settings.UseWLED;
            }

            HatcherModule hatcherModule = modules.Find(module => module.Name == "hatcher") as HatcherModule;
            if (hatcherModule != null && e.SettingName == nameof(Settings.RunLogWatcherAtStart))
            {
                hatcherModule.IsEnabled = settings.RunLogWatcherAtStart;
            }

            // Refresh the modules list in the THFHA form
            if (this.Owner is THFHA thfha)
            {
                thfha.PopulateModulesList();
            }
        }

        #region buttons

        private void bt_hasettings_Click(object sender, EventArgs e)
        {
            hasettings hasettings = new hasettings();

            // Show the SettingsForm
            hasettings.ShowDialog();
            if (settings.Haurl == "" || settings.Hatoken == "")
            {
                cb_useha.Checked = false;
                cb_useha.Enabled = false;
            }
            else
            {

                cb_useha.Enabled = true;
            }
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
        private void btn_hatchersettings_Click(object sender, EventArgs e)
        {
            hatchersettings hatchersettings = new hatchersettings();
            hatchersettings.ShowDialog();

        }
        #endregion
        #region checkboxes
        private void cb_useha_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseHA = cb_useha.Checked;
            settings.Save();
            HomeassistantModule haModule = modules.Find(module => module.Name == "Homeassistant") as HomeassistantModule;
            if (haModule != null)
            {
                haModule.IsEnabled = cb_useha.Checked;
            }

        }

        private void cb_usehue_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseHue = cb_usehue.Checked;
            settings.Save();
            HueModule hueModule = modules.Find(module => module.Name == "hue") as HueModule;
            if (hueModule != null)
            {
                hueModule.IsEnabled = cb_usehue.Checked;
            }
        }

        private void cb_usemqtt_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseMQTT = cb_usemqtt.Checked;
            settings.Save();
            MqttModule mqttModule = modules.Find(module => module.Name == "mqtt") as MqttModule;
            if (mqttModule != null)
            {
                mqttModule.IsEnabled = cb_usemqtt.Checked;
            }
        }

        private void cb_usewled_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseWLED = cb_usewled.Checked;
            settings.Save();
            WledModule wledModule = modules.Find(module => module.Name == "wled") as WledModule;
            if (wledModule != null)
            {
                wledModule.IsEnabled = cb_usewled.Checked;
            }
        }

        private void cb_runLogWatcherAtStart_CheckedChanged(object sender, EventArgs e)
        {

            settings.RunLogWatcherAtStart = cb_runLogWatcherAtStart.Checked;
            settings.Save();

        }



        private void cb_hatchersettings_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseHatcher = cb_hatchersettings.Checked;
            settings.Save();
            HatcherModule hatcherModule = modules.Find(module => module.Name == "Hatcher") as HatcherModule;
            if (hatcherModule != null)
            {
                hatcherModule.IsEnabled = cb_hatchersettings.Checked;
            }
        }
        #endregion
    }
}
