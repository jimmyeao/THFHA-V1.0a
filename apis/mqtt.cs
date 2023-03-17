using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;

namespace THFHA_V1._0.apis
{
    public class MqttModule : IModule
    {
        #region Private Fields

        private IMqttClient client;
        private System.Timers.Timer connectionCheckTimer = new System.Timers.Timer();
        private MqttFactory factory;
        private bool isEnabled = false;
        private string name = "Mqtt";
        private Settings settings;
        private State stateInstance;

        #endregion Private Fields

        #region Public Constructors

        public MqttModule()
        {
            // This is the parameterless constructor that will be used by the ModuleManager class
        }

        public MqttModule(State state) : this()
        {
            stateInstance = state;
            settings = Settings.Instance;
            // Initialize your module here
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler? StateChanged;

        #endregion Public Events

        #region Public Enums

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

        #endregion Public Enums

        #region Public Properties

        private static readonly List<string> SensorNames = new List<string> { "client", "status", "activity", "camera", "microphone", "blurred", "recording", "handup" };

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
                    if (!settings.IsMqttModuleSettingsValid)
                    {
                        return;  //no point doing anything if its not enabled!
                    }
                    OnStopMonitoringRequested();
                }
                else
                {
                    stateInstance.StateChanged += OnStateChanged;
                    _ = Start(stateInstance);
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

        private static List<string> GetSensorNames()
        {
            return SensorNames;
        }

        #endregion Public Properties

        #region Public Methods

        public Form GetSettingsForm()
        {
            return new mqttsettings(); // Replace with your module's settings form
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

        public void Start()
        {
            _ = Start(stateInstance);
        }

        public void UpdateSettings(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        private async Task Start(State state)
        {
            try
            {
                int retryCount = 0;
                int maxRetryCount = 5;
                TimeSpan delay = TimeSpan.FromSeconds(1);
                MyMqttClient.MyMqttClient.Instance.SetConnectionParameters(settings.Mqttip, settings.Mqttusername, settings.Mqttpassword);

                // Connect using the singleton instance
                await MyMqttClient.MyMqttClient.Instance.ConnectAsync();
                _ = PublishMqttUpdate(stateInstance);
                _ = PublishMqttConfig(stateInstance);
            }
            catch (Exception ex)
            {
                Log.Error("Error starting MQTT " + ex.Message);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetActivityIcon(string activity)
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

        private static string GetStatusIcon(string status)
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

        private string GetIcon(string iconName, string state, string onValue, string offValue)
        {
            string icon;
            if (state == onValue)
            {
                icon = iconName;
            }
            else if (state == offValue)
            {
                icon = $"{iconName}-off";
            }
            else
            {
                icon = "mdi:eye";
            }
            return icon;
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            if (IsEnabled && THFHA.logWatcher?.IsRunning == true)
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

        private async void OnStopMonitoringRequested()
        {
            // Stop monitoring here
            
            var activityicon = "mdi:account-off";
            var statusicon = "mdi:account-off";
            var status = stateInstance;
            status.Status = "Not Running";
            status.Activity = "Not Running";
            await PublishMqttConfig(status);
           // await PublishMqttUpdate(status);
            var isMonitoring = false;

            if (client != null)
            {
                client.Dispose();
                Log.Information("MQTTclient disposed");
            }
        }

        private async Task PublishMqttConfig(State state)
        {
            // Use a constant for the list of sensors

            if (string.IsNullOrEmpty(settings.Mqtttopic))
            {
                // Use the null-coalescing operator instead of an if statement
                settings.Mqtttopic = "Default";
            }

            foreach (var sensor in SensorNames)
            {
                string sensorName = $"{settings.Mqtttopic} {sensor}".Replace(" ", "_");
                string configSensorId = sensorName.ToLower().Replace(" ", "_");
                string uniqueId = configSensorId;
                string stateTopicId = $"{settings.Mqtttopic.ToLower().Replace(" ", "_")}_teams_monitor";
                string stateTopic = $"homeassistant/sensor/{stateTopicId}/state";
                string topic = $"homeassistant/sensor/{configSensorId}/config";
                string valueTemplate = $"{{{{ value_json.{sensor.ToLower()}.value }}}}";
                string icon;

                // Use a switch expression instead of a switch statement
                icon = sensor switch
                {
                    "client" => "mdi:television-classic",
                    "status" => GetStatusIcon(state.Status),
                    "activity" => GetActivityIcon(state.Activity),
                    "camera" => state.Camera == "On" ? "mdi:camera" : "mdi:camera-off",
                    "microphone" => state.Microphone == "Mute On" ? "mdi:microphone-off" : "mdi:microphone",
                    "blurred" => state.Blurred == "Blurred" ? "mdi:blur" : "mdi:blur-off",
                    "recording" => state.Recording == "On" ? "mdi:record-rec" : "mdi:record",
                    "handup" => state.Handup == "Raised" ? "hand-back-left" : "mdi:hand-back-left-off",
                    _ => "mdi:eye"
                };
                

                var payload = new
                {
                    name = sensorName,
                    unique_id = uniqueId,
                    state_topic = stateTopic,
                    value_template = valueTemplate,
                    icon = icon
                };

                string jsonPayload = JsonConvert.SerializeObject(payload);

                try
                {
                    await MyMqttClient.MyMqttClient.Instance.PublishAsync(settings.Mqtttopic, jsonPayload, retain: false);
                }
                catch
                {
                    Log.Error("Error publishing MQTT config");
                }
            }
        }

        private async Task PublishMqttUpdate(State state)
        {
            if (settings.Mqtttopic == "")
            {
                settings.Mqtttopic = "Default";
            }

            var statusIcon = GetStatusIcon(state.Status);
            var activityIcon = GetActivityIcon(state.Activity);
            var camIcon = state.Camera == "On" ? "mdi:camera" : "mdi:camera-off";
            var micIcon = state.Microphone == "Mute Off" ? "mdi:microphone" : "mdi:microphone-off";
            var blurredIcon = state.Blurred == "Blurred" ? "mdi:blur" : "mdi:blur-off";
            var recordIcon = state.Recording == "On" ? "mdi:record-rec" : "mdi:record";
            var handIcon = state.Handup == "Raised" ? "hand-back-left" : "mdi:hand-back-left-off";

            string stateTopicID = settings.Mqtttopic.ToLower().Replace(" ", "_") + "_teams_monitor";
            string stateTopic = "homeassistant/sensor/" + stateTopicID + "/state";

            var payload = new
            {
                client = new
                {
                    value = "Running",
                    icon = new { value = "mdi:television-classic" }
                },
                status = new
                {
                    value = state.Status,
                    icon = new { value = statusIcon }
                },
                activity = new
                {
                    value = state.Activity,
                    icon = new { value = activityIcon }
                },
                camera = new
                {
                    value = state.Camera,
                },
                microphone = new
                {
                    value = state.Microphone,
                },
                blurred = new
                {
                    value = state.Blurred,
                },
                recording = new
                {
                    value = state.Recording,
                },
                handup = new
                {
                    value = state.Handup,
                }
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);

            // Retry connection if it fails
            int retryCount = 0;
            int maxRetryCount = 5;
            TimeSpan delay = TimeSpan.FromSeconds(1);

            while (retryCount < maxRetryCount)
            {
                try
                {
                    Log.Information("Publishing MQTT {jsonPayload}", jsonPayload);
                    await MyMqttClient.MyMqttClient.Instance.PublishAsync(stateTopic, jsonPayload, retain: false);
                    break; // Exit loop if successful
                }
                catch (Exception ex)
                {
                    Log.Error("Error publishing MQTT: {ex}", ex.Message);
                    await Task.Delay(delay);
                    delay = TimeSpan.FromSeconds(delay.TotalSeconds * 2); // Exponential backoff
                    retryCount++;
                }
            }
        }

        #endregion Private Methods
    }
}