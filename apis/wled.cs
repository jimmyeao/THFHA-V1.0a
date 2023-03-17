using Newtonsoft.Json;
using Serilog;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;

namespace THFHA_V1._0.apis
{
    public class WledModule : IModule
    {
        #region Private Fields

        private readonly SemaphoreSlim _colorLock;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly object _lockObject = new object();
        private dynamic _currentState = new ExpandoObject();
        private bool _enabled = false;
        private dynamic _originalState = new ExpandoObject();
        private bool isEnabled = false;
        private string name = "Wled";
        private Settings settings;
        private State stateInstance;
        private bool staterecorded = false;

        #endregion Private Fields

        #region Public Constructors

        public WledModule()
        {
            // This is the parameterless constructor that will be used by the ModuleManager class
            settings = Settings.Instance;
            _colorLock = new SemaphoreSlim(1);
        }

        public WledModule(State state) : this()
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
                    Log.Debug("WledModule has been disabled.");
                    OnStopMonitoringRequested();
                }
                else
                {
                    Log.Debug("WledModule has been enabled.");
                    _originalState = GetCurrentState(settings.SelectedWled.Ip);
                    Start();
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

        public async Task ChangeColor(string newColor)
        {
            await _colorLock.WaitAsync();
            try
            {
                // Creating a payload to send to the WLED light
                var colorArray = newColor.Split(',').Select(x => int.Parse(x)).ToArray();
                var payload = new { on = true, seg = new[] { new { col = new[] { colorArray } } } };
                var jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload, new JsonSerializerOptions());
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                // Sending a PUT request to change the color of the WLED light
                using var client = new HttpClient();
                //TODO change below to use the ip instead of the .local name.
                var response = await client.PutAsync($"http://{settings.SelectedWled.Ip}/json/state", content);
                Log.Information("Changing state of {WledDev} to {jsonPayload}", settings.SelectedWled.Name.ToString(), jsonPayload);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Log.Information("Error changing state of WLED light {responseContent}", responseContent.ToString());
                }
            }
            catch (HttpRequestException e)
            {
                Log.Error("Error connecting to {WLEDDev}: {Message}", settings.SelectedWled.Name.ToString(), e.Message.ToString());
            }
            catch (FormatException e)
            {
                Log.Error("Error parsing color value {messsage}", e.Message.ToString());
            }
            finally
            {
                _colorLock.Release();
            }
        }

        public async Task<dynamic> GetCurrentState(string ip)
        {
            if (staterecorded == false)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"http://{settings.SelectedWled.Ip}/json/state");
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    Log.Information("Current state of WLED light {wledDev}: {state}", settings.WledDev.ToString(), json);
                    _currentState = JsonConvert.DeserializeObject<dynamic>(json);

                    if (_currentState == null)
                    {
                        _originalState = _currentState;
                        SaveState();
                    }

                    if (!staterecorded)
                    {
                        SaveState();
                        staterecorded = true;
                    }

                    return _currentState;
                }
                catch (HttpRequestException e)
                {
                    Log.Error("Error connecting to {WLEDDev}: {Message}", settings.WledDev.ToString(), e.Message);
                }
                catch (System.Text.Json.JsonException e)
                {
                    Log.Error("Error deserializing JSON response: {Message}", e.Message);
                }
                catch (Exception ex)
                {
                    Log.Error("Error getting current state: {Message}", ex.Message);
                }
            }

            return _currentState;
        }

        public Form GetSettingsForm()
        {
            return new wledsettings(); // Replace with your module's settings form
        }

        public async Task OnFormClosing()
        {
            stateInstance.StateChanged -= OnStateChanged;
         //   RestoreState();
            // Handle the form closing event here
            var isMonitoring = false;
            Log.Debug("Stop Wled monitoring requested");
            if (IsEnabled)
            {
                OnStopMonitoringRequested();
            }
        }

        public async Task RestoreState()
        {
            try
            {
                LoadState();
                var json = JsonConvert.SerializeObject(_originalState);
                Log.Information("Restoring state of WLED light {wledDev}: {state}", settings.WledDev.ToString(), json);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                var response = await client.PutAsync($"http://{settings.SelectedWled.Ip}/json/state", data);
                response.EnsureSuccessStatusCode();
                Log.Information($"{response.StatusCode}");
                _enabled = false;
            }
            catch (HttpRequestException e)
            {
                Log.Error("Error connecting to {WLEDDev}: {Message}", settings.WledDev.ToString(), e.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Error restoring state: {Message}", ex.Message);
            }
        }

        public async void Start()
        {
            if (isEnabled && THFHA.logWatcher?.IsRunning == true)
            {
                _originalState = GetCurrentState(settings.SelectedWled.Ip);
                //stateInstance = (State)sender;
                var status = stateInstance.Status;
                if (stateInstance.Activity == "On the phone" || stateInstance.Activity == "In a call" || stateInstance.Activity == "In a meeting")
                {
                    status = "On the Phone";
                }
                StateChanged?.Invoke(this, EventArgs.Empty);
                switch (status)
                {
                    case "Busy":
                        await ChangeColor("255,0,0");
                        break;

                    case "On the phone":
                        await ChangeColor("255,0,0");
                        break;

                    case "Do not disturb":
                        await ChangeColor("255,0,0");
                        break;

                    case "Away":
                        await ChangeColor("255,255,0");
                        break;

                    case "Be right back":
                        await ChangeColor("255,255,0");
                        break;

                    case "Available":
                        await ChangeColor("0,255,0");
                        break;

                    case "Offline":
                        await ChangeColor("0,0,0");
                        break;
                }
            }
        }

        public void UpdateSettings(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadState()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(appDataFolder, "TeamsHelper");
            string filePath = Path.Combine(folderPath, "wledstate.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                _originalState = JsonConvert.DeserializeObject<dynamic>(json);
            }
            else
            {
                _originalState = new ExpandoObject();
            }
        }

        private async void OnStateChanged(object sender, EventArgs e)
        {
            if (isEnabled)
            {
                if (_currentState == null && staterecorded == false)
                {
                    _originalState = await GetCurrentState(settings.SelectedWled.Ip);
                    staterecorded = true;
                }
                if (IsEnabled && THFHA.logWatcher?.IsRunning == true)
                {
                    stateInstance = (State)sender;
                    var status = stateInstance.Status;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                    //if we are in a call we are always busy...
                    //so we need to check the activity as well
                    if (stateInstance.Activity == "On the phone" || stateInstance.Activity == "In a call" || stateInstance.Activity == "In a meeting")
                    {
                        status = "On the Phone";
                    }

                    switch (status)
                    {
                        case "Busy":
                            await ChangeColor("255,0,0");
                            break;

                        case "On the phone":
                            await ChangeColor("255,0,0");
                            break;

                        case "On the Phone":
                            await ChangeColor("255,0,0");
                            break;

                        case "Do not disturb":
                            await ChangeColor("255,0,0");
                            break;

                        case "Away":
                            await ChangeColor("255,255,0");
                            break;

                        case "Be right back":
                            await ChangeColor("255,255,0");
                            break;

                        case "Available":
                            await ChangeColor("0,255,0");
                            break;

                        case "Offline":
                            await ChangeColor("0,0,0");
                            break;
                    }
                }
            }
        }

        private void OnStopMonitoringRequested()
        {
            if (settings.IsWledModuleSettingsValid)
            {
                try
                {
                    // Stop monitoring here
                    var isMonitoring = false;
                    LoadState();
                    _ = RestoreState();
                }
                catch (Exception ex)
                {
                    Log.Error("Something went wrong stopping WLED");
                }
            }
        }
        public void Stop()
        {
            var isMonitoring = false;
            Log.Debug("Stop wled monitoring requested");
            if (IsEnabled)
            {
                OnStopMonitoringRequested();
            }
        }

        private void SaveState()
        {
            // Get the path to the local user data folder
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(appDataFolder, "TeamsHelper");
            string filePath = Path.Combine(folderPath, "wledstate.json");

            // Create the folder if it doesn't exist
            Directory.CreateDirectory(folderPath);

            string json = JsonConvert.SerializeObject(_currentState);
            File.WriteAllText(filePath, json);
        }

        #endregion Private Methods
    }
}