using System.Diagnostics;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
using System.IO;



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
            PopulateModulesList();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open the settings form
            // Create an instance of the SettingsForm
            SettingsForm settingsForm = new SettingsForm(modules);

            // Show the SettingsForm
            settingsForm.ShowDialog();

        }

        private void PopulateModulesList()
        {
            lbx_modules.Items.Clear();
            foreach (IModule module in modules)
            {
                lbx_modules.Items.Add(module.Name);
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
    }
}
