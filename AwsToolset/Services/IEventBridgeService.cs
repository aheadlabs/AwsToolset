using Amazon.Lambda.Core;
using AwsToolset.Models.Events;
using System.Text.Json;

namespace AwsToolset.Services
{
	public interface IEventBridgeService
	{
		/// <summary>
		/// Deserializes input event JSON data
		/// </summary>
		/// <param name="input">Input data</param>
		/// <param name="context">Lambda context</param>
		/// <returns>Deserialized data</returns>
		Message DeserializeEventMessage(JsonElement input, ILambdaContext context);
	}
}
