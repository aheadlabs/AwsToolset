using System.Net;

namespace AwsToolset.Models.Ses
{
    public class SendEmailResponse
    {
        public long ContentLength { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string MessageId { get; set; }
        public SendEmailResponseMetadata Metadata { get; set; }
    }
}