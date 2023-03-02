using Serilog;
using System.Diagnostics;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;

namespace THFHA_V1._0
{
    public partial class THFHA : Form
    {
        private List<IModule> modules;
        public static LogWatcher logWatcher;
        private State state;
        private Settings settings;
        public event EventHandler? StopMonitoringRequested;
        public event EventHandler? ApplicationClosing;

        public THFHA(List<IModule> modules, State state)
        {
            InitializeComponent();
            this.settings = Settings.Instance;
            this.modules = modules;
            this.state = state; // set the state


            // Initialize the IsEnabled property of each module based on the value stored in the Settings singleton
            updatemodules();

            string _appDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string _logPath = _appDir + @"\Microsoft\Teams\";
            string _logFile = _logPath + "logs.txt";

            state.StateChanged += OnStateChanged;
            Log.Debug("State.StateChanged event subscribed");

            PopulateModulesList();
            if (settings.RunLogWatcherAtStart)
            {
                Settings.SettingChanged += Settings_SettingChanged; // Subscribe to the SettingChanged event

                StartLogWatcher();
                btn_start.Enabled = false; btn_stop.Enabled = true;
            }
            else
            {
                btn_start.Enabled = true; btn_stop.Enabled = false;
                foreach (IModule module in modules)
                {
                    //module.OnFormClosing();
                }
            }

        }
        private void updatemodules()
        {
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
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            // Get the updated state values
            Log.Debug("OnStateChange Trigerred!!!!!!!!!!!");
            UpdateLabel(lbl_status, state.Status);
            UpdateLabel(lbl_activity, state.Activity);
            UpdateLabel(lbl_camera, state.Camera);
            UpdateLabel(lbl_mute, state.Microphone);
            UpdateStatusIcons(state.Status);
            UpdateActivityIcons(state.Activity);
            UpdateMuteStatus(state.Microphone);
            // var icon = UpdateStatusIcon(state.Status);
            // UpdateStatusIcons(icon);
            // _ = UpdateMuteIcon();
            // _ = UpdateActivityIcon(state.Activity);

        }

        private void UpdateStatusIcons(string status)
        {
            var icon = GetStatusIcon(status);
            UpdateStatusIcon(icon);
            UpdateNotifyMenuStatus(icon);
        }
        private void UpdateNotifyMenuStatus(Bitmap icon)
        {
            if (icon == Resource1.outofoffice)
            {
                // notifyMenuStatus.Image = Resource1.outofoffice;
                // notifyMenuStatus.Text = "Offline";
            }
            else
            {
                // notifyMenuStatus.Image = icon;
                // notifyMenuStatus.Text = state.Status;
            }
        }
        public void UpdateMuteStatus(string micstatus)
        {
            if (pb_mute.InvokeRequired)
            {
                pb_mute.Invoke((MethodInvoker)delegate
                {
                    switch (micstatus)
                    {
                        case ("Mute On"):
                            pb_mute.BackgroundImage = Resource1.mute;
                            //toolTip3.SetToolTip(pb_mute, "Mute On");
                            pb_mute.Refresh();
                            break;
                        case ("Mute Off"):
                            pb_mute.BackgroundImage = Resource1.mic_icon;
                            //toolTip3.SetToolTip(pb_mute, "Mute Off");
                            pb_mute.Refresh();
                            break;
                    }
                });
            }
            else
            {
                switch (micstatus)
                {
                    case ("Mute ON"):
                        pb_mute.BackgroundImage = Resource1.mute;
                        //toolTip3.SetToolTip(pb_mute, "Mute On");
                        pb_mute.Refresh();
                        break;
                    case ("Mute Off"):
                        pb_mute.BackgroundImage = Resource1.mic_icon;
                        //toolTip3.SetToolTip(pb_mute, "Mute Off");
                        pb_mute.Refresh();
                        break;
                }
            }
        }

