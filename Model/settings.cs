using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;

namespace THFHA_V1._0.Model
{
    public class Settings : INotifyPropertyChanged
    {
        private static readonly Settings instance = new Settings();

        public event EventHandler<EventArgs> SettingsUpdated;

        private Settings()
        {
            Load();
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

        // MQTT
        private bool _useMQTT;
        public bool UseMQTT
        {
            get { return _useMQTT; }
            set { _useMQTT = value; OnPropertyChanged(nameof(UseMQTT)); }
        }

        //Phillips Hue
        private bool _useHue;
        public bool UseHue
        {
            get { return _useHue; }
            set { _useHue = value; OnPropertyChanged(nameof(UseHue)); }
        }


        // WLED
        private bool _useWLED;
        public bool UseWLED
        {
            get { return _useWLED; }
            set { _useWLED = value; OnPropertyChanged(nameof(UseWLED)); }
        }

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
    }
}
