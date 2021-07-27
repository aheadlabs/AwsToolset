namespace AwsToolset.Models.Sns
{
	public class SnsMessage
	{
		public SnsType Type { get; set; }
		public string MessageId { get; set; }
		public string TopicArn { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public string Timestamp { get; set; }
		public string SignatureVersion { get; set; }
		public string Signature { get; set; }
		public string SigningCertUrl { get; set; }
		public string UnsubscribeUrl { get; set; }
		public MessageAttributes MessageAttributes { get; set; }
	}
}
