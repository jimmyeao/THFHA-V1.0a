﻿using Microsoft.Win32;
using Serilog;
using System.Text;

namespace THFHA_V1._0.Model
{
    public class LogWatcher : IDisposable
    {
        private FileSystemWatcher watcher;
        private StreamReader streamReader;
        private string filePath;
        private CancellationTokenSource _cts;
        private State state;
        private bool isRunning;
        string _status = "";
        string _activity = "";
        string _mute = "";
        public event EventHandler? StateChanged;

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
        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();

            streamReader?.Dispose();
            watcher?.Dispose();
        }
        private async Task ReadLogFileAsync(CancellationToken cancellationToken)
        {

            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    // open the file
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                    {
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
                //Check camera
                var userName = Environment.UserName;
                var registryPath = @"Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam\NonPackaged\";
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(registryPath);
                if (registryKey != null)
                {
                    string[] subkeys = registryKey.GetSubKeyNames();
                    foreach (var subkey in subkeys)
                    {
                        if (subkey.Contains($"C:#Users#{Environment.UserName}#AppData#Local#Microsoft#Teams#current#Teams.exe"))
                        {
                            using (RegistryKey nonPackagedKey = registryKey.OpenSubKey(subkey))
                            {
                                string lastUsedTimeStop = nonPackagedKey?.GetValue("LastUsedTimeStop")?.ToString();
                                if (!string.IsNullOrEmpty(lastUsedTimeStop))
                                {
                                    state.Camera = lastUsedTimeStop == "0" ? "On" : "Off";
                                }
                            }
                        }
                    }
                }
                if (state.Activity != _activity)
                {
                    state.Activity = _activity;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
                if (state.Status!= _status)
                {
                    state.Status = _status;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                }
                if (state.Microphone != _mute) { state.Microphone = _mute; }
                await Task.Delay(1000, cancellationToken); // Example delay
            }

        }
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
            //

            { "DeviceCallControlManager Desktop: reportCallEnded processed",("",  "Not in a call")}
           
            //{ "Resuming daemon App updates", ("Not In A Call", "") },
            //{ "Pausing daemon App updates", ("In A Call", "") },
            //{ "Attempting to play audio for notification type 1", ("Incoming Call", "") },
            //{ "DeviceCallControlManager Desktop: reportCallEnded processed", ("Not In a Call", "") },
            //{ "name: desktop_call_state_change_send, isOngoing", ("In A Call", "") }
        };

        public async Task Stop()
        {
            isRunning = false;
            _cts.Cancel();
            Dispose();
            Log.Information("Teams log monitoring has stopped");
        }
        public bool IsRunning
        {
            get { return isRunning; }
        }

        public async Task Start()
        {
            isRunning = true;
            Task.Run(() => ReadLogFileAsync(_cts.Token), _cts.Token);

        }
        public void OnLogFileChanged(object sender, FileSystemEventArgs e)
        {
            // Handle the file change event here
        }
    }



}