using Newtonsoft.Json;
using Q42.HueApi;
using Kevsoft.WLED;
using Serilog;
using System.ComponentModel;
using System.Web;
using THFHA_V1._0.Views;

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
                this.Id = light.Id;
                this.Name = light.Name;
                // Other properties
            }
        }


        public Light ToLight()
        {
            return new Light()
            {
                Id = this.Id,
                Name = this.Name,
                // Other properties
            };
        }
    }
    public class Settings : INotifyPropertyChanged
    {
        #region setup
        private static readonly Settings instance = new Settings();
        public event EventHandler<EventArgs> SettingsUpdated;

        private Settings()
        {
            Load();
        }
        public static Settings Instance
        {
            get { return instance; }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
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


        #region Homeassistant
        // Home assistant
        private bool _useHA;
        public bool UseHA
        {
            get { return _useHA; }
            set { _useHA = value; OnPropertyChanged(nameof(UseHA)); }
        }

        private string _hatoken;
        public string Hatoken
        {
            get { return _hatoken; }
            set { _hatoken = value; OnPropertyChanged(nameof(Hatoken)); }
        }
        private string _haurl;
        public string Haurl
        {
            get { return _haurl; }
            set { _haurl = value; OnPropertyChanged(nameof(Haurl)); }
        }
        #endregion
        #region mqtt
        // MQTT
        private bool _useMQTT;
        public bool UseMQTT
        {
            get { return _useMQTT; }
            set { _useMQTT = value; OnPropertyChanged(nameof(UseMQTT)); }
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


        #endregion
        #region Hue
        //Phillips Hue
        private bool _useHue;
        public bool UseHue
        {
            get { return _useHue; }
            set { _useHue = value; OnPropertyChanged(nameof(UseHue)); }
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
        #endregion
        #region WLED

        // WLED
        private bool _useWLED;
        public bool UseWLED
        {
            get { return _useWLED; }
            set { _useWLED = value; OnPropertyChanged(nameof(UseWLED)); }
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
      
        #endregion
        #region operations
        public event PropertyChangedEventHandler PropertyChanged;

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText("settings.json", json);
        }

        public void UpdateSettings()
        {
            SettingsUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void Load()
        {
            if (File.Exists("settings.json"))
            {
                var json = File.ReadAllText("settings.json");
                try
                {
                    JsonConvert.PopulateObject(json, this);
                    Log.Information("Settings loaded");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error loading settings");
                }
            }
        }
        #endregion
    }

}
