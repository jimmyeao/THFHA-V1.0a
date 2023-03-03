using Serilog;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;

namespace THFHA_V1._0.apis
{
    public class HatcherModule : IModule
    {
        #region Private Fields

        private bool isEnabled = false;
        private string name = "Hatcher";
        private Settings settings;
        private State stateInstance;

        #endregion Private Fields

        #region Public Constructors

        public HatcherModule()
        {
            // This is the parameterless constructor that will be used by the ModuleManager class
            settings = Settings.Instance;
        }

        public HatcherModule(State state) : this()
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
                    Log.Debug("Hatcher Module has been disabled.");
                    OnStopMonitoringRequested();
                }
                else
                {
                    Log.Debug("Hatcher Module has been enabled.");
                    _ = ShowImage(stateInstance);
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
            return new hatchersettings(); // Replace with your module's settings form
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

        // fuinctionality here
        public async Task ShowImage(State state)
        {
            if (isEnabled && THFHA.logWatcher?.IsRunning == true)
            {
                if (settings.Hatcherip == "")
                {
                    return;
                }

                //_state.PropertyChanged += State_PropertyChanged;

                string status = state.Status;
                if (state.Activity == "On the phone")
                {
                    status = "On the Phone";
                }

                var uri = new Uri("http://" + settings.Hatcherip + ":5000/showimage");

                Log.Information("Changing Hatcher state to {state} ", status);

                var keyValues = status switch
                {
                    "Available" => new List<KeyValuePair<string, string>>
            {
                new("image_type", "available"),
                new("text1", "Available")
            },
                    "Busy" => new List<KeyValuePair<string, string>>
            {
                new("image_type", "busy"),
                new("text1", "Busy")
            },
                    "Do not disturb" => new List<KeyValuePair<string, string>>
            {
                new("image_type", "dnd"),
                new("text1", "Do Not"),
                new("text2", "Disturb")
            },
                    "Offline" => new List<KeyValuePair<string, string>>
            {
                new("image_type", "offline"),
                new("text1", "Offline")
            },
                    "On the Phone" => new List<KeyValuePair<string, string>>
            {
                new("image_type", "onthephone"),
                new("text1", "On The Phone")
            },
                    "Be Right Back" => new List<KeyValuePair<string, string>>
            {
                new("image_type", "away"),
                new("text1", "Be Right Back")
            },
                    "Away" => new List<KeyValuePair<string, string>>
            {
                new("image_type", "away"),
                new("text1", "Away")
            },
                    ".." => new List<KeyValuePair<string, string>>
            {
                new("image_type", "offline"),
                new("text1", "Offline")
            },
                    _ => new List<KeyValuePair<string, string>>
            {
                new("image_type", "offline"),
                new("text1", "Offline")
            }
                };

                var content = new FormUrlEncodedContent(keyValues);
                using (var client = new HttpClient())
                {
                    try
                    {
                        Task delay = Task.Delay(1000);
                        var response = await client.PostAsync(uri, content);
                        Task delay2 = Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error Setting hatcher state" + ex);
                        new Thread(() => System.Windows.Forms.MessageBox.Show("Hatcher failed." + Environment.NewLine + "Has the IP changed?")).Start();
                    }
                }
            }
        }


        public void Start()
        {
            ShowImage(stateInstance);
        }

        public void UpdateSettings(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnStateChanged(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                stateInstance = (State)sender;
                StateChanged?.Invoke(this, EventArgs.Empty);
                _ = ShowImage(stateInstance);
            }
        }

        private void OnStopMonitoringRequested()
        {
            try
            {
                // Stop monitoring here
                var isMonitoring = false;

                Log.Debug("Stop monitoring requested");
                if (settings.Hatcherip == null)  //just in case we enabled the module with no ip address!
                {
                    return;
                }
                try
                {
                    var uri = new Uri("http://" + settings.Hatcherip + ":5000/showimage");

                    var keyValues = new List<KeyValuePair<string, string>>
        {
            new("image_type", "offline"),
            new("text1", "Offline")
        };
                    var content = new FormUrlEncodedContent(keyValues);
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)); // Timeout after 10 seconds
                                                                                             //var response = await client.PostAsync(uri, content, cts.Token);
                            Task.WaitAll(new Task[] { client.PostAsync(uri, content, cts.Token) });
                            Log.Information("Hatcher state set to offline");
                        }
                        catch (OperationCanceledException)
                        {
                            Log.Error("Error Setting hatcher state: request timed out");
                            new Thread(() => System.Windows.Forms.MessageBox.Show("Hatcher request timed out.")).Start();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error Setting hatcher state" + ex);
                            new Thread(() => System.Windows.Forms.MessageBox.Show("Hatcher failed." + Environment.NewLine + "Has the IP changed?")).Start();
                        }
                    }
                }
                catch
                {
                    Log.Error("Error Setting hatcher state: no ip address");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error Setting hatcher state: " + ex);
            }
        }

        #endregion Private Methods
    }
}