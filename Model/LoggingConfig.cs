using Serilog;
using Serilog.Sinks.File;
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
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\TMHA_Log.txt", rollingInterval: RollingInterval.Day, hooks: filePathHook)
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