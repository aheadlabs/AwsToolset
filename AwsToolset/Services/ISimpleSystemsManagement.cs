using Amazon;
using System.Threading.Tasks;

namespace AwsToolset.Services
{
	/// <summary>
	/// Manages AWS systems via AWS SSM (Simple Systems Management)
	/// </summary>
	public interface ISimpleSystemsManagement
	{
		/// <summary>
		/// AWS region endpoint
		/// </summary>
		RegionEndpoint RegionEndpoint { get; set; }

		/// <summary>
		/// Gets a string parameter given its name
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <returns>Parameter string value</returns>
		Task<string> GetStringParameter(string name);
	}
}
