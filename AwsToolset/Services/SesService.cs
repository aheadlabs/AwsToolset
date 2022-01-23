using Amazon;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toolset = AwsToolset.Models.Ses;

namespace AwsToolset.Services
{
    public class SesService : ISesService
    {
        /// <inheritdoc />
        public async Task<Toolset.SendEmailResponse> SendTemplate(string region, string sender, List<string> to, List<string> cc, List<string> bcc,
            string template, Dictionary<string, string> templateData)
        {
            RegionEndpoint regionEndpoint = RegionEndpoint.GetBySystemName(region);

            SendEmailRequest request = new SendEmailRequest
            {
                Content = new EmailContent
                {
                    Template = new Template
                    {
                        TemplateName = template,
                        TemplateData = JsonConvert.SerializeObject(templateData)
                    }
                },
                FromEmailAddress = sender,
                Destination = new Destination
                {
                    ToAddresses = to,
                    CcAddresses = cc,
                    BccAddresses = bcc
                }
            };

            using AmazonSimpleEmailServiceV2Client client = new AmazonSimpleEmailServiceV2Client(regionEndpoint);
            SendEmailResponse response = await client.SendEmailAsync(request);

            Toolset.SendEmailResponse result = new Toolset.SendEmailResponse
            {
                ContentLength = response.ContentLength,
                HttpStatusCode = response.HttpStatusCode,
                MessageId = response.MessageId,
                Metadata = new Toolset.SendEmailResponseMetadata
                {
                    Metadata = response.ResponseMetadata.Metadata,
                    RequestId = response.ResponseMetadata.RequestId
                }
            };

            return result;
        }

        /// <inheritdoc />
        public async Task<string[]> GetEmailTemplates(string region)
        {
            RegionEndpoint regionEndpoint = RegionEndpoint.GetBySystemName(region);
            using AmazonSimpleEmailServiceV2Client client = new AmazonSimpleEmailServiceV2Client(regionEndpoint);

            List<string> templates = new List<string>();
            ListEmailTemplatesResponse response = null;
            do
            {
                ListEmailTemplatesRequest request = new ListEmailTemplatesRequest
                {
                    NextToken = response?.NextToken,
                    PageSize = 10
                };
                response = await client.ListEmailTemplatesAsync(request);
                templates.AddRange(response.TemplatesMetadata.Select(t => t.TemplateName));
            } while (response.NextToken != null);

            return templates.ToArray();
        }

        /// <inheritdoc />
        public async Task<string> GetLocalizedTemplate(string template, string language, string region)
        {
            // Get all templates in AWS SES
            string[] templates = await GetEmailTemplates(region);

            // Try to match template with one on the list: matches, no localized template; no matches, try adding language suffix
            if (templates.Any(t => t == template))
            {
                // return not localized template
                return template;
            }

            string localizedTemplateGuess = $"{template}{char.ToUpper(language[0]) + language.Substring(1)}";

            if (templates.Any(t => t == localizedTemplateGuess))
            {
                // return localized template
                return localizedTemplateGuess;
            }

            // no template found
            return null;
        }
    }
}
