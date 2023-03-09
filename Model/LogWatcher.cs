using Microsoft.Win32;
using Serilog;
using System.Text;

namespace THFHA_V1._0.Model
{
    public class LogWatcher : IDisposable
    {
        #region Private Fields

        private readonly Dictionary<string, (string StatusText, string ActivityText)> matchDictionary = new()
        {
            { "StatusIndicatorStateService: Added Available", ("Available", "")},
            { "StatusIndicatorStateService: Added Busy", ("Busy", "")},
            { "StatusIndicatorStateService: Added DoNotDisturb", ("Do not disturb", "")},
            { "StatusIndicatorStateService: Added BeRightBack", ("Be Right Back", "")},
            { "StatusIndicatorStateService: Added Away", ("Away", "")},
            { "StatusIndicatorStateService: Added AvailableIdle", ("Available Idle", "")},
            { "StatusIndicatorStateService: Added BusyIdle", ("Busy Idle", "")},
            { "StatusIndicatorStateService: Added NewActivity", ("", "") },

            { "StatusIndicatorStateService: Added InACall", ("Busy", "In a call")},

            { "StatusIndicatorStateService: Added OnThePhone", ("Busy", "On the phone")},
            { "StatusIndicatorStateService: Added Offline", ("Offline", "Offline")},
            { "StatusIndicatorStateService: Added PresenceUnknown", ("Presence Unknown", "Presence Unknown")},
            { "StatusIndicatorStateService: Added NoNetwork", ("No Network", "No Network") },
            { "StatusIndicatorStateService: Added ConnectionError", ("Connection Error", "Connection Error") },

            { "StatusIndicatorStateService: Added InAMeeting", ("", "In a meeting")},
            { "StatusIndicatorStateService: Added InAConferenceCall", ("", "In A Conference Call")},
            { "StatusIndicatorStateService: Added Inactive", ("", "Inactive")},
            { "StatusIndicatorStateService: Added OffWork", ("", "Off Work")},
            { "StatusIndicatorStateService: Added OutOfOffice", ("", "Out Of Office")},
            { "StatusIndicatorStateService: Added Presenting", ("", "Presenting")},
            { "StatusIndicatorStateService: Added UrgentInterruptionsOnly", ("", "Urgent Interruptions Only")},
            { "DeviceCallControlManager Desktop:  + CallManager: NotifyCallEnded",("","Not in a Call")},
            { "StatusIndicatorStateService: Added Unknown", ("", "") },
            {"DeviceCallControlManager Desktop: requestStartCall completed ",("","In a call")},
            { "CallManager: NotifyCallMuteStateChanged (muteState: 0;", ("Mute", "Mute Off")},
            { "CallManager: NotifyCallMuteStateChanged (muteState: 1;", ("Mute", "Mute On")},
            // these maybe redundant?
            { "SfB:TeamsNoCall", ("", "Not In a Call")},
            { "SfB:TeamsPendingCall", ("", "Pending Call")},
            { "SfB:TeamsActiveCall", ("Busy", "In A Call")},

            { "DeviceCallControlManager Desktop: reportCallEnded processed",("",  "Not in a call")}

            //{ "Resuming daemon App updates", ("Not In A Call", "") },
            //{ "Pausing daemon App updates", ("In A Call", "") },
            //{ "Attempting to play audio for notification type 1", ("Incoming Call", "") },
            //{ "DeviceCallControlManager Desktop: reportCallEnded processed", ("Not In a Call", "") },
            //{ "name: desktop_call_state_change_send, isOngoing", ("In A Call", "") }
        };

        private string _activity = "";
        private CancellationTokenSource _cts;
        private string _mute = "";
        private string _status = "";
        private string filePath;
        private bool isRunning;
        private State state;
        private StreamReader streamReader;
        private FileSystemWatcher watcher;

        #endregion Private Fields

        #region Public Constructors

        public LogWatcher(State state)
        {
            string _appDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string _logPath = _appDir + @"\Microsoft\Teams\";
            string _logFile = _logPath + "logs.txt";
            this.filePath = _logFile;
            this.state = state;
            _cts = new CancellationTokenSource();
            isRunning = false;
            // Start a background task to read the file continuously
            Task.Run(() => ReadLogFileAsync(_cts.Token), _cts.Token);
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler? StateChanged;

        #endregion Public Events

        #region Public Properties

        public bool IsRunning
        {
            get { return isRunning; }
        }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();

            streamReader?.Dispose();
            watcher?.Dispose();
        }

        public void OnLogFileChanged(object sender, FileSystemEventArgs e)
        {
            // Handle the file change event here
        }

        public async Task Start()
        {
            isRunning = true;
            Task.Run(() => ReadLogFileAsync(_cts.Token), _cts.Token);
        }

        public async Task Stop()
        {
            isRunning = false;
            _cts.Cancel();
            Dispose();
            Log.Information("Teams log monitoring has stopped");
        }

        #endregion Public Methods

        #region Private Methods

        private async Task ReadLogFileAsync(CancellationToken cancellationToken)
        {
            StreamReader sr = null;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // open the file
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    
                    {
                        sr = new StreamReader(fs, Encoding.UTF8);
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            //check if the line contains a dictionary key
                            var match = matchDictionary.FirstOrDefault(x => line.Contains(x.Key));
                            if (match.Key != null)
                            {
                                if (match.Value.StatusText == "Mute")
                                {
                                    _mute = match.Value.ActivityText;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(match.Value.ActivityText))
                                        _activity = match.Value.ActivityText;
                                    if (!string.IsNullOrEmpty(match.Value.StatusText))
                                        _status = match.Value.StatusText;
                                }
                            }
                        }
                    }
                }
                catch
                {//fail!
                    Log.Error("Failed to read log file");
                }
                finally
                {
                    sr?.Dispose();
                }

                if (State.Instance.Status != _status)
                {
                    State.Instance.Status = _status;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    // Dispose of the StreamReader and exit the method
                    sr.Dispose();
                    return;
                }
                //if (state.Microphone != _mute) { state.Microphone = _mute; }
                await Task.Delay(1000, cancellationToken); // Example delay
            }
        }

        #endregion Private Methods
    }
}