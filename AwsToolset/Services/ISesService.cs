using System.Collections.Generic;
using System.Threading.Tasks;
using Toolset = AwsToolset.Models.Ses;

namespace AwsToolset.Services
{
    public interface ISesService
    {
        /// <summary>
        /// Sends an e-mail using an AWS SES template (must exist in AWS)
        /// </summary>
        /// <param name="region">Region endpoint in the eu-west-1 format</param>
        /// <param name="sender">Sender e-mail address</param>
        /// <param name="template">Template name (must exist in AWS)</param>
        /// <param name="templateData">Key-value pairs for replacing template variables</param>
        /// <param name="to">List of e-mail addresses to send the message to</param>
        /// <param name="cc">List of e-mail addresses to send a copy of the message to</param>
        /// <param name="bcc">List of e-mail addresses to send a hidden copy of the message to</param>
        /// <returns></returns>
        Task<Toolset.SendEmailResponse> SendTemplate(string region, string sender, string template, Dictionary<string, string> templateData,
            List<string> to, List<string> cc, List<string> bcc);
    }
}
