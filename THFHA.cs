using System.Diagnostics;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
using System.IO;
using Serilog;



namespace THFHA_V1._0
{
    public partial class THFHA : Form
    {
        private List<IModule> modules;
        private LogWatcher logWatcher;
        public THFHA(List<IModule> modules)
        {
            InitializeComponent();

            this.modules = modules;

            // Initialize the IsEnabled property of each module based on the value stored in the Settings singleton
            foreach (IModule module in this.modules)
            {
                switch (module.Name.ToLower())
                {
                    case "hue":
                        module.IsEnabled = Settings.Instance.UseHue;
                        break;
                    case "homeassistant":
                        module.IsEnabled = Settings.Instance.UseHA;
                        break;
                    case "mqtt":
                        module.IsEnabled = Settings.Instance.UseMQTT;
                        break;
                    case "wled":
                        module.IsEnabled = Settings.Instance.UseWLED;
                        break;
                    case "hatcher":
                        module.IsEnabled = Settings.Instance.UseHatcher;
                        break;
                }
            }

            PopulateModulesList();
            Settings.SettingChanged += Settings_SettingChanged; // Subscribe to the SettingChanged event
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open the settings form
            // Create an instance of the SettingsForm
            SettingsForm settingsForm = new SettingsForm(modules);

            // Show the SettingsForm
            settingsForm.ShowDialog();
            PopulateModulesList();

        }
        private void Settings_SettingChanged(object sender, THFHA_V1._0.Model.Settings.SettingChangedEventArgs e)
        {
            // Update the module status based on the changed setting
            foreach (IModule module in modules)
            {
                switch (module.Name.ToLower())
                {
                    case "hue":
                        module.IsEnabled = Settings.Instance.UseHue;
                        break;
                    case "ha":
                        module.IsEnabled = Settings.Instance.UseHA;
                        break;
                    case "mqtt":
                        module.IsEnabled = Settings.Instance.UseMQTT;
                        break;
                    case "wled":
                        module.IsEnabled = Settings.Instance.UseWLED;
                        break;
                    case "hatcher":
                        module.IsEnabled = Settings.Instance.UseHatcher;
                        break;
                }
            }

            // Refresh the modules list in the THFHA form
            if (this.Owner is THFHA thfha)
            {
                thfha.PopulateModulesList();
            }
        }




        public void PopulateModulesList()
        {
            lbx_modules.Items.Clear();
            foreach (IModule module in modules)
            {
                lbx_modules.Items.Add(module.Name + " [" + (module.IsEnabled ? "Enabled" : "Disabled") + "]");
            }
        }




        private async Task StartLogWatcher(string filePath)
        {
            logWatcher = new LogWatcher(filePath, new State());

            await logWatcher.Start();
        }


        private async Task StopLogWatcher()
        {
            await logWatcher.Stop();

        }

        private void lbx_modules_DoubleClick(object sender, EventArgs e)
        {
            // Get the selected module
            IModule selectedModule = modules[lbx_modules.SelectedIndex];

            // Get the module's settings form
            Form settingsForm = selectedModule.GetSettingsForm();

            // Show the settings form
            settingsForm.ShowDialog();
        }

        private void THFHA_MouseDown(object sender, MouseEventArgs e)

        {
            if (e.Button == MouseButtons.Right)
            {
                // Get the index of the item that was clicked
                int index = lbx_modules.IndexFromPoint(e.Location);

                if (index >= 0 && index < lbx_modules.Items.Count)
                {
                    // Select the item that was clicked
                    lbx_modules.SelectedIndex = index;

                    // Show the context menu at the mouse position
                    contextMenuStrip1.Show(lbx_modules, e.Location);
                }
            }
        }

        private void enableModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbx_modules.SelectedIndex >= 0)
            {
                IModule selectedModule = modules[lbx_modules.SelectedIndex];
                selectedModule.IsEnabled = true;
                selectedModule.UpdateSettings(selectedModule.IsEnabled); // Update the settings based on the new state of the module
                switch (selectedModule.Name.ToLower())
                {
                    case "hue":
                        Settings.Instance.UseHue = selectedModule.IsEnabled;
                        break;
                    case "homeassistant":
                        Settings.Instance.UseHA = selectedModule.IsEnabled;
                        break;
                    case "mqtt":
                        Settings.Instance.UseMQTT = selectedModule.IsEnabled;
                        break;
                    case "wled":
                        Settings.Instance.UseWLED = selectedModule.IsEnabled;
                        break;
                    case "hatcher":
                        Settings.Instance.UseHatcher = selectedModule.IsEnabled;
                        break;
                }
                PopulateModulesList(); // Refresh the list to update the module status
            }
        }

        private void disableModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbx_modules.SelectedIndex >= 0)
            {
                IModule selectedModule = modules[lbx_modules.SelectedIndex];
                selectedModule.IsEnabled = false;
                selectedModule.UpdateSettings(selectedModule.IsEnabled); // Update the settings based on the new state of the module
                switch (selectedModule.Name.ToLower())
                {
                    case "hue":
                        Settings.Instance.UseHue = selectedModule.IsEnabled;
                        break;
                    case "homeassistant":
                        Settings.Instance.UseHA = selectedModule.IsEnabled;
                        break;
                    case "mqtt":
                        Settings.Instance.UseMQTT = selectedModule.IsEnabled;
                        break;
                    case "wled":
                        Settings.Instance.UseWLED = selectedModule.IsEnabled;
                        break;
                    case "hatcher":
                        Settings.Instance.UseHatcher = selectedModule.IsEnabled;
                        break;
                }
                PopulateModulesList(); // Refresh the list to update the module status
            }
        }




        private void lbx_modules_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = lbx_modules.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    lbx_modules.SelectedIndex = index;
                    contextMenuStrip1.Show(lbx_modules, e.Location);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}