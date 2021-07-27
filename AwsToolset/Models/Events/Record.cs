using AwsToolset.Models.Sns;
using Newtonsoft.Json;

namespace AwsToolset.Models.Events
{
	public class Record
	{
		public string EventSource { get; set; }
		public string EventVersion { get; set; }
		public string EventSubscriptionArn { get; set; }
		[JsonProperty(PropertyName = "Sns")]
		public SnsMessage Sns { get; set; }
	}
}
