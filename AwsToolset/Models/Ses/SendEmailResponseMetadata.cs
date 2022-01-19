using System.Collections.Generic;

namespace AwsToolset.Models.Ses
{
    public class SendEmailResponseMetadata
    {
        public IDictionary<string, string> Metadata { get; set; }
        public string RequestId { get; set; }
    }
}