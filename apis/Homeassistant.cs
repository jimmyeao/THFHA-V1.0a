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
            set { isEnabled = value; }
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
            // handle state changed event here
            stateInstance = (State)sender;

            // raise the StateChanged event
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
   
        public HomeassistantModule()
        {
            // This is the parameterless constructor that will be used by the ModuleManager class
        }

        public HomeassistantModule(State state) : this()
        {
            stateInstance = state;
            stateInstance.StateChanged += OnStateChanged;
            // Initialize your module here
        }
    }

}
