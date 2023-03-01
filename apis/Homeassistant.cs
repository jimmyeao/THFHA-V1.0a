using Serilog;
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
                    Log.Debug("HomeAssistanrt Module has been disabled.");
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
            return new hasettings(); // Replace with your module's settings form
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
            }
        }

        public HomeassistantModule()
        {
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
    }

}
