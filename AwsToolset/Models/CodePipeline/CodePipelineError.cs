namespace AwsToolset.Models.CodePipeline
{
	public class CodePipelineError
	{
		public string Account { get; set; }
		public string DetailType { get; set; }
		public string Region { get; set; }
		public string Source { get; set; }
		public string Time { get; set; }
		public string NotificationRuleArn { get; set; }
		public Detail Detail { get; set; }
		public string[] Resources { get; set; }
		public AdditionalAttributes AdditionalAttributes { get; set; }
	}
}
