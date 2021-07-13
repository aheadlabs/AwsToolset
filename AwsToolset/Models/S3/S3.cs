namespace AwsToolset.Models.S3
{
	public class S3
    {
        public string BucketName { get; set; }
        public string BucketArn { get; set; }
        public string BaseDocumentListKeyName { get; set; }
        public string BaseDocumentListKeyNamePrefix { get; set; }
        public string BaseDocumentKeyName { get; set; }
        public string Region { get; set; }
    }
}
