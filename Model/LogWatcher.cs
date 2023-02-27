namespace THFHA_V1._0.Model
{
    public class LogWatcher
    {
        private FileSystemWatcher watcher;
        private string filePath;
        private State state;

        public LogWatcher(string filePath, State state)
        {
            this.filePath = filePath;
            this.state = state;

            watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath));
            watcher.Filter = Path.GetFileName(filePath);
            watcher.EnableRaisingEvents = true;
            watcher.Changed += OnLogFileChanged;
        }
        public async Task Start()
        {

        }
        public async Task Stop()
        {

        }
        public void OnLogFileChanged(object sender, FileSystemEventArgs e)
        {
            //did something change?
        }
    }

    public class State
    {
        // Define a delegate for the event handler
        public delegate void StateChangedEventHandler(object sender, EventArgs e);

        // Define the event that will be triggered when the state changes
        public event StateChangedEventHandler StateChanged;

        // Define properties for the different components of the state
        public string Status { get; set; }
        public string Activity { get; set; }
        public string Camera { get; set; }
        public string Microphone { get; set; }
        public string Message { get; set; }

        // Define a method to update the state and trigger the StateChanged event
        public void UpdateState(string status, string activity, string camera, string microphone, string message)
        {
            Status = status;
            Activity = activity;
            Camera = camera;
            Microphone = microphone;
            Message = message;

            // Trigger the StateChanged event to notify all registered event handlers that the state has changed
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
