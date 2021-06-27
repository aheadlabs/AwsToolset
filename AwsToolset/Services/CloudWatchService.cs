using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Lambda.Core;
using AwsToolset.Enums;
using AwsToolset.ExtensionMethods;
using DotnetToolset.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Res = AwsToolset.Resources.Literals;

namespace AwsToolset.Services
{
	public class CloudWatchService : ICloudWatchService
    {
        #region Properties

        /// <inheritdoc />
        public ILambdaContext LambdaContext { get; set; }

        /// <inheritdoc />
        public AmazonCloudWatchLogsClient AmazonCloudWatchLogsClient { get; set; }

        #endregion Properties

        /// <inheritdoc />
        public async Task<HttpStatusCode> DeleteLogGroup(string logGroupName)
        {
            try
            {
                DeleteLogGroupRequest deleteLogGroupRequest = new DeleteLogGroupRequest(logGroupName);
                DeleteLogGroupResponse deleteLogGroupResponse = await AmazonCloudWatchLogsClient.DeleteLogGroupAsync(deleteLogGroupRequest).ConfigureAwait(false);
                LogLine(LogSeverity.Info, Res.p_LogGroupReset.ParseParameter(logGroupName));
                return deleteLogGroupResponse.HttpStatusCode;
            }
            catch (ResourceNotFoundException ex)
            {
                LogLine(LogSeverity.Error, ex.Message);
                LogLine(LogSeverity.Error, Res.p_LogGroupNotReset.ParseParameter(logGroupName));
                return HttpStatusCode.NotFound;
            }
        }

        /// <inheritdoc />
        public void Log(LogSeverity severity, string message) => LambdaContext.Logger.Log($"[{severity.Name()}] {message}");

        /// <inheritdoc />
        public void LogLine(LogSeverity severity, string message) => LambdaContext.Logger.LogLine($"[{severity.Name()}] {message}");

        /// <inheritdoc />
        public async Task<IEnumerable<string>> GetLogEventsBySeverityAsync(string logGroup, string logStream, LogSeverity severity)
        {
            FilterLogEventsRequest request = new FilterLogEventsRequest
            {
                LogStreamNames = new List<string> {logStream},
                FilterPattern = $"{severity.Name()}",
                LogGroupName = logGroup
            };

            FilterLogEventsResponse response = await AmazonCloudWatchLogsClient.FilterLogEventsAsync(request).ConfigureAwait(false);
            return response.Events.Select(e => e.Message);
        }

        /// <inheritdoc />
        public async Task<string> GetCurrentLogStreamFromLogGroupAsync(string logGroupName)
        {
            DescribeLogStreamsRequest request = new DescribeLogStreamsRequest
            {
                Descending = true,
                LogGroupName = logGroupName,
            };
            DescribeLogStreamsResponse response = await AmazonCloudWatchLogsClient.DescribeLogStreamsAsync(request).ConfigureAwait(false);
            return response.LogStreams.FirstOrDefault()?.LogStreamName;
        }

    }
}
