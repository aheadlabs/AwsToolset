namespace AwsToolset.Models.Sns
{
	public class MessageAttributes
	{
		public TypeValuePair EventType { get; set; }
		public TypeValuePair State { get; set; }
		public TypeValuePair AccountId { get; set; }
		public TypeValuePair Id { get; set; }
		public TypeValuePair StartTime { get; set; }
	}
}
