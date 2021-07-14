using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using System.Threading.Tasks;

namespace AwsToolset.Services
{
	/// <summary>
	/// Manages AWS systems via AWS SSM (Simple Systems Management)
	/// </summary>
	public class SimpleSystemsManagement : ISimpleSystemsManagement
	{
		private readonly AmazonSimpleSystemsManagementClient _ssmClient;

		/// <inheritdoc />
		public RegionEndpoint RegionEndpoint { get; set; }

		public SimpleSystemsManagement() : this(RegionEndpoint.USEast1)
		{
		}

		public SimpleSystemsManagement(RegionEndpoint regionEndpoint)
		{
			_ssmClient = new AmazonSimpleSystemsManagementClient(regionEndpoint);
			RegionEndpoint = regionEndpoint;
		}

		/// <inheritdoc />
		public async Task<string> GetStringParameter(string name)
		{
			GetParameterRequest getParameterRequest = new GetParameterRequest{Name = name};
			GetParameterResponse parameterResponse = await _ssmClient.GetParameterAsync(getParameterRequest);

			return parameterResponse.Parameter.Value;
		}
	}
}
