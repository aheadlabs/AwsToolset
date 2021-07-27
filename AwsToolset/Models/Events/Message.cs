namespace AwsToolset.Models.Events
{
	/// <summary>
	/// AWS SNS message with n records
	/// </summary>
	public class Message
	{
		public Record[] Records { get; set; }
	}
}
