using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
using Serilog;
namespace THFHA_V1._0.apis
{
    public class HueModule : IModule
    {
        private string name = "Hue";
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
            set { isEnabled = value; }
        }

        public string State
        {
            get { return stateInstance.ToString(); }
            set { /* You can leave this empty since the State property is read-only */ }
        }
        public Form GetSettingsForm()
        {
            return new huesettings(); // Replace with your module's settings form
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
        public HueModule()
        {
            // This is the parameterless constructor that will be used by the ModuleManager class
        }
        public void OnFormClosing()
        {
            // Handle the form closing event here
            var isMonitoring = false;
            Log.Debug("Stop monitoring requested");
        }

        public HueModule(State state) : this()
        {
            stateInstance = state;
            stateInstance.StateChanged += OnStateChanged;
            // Initialize your module here
        }
    }

}
