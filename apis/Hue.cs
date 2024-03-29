﻿using Newtonsoft.Json;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Original;
using Q42.HueApi.Interfaces;
using Serilog;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
using State = THFHA_V1._0.Model.State;

namespace THFHA_V1._0.apis
{
    public class HueModule : IModule
    {
        #region Private Fields

        private IHueClient client;
        private bool isEnabled = false;
        private string name = "Hue";
        private Q42.HueApi.State? originalState;
        private Settings settings;
        private State stateInstance;
        private bool staterecorded = false;

        #endregion Private Fields

        #region Public Constructors

        public HueModule()
        {
            // This is the parameterless constructor that will be used by the ModuleManager class
            settings = Settings.Instance;
            if (settings.Hueip == "" || settings.Hueusername == "")
            {
                Log.Error("Hue ip or username is null");
                return;
            }
            if (!settings.IsHueModuleSettingsValid)
            {
                Log.Warning("Hue module settings are not valid. Not subscribing to state changes.");
                return;
            }
            ILocalHueClient localClient = new LocalHueClient(settings.Hueip);
            localClient.Initialize(settings.Hueusername);
            client = localClient;
            // Initialize stateInstance here
        }

        public HueModule(State state) : this()
        {
            stateInstance = state;
            stateInstance.StateChanged += OnStateChanged;
            //stateInstance.StateChanged += OnStateChanged;
            // Initialize your module here
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler? StateChanged;

        #endregion Public Events

        #region Public Properties

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                if (!isEnabled)
                {
                    // Perform some actions when the module is disabled
                    Log.Debug("Hue Module has been disabled.");
                    if (!settings.IsHueModuleSettingsValid)
                    {
                        return;

                    }
                    _ = GetState();
                    OnStopMonitoringRequested();

                }
                else
                {
                    if (settings.IsHueModuleSettingsValid)
                    {
                        _ = GetState();
                        _ = PublishHueUpdate(stateInstance);
                    }
                }
            }
        }

        public string Name
        {
            get { return name; }
        }

        public string State
        {
            get { return stateInstance.ToString(); }
            set { /* You can leave this empty since the State property is read-only */ }
        }
        public void Stop()
        {
            var isMonitoring = false;
            Log.Debug("Stop Hue monitoring requested");
            if (IsEnabled)
            {
                OnStopMonitoringRequested();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public Form GetSettingsForm()
        {
            return new huesettings(); // Replace with your module's settings form
        }

        public async Task OnFormClosing()
        {
            // Handle the form closing event here
            stateInstance.StateChanged -= OnStateChanged;
            if (IsEnabled)
            {
                OnStopMonitoringRequested();
            }
            var isMonitoring = false;
            Log.Debug("Stop Hue monitoring requested");
        }

        public async Task Start()
        {
            if (settings.Hueip == null || settings.Hueusername == null || settings.IsHueModuleSettingsValid == false)
            {
                Log.Error("Hue ip or username is null");
                return;
            }
            if (isEnabled)
            {
                _ = GetState();
                _ = PublishHueUpdate(stateInstance);
            }
        }

        public void UpdateSettings(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        #endregion Public Methods

        #region Private Methods

        private RGBColor GetRGBColorForState(State state)
        {
            var status = state.Status;
            if (stateInstance.Activity == "On the phone" || stateInstance.Activity == "In a call" || stateInstance.Activity == "In a meeting")
            {
                status = "On the Phone";
            }
            return status switch
            {
                "Busy" => new RGBColor("ff0000"),
                "On the Phone" => new RGBColor("ff0000"),
                "Do not disturb" => new RGBColor("ff0000"),
                "Away" => new RGBColor("dc8f34"),
                "Be right back" => new RGBColor("dc8f34"),
                "Available" => new RGBColor("00ff00"),
                "Offline" => new RGBColor("000000"),
                "In a meeting" => new RGBColor("ff0000"),
                "Out of office" => new RGBColor("A020F0"),
                "Be Right Back" => new RGBColor("ffff00"),
                ".." => new RGBColor("000000"),
                "" => new RGBColor("000000"),
                _ => throw new ArgumentException($"Invalid state: {stateInstance.Status}")
            };
        }

        private async Task GetState()
        {
            if (!staterecorded)
            {
                if (client == null)
                {
                    ILocalHueClient localClient = new LocalHueClient(settings.Hueip);
                    localClient.Initialize(settings.Hueusername);
                    client = localClient;
                    stateInstance = new State(); // Initialize stateInstance here
                    stateInstance.StateChanged += OnStateChanged;
                }
                var light = await client.GetLightAsync(settings.SelectedLightId);
                Log.Information("got the state of {light}", light);
                if (light != null)
                {
                    originalState = light.State;

                    // Save the original state to a file Get the path to the local user data folder
                    string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string folderPath = Path.Combine(appDataFolder, "TeamsHelper");
                    string filePath = Path.Combine(folderPath, "huesettings.json");

                    // Create the folder if it doesn't exist
                    Directory.CreateDirectory(folderPath);
                    var json = JsonConvert.SerializeObject(originalState);
                    File.WriteAllText(filePath, json);
                    Log.Information("Original state saved to file.");
                    staterecorded = true;
                }
            }
        }

        private Q42.HueApi.State? LoadOriginalState()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(appDataFolder, "TeamsHelper");
            string filePath = Path.Combine(folderPath, "huesettings.json");
            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath);
                    var state = JsonConvert.DeserializeObject<Q42.HueApi.State>(json);
                    Log.Information("Original state loaded from file.");
                    return state;
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to load original state from file: {ex}", ex.Message);
                }
            }

            return null;
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                stateInstance = (State)sender;
                StateChanged?.Invoke(this, EventArgs.Empty);
                _ = PublishHueUpdate(stateInstance);
            }
        }

        private void OnStopMonitoringRequested()
        {
            if (!settings.IsHueModuleSettingsValid )
            {
                return;
            }

            // Stop monitoring here
            var isMonitoring = false;
            originalState = LoadOriginalState();
            if (originalState != null)
            {
                Log.Information("Restoring state of hue lights");
                var client = new LocalHueClient(settings.Hueip);
                client.Initialize(settings.Hueusername);
                var command = new LightCommand()
                {
                    On = originalState.On,
                    Brightness = originalState.Brightness,
                    Hue = originalState.Hue,
                    Saturation = originalState.Saturation,
                    ColorCoordinates = originalState.ColorCoordinates
                };
                try
                {
                    Task.WaitAll(new Task[] { client.SendCommandAsync(command, new[] { settings.SelectedLightId }) });
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to restore state of hue lights: {ex}", ex.Message);
                }
            }
        }

        private async Task PublishHueUpdate(State state)
        {
            if (isEnabled && THFHA.logWatcher?.IsRunning == true)
            {
                var color = GetRGBColorForState(state);

                var client = new LocalHueClient(settings.Hueip);
                client.Initialize(settings.Hueusername);

                var command = new LightCommand { On = true }.SetColor(color);
                await client.SendCommandAsync(command, new List<string> { settings.SelectedLightId });

                Log.Information("Hue Light set to {status}", stateInstance.Status);
            }
        }

        #endregion Private Methods
    }
}