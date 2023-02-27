using THFHA_V1._0.Model;

namespace THFHA_V1._0.apis
{
    public class WledModule : IModule
    {
        private string name = "Wled";
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

        public WledModule()
        {
            // Initialize your module here
        }
    }

}
