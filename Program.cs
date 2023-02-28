using Serilog;
using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
namespace THFHA_V1._0
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            var state = new State(); // create a new instance of State
            Application.SetCompatibleTextRenderingDefault(false);
            LoggingConfig.Configure();
            ModuleManager<IModule> moduleManager = new ModuleManager<IModule>();
            List<IModule> modules = moduleManager.Modules;

           
            SettingsForm settingsForm = new SettingsForm(modules);

            // create the LogWatcher and pass the state to it
            var logWatcher = new LogWatcher(state);

            // create the THFHA form and pass the modules and state to it
            var thfha = new THFHA(modules, state);



            Application.Run(thfha);
            Log.CloseAndFlush();
        }
    }
}