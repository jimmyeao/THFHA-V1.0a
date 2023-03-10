using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Diagnostics;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
using THFHA_V1._0.TeamsAPI;


namespace THFHA_V1._0
{
    public partial class THFHA : Form
    {
        #region Public Fields

        public static LogWatcher logWatcher;

        #endregion Public Fields

        #region Private Fields

        private readonly Dictionary<string, object> _meetingState;
        private  WebSocketClient _webSocketClient;
        private List<IModule> modules;
        private Settings settings;
        private State state;

        #endregion Private Fields

        #region Public Constructors

        public THFHA(List<IModule> modules, State state)
        {
            InitializeComponent();
            this.settings = Settings.Instance;
            this.modules = modules;
            this.state = state; // set the state

            // Initialize the IsEnabled property of each module based on the value stored in the
            // Settings singleton
            updatemodules();

            string _appDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string _logPath = _appDir + @"\Microsoft\Teams\";
            string _logFile = _logPath + "logs.txt";
            string token = "286b3935-a0d9-4bb6-95b6-0820c6315b86";
            State.Instance.StateChanged += OnStateChanged;
            Log.Debug("State.StateChanged event subscribed");

            PopulateModulesList();
            if (settings.RunLogWatcherAtStart)
            {
                Settings.SettingChanged += Settings_SettingChanged; // Subscribe to the SettingChanged event

                StartLogWatcher();
                if (settings.TeamsApi != "")
                {
                    _webSocketClient = new WebSocketClient(new Uri("ws://localhost:8124?token=" + settings.TeamsApi + "&protocol-version=1.0.0&manufacturer=Jimmyeao&device=THFHA&app=THFHA&app-version=1.0"), state);

                    // _webSocketClient.MessageReceived += WebSocketClient_MessageReceived;
                }
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
            //if (settings.TeamsApi != "")
            //{
            //    _webSocketClient = new WebSocketClient(new Uri("ws://localhost:8124?token=" + settings.TeamsApi + "&protocol-version=1.0.0&manufacturer=Jimmyeao&device=THFHA&app=THFHA&app-version=1.0"), state);

            //   // _webSocketClient.MessageReceived += WebSocketClient_MessageReceived;
            //}


        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler? ApplicationClosing;

        public event EventHandler? StopMonitoringRequested;

        #endregion Public Events

        #region Private Enums

        private enum ActivityStatus
        {
            InACall,
            OnThePhone,
            Presenting,
            NotInACall,
            IncomingCall,
            InAMeeting
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

        #endregion Private Enums

        #region Public Methods

        public string AddQuotesIfRequired(string path)
        {
            return !string.IsNullOrWhiteSpace(path) ?
                path.Contains(" ") && (!path.StartsWith("\"") && !path.EndsWith("\"")) ?
                    "\"" + path + "\"" : path :
                string.Empty;
        }

        public void PopulateModulesList()
        {
            lbx_modules.Items.Clear();
            foreach (IModule module in modules)
            {
                lbx_modules.Items.Add(module.Name + " [" + (module.IsEnabled ? "Enabled" : "Disabled") + "]");
            }
        }

        public void UpdateActivityIcons(string activity)
        {
            var icon = GetActivityIcon(State.Instance.Activity);
            UpdateActivityIcon(icon);
            //UpdateNotifyMenuActivity(icon);
        }

        public void UpdateMuteStatus(string micstatus)
        {
            if (pb_mute.InvokeRequired)
            {
                pb_mute.Invoke((MethodInvoker)delegate
                {
                    switch (micstatus)
                    {
                        case ("On"):
                            pb_mute.BackgroundImage = Resource1.mute;
                            //toolTip3.SetToolTip(pb_mute, "Mute On");
                            pb_mute.Refresh();
                            break;

                        case ("Off"):
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
                    case ("On"):
                        pb_mute.BackgroundImage = Resource1.mute;
                        //toolTip3.SetToolTip(pb_mute, "Mute On");
                        pb_mute.Refresh();
                        break;

                    case ("Off"):
                        pb_mute.BackgroundImage = Resource1.mic_icon;
                        //toolTip3.SetToolTip(pb_mute, "Mute Off");
                        pb_mute.Refresh();
                        break;
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
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

        private void btn_start_Click(object sender, EventArgs e)
        {
            // Initialize the state and settings objects
            // Initialize the state and settings objects
            State state = new State();
            

            Settings.SettingChanged += Settings_SettingChanged; // Subscribe to the SettingChanged event

            btn_start.Enabled = false; btn_stop.Enabled = true;
            if (settings.TeamsApi != "")
            {
                _webSocketClient = new WebSocketClient(new Uri("ws://localhost:8124?token=" + settings.TeamsApi + "&protocol-version=1.0.0&manufacturer=Jimmyeao&device=THFHA&app=THFHA&app-version=1.0"), state);

               
            }
            _ = StartLogWatcher();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Settings.SettingChanged -= Settings_SettingChanged; // Subscribe to the SettingChanged event
            _ = _webSocketClient.StopAsync();
            btn_start.Enabled = true; btn_stop.Enabled = false;
            btn_start.Enabled = true; btn_stop.Enabled = false;
            _ = StopLogWatcher();
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

        private void label1_Click(object sender, EventArgs e)
        {
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

        private void OnStateChanged(object sender, EventArgs e)
        {
            // Get the updated state values
            Log.Debug("OnStateChange Trigerred!!!!!!!!!!!");
            BeginInvoke((MethodInvoker)delegate { UpdateAll(); });

            // var icon = UpdateStatusIcon(state.Status); UpdateStatusIcons(icon); _ =
            // UpdateMuteIcon(); _ = UpdateActivityIcon(state.Activity);
        }

        private void SetActivityIcon(Bitmap icon)
        {
            pb_Activity.BackgroundImage = icon;
            //toolTip2.SetToolTip(pb_Activity, state.Activity);
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

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open the settings form Create an instance of the SettingsForm
            SettingsForm settingsForm = new SettingsForm(modules);

            // Show the SettingsForm
            settingsForm.ShowDialog();
            updatemodules();
            PopulateModulesList();
        }

        private async Task StartLogWatcher()
        {
            logWatcher = new LogWatcher(new State());

            await logWatcher.Start();
            Task delay = Task.Delay(1000);
            statuslabel.Text = "Monitoring Started";
            UpdateAll();
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

        private void THFHA_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (IModule module in modules)
            {
                module.OnFormClosing();
            }
            ApplicationClosing?.Invoke(this, EventArgs.Empty);
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

        private void THFHA_Shown(object sender, EventArgs e)
        {
            UpdateAll();
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

        private async Task UpdateAll()
        {
            if (lbl_status.Text != State.Instance.Status)
            {
                UpdateLabel(lbl_status, State.Instance.Status);
            }

            if (lbl_activity.Text != State.Instance.Activity)
            {
                UpdateLabel(lbl_activity, State.Instance.Activity);
                UpdateActivityIcons(State.Instance.Activity);
            }

            if (lbl_camera.Text != State.Instance.Camera)
            {
                UpdateLabel(lbl_camera, State.Instance.Camera);
            }

            if (lbl_mute.Text != State.Instance.Microphone)
            {
                UpdateLabel(lbl_mute, State.Instance.Microphone);
                UpdateMuteStatus(State.Instance.Microphone);
            }

            if (lbl_blurred.Text != ("Background: " + (string.IsNullOrEmpty(State.Instance.Blurred) ? "Not Configured" : State.Instance.Blurred)))
            {
                UpdateLabel(lbl_blurred, "Background: " + (string.IsNullOrEmpty(State.Instance.Blurred) ? "Not Configured" : State.Instance.Blurred));
            }

            if (lbl_recording.Text != ("Recording: " + (string.IsNullOrEmpty(State.Instance.Recording) ? "Not Configured" : State.Instance.Recording)))
            {
                UpdateLabel(lbl_recording, "Recording: " + (string.IsNullOrEmpty(State.Instance.Recording) ? "Not Configured" : State.Instance.Recording));
            }

            if (lbl_hand.Text != ("Hand: " + (string.IsNullOrEmpty(State.Instance.Handup) ? "Not Configured" : State.Instance.Handup)))
            {
                UpdateLabel(lbl_hand, "Hand: " + (string.IsNullOrEmpty(State.Instance.Handup) ? "Not Configured" : State.Instance.Handup));
            }

            if (lbl_meeting.Text != ("Activity: " + (string.IsNullOrEmpty(State.Instance.Activity) ? "Not Configured" : State.Instance.Activity)))
            {
                UpdateLabel(lbl_meeting, "Activity: " + (string.IsNullOrEmpty(State.Instance.Activity) ? "Not Configured" : State.Instance.Activity));
            }

            if (lbl_cam.Text != ("Camera: " + (string.IsNullOrEmpty(State.Instance.Camera) ? "Not Configured" : State.Instance.Camera)))
            {
                UpdateLabel(lbl_cam, "Camera: " + (string.IsNullOrEmpty(State.Instance.Camera) ? "Not Configured" : State.Instance.Camera));
            }

            if (lbl_muted.Text != ("Mute: " + (string.IsNullOrEmpty(State.Instance.Microphone) ? "Not Configured" : State.Instance.Microphone)))
            {
                UpdateLabel(lbl_muted, "Mute: " + (string.IsNullOrEmpty(State.Instance.Microphone) ? "Not Configured" : State.Instance.Microphone));
            }
            UpdateStatusIcons(State.Instance.Status);
        }



        private void UpdateLabel(Label label, string text)
        {
            if (!label.IsHandleCreated || label.Disposing || label.IsDisposed)
            {
                // The label is not yet created or is disposed, so we can't update it.
                return;
            }

            label.BeginInvoke((MethodInvoker)delegate { label.Text = text; });
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

        private void UpdateNotifyMenuStatus(Bitmap icon)
        {
            if (icon == Resource1.outofoffice)
            {
                // notifyMenuStatus.Image = Resource1.outofoffice; notifyMenuStatus.Text = "Offline";
            }
            else
            {
                // notifyMenuStatus.Image = icon; notifyMenuStatus.Text = state.Status;
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

        private void UpdateStatusIcons(string status)
        {
            var icon = GetStatusIcon(status);
            UpdateStatusIcon(icon);
            UpdateNotifyMenuStatus(icon);
        }

        #endregion Private Methods


    }
}