using Amazon;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toolset = AwsToolset.Models.Ses;

namespace AwsToolset.Services
{
    public class SesService : ISesService
    {
        public async Task<Toolset.SendEmailResponse> SendTemplate(string region, string sender, string template, Dictionary<string, string> templateData, List<string> to, List<string> cc, List<string> bcc)
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
    }
}
