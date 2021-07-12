using Newtonsoft.Json;

namespace AwsToolset.Models.CodePipeline
{
	public class Detail
	{
		public string Pipeline { get; set; }
		[JsonProperty(PropertyName = "execution-id")]
		public string ExecutionId { get; set; }
		public string Stage { get; set; }
		[JsonProperty(PropertyName = "execution-result")]
		public ExecutionResult ExecutionResult { get; set; }
		public string Action { get; set; }
		public string State { get; set; }
		public string Region { get; set; }
		public Type Type { get; set; }
		public float Version { get; set; }
	}
}
