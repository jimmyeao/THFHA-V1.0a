using Serilog;
using Serilog.Sinks.File;
using System;
using System.IO;
using System.Text;

namespace THFHA_V1._0.Model
{
    public static class LoggingConfig
    {
        #region Public Fields

        public static string logFileFullPath;

        #endregion Public Fields

        #region Public Methods

        public static void Configure()
        {
            var filePathHook = new CaptureFilePathHook();
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(appDataFolder, "TeamsHelper");
            var logFilePath = Path.Combine(folderPath, "TMHA_Log.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, hooks: filePathHook)
                .WriteTo.Debug()
                .CreateLogger();
            Log.Information("Logger Created");
            logFileFullPath = filePathHook.Path;
        }

        #endregion Public Methods
    }
}

internal class CaptureFilePathHook : FileLifecycleHooks
{
    #region Public Properties

    public string? Path { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public override Stream OnFileOpened(string path, Stream underlyingStream, Encoding encoding)
    {
        Path = path;
        return base.OnFileOpened(path, underlyingStream, encoding);
    }

    #endregion Public Methods
}
