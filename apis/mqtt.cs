﻿using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using System.Runtime;
using System.Text;
using System.Timers;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
namespace THFHA_V1._0.apis
{
    public class MqttModule : IModule
    {
        private string name = "MQTT";
        private bool isEnabled = false;
        private MqttFactory factory;
        private Settings settings;
        private System.Timers.Timer connectionCheckTimer = new System.Timers.Timer();
        private IMqttClient client;
        private State stateInstance;
        public event EventHandler? StateChanged;
        public string Name
        {
            get { return name; }
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
                    Log.Debug("mqtt Module has been disabled.");
                    OnStopMonitoringRequested();
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
            return new mqttsettings(); // Replace with your module's settings form
        }
        public void UpdateSettings(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
        private void OnStateChanged(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                stateInstance = (State)sender;
                StateChanged?.Invoke(this, EventArgs.Empty);
                Start(stateInstance);
                _ = PublishMqttUpdate(stateInstance);
                _ = PublishMqttConfig(stateInstance);
            }
            else
            {
                OnStopMonitoringRequested();
            }
        }
        public MqttModule()
        {
            this.settings = Settings.Instance;
            this.factory = new MqttFactory(); // Initialize the factory object
            this.settings = Settings.Instance;
            // This is the parameterless constructor that will be used by the ModuleManager class
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
        public MqttModule(State state) : this()
        {
            stateInstance = state;
            stateInstance.StateChanged += OnStateChanged;
            // Initialize your module here
        }
        
        public async Task Start(State state)
        {
            try
            {
                // dispose the timer if it's already running
                // create a new instance of the client
                client = factory.CreateMqttClient();
                var options = new MqttClientOptionsBuilder()
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithTcpServer(settings.Mqttip)
                .WithCredentials(settings.Mqttusername, settings.Mqttpassword)
                    .Build();
                //start the client
                var connectionsresponse = await client.ConnectAsync(options);
            
            // check if it's started
            await CheckConnection();
            //send the config to broker
                if (connectionsresponse.ResultCode == MQTTnet.Client.MqttClientConnectResultCode.Success)
                {
                    _ = PublishMqttUpdate(state);
                    _ = PublishMqttConfig(state);

                }
            }
            catch (Exception ex)
            {
                Log.Error("Error starting MQTT " + ex.Message);
            }

        }
        /// <summary>
        /// Functionality code below
        /// </summary>
        public enum StatusIcon
        {
            Busy,
            OnThePhone,
            DoNotDisturb,
            Away,
            BeRightBack,
            Available,
            Offline
        }
        private void OnStopMonitoringRequested()
        {
            // Stop monitoring here
            var isMonitoring = false;

            if (client != null)
            {
                client.Dispose();
                Log.Information("MQTTclient disposed");
            }
        }
        public enum ActivityIcon
        {
            InACall,
            OnThePhone,
            Offline,
            InAMeeting,
            InAConferenceCall,
            OutOfOffice,
            NotInACall,
            Presenting
        }
        static string GetStatusIcon(string status)
        {
            return status switch
            {
                "Busy" => "mdi:account-cancel",
                "On the phone" => "mdi:phone-in-talk-outline",
                "Do not disturb" => "mdi:minus-circle-outline",
                "Away" => "mdi:timer-sand",
                "Be right back" => "mdi:timer-sand",
                "Available" => "mdi:account",
                "Offline" => "mdi:account-off",
                _ => "mdi:account-off",
            };
        }
        private async Task CheckConnection()
                {

                }
        static string GetActivityIcon(string activity)
        {
            return activity switch
            {
                "In a call" => "mdi:phone-in-talk-outline",
                "On the phone" => "mdi:phone-in-talk-outline",
                "Offline" => "mdi:account-off",
                "In a meeting" => "mdi:acc",
                "In A Conference Call" => "mdi:phone-in-talk-outline",
                "Out of Office" => "mdi:account-off",
                "Not in a Call" => "mdi:account",
                "Presenting" => "mdi:presentation-play",
                _ => "mdi:account-off",
            };
        }
        private async Task PublishMqttUpdate(State state)
        {
            //PublishMqttConfig();
            if (settings.Mqtttopic == "")
            {
                settings.Mqtttopic = "Default";
            }
            var statusicon = "";
            var activityicon = "";
            var camicon = "";
            var micicon = "";

            var statusIcon = GetStatusIcon(state.Status);
            var activityIcon = GetActivityIcon(state.Activity);

            if (state.Camera == "On")
            {
                camicon = "mdi:camera";
            }
            else
            {
                camicon = "mdi:camera-off";
            }
            if (state.Microphone == "Mute Off")
            {
                micicon = "mdi:microphone";
            }
            else
            {
                micicon = "mdi:microphone-off";
            }
            string StateTopicID = settings.Mqtttopic.ToLower().Replace(" ", "_") + "_teams_monitor";
            string StateTopic = "homeassistant/sensor/" + StateTopicID + "/state";

            var payload = new
            {
                client = new
                {
                    //value = state.Running,   need to fix this
                    value = "Running",
                    icon = new { value = "mdi:television-classic" }
                },
                status = new
                {
                    value = state.Status,
                    icon = new { value = statusicon }
                },
                activity = new
                {
                    value = state.Activity,
                    icon = new { value = activityicon }
                },
                camera = new
                {
                    value = state.Camera,
                },
                microphone = new
                {
                    value = state.Microphone,

                }
            };


            string jsonPayload = JsonConvert.SerializeObject(payload);

            // are we connected?
            if (!client.IsConnected)
            {
                Log.Information("OOOps MQTT not connected {jsonPayload}", jsonPayload);

            }
            try
            {
                Log.Information("Publishing MQTT {jsonPayload}", jsonPayload);
                await client.PublishBinaryAsync(StateTopic, Encoding.UTF8.GetBytes(jsonPayload));
            }
            catch
            {
                //something went wrong
                Log.Information("Error publishing MQTT");
            }
        }
        private async Task PublishMqttConfig(State state)
        {
            if( settings.Mqtttopic == "")
            {
                settings.Mqtttopic = "Default";
            }

            //teamslient, teams status, teams activity, teams camera

            List<String> Sensors = new List<string>() { "client", "status", "activity", "camera", "microphone" };

            foreach (var sensor in Sensors)
            {
                var statusicon = "";
                var activityicon = "";
                var camicon = "";
                var micicon = "";

                switch (state.Status)
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
                switch (state.Activity)
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
                if (state.Camera == "On")
                {
                    camicon = "mdi:camera";
                }
                else
                {
                    camicon = "mdi:camera-off";
                }
                if (state.Microphone == "Mute On")
                {
                    micicon = "mdi:microphone-off";
                }
                else
                {
                    micicon = "mdi:microphone";
                }
                string Sensor = sensor;
                string SensorName = (settings.Mqtttopic + " " + Sensor).Replace(" ", "_");
                string ConfigSensorID = SensorName.ToLower().Replace(" ", "_");
                string UniqueID = ConfigSensorID;
                if (settings.Mqtttopic == null)
                {
                    settings.Mqtttopic = System.Environment.MachineName;
                }
                string StateTopicID = settings.Mqtttopic.ToLower().Replace(" ", "_") + "_teams_monitor";
                string StateTopic = "homeassistant/sensor/" + StateTopicID + "/state";
                string Topic = "homeassistant/sensor/" + ConfigSensorID + "/config";
                //string ValueTemplate = "{{ value_json." + Sensor.ToLower() + "}}";
                string ValueTemplate = "{{ value_json." + Sensor.ToLower() + ".value}}";
                string Icon = "mdi:eye"; // Replace this with the desired icon name
                switch (sensor)
                {
                    case "client":
                        Icon = "mdi:television-classic";
                        break;
                    case "status":
                        Icon = statusicon;
                        break;
                    case "activity":
                        Icon = activityicon;
                        break;
                    case "camera":
                        Icon = camicon;
                        break;
                    case "microphone":
                        Icon = micicon;
                        break;
                    default:
                        Icon = "mdi:eye";
                        break;
                }

                var payload = new
                {
                    name = SensorName,
                    unique_id = UniqueID,
                    state_topic = StateTopic,
                    value_template = ValueTemplate,
                    icon = Icon
                };
                string jsonPayload = JsonConvert.SerializeObject(payload);
                // are we connected?
                if (!client.IsConnected)
                {
                    Log.Information("Ooops MQTT not conected");
                    //TODO Add some retry code here, the problem is that this is triggered before the connect has completed
                    //await AttemptReconnect();
                }

                if (client.IsConnected)
                {
                    try
                    {
                        await client.PublishBinaryAsync(Topic, Encoding.UTF8.GetBytes(jsonPayload));
                    }
                    catch
                    {
                        //something went wrong
                        Log.Error("Error publishing MQTT config");
                    }
                }
            }
        }

    }
}
