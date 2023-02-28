using THFHA_V1._0.Model;
using THFHA_V1._0.Views;

namespace THFHA_V1._0.apis
{
    public class hatchermodule : IModule
    {
        private string name = "Hatcher";
        private bool isEnabled = false;
        private string state = "Disconnected";

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
            get { return state; }
            set { state = value; }
        }

        public Form GetSettingsForm()
        {
            return new hatcheersettings(); // Replace with your module's settings form
        }
        public hatchermodule()
        {
            // Initialize your module here
        }
    }
}
