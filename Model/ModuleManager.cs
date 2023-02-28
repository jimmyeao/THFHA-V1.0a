using System.Reflection;

namespace THFHA_V1._0.Model
{
    public class ModuleManager<T> where T : IModule
    {
        private List<T> modules;
        private State state;

        public List<T> Modules
        {
            get { return modules; }
        }

        public ModuleManager(State state)
        {
            this.state = state;
            modules = new List<T>();
            LoadModules();
        }

        private void LoadModules()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                if (typeof(T).IsAssignableFrom(type) && !type.IsAbstract && type.Namespace == "THFHA_V1._0.apis")
                {
                    T module = CreateInstance<T>(type, state);
                    modules.Add(module);
                }
            }
        }

        private T CreateInstance<T>(Type type, State state) where T : IModule
        {
            if (type.GetConstructor(new Type[] { typeof(State) }) == null)
            {
                throw new ArgumentException($"Type {type.FullName} does not have a public constructor that takes a State parameter.");
            }

            return (T)Activator.CreateInstance(type, state);
        }
    }




    public interface IModule
    {
        string Name { get; }
        bool IsEnabled { get; set; }
        string State { get; }
        event EventHandler StateChanged; // Add StateChanged event to interface

        Form GetSettingsForm();

        void UpdateSettings(bool isEnabled);
        void OnFormClosing();
    }




}
