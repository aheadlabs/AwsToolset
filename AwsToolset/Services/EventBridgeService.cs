using Amazon.Lambda.Core;
using AwsToolset.Models.Events;
using Newtonsoft.Json;
using System;
using System.Text.Json;

namespace AwsToolset.Services
{
	/// <inheritdoc />
	public class EventBridgeService : IEventBridgeService
	{
		/// <inheritdoc />
		public Message DeserializeEventMessage(JsonElement input, ILambdaContext context)
		{
			context.Logger.LogLine($"Input event JSON data => {input}");

			try
			{
				return JsonConvert.DeserializeObject<Message>(input.GetRawText());
			}
			catch (InvalidOperationException ex)
			{
				context.Logger.LogLine($"Error deserializing input event JSON data => {ex.Message}");
				return null;
			}
		}
	}
}
