using Newtonsoft.Json;
using Q42.HueApi;
using Serilog;
using System.ComponentModel;

namespace THFHA_V1._0.Model
{
    public class CustomLight
    {
        #region Public Constructors

        public CustomLight(Light light)
        {
            if (light != null)
            {
                Id = light.Id;
                Name = light.Name;
                // Other properties
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public string Id { get; set; }
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        // Other properties
        public Light ToLight()
        {
            return new Light()
            {
                Id = Id,
                Name = Name,
                // Other properties
            };
        }

        #endregion Public Methods
    } //custom light class used for Hue lights

    public class Settings : INotifyPropertyChanged
    {
        #region setup

        private static readonly Settings instance = new Settings();

        private bool _autorun = false;

        private bool _runLogWatcherAtStart;

        private Settings()
        {
            Load();
        }

        public delegate void SettingChangedEventHandler(object sender, SettingChangedEventArgs e);

        public static event SettingChangedEventHandler SettingChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<EventArgs> SettingsUpdated;

        public static Settings Instance
        {
            get { return instance; }
        }

        public bool Autorun
        {
            get { return _autorun; }
            set { _autorun = value; OnPropertyChanged(nameof(Autorun)); }
        }

        public bool RunLogWatcherAtStart
        {
            get { return _runLogWatcherAtStart; }
            set { _runLogWatcherAtStart = value; OnPropertyChanged(nameof(RunLogWatcherAtStart)); }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class SettingChangedEventArgs : EventArgs
        {
            #region Public Constructors

            public SettingChangedEventArgs(string settingName)
            {
                SettingName = settingName;
            }

            #endregion Public Constructors

            #region Public Properties

            public string SettingName { get; set; }

            #endregion Public Properties
        }

        #endregion setup

        #region Homeassistant

        private string _hatoken;

        private string _haurl;

        private bool _ishaModuleSettingsValid;

        // Home assistant
        private bool _useHA;

        public string Hatoken
        {
            get { return _hatoken; }
            set
            {
                _hatoken = value; OnPropertyChanged(nameof(Hatoken));
            }
        }

        public string Haurl
        {
            get { return _haurl; }
            set { _haurl = value; OnPropertyChanged(nameof(Haurl)); }
        }

        public bool IsHomeassistantModuleSettingsValid
        {
            get { return _ishaModuleSettingsValid; }
            set { _ishaModuleSettingsValid = value; OnPropertyChanged(nameof(IsHomeassistantModuleSettingsValid)); }
        }

        public bool UseHA
        {
            get { return _useHA; }
            set
            {
                _useHA = value;
                OnPropertyChanged(nameof(UseHA));
                SettingChanged?.Invoke(this, new SettingChangedEventArgs(nameof(UseHA)));
            }
        }

        #endregion Homeassistant

        #region mqtt

        private bool _ismqttModuleSettingsValid;

        private string _mqttip;

        private string _mqttpassword;

        private string _mqtttopic;

        private string _mqttusername;

        // MQTT
        private bool _useMQTT;

        public bool IsMqttModuleSettingsValid
        {
            get { return _ismqttModuleSettingsValid; }
            set { _ismqttModuleSettingsValid = value; OnPropertyChanged(nameof(IsMqttModuleSettingsValid)); }
        }

        public string Mqttip
        {
            get { return _mqttip; }
            set { _mqttip = value; OnPropertyChanged(nameof(Mqttip)); }
        }

        public string Mqttpassword
        {
            get { return _mqttpassword; }
            set { _mqttpassword = value; OnPropertyChanged(nameof(Mqttpassword)); }
        }

        public string Mqtttopic
        {
            get { return _mqtttopic; }
            set { _mqtttopic = value; OnPropertyChanged(nameof(Mqtttopic)); }
        }

        public string Mqttusername
        {
            get { return _mqttusername; }
            set { _mqttusername = value; OnPropertyChanged(nameof(Mqttusername)); }
        }

        public bool UseMQTT
        {
            get { return _useMQTT; }
            set
            {
                _useMQTT = value;
                OnPropertyChanged(nameof(UseMQTT));
                SettingChanged?.Invoke(this, new SettingChangedEventArgs(nameof(UseMQTT))); // pass the EventArgs parameter
            }
        }

        #endregion mqtt

        #region Hue

        private string _hueip;

        private List<CustomLight> _HueLight = new List<CustomLight>();

        private string _hueusername;

        private bool _isHueModuleSettingsValid;

        private string _selectedLightId;

        //Phillips Hue
        private bool _useHue;

        public string Hueip
        {
            get { return _hueip; }
            set { _hueip = value; OnPropertyChanged(nameof(Hueip)); }
        }

        public List<CustomLight> HueLight
        {
            get { return _HueLight; }
            set { _HueLight = value; OnPropertyChanged(nameof(HueLight)); }
        }

        public string Hueusername
        {
            get { return _hueusername; }
            set { _hueusername = value; OnPropertyChanged(nameof(Hueusername)); }
        }

        public bool IsHueModuleSettingsValid
        {
            get { return _isHueModuleSettingsValid; }
            set { _isHueModuleSettingsValid = value; OnPropertyChanged(nameof(IsHueModuleSettingsValid)); }
        }

        public string SelectedLightId
        {
            get { return _selectedLightId; }
            set { _selectedLightId = value; OnPropertyChanged(nameof(SelectedLightId)); }
        }

        public bool UseHue
        {
            get { return _useHue; }
            set
            {
                _useHue = value;
                OnPropertyChanged(nameof(UseHue));
                SettingChanged?.Invoke(this, new SettingChangedEventArgs(nameof(UseHue))); // pass the EventArgs parameter
            }
        }

        #endregion Hue

        #region WLED

        private bool _isWledModuleSettingsValid;

        // WLED
        private bool _useWLED;

        private string _WledDev = "";

        private List<WLED> _wledDevices = new List<WLED>();

        private string _wledip = "";

        public bool IsWledModuleSettingsValid
        {
            get { return _isWledModuleSettingsValid; }
            set { _isWledModuleSettingsValid = value; OnPropertyChanged(nameof(IsWledModuleSettingsValid)); }
        }

        public WLED SelectedWled { get; set; }

        public bool UseWLED
        {
            get { return _useWLED; }
            set
            {
                _useWLED = value;
                OnPropertyChanged(nameof(UseWLED));
                SettingChanged?.Invoke(this, new SettingChangedEventArgs(nameof(UseWLED))); // pass the EventArgs parameter
            }
        }

        public string WledDev
        {
            get { return _WledDev; }
            set { _WledDev = value; OnPropertyChanged(nameof(WledDev)); }
        }

        // Add this property to store the selected WLED light.
        public List<WLED> WledDevices
        {
            get { return _wledDevices; }
            set { _wledDevices = value; OnPropertyChanged(nameof(WledDevices)); }
        }

        public string WLEDIP
        {
            get { return _wledip; }
            set { _wledip = value; OnPropertyChanged(nameof(WLEDIP)); }
        }

        #endregion WLED

        #region Hatcher

        private string _hatcherip;
        private bool _isHatcherModuleSettingsValid;
        private bool _usehatcher;

        public string Hatcherip
        {
            get { return _hatcherip; }
            set { _hatcherip = value; OnPropertyChanged(nameof(Hatcherip)); }
        }

        public bool IsHatcherModuleSettingsValid
        {
            get { return _isHatcherModuleSettingsValid; }
            set { _isHatcherModuleSettingsValid = value; OnPropertyChanged(nameof(IsHatcherModuleSettingsValid)); }
        }

        public bool UseHatcher
        {
            get { return _usehatcher; }
            set
            {
                _usehatcher = value;
                OnPropertyChanged(nameof(UseHatcher));
                SettingChanged?.Invoke(this, new SettingChangedEventArgs(nameof(UseHatcher))); // pass the EventArgs parameter
            }
        }

        #endregion Hatcher

        #region Teams
        private string _temasapikey;
        public string TeamsApi
        {
            get { return _temasapikey; }
            set { _temasapikey = value; OnPropertyChanged(nameof(TeamsApi)); }
        }
        #endregion

        #region operations

        public void Load()
        {
            try
            {
                // Get the path to the local user data folder
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string folderPath = Path.Combine(appDataFolder, "TeamsHelper");
                string filePath = Path.Combine(folderPath, "settings.json");

                // Load the settings from the file
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    JsonConvert.PopulateObject(json, this);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error loading settings: " + ex.Message);
            }
        }

        public void Save()
        {
            try
            {
                // Get the path to the local user data folder
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string folderPath = Path.Combine(appDataFolder, "TeamsHelper");
                string filePath = Path.Combine(folderPath, "settings.json");

                // Create the folder if it doesn't exist
                Directory.CreateDirectory(folderPath);

                // Serialize the settings to JSON
                string json = JsonConvert.SerializeObject(this);

                // Write the JSON to the file
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Log.Error("Error saving settings: " + ex.Message);
            }
        }

        // public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateSettings()
        {
            SettingsUpdated?.Invoke(this, EventArgs.Empty);
        }

        #endregion operations
    }
}