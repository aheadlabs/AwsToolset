
namespace AwsToolset.Enums
{
	public enum LogSeverity
    {
        /// <summary>
        /// Developing purposes logs
        /// </summary>
        Debug,

        /// <summary>
        /// General info logs
        /// </summary>
        Info,

        /// <summary>
        /// General info logs that must be notified through mail
        /// </summary>
        InfoNotifiable,

        /// <summary>
        /// A problem can occur
        /// </summary>
        Warning,

        /// <summary>
        /// A problem has occurred.
        /// </summary>
        Error,

        /// <summary>
        /// A problem has occurred and must be notified through mail
        /// </summary>
        ErrorNotifiable
    }
}
