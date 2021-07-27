using AwsToolset.Models.Events;
using AwsToolset.Models.Sns;

namespace AwsToolset.Services
{
	public interface ISnsService
	{
		/// <summary>
		/// Get the message type (service that sends it) inside the SNS JSON code
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		SnsContentType GetSnsContentType(Record record);
	}
}
