using THFHA_V1._0.apis;
using THFHA_V1._0.Model;
using static THFHA_V1._0.Model.Settings;

namespace THFHA_V1._0.Views
{
    public partial class SettingsForm : Form
    {
        #region Private Fields

        private List<IModule> modules;
        private Settings settings;

        #endregion Private Fields

        #region Public Constructors

        public SettingsForm(List<IModule> modules)
        {
            InitializeComponent();
            settings = Settings.Instance;
            this.modules = modules;
            cb_runLogWatcherAtStart.Checked = settings.RunLogWatcherAtStart;
            cb_usehue.Checked = settings.UseHue;
            cb_useha.Checked = settings.UseHA;
            cb_usemqtt.Checked = settings.UseMQTT;
            cb_hatchersettings.Checked = settings.UseHatcher;
            cb_usewled.Checked = settings.UseWLED;
            if (settings.TeamsApi != "")
            {
                textBox1.Text = settings.TeamsApi;
            }

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

        #endregion Public Constructors

        #region Private Methods

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            if (!settings.IsHueModuleSettingsValid)
            {
                cb_usehue.Enabled = false; cb_usehue.Checked = false;
            }
            else
            {
                cb_usehue.Enabled = true;
            }
            if (!settings.IsHomeassistantModuleSettingsValid)
            {
                cb_useha.Enabled = false; cb_useha.Checked = false;
            }
            else
            {
                cb_useha.Enabled = true;
            }
            if (!settings.IsHatcherModuleSettingsValid)
            {
                cb_hatchersettings.Enabled = false; cb_hatchersettings.Checked = false;
            }
            else
            {
                cb_hatchersettings.Enabled = true;
            }
            if (!settings.IsMqttModuleSettingsValid)
            {
                cb_usemqtt.Enabled = false; cb_usemqtt.Checked = false;
            }
            else
            {
                cb_usemqtt.Enabled = true;
            }
            if (!settings.IsWledModuleSettingsValid)
            {
                cb_usewled.Enabled = false; cb_usewled.Checked = false;
            }
            else
            {
                cb_usewled.Enabled = true;
            }
        }

        private void SettingsForm_Settings_SettingChanged(object sender, SettingChangedEventArgs e)
        {
            // Update the module status based on the changed setting
            HueModule hueModule = modules.Find(module => module.Name == "Hue") as HueModule;
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
            if (Owner is THFHA thfha)
            {
                thfha.PopulateModulesList();
            }
        }

        #endregion Private Methods

        #region buttons

        private void bt_hasettings_Click(object sender, EventArgs e)
        {
            Homeassistantsettings hasettings = new Homeassistantsettings();

            // Show the SettingsForm
            hasettings.ShowDialog();
            if (settings.IsHomeassistantModuleSettingsValid)
            {
                cb_useha.Enabled = true;
            }
            else
            {
                cb_useha.Enabled = false;
            }
        }

        private void bt_huesettings_Click(object sender, EventArgs e)
        {
            huesettings huesettings = new huesettings();

            // Show the SettingsForm
            huesettings.ShowDialog();
            if (settings.IsHueModuleSettingsValid)
            {
                cb_usehue.Enabled = true;
            }
            else
            {
                cb_usehue.Enabled = false;
            }
        }

        private void bt_mqttsettings_Click(object sender, EventArgs e)
        {
            mqttsettings mqttsettings = new mqttsettings();

            // Show the SettingsForm
            mqttsettings.ShowDialog();
            if (settings.IsMqttModuleSettingsValid)
            {
                cb_usemqtt.Enabled = true;
            }
            else
            {
                cb_usemqtt.Enabled = false;
            }
        }

        private void bt_usewled_Click(object sender, EventArgs e)
        {
            wledsettings wledsettings = new wledsettings();

            // Show the SettingsForm
            wledsettings.ShowDialog();
            if (settings.IsWledModuleSettingsValid)
            {
                cb_usewled.Enabled = true;
            }
            else
            {
                cb_usewled.Enabled = false;
            }
        }

        private void btn_hatchersettings_Click(object sender, EventArgs e)
        {
            hatchersettings hatchersettings = new hatchersettings();
            hatchersettings.ShowDialog();
            if (settings.IsHatcherModuleSettingsValid)
            {
                cb_hatchersettings.Enabled = true;
            }
            else
            {
                cb_hatchersettings.Enabled = false;
            }
        }

        #endregion buttons

        #region checkboxes

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

        private void cb_runLogWatcherAtStart_CheckedChanged(object sender, EventArgs e)
        {
            settings.RunLogWatcherAtStart = cb_runLogWatcherAtStart.Checked;
            settings.Save();
        }

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

        #endregion checkboxes

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            settings.TeamsApi = textBox1.Text;
            settings.Save();
        }
    }
}