using NLog;
using System;

namespace SDA.Core
{
    public class SDA
    {
        private static SDA _instance;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static SDA Instance
        {
            get => _instance;
        }

        private static void SetupLoggers()
        {
            NLog.Config.LoggingConfiguration config = new();

            // logger target
            NLog.Targets.FileTarget logFile = new("logfile")
            {
                FileName = "SDA.log"
            };
            NLog.Targets.FileTarget debugLogFile = new("debuglogfile")
            {
                FileName = "SDA_Debug.log"
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
            SetupLoggers();
            Logger.Info("Launching SeewoDesktopAssistant");
            if (_instance != null)
            {
                throw new InvalidOperationException("Application is already launched!");
            }
            _instance = this;

        }
    }
}
