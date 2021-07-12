using Newtonsoft.Json;

namespace AwsToolset.Models.CodePipeline
{
	public class ExecutionResult
	{
		[JsonProperty(PropertyName = "external-execution-url")]
		public string ExternalExecutionResultUrl { get; set; }
		[JsonProperty(PropertyName = "external-execution-summary")]
		public string ExternalExecutionResultSummary { get; set; }
		[JsonProperty(PropertyName = "external-execution-id")]
		public string ExternalExecutionResultId { get; set; }
		[JsonProperty(PropertyName = "error-code")]
		public string ErrorCode { get; set; }
	}
}