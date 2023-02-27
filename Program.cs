using THFHA_V1._0.Model;
using THFHA_V1._0.Views;
using Serilog;
namespace THFHA_V1._0
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoggingConfig.Configure();
            ModuleManager<IModule> moduleManager = new ModuleManager<IModule>();
            List<IModule> modules = moduleManager.Modules;

            THFHA mainForm = new THFHA(modules);
            SettingsForm settingsForm = new SettingsForm(modules);

            Application.Run(mainForm);
            Log.CloseAndFlush();
        }
    }
}