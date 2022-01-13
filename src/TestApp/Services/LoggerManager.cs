using NLog;
using TestApp.Utils;

namespace TestApp.Services
{
    public static class LoggerManager
    {
        private static Logger logger;

        public static Logger GetLogger()
        {
            if (logger == null)
            {
                var config = new NLog.Config.LoggingConfiguration();

                // Targets where to log to: File and Console
                var logfile = new NLog.Targets.FileTarget("logfile") { FileName = PathUtil.GetPathLog() };
                var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

                // Rules for mapping loggers to targets            
                config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
                config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

                // Apply config           
                LogManager.Configuration = config;
                logger = LogManager.GetCurrentClassLogger();
            }

            return logger;
        }
    }
}