        private enum PresenceStatus
        {
            Away,
            Available,
            Busy,
            DoNotDisturb,
            OnThePhone,
            BeRightBack,
            OutOfOffice,
            Offline,
            Presenting,
            Focusing,
            InAMeeting
        }
        private Bitmap GetStatusIcon(string status)
        {
            status = status.Replace(" ", ""); // remove spaces from status

            PresenceStatus presenceStatus;
            if (!Enum.TryParse(status, true, out presenceStatus)) // ignore case when parsing
            {
                presenceStatus = PresenceStatus.Offline;
            }
            switch (presenceStatus)
            {
                case PresenceStatus.Away:
                    return Resource1.Away;
                case PresenceStatus.Available:
                    return Resource1.available;
                case PresenceStatus.Busy:
                case PresenceStatus.OnThePhone:
                case PresenceStatus.InAMeeting:
                case PresenceStatus.Focusing:
                    return Resource1.Busy;
                case PresenceStatus.DoNotDisturb:
                case PresenceStatus.Presenting:
                    return Resource1.DND;
                case PresenceStatus.BeRightBack:
                    return Resource1.Away;
                case PresenceStatus.OutOfOffice:
                case PresenceStatus.Offline:
                default:
                    return Resource1.outofoffice;
            }
        }
        private void UpdateStatusIcon(Bitmap icon)
        {
            if (pb_Status.InvokeRequired)
            {
                pb_Status.Invoke((MethodInvoker)delegate
                {
                    pb_Status.BackgroundImage = icon;
                    //toolTip1.SetToolTip(pb_Status, state.Status);
                });
            }
            else
            {
                pb_Status.BackgroundImage = icon;
                //toolTip1.SetToolTip(pb_Status, state.Status);
            }
        }
        private enum ActivityStatus
        {
            InACall,
            OnThePhone,
            Presenting,
            NotInACall,
            IncomingCall,
            InAMeeting
        }
        public void UpdateActivityIcons(string activity)
        {
            var icon = GetActivityIcon(state.Activity);
            UpdateActivityIcon(icon);
            //UpdateNotifyMenuActivity(icon);
        }
        private Bitmap GetActivityIcon(string activity)
        {
            ActivityStatus activityStatus;
            if (!Enum.TryParse(activity.Replace(" ", string.Empty), true, out activityStatus))
            {
                activityStatus = ActivityStatus.NotInACall;
            }

            switch (activityStatus)
            {
                case ActivityStatus.InACall:
                case ActivityStatus.IncomingCall:
                case ActivityStatus.OnThePhone:
                    return Resource1.inacall;
                case ActivityStatus.Presenting:
                    return Resource1.presenting;
                case ActivityStatus.InAMeeting:
                    return Resource1.Busy;
                case ActivityStatus.NotInACall:
                default:
                    return Resource1.notinacall;
            }
        }
        private void UpdateActivityIcon(Bitmap icon)
        {
            if (pb_Activity.InvokeRequired)
            {
                pb_Activity.Invoke((MethodInvoker)delegate
                {
                    SetActivityIcon(icon);
                });
            }
            else
            {
                SetActivityIcon(icon);
            }
        }
        private void SetActivityIcon(Bitmap icon)
        {
            pb_Activity.BackgroundImage = icon;
            //toolTip2.SetToolTip(pb_Activity, state.Activity);
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open the settings form
            // Create an instance of the SettingsForm
            SettingsForm settingsForm = new SettingsForm(modules);

            // Show the SettingsForm
            settingsForm.ShowDialog();
            updatemodules();
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
        private async Task StartLogWatcher()
        {
            logWatcher = new LogWatcher(new State());

            await logWatcher.Start();
            Task delay = Task.Delay(1000);
            statuslabel.Text = "Monitoring Started";
            UpdateLabel(lbl_status, state.Status);
            UpdateLabel(lbl_activity, state.Activity);
            UpdateLabel(lbl_camera, state.Camera);
            UpdateLabel(lbl_mute, state.Microphone);
            foreach (IModule module in modules)
            {
                if (module.IsEnabled)
                {
                    module.Start();
                }
            }
        }
        private async Task StopLogWatcher()
        {
            if (logWatcher != null)
            {
                await logWatcher.Stop();
                foreach (IModule module in modules)
                {
                    module.OnFormClosing();
                }
            }

            statuslabel.Text = "Monitoring Stopped";
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

                // Check if the module settings are valid
                string propertyName = "Is" + selectedModule.Name + "ModuleSettingsValid";
                var moduleSettingsValidProp = typeof(Settings).GetProperty(propertyName);
                if (moduleSettingsValidProp != null && !(bool)moduleSettingsValidProp.GetValue(Settings.Instance))
                {
                    MessageBox.Show(selectedModule.Name + " module settings are invalid. Please check the settings.", "Invalid settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                selectedModule.IsEnabled = true;
                selectedModule.UpdateSettings(selectedModule.IsEnabled); // Update the settings based on the new state of the module
                switch (selectedModule.Name.ToLower())
                {
                    case "hue":
                        Settings.Instance.UseHue = selectedModule.IsEnabled;
                        statuslabel.Text = selectedModule.Name + " Enabled";
                        break;
                    case "homeassistant":
                        Settings.Instance.UseHA = selectedModule.IsEnabled;
                        statuslabel.Text = selectedModule.Name + " Enabled";
                        break;
                    case "mqtt":
                        Settings.Instance.UseMQTT = selectedModule.IsEnabled;
                        statuslabel.Text = selectedModule.Name + " Enabled";
                        break;
                    case "wled":
                        Settings.Instance.UseWLED = selectedModule.IsEnabled;
                        statuslabel.Text = selectedModule.Name + " Enabled";
                        break;
                    case "hatcher":
                        Settings.Instance.UseHatcher = selectedModule.IsEnabled;
                        statuslabel.Text = selectedModule.Name + " Enabled";
                        break;
                }
                PopulateModulesList(); // Refresh the list to update the module status
                settings.Save();
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

                        statuslabel.Text = selectedModule.Name + " Disabled";
                        break;
                    case "homeassistant":
                        Settings.Instance.UseHA = selectedModule.IsEnabled;
                        statuslabel.Text = selectedModule.Name + " Disabled";
                        break;
                    case "mqtt":
                        Settings.Instance.UseMQTT = selectedModule.IsEnabled;
                        statuslabel.Text = selectedModule.Name + " Disabled";
                        break;
                    case "wled":
                        Settings.Instance.UseWLED = selectedModule.IsEnabled;
                        statuslabel.Text = selectedModule.Name + " Disabled";
                        break;
                    case "hatcher":
                        Settings.Instance.UseHatcher = selectedModule.IsEnabled;
                        statuslabel.Text = selectedModule.Name + " Disabled";
                        break;
                }
                PopulateModulesList(); // Refresh the list to update the module status
                settings.Save();
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
        private void btn_start_Click(object sender, EventArgs e)
        {
            Settings.SettingChanged += Settings_SettingChanged; // Subscribe to the SettingChanged event

            btn_start.Enabled = false; btn_stop.Enabled = true;

            _ = StartLogWatcher();
        }
        private void btn_stop_Click(object sender, EventArgs e)
        {
            Settings.SettingChanged -= Settings_SettingChanged; // Subscribe to the SettingChanged event

            btn_start.Enabled = true; btn_stop.Enabled = false;
            _ = StopLogWatcher();
        }
        private void UpdateLabel(Label label, string text)
        {
            if (!label.IsHandleCreated || label.Disposing || label.IsDisposed)
            {
                // The label is not yet created or is disposed, so we can't update it.
                return;
            }

            if (label.InvokeRequired)
            {
                label.Invoke((MethodInvoker)delegate { UpdateLabel(label, text); });
            }
            else
            {
                label.Text = text;
            }
        }

        private void THFHA_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (IModule module in modules)
            {
                module.OnFormClosing();
            }
            ApplicationClosing?.Invoke(this, EventArgs.Empty);
        }

        private void THFHA_Shown(object sender, EventArgs e)
        {
            UpdateLabel(lbl_status, state.Status);
            UpdateLabel(lbl_activity, state.Activity);
            UpdateLabel(lbl_camera, state.Camera);
            UpdateLabel(lbl_mute, state.Microphone);
        }

        private void applicationLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var templogpath = LoggingConfig.logFileFullPath;
            var logpath = AddQuotesIfRequired(templogpath);
            var notepadapp = HasNotepadplusplus();
            if (notepadapp != null)
            {
                Process.Start(notepadapp, logpath);
            }
            else
            {
                Process.Start("notepad.exe", logpath);
            }
        }
        public string AddQuotesIfRequired(string path)
        {
            return !string.IsNullOrWhiteSpace(path) ?
                path.Contains(" ") && (!path.StartsWith("\"") && !path.EndsWith("\"")) ?
                    "\"" + path + "\"" : path :
                string.Empty;
        }
        private void teamsLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var username = Environment.UserName;
            var logpath = "c:\\users\\" + username + "\\AppData\\roaming\\Microsoft\\Teams\\logs.txt";
            var notepadapp = HasNotepadplusplus();
            if (notepadapp != null)
            {
                Process.Start(notepadapp, logpath);
            }
            else
            {
                Process.Start("notepad.exe", logpath);
            }
        }
        private string HasNotepadplusplus()
        {

            string[] notepadPaths = new string[]
            {
                @"C:\Program Files (x86)\Notepad++\notepad++.exe",
                @"C:\Program Files\Notepad++\notepad++.exe",
            };

            foreach (string path in notepadPaths)
            {
                if (File.Exists(path))
                {

                    return path;
                }

            }
            return null;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }
    }
}