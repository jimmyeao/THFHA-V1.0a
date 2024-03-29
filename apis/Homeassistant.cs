﻿using Q42.HueApi.Models;
using Serilog;
using System.Net.Http.Headers;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
using static THFHA_V1._0.apis.MqttModule;

namespace THFHA_V1._0.apis
{
    public class HomeassistantModule : IModule
    {
        #region Private Fields

        private readonly HttpClient _httpClient = new HttpClient();
        private bool isEnabled = false;
        private string name = "Homeassistant";
        private string? oldact = null;
        private string? oldcam = null;
        private string? oldmic = null;
        private string? oldstattus = null;
        private Settings settings;
        private State stateInstance;

        #endregion Private Fields

        #region Public Constructors

        public HomeassistantModule()
        {
            // This is the parameterless constructor that will be used by the ModuleManager class
            settings = Settings.Instance;
            
        }

        public HomeassistantModule(State state) : this()
        {
            stateInstance = state;
            stateInstance.StateChanged += OnStateChanged;

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
                    Log.Debug("HomeAssistant Module has been disabled.");
                    OnStopMonitoringRequested();
                }
                else
                {
                    // Check for valid URL and token
                    if (string.IsNullOrWhiteSpace(settings.Haurl))
                    {
                        Log.Error("HomeAssistant URL is missing.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(settings.Haurl))
                    {
                        Log.Error("HomeAssistant token is missing.");
                        return;
                    }

                    // Start monitoring if URL and token are valid
                    _ = Start();
                }
            }
        }
        public void Stop()
        {
            var isMonitoring = false; 
            Log.Debug("Stop HomeAsisstant monitoring requested");
            if (IsEnabled)
            {
                OnStopMonitoringRequested();
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

        #endregion Public Properties

        #region Public Methods

        public static bool IsValidUrl(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public async Task Create_Entity(string entity)
        {
            if (!IsValidUrl(settings.Haurl))
            { return; }
            var client = new HttpClient();
            client.BaseAddress = new Uri(settings.Haurl + "/api/");

            var token = "Bearer " + settings.Hatoken;
            client.DefaultRequestHeaders.Add("Authorization", token);

            var content = new StringContent($@"{{
                ""entity_id"": ""{entity}"",
                ""state"": ""Unknown""
            }}");

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await client.PostAsync($"states/{entity}", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Log.Error("Error creating {entity} in Home assistant: {ex}", entity, ex.Message);
            }

            client.Dispose();
        }

        public async Task<bool> EntityExists(string entity)
        {
            if (isEnabled && settings.IsHomeassistantModuleSettingsValid)
            {
                if (!IsValidUrl(settings.Haurl))
                { return false; }
                var client = new HttpClient();
                client.BaseAddress = new Uri(settings.Haurl + "/api/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + settings.Hatoken);
                try
                {
                    var response = await client.GetAsync($"states/{entity}");
                    return response.IsSuccessStatusCode;
                    client.Dispose();
                }
                catch
                {
                    Log.Error($"Error Connecting: {entity}");
                    return false;
                }
            }
            return false;
        }

        public Form GetSettingsForm()
        {
            return new Homeassistantsettings(); // Replace with your module's settings form
        }

        public async Task OnFormClosing()
        {
            // Handle the form closing event here
            stateInstance.StateChanged -= OnStateChanged;

            Log.Debug("Stop Homeassistant monitoring requested");
            if (IsEnabled)
            {
                //await OnStopMonitoringRequested();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(settings.Haurl + "/api/");
                    var token = "Bearer " + settings.Hatoken;
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    System.Net.Http.HttpResponseMessage response;
                    var payload = $@"{{
                ""state"": ""Not Running"",
                ""entity_id"": ""sensor.thfha_status"",
                ""attributes"": {{
                    ""icon"": ""mdi:account-off""
                }}
            }}";

                    var content = new StringContent(payload);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    Log.Debug("Attempting to set homeassistant status off");
                    try
                    {
                        response = await client.PostAsync($"states/sensor.thfha_status", content);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }
                    payload = $@"{{
                ""state"": ""Not Running"",
                ""entity_id"": ""sensor.thfha_activity"",
                ""attributes"": {{
                    ""icon"": ""mdi:account-off""
                }}
            }}";

                    content = new StringContent(payload);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    Log.Debug("Attempting to set homeassistant activity off");
                    try
                    {
                        response = await client.PostAsync($"states/sensor.thfha_activity", content);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }
                }
            }
        }


        public async Task Start()
        {
            if (string.IsNullOrWhiteSpace(settings.Haurl))
            {
                Log.Error("HomeAssistant URL is missing.");
                return;
            }

            if (string.IsNullOrWhiteSpace(settings.Haurl))
            {
                Log.Error("HomeAssistant token is missing.");
                return;
            }

            bool exists = false;
            exists = await EntityExists("sensor.thfha_status");
            if (!exists)
            {
                await Create_Entity("sensor.thfha_status");
            }
            exists = await EntityExists("sensor.thfha_activity");
            if (!exists)
            {
                await Create_Entity("sensor.thfha_activity");
            }
            exists = await EntityExists("sensor.thfha_camera");
            if (!exists)
            {
                await Create_Entity("sensor.thfha_camera");
            }
            exists = await EntityExists("sensor.thfha_microphone");
            if (!exists)
            {
                await Create_Entity("sensor.thfha_microphone");
            }
            exists = await EntityExists("sensor.thfha_blurred");
            if (!exists)
            {
                await Create_Entity("sensor.thfha_blurred");
            }
            exists = await EntityExists("sensor.thfha_hand");
            if (!exists)
            {
                await Create_Entity("sensor.thfha_hand");
            }
            exists = await EntityExists("sensor.thfha_recording");
            if (!exists)
            {
                await Create_Entity("sensor.thfha_recording");
            }
            //lets update the entities
            var statusicon = "";
            var activityicon = "";

            switch (stateInstance.Status)
            {
                case "Busy":
                    statusicon = "mdi:account-cancel";
                    break;

                case "On the phone":
                    statusicon = "mdi:phone-in-talk-outline";
                    break;

                case "Do not disturb":
                    statusicon = "mdi:minus-circle-outline";
                    break;

                case "Away":
                    statusicon = "mdi:timer-sand";
                    break;

                case "Be right back":
                    statusicon = "mdi:timer-sand";
                    break;

                case "Available":
                    statusicon = "mdi:account";
                    break;

                case "Offline":
                    statusicon = "mdi:account-off";
                    break;

                default:
                    statusicon = "mdi:account-off";
                    break;
            }
            switch (stateInstance.Activity)
            {
                case "In a call":
                    activityicon = "mdi:phone-in-talk-outline";
                    break;

                case "On the phone":
                    activityicon = "mdi:phone-in-talk-outline";
                    break;

                case "Offline":
                    activityicon = "mdi:account-off";
                    break;

                case "In a meeting":
                    activityicon = "mdi:phone-in-talk-outline";
                    break;

                case "In A Conference Call":
                    activityicon = "mdi:phone-in-talk-outline";
                    break;

                case "Out of Office":
                    activityicon = "mdi:account-off";
                    break;

                case "Not in a Call":
                    activityicon = "mdi:account";
                    break;

                case "Presenting":
                    activityicon = "mdi:presentation-play";
                    break;

                default:
                    activityicon = "mdi:account-off";
                    break;
            }

            await UpdateEntity("sensor.thfha_status", stateInstance.Status, statusicon);

            await UpdateEntity("sensor.thfha_activity", stateInstance.Activity, activityicon);

            await UpdateEntity("sensor.thfha_camera", stateInstance.Camera, "mdi:camera");

            await UpdateEntity("sensor.thfha_camera", stateInstance.Camera, "mdi:camera-off");

            await UpdateEntity("sensor.thfha_hand", stateInstance.Handup, "mdi:hand-back-left-off");

            await UpdateEntity("sensor.thfha_blurred", stateInstance.Blurred, "mdi:blur");

            await UpdateEntity("sensor.thfha_recording", stateInstance.Recording, "mdi:record");

            if (stateInstance.Microphone == "Off")
            {
                await UpdateEntity("sensor.thfha_microphone", stateInstance.Microphone, "mdi:microphone");
            }
            else
            {
                await UpdateEntity("sensor.thfha_microphone", stateInstance.Microphone, "mdi:microphone-off");
            }

            return;
        }

        public async Task UpdateEntity(string entityName, string stateText, string icon)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(settings.Haurl + "/api/");
            var token = "Bearer " + settings.Hatoken;
            client.DefaultRequestHeaders.Add("Authorization", token);
            System.Net.Http.HttpResponseMessage response;

            try
            {
                response = await client.GetAsync($"states/{entityName}");
                //Log.Debug("Response to GET request: {response}", response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Log.Error("Error updating {entity} in Home assistant: {ex}", entityName, ex.Message);
                client.Dispose();
                isEnabled = false;
                return;
            }
            //Log.Debug("Response to GET request: {response}", response);

            if (response.IsSuccessStatusCode)
            {
                //Log.Information("Updating {entity} in Home assistant to {state}", entityName, stateText);

                var payload = $@"{{
                    ""state"": ""{stateText}"",
                    ""entity_id"": ""{entityName}"",
                    ""attributes"": {{
                        ""icon"": ""{icon}""
                    }}
                }}";

                var content = new StringContent(payload);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //Log.Debug("Attempting to set homeassistant "+content.ToString());
                try
                {
                    response = await client.PostAsync($"states/{entityName}", content);
                }catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    client.Dispose();
                }
                //Log.Debug("Response to POST request: {response}", response.ReasonPhrase);
            }
            else
            {
                Log.Error("Error updating {entity} in Home assistant: {status}", entityName, response.StatusCode);
                client.Dispose();
                isEnabled = false;
                settings.UseHA = false;
                settings.Save();
                OnStopMonitoringRequested();
                return;
            }

            response.EnsureSuccessStatusCode();
            client.Dispose();
        }

        public void UpdateSettings(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        #endregion Public Methods

        #region Private Methods

        private async void OnStateChanged(object sender, EventArgs e)
        {
            if (IsEnabled && THFHA.logWatcher?.IsRunning == true)
            {
                stateInstance = (State)sender;
                StateChanged?.Invoke(this, EventArgs.Empty);
                var statusicon = "";
                var activityicon = "";
                switch (stateInstance.Status)
                {
                    case "Busy":
                        statusicon = "mdi:account-cancel";
                        break;

                    case "On the phone":
                        statusicon = "mdi:phone-in-talk-outline";
                        break;

                    case "Do not disturb":
                        statusicon = "mdi:minus-circle-outline";
                        break;

                    case "Away":
                        statusicon = "mdi:timer-sand";
                        break;

                    case "Be right back":
                        statusicon = "mdi:timer-sand";
                        break;

                    case "Available":
                        statusicon = "mdi:account";
                        break;

                    case "Offline":
                        statusicon = "mdi:account-off";
                        break;

                    default:
                        statusicon = "mdi:account-off";
                        break;
                }
                switch (stateInstance.Activity)
                {
                    case "In a call":
                        activityicon = "mdi:phone-in-talk-outline";
                        break;

                    case "On the phone":
                        activityicon = "mdi:phone-in-talk-outline";
                        break;

                    case "Offline":
                        activityicon = "mdi:account-off";
                        break;

                    case "In a meeting":
                        activityicon = "mdi:phone-in-talk-outline";
                        break;

                    case "In A Conference Call":
                        activityicon = "mdi:phone-in-talk-outline";
                        break;

                    case "Out of Office":
                        activityicon = "mdi:account-off";
                        break;

                    case "Not in a Call":
                        activityicon = "mdi:account";
                        break;

                    case "Presenting":
                        activityicon = "mdi:presentation-play";
                        break;

                    default:
                        activityicon = "mdi:account-off";
                        break;
                }
                if (stateInstance.Status != oldstattus)
                {
                    await UpdateEntity("sensor.thfha_status", stateInstance.Status, statusicon);
                }
                if (stateInstance.Activity != oldact)
                {
                    await UpdateEntity("sensor.thfha_activity", stateInstance.Activity, activityicon);
                }

                if (stateInstance.Camera == "On")
                {
                    await UpdateEntity("sensor.thfha_camera", stateInstance.Camera, "mdi:camera");
                }
                else
                {
                    await UpdateEntity("sensor.thfha_camera", stateInstance.Camera, "mdi:camera-off");
                }

                if (stateInstance.Microphone == "Off")
                {
                    await UpdateEntity("sensor.thfha_microphone", stateInstance.Microphone, "mdi:microphone");
                }
                else
                {
                    await UpdateEntity("sensor.thfha_microphone", stateInstance.Microphone, "mdi:microphone-off");
                }
                if (stateInstance.Blurred == "Blurred")
                {
                    await UpdateEntity("sensor.thfha_blurred", stateInstance.Blurred, "mdi:blur");
                }
                else
                {
                    await UpdateEntity("sensor.thfha_blurred", stateInstance.Blurred, "mdi:blur-off");
                }
                if (stateInstance.Handup == "Raised")
                {
                    await UpdateEntity("sensor.thfha_hand", stateInstance.Handup, "mdi:hand-back-left");
                }
                else
                {
                    await UpdateEntity("sensor.thfha_hand", stateInstance.Handup, "mdi:hand-back-left-off");
                }
                if (stateInstance.Recording == "On")
                {
                    await UpdateEntity("sensor.thfha_recording", stateInstance.Recording, "mdi:record-circle");
                }
                else
                {
                    await UpdateEntity("sensor.thfha_recording", stateInstance.Recording, "mdi:record");
                }

                oldstattus = stateInstance.Status;
                oldact = stateInstance.Activity;
                oldcam = stateInstance.Camera;
                oldmic = stateInstance.Microphone;
                return;
            }
        }

        private async Task OnStopMonitoringRequested()
        {
            // Stop monitoring here
            
            Log.Information("Setting Status to Not Running");
            await UpdateEntity("sensor.thfha_status", "Not Running", "mdi:account-off");
            Log.Information("Setting Activity to Not Running");
            await UpdateEntity("sensor.thfha_activity", "Not Running", "mdi:account-off");
            var isMonitoring = false;

        }

        #endregion Private Methods
    }
}