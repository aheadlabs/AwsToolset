using Amazon.CloudWatchLogs;
using Amazon.Lambda.Core;
using AwsToolset.Enums;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AwsToolset.Services
{
	public interface ICloudWatchService
    {
        #region Properties

        /// <summary>
        /// Contains the Lambda context
        /// </summary>
        ILambdaContext LambdaContext { get; set; }

        /// <summary>
        /// Contains the CloudWatchLogsClient from Amazon
        /// </summary>
        AmazonCloudWatchLogsClient AmazonCloudWatchLogsClient { get; set; }

        #endregion

        /// <summary>
        /// Deletes a log group from CloudWatch
        /// </summary>
        /// <param name="logGroupName"></param>
        Task<HttpStatusCode> DeleteLogGroup(string logGroupName);

        /// <summary>
        /// Logs a message to AWS CloudWatch Logs adding a prefix based on severity.
        /// 
        /// Logging will not be done:
        ///  If the role provided to the function does not have sufficient permissions.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="message"></param>
        void Log(LogSeverity severity, string message);

        /// <summary>
        /// Logs a line to AWS CloudWatch Logs adding a prefix based on severity.
        /// 
        /// Logging will not be done:
        ///  If the role provided to the function does not have sufficient permissions.
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="message"></param>
        void LogLine(LogSeverity severity, string message);

        /// <summary>
        /// Get log messages filtered by severity and log stream name.
        /// </summary>
        /// <param name="logGroup"></param>
        /// <param name="logStream"></param>
        /// <param name="severity"></param>
        Task<IEnumerable<string>> GetLogEventsBySeverityAsync(string logGroup, string logStream, LogSeverity severity);

        /// <summary>
        /// Gets the current log stream from a log group
        /// </summary>
        /// <param name="logGroupName"></param>
        Task<string> GetCurrentLogStreamFromLogGroupAsync(string logGroupName);
    }
}
