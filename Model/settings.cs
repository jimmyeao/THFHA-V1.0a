using Newtonsoft.Json;
using Q42.HueApi;
using Serilog;
using System.ComponentModel;

namespace THFHA_V1._0.Model
{
    public class CustomLight
    {
        public string Id { get; set; }
        public string Name { get; set; }
        // Other properties

        public CustomLight(Light light)
        {
            if (light != null)
            {
                Id = light.Id;
                Name = light.Name;
                // Other properties
            }
        }

        public Light ToLight()
        {
            return new Light()
            {
                Id = Id,
                Name = Name,
                // Other properties
            };
        }
    } //custom light class used for Hue lights

    public class Settings : INotifyPropertyChanged
    {
        #region setup

        private static readonly Settings instance = new Settings();

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<EventArgs> SettingsUpdated;

        public delegate void SettingChangedEventHandler(object sender, SettingChangedEventArgs e);

        public static event SettingChangedEventHandler SettingChanged;

        private Settings()
        {
            Load();
        }

        public static Settings Instance
        {
            get { return instance; }
        }

        public class SettingChangedEventArgs : EventArgs
        {
            public string SettingName { get; set; }

            public SettingChangedEventArgs(string settingName)
            {
                SettingName = settingName;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool _runLogWatcherAtStart;

        public bool RunLogWatcherAtStart
        {
            get { return _runLogWatcherAtStart; }
            set { _runLogWatcherAtStart = value; OnPropertyChanged(nameof(RunLogWatcherAtStart)); }
        }

        private bool _autorun = false;

        public bool Autorun
        {
            get { return _autorun; }
            set { _autorun = value; OnPropertyChanged(nameof(Autorun)); }
        }

        #endregion setup

        #region Homeassistant

        // Home assistant
        private bool _useHA;

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

        private bool _ishaModuleSettingsValid;

        public bool IsHomeassistantModuleSettingsValid
        {
            get { return _ishaModuleSettingsValid; }
            set { _ishaModuleSettingsValid = value; OnPropertyChanged(nameof(IsHomeassistantModuleSettingsValid)); }
        }

        private string _hatoken;

        public string Hatoken
        {
            get { return _hatoken; }
            set
            {
                _hatoken = value; OnPropertyChanged(nameof(Hatoken));
            }
        }

        private string _haurl;

        public string Haurl
        {
            get { return _haurl; }
            set { _haurl = value; OnPropertyChanged(nameof(Haurl)); }
        }

        #endregion Homeassistant

        #region mqtt

        // MQTT
        private bool _useMQTT;

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

        private bool _ismqttModuleSettingsValid;

        public bool IsMqttModuleSettingsValid
        {
            get { return _ismqttModuleSettingsValid; }
            set { _ismqttModuleSettingsValid = value; OnPropertyChanged(nameof(IsMqttModuleSettingsValid)); }
        }

        private string _mqttip;

        public string Mqttip
        {
            get { return _mqttip; }
            set { _mqttip = value; OnPropertyChanged(nameof(Mqttip)); }
        }

        private string _mqttusername;

        public string Mqttusername
        {
            get { return _mqttusername; }
            set { _mqttusername = value; OnPropertyChanged(nameof(Mqttusername)); }
        }

        private string _mqttpassword;

        public string Mqttpassword
        {
            get { return _mqttpassword; }
            set { _mqttpassword = value; OnPropertyChanged(nameof(Mqttpassword)); }
        }

        private string _mqtttopic;

        public string Mqtttopic
        {
            get { return _mqtttopic; }
            set { _mqtttopic = value; OnPropertyChanged(nameof(Mqtttopic)); }
        }

        #endregion mqtt

        #region Hue

        //Phillips Hue
        private bool _useHue;

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

        private bool _isHueModuleSettingsValid;

        public bool IsHueModuleSettingsValid
        {
            get { return _isHueModuleSettingsValid; }
            set { _isHueModuleSettingsValid = value; OnPropertyChanged(nameof(IsHueModuleSettingsValid)); }
        }

        private string _hueip;

        public string Hueip
        {
            get { return _hueip; }
            set { _hueip = value; OnPropertyChanged(nameof(Hueip)); }
        }

        private string _hueusername;

        public string Hueusername
        {
            get { return _hueusername; }
            set { _hueusername = value; OnPropertyChanged(nameof(Hueusername)); }
        }

        private List<CustomLight> _HueLight = new List<CustomLight>();

        public List<CustomLight> HueLight
        {
            get { return _HueLight; }
            set { _HueLight = value; OnPropertyChanged(nameof(HueLight)); }
        }

        private string _selectedLightId;

        public string SelectedLightId
        {
            get { return _selectedLightId; }
            set { _selectedLightId = value; OnPropertyChanged(nameof(SelectedLightId)); }
        }

        #endregion Hue

        #region WLED

        // WLED
        private bool _useWLED;

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

        private bool _isWledModuleSettingsValid;

        public bool IsWledModuleSettingsValid
        {
            get { return _isWledModuleSettingsValid; }
            set { _isWledModuleSettingsValid = value; OnPropertyChanged(nameof(IsWledModuleSettingsValid)); }
        }

        public WLED SelectedWled { get; set; } // Add this property to store the selected WLED light.
        private List<WLED> _wledDevices = new List<WLED>();

        public List<WLED> WledDevices
        {
            get { return _wledDevices; }
            set { _wledDevices = value; OnPropertyChanged(nameof(WledDevices)); }
        }

        private string _WledDev = "";

        public string WledDev
        {
            get { return _WledDev; }
            set { _WledDev = value; OnPropertyChanged(nameof(WledDev)); }
        }

        private string _wledip = "";

        public string WLEDIP
        {
            get { return _wledip; }
            set { _wledip = value; OnPropertyChanged(nameof(WLEDIP)); }
        }

        #endregion WLED

        #region Hatcher

        private bool _usehatcher;

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

        private bool _isHatcherModuleSettingsValid;

        public bool IsHatcherModuleSettingsValid
        {
            get { return _isHatcherModuleSettingsValid; }
            set { _isHatcherModuleSettingsValid = value; OnPropertyChanged(nameof(IsHatcherModuleSettingsValid)); }
        }

        private string _hatcherip;

        public string Hatcherip
        {
            get { return _hatcherip; }
            set { _hatcherip = value; OnPropertyChanged(nameof(Hatcherip)); }
        }

        #endregion Hatcher

        #region operations

        // public event PropertyChangedEventHandler PropertyChanged;
  public void UpdateSettings()
        {
            SettingsUpdated?.Invoke(this, EventArgs.Empty);
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

        #endregion operations
    }
}