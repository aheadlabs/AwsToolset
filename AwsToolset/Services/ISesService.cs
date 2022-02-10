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
        /// <param name="to">List of e-mail addresses to send the message to</param>
        /// <param name="cc">List of e-mail addresses to send a copy of the message to</param>
        /// <param name="bcc">List of e-mail addresses to send a hidden copy of the message to</param>
        /// <param name="template">Template name (must exist in AWS)</param>
        /// <param name="templateData">Key-value pairs for replacing template variables</param>
        /// <param name="configurationSet">AWS SES Configuration set name</param>
        /// <returns></returns>
        Task<Toolset.SendEmailResponse> SendTemplate(string region, string sender, List<string> to, List<string> cc, List<string> bcc,
            string template, Dictionary<string, string> templateData, string configurationSet = null);

        /// <summary>
        /// Gets all e-mail templates from the AWS SES service
        /// </summary>
        /// <param name="region">Region endpoint in the eu-west-1 format</param>
        /// <returns></returns>
        Task<string[]> GetEmailTemplates(string region);

        /// <summary>
        /// Gets the localized name for a template if it has one. Templates must be in the form Template1 for non localized templates and Template1En for localized templates.
        /// </summary>
        /// <param name="template">Name of the template with no localized suffix</param>
        /// <param name="language">ISO code 2 of the language</param>
        /// <param name="region">Region endpoint in the eu-west-1 format</param>
        /// <returns>Localized name for the template</returns>
        Task<string> GetLocalizedTemplate(string template, string language, string region);
    }
}
