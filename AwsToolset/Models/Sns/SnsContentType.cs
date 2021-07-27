namespace AwsToolset.Models.Sns
{
	/// <summary>
	/// Type of SNS message content
	/// </summary>
	public enum SnsContentType
	{
		/// <summary>
		/// The message is sent from AWS CodePipeline
		/// </summary>
		CodePipeline,

		/// <summary>
		/// The message is sent from AWS Backup
		/// </summary>
		Backup
	}
}
