﻿using Serilog;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
namespace THFHA_V1._0.apis
{
    public class HomeassistantModule : IModule
    {
        private string name = "Homeassistant";
        private bool isEnabled = false;
        private State stateInstance;
        public event EventHandler? StateChanged;
        private Settings settings;
        private readonly HttpClient _httpClient = new HttpClient();
        private string oldstattus = null;
        private string oldact = null;
        private string oldcam = null;
        private string oldmic = null;
        public string Name
        {
            get { return name; }
        }
        
        public async void Start()
        {
            
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
                    activityicon = "mdi:acc";
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
            


            if (stateInstance.Microphone == "Mute Off")
            {
                await UpdateEntity("sensor.thfha_microphone", stateInstance.Microphone, "mdi:microphone");
            }
            else
            {
                await UpdateEntity("sensor.thfha_microphone", stateInstance.Microphone, "mdi:microphone-off");
            }

            return;
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                if (!isEnabled)
                {
                    // Perform some actions when the module is disabled
                    Log.Debug("HomeAssistanrt Module has been disabled.");
                    OnStopMonitoringRequested();
                }
                else
                {
                    Start();
                }
            }
        }

        public string State
        {
            get { return stateInstance.ToString(); }
            set { /* You can leave this empty since the State property is read-only */ }
        }
        public Form GetSettingsForm()
        {
            return new hasettings(); // Replace with your module's settings form
        }
        public void UpdateSettings(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
        private async void OnStateChanged(object sender, EventArgs e)
        {
            if (IsEnabled)
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
                        activityicon = "mdi:acc";
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


                if (stateInstance.Microphone == "Mute Off")
                {
                    await UpdateEntity("sensor.thfha_microphone", stateInstance.Microphone, "mdi:microphone");
                }
                else
                {
                    await UpdateEntity("sensor.thfha_microphone", stateInstance.Microphone, "mdi:microphone-off");
                }

                oldstattus = stateInstance.Status;
                oldact = stateInstance.Activity;
                oldcam = stateInstance.Camera;
                oldmic = stateInstance.Microphone;
                return;
            }
        }
        public HomeassistantModule()
        {
            // This is the parameterless constructor that will be used by the ModuleManager class
            this.settings = Settings.Instance;


        }
        public void OnFormClosing()
        {
            // Handle the form closing event here
            var isMonitoring = false;
            Log.Debug("Stop monitoring requested");
            if (IsEnabled)
            {
                OnStopMonitoringRequested();

            }
        }
        public HomeassistantModule(State state) : this()
        {
            stateInstance = state;
            stateInstance.StateChanged += OnStateChanged;

            // Initialize your module here

        }
        private void OnStopMonitoringRequested()
        {
            // Stop monitoring here
            var isMonitoring = false;
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
            }
            catch (Exception ex)
            {
                Log.Error("Error updating {entity} in Home assistant: {ex}", entityName, ex.Message);
                client.Dispose();
                return;
            }
            Log.Debug("Response to GET request: {response}", response);

            if (response.IsSuccessStatusCode)
            {
                Log.Information("Updating {entity} in Home assistant to {state}", entityName, stateText);

                var payload = $@"{{
                    ""state"": ""{stateText}"",
                    ""entity_id"": ""{entityName}"",
                    ""attributes"": {{
                        ""icon"": ""{icon}""
                    }}
                }}";

                var content = new StringContent(payload);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                response = await client.PostAsync($"states/{entityName}", content);
                Log.Debug("Response to POST request: {response}", response);
            }
            else
            {
                Log.Error("Error updating {entity} in Home assistant: {status}", entityName, response.StatusCode);
                client.Dispose();
                return;
            }

            response.EnsureSuccessStatusCode();
            client.Dispose();
        }
        public async Task Create_Entity(string entity)
        {
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
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(settings.Haurl + "/api/");
                    client.DefaultRequestHeaders.Add("Authorization", settings.Hatoken);

                    var response = await client.GetAsync($"states/{entity}");

                    return response.IsSuccessStatusCode;
                    client.Dispose();
                }


    }

}
