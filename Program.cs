using Serilog;
using THFHA_V1._0.apis;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;

namespace THFHA_V1._0
{
    public static class Program
    {
        #region Private Methods

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            SetProcessDPIAware();
            Application.SetCompatibleTextRenderingDefault(false);
            LoggingConfig.Configure();

            //var state = new State(); // create a new instance of State
            State state = State.Instance;
            ModuleManager<IModule> moduleManager = new ModuleManager<IModule>(state);
            List<IModule> modules = moduleManager.Modules;
            _ = new SettingsForm(modules);
            // create the LogWatcher and pass the state to it
            _ = new LogWatcher(state);
            // create the THFHA form and pass the modules and state to it
            var thfha = new THFHA(modules, state);
            _ = new HatcherModule(state);
            _ = new HomeassistantModule(state);
            _ = new HueModule(state);
            Application.Run(thfha);
            Log.CloseAndFlush();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        #endregion Private Methods
    }
}