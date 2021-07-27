using AwsToolset.Models.CodePipeline;
using AwsToolset.Models.Events;
using AwsToolset.Models.Sns;
using Newtonsoft.Json;
using System;

namespace AwsToolset.Services
{
	/// <inheritdoc />
	public class SnsService : ISnsService
	{
		/// <inheritdoc />
		public SnsContentType GetSnsContentType(Record record)
		{
			string content = record.Sns.Message;

			// AWS Backup
			if (content.StartsWith("An AWS Backup"))
			{
				return SnsContentType.Backup;
			}

			// AWS CodePipeline
			try
			{
				JsonConvert.DeserializeObject<CodePipelineSnsNotification>(content);
				return SnsContentType.CodePipeline;
			}
			catch (InvalidOperationException)
			{
				throw new NotImplementedException();
			}
		}
	}
}