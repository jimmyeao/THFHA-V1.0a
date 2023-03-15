using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using Serilog;
using System.Text;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
using THFHA_V1._0.MyMqttClient;

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
               
            }
            catch (Exception ex)
            {
                Log.Error("Error starting MQTT " + ex.Message);
            }
        }


        public void UpdateSettings(bool isEnabled)
        {
            IsEnabled = isEnabled;
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

        private async Task PublishMqttConfig(State state)
        {
            if (settings.Mqtttopic == "")
            {
                settings.Mqtttopic = "Default";
            }

            //teamslient, teams status, teams activity, teams camera

            List<String> Sensors = new List<string>() { "client", "status", "activity", "camera", "microphone", "blurred", "recording", "handup" };

            foreach (var sensor in Sensors)
            {
                var statusicon = "";
                var activityicon = "";
                var camicon = "";
                var micicon = "";
                var blurredicon = "";
                var recordicon = "";
                var handicon = "";

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
                if (state.Blurred == "Blurred")
                {
                    blurredicon = "mdi:blur";
                }
                else
                {
                    blurredicon = "mdi:blur-off";
                }
                if (state.Recording == "On")
                {
                    recordicon = "mdi:record-rec";
                }
                else
                {
                    recordicon = "mdi:record";
                }
                if (state.Handup == "Raised")
                {
                    handicon = "hand-back-left";
                }
                else
                {
                    handicon = "mdi:hand-back-left-off";
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

                    case "blurred":
                        Icon = blurredicon;
                        break;

                    case "recording":
                        Icon = recordicon;
                        break;

                    case "handup":
                        Icon = handicon;
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
                int retryCount = 0;
                int maxRetryCount = 5;
                TimeSpan delay = TimeSpan.FromSeconds(1);

               

                
                    try
                    {

                    MyMqttClient.MyMqttClient.Instance.SetConnectionParameters(settings.Mqttip, settings.Mqttusername, settings.Mqttpassword);
                    // Connect using the singleton instance
                    await MyMqttClient.MyMqttClient.Instance.ConnectAsync();
                    await MyMqttClient.MyMqttClient.Instance.PublishAsync(Topic, jsonPayload, retain: false);
                       // await client.PublishBinaryAsync(Topic, Encoding.UTF8.GetBytes(jsonPayload));
                    }
                    catch
                    {
                        //something went wrong
                        Log.Error("Error publishing MQTT config");
                    }
                
            }
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
            var blurredicon = "";
            var recordicon = "";
            var handicon = "";

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
            if (state.Blurred == "Blurred")
            {
                blurredicon = "mdi:blur";
            }
            else
            {
                blurredicon = "mdi:blur-off";
            }
            if (state.Recording == "On")
            {
                recordicon = "mdi:record-rec";
            }
            else
            {
                recordicon = "mdi:record";
            }
            if (state.Handup == "Raised")
            {
                handicon = "hand-back-left";
            }
            else
            {
                handicon = "mdi:hand-back-left-off";
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

            // are we connected?
            int retryCount = 0;
            int maxRetryCount = 5;
            TimeSpan delay = TimeSpan.FromSeconds(1);


                try
                {
                    Log.Information("Publishing MQTT {jsonPayload}", jsonPayload);
                    MyMqttClient.MyMqttClient.Instance.SetConnectionParameters(settings.Mqttip, settings.Mqttusername, settings.Mqttpassword);
                    // Connect using the singleton instance
                    await MyMqttClient.MyMqttClient.Instance.ConnectAsync();
                    await MyMqttClient.MyMqttClient.Instance.PublishAsync(StateTopic, jsonPayload, retain: false);
                }
                catch
                {
                    //something went wrong
                    Log.Information("Error publishing MQTT");
                }
            

        }
      



        #endregion Private Methods
    }
}