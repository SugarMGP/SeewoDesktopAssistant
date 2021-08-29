using NLog;
using System;
using System.IO;
using System.Reflection;

namespace SDA.Core
{
    public class SDA
    {
        private static SDA _instance;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private string _logsDir;

        public static SDA Instance
        {
            get => _instance;
        }

        private void SetupLoggers()
        {
            _logsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Logs");
            if (!Directory.Exists(_logsDir))
            {
                Directory.CreateDirectory(_logsDir);
            }
            string date = DateTime.Today.ToString("yyyy_MM_dd");
            string logFileName = Path.Combine(_logsDir, "SDA_" + date + "_");
            string debugLogFileName = Path.Combine(_logsDir, "SDA_Debug_" + date + "_");
            for (int i = 1; ; i++)
            {
                if (!File.Exists(logFileName + i + ".log") || !File.Exists(debugLogFileName + i + ".log"))
                {
                    logFileName += i;
                    debugLogFileName += i;
                    break;
                }
            }
            logFileName += ".log";
            debugLogFileName += ".log";

            NLog.Config.LoggingConfiguration config = new();

            // logger target
            NLog.Targets.FileTarget logFile = new("logfile")
            {
                FileName = logFileName
            };
            NLog.Targets.FileTarget debugLogFile = new("debuglogfile")
            {
                FileName = debugLogFileName
            };

            // logger rule
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logFile);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, debugLogFile);

            // apply configurations
            LogManager.Configuration = config;
        }

        /// <summary>
        /// 整个应用的入口，需要创建实例后调用。
        /// </summary>
        public void Launch()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Application is already launched!");
            }
            SetupLoggers();
            _logger.Info("Launching SeewoDesktopAssistant");
            _instance = this;

        }
    }
}
