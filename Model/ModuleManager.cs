using System.Reflection;

namespace THFHA_V1._0.Model
{
    public class ModuleManager<T> where T : IModule
    {
        private List<T> modules;

        public List<T> Modules
        {
            get { return modules; }
        }

        public ModuleManager()
        {
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
                    T module = CreateInstance<T>(type);
                    modules.Add(module);
                }
            }
        }

        private T CreateInstance<T>(Type type) where T : IModule
        {
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new ArgumentException($"Type {type.FullName} does not have a public parameterless constructor.");
            }

            return (T)Activator.CreateInstance(type);
        }
    }

    public interface IModule
    {
        string Name { get; }
        bool IsEnabled { get; set; }
        string State { get; }
        
        Form GetSettingsForm();

    }




}
