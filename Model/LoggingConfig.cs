using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serilog.Sinks.File;
using System.Threading.Tasks;

namespace THFHA_V1._0.Model
{
    public static class LoggingConfig
    {
        public static string logFileFullPath;
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
    }
}
internal class CaptureFilePathHook : FileLifecycleHooks
{
    public string? Path { get; private set; }

    public override Stream OnFileOpened(string path, Stream underlyingStream, Encoding encoding)
    {
        Path = path;
        return base.OnFileOpened(path, underlyingStream, encoding);
    }
}
