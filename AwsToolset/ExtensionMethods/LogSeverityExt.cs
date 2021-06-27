using AwsToolset.Enums;

namespace AwsToolset.ExtensionMethods
{
	public static class LogSeverityExt
    {
        public static string Name ( this LogSeverity logSeverity)
        {
            switch (logSeverity)
            {
                case LogSeverity.Info:
                    return "INFO";
                case LogSeverity.Debug:
                    return "DEBUG";
                case LogSeverity.Warning:
                    return "WARN";
                case LogSeverity.Error:
                    return "ERROR";
                case LogSeverity.InfoNotifiable:
                    return "INFO-NOTIFIABLE";
                case LogSeverity.ErrorNotifiable:
                    return "ERROR-NOTIFIABLE";
                default: return "INFO";
            }
        }
    }
}
