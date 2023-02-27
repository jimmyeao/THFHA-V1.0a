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
using THFHA_V1._0.Views;


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
        private void StartLogWatcher(string filePath)
        {
            logWatcher = new LogWatcher(filePath, new State());
            
            logWatcher.Start();
        }


        private void StopLogWatcher()
        {
            logWatcher.Stop();
            
        }


    }
}
