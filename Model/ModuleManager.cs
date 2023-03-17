using System.Reflection;

namespace THFHA_V1._0.Model
{
    public interface IModule
    {
        #region Public Events

        event EventHandler StateChanged;

        #endregion Public Events

        #region Public Properties

        bool IsEnabled { get; set; }
        string Name { get; }
        string State { get; }

        #endregion Public Properties

        // Add StateChanged event to interface

        #region Public Methods

        Form GetSettingsForm();

        Task OnFormClosing();

        Task Start();
        void Stop();
        void UpdateSettings(bool isEnabled);

        #endregion Public Methods
    }

    public class ModuleManager<T> where T : IModule
    {
        #region Private Fields

        private List<T> modules;
        private State state;

        #endregion Private Fields

        #region Public Constructors

        public ModuleManager(State state)
        {
            this.state = state;
            modules = new List<T>();
            LoadModules();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<T> Modules
        {
            get { return modules; }
        }

        #endregion Public Properties

        #region Private Methods

        private T CreateInstance<T>(Type type, State state) where T : IModule
        {
            if (type.GetConstructor(new Type[] { typeof(State) }) == null)
            {
                throw new ArgumentException($"Type {type.FullName} does not have a public constructor that takes a State parameter.");
            }

            return (T)Activator.CreateInstance(type, state);
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

        #endregion Private Methods
    }
}