using Amazon;
using Amazon.S3.Transfer;
using AwsToolset.Services;
using DotnetToolset.Services;
using Moq;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AwsToolset.Tests.Services
{
	public class S3ServiceTests
	{
		private const string RegionName = "eu-west-1";

		private S3Service _sut;
		private readonly ICloudWatchService _cloudWatchService = new CloudWatchService();
		private readonly RestService<MemoryStream> _restServiceMemoryStream;

        private readonly Mock<IS3Service> _s3ServiceMock;
		private readonly Mock<ITransferUtility> _transferUtilityMock;
        private readonly Mock<ILogger<S3Service>> _loggerMock;

		public S3ServiceTests()
		{
			_s3ServiceMock = new Mock<IS3Service>() { CallBase = true };
			_restServiceMemoryStream = new RestService<MemoryStream>();
			_transferUtilityMock = new Mock<ITransferUtility>();
            _loggerMock = new Mock<ILogger<S3Service>>();
        }

		#region GetFileAsStream

		[Fact]
		public void GetFileAsStream_Calls_Filetransferutility_OpenStream()
		{
			// Arrange
			string _bucketName = "bucket_name";
			string _objectKey = "key";
			_transferUtilityMock.Setup(t => t.OpenStream(_bucketName, _objectKey)).Returns(Stream.Null);
			_s3ServiceMock.Setup(s => s.GetFileAsStream(_bucketName, _objectKey))
				.Returns(_transferUtilityMock.Object.OpenStream(_bucketName, _objectKey));

			// Act
			_s3ServiceMock.Object.BucketRegion = RegionName;
			Stream result = _s3ServiceMock.Object.GetFileAsStream(_bucketName, _objectKey);

			// Assert
			Assert.Equal(result, Stream.Null);
			_transferUtilityMock.Verify(v => v.OpenStream(_bucketName, _objectKey), Times.Once());

		}

		[Fact]
		public void GetFileAsStream_ThrowsAggregateException_WhenExceptionOccurs()
		{
			// Arrange
			const string badBucketName = "non-existing-bucket_name";
			const string badObjectKey = "non-existing-key";

			// Act & Assert
			_sut = new S3Service(_cloudWatchService, _restServiceMemoryStream, _loggerMock.Object)
			{
				BucketRegion = RegionName
			};
			Assert.Throws<AggregateException>(() => _sut.GetFileAsStream(badBucketName, badObjectKey));
		}

		#endregion GetFileAsStream
    }
}
