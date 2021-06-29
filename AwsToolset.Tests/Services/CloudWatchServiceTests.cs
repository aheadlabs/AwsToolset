using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using AwsToolset.Enums;
using AwsToolset.ExtensionMethods;
using AwsToolset.Services;
using Moq;
using Xunit;

namespace AwsToolset.Tests.Services
{
	public class CloudWatchServiceTests
	{

		private CloudWatchService _sut;
		private readonly Mock<ILambdaContext> _lambdaContextMock;

		public CloudWatchServiceTests()
		{
			_lambdaContextMock = new Mock<ILambdaContext>();
		}

		#region Properties

		[Fact]
		public void CanGetSetLambdaContext()
		{

			// Arrange
			string propertyExpected = "test-aws-id";
			ILambdaContext lambdaContext = new TestLambdaContext() { AwsRequestId = propertyExpected };

			// Act
			_sut = new CloudWatchService() { LambdaContext = lambdaContext };
			
			// Assert
			Assert.Equal(lambdaContext.AwsRequestId, _sut.LambdaContext.AwsRequestId);

		}

		#endregion

		#region Log

		[Fact]
		public void LogAppendsPrefixToDefaultLambdaLogger()
		{
			// Arrange
			string testMessage = "test-message";
			LogSeverity testSeverity = LogSeverity.Debug;
			string expectedLogMessage = $"[{testSeverity.Name()}] {testMessage}";
			_lambdaContextMock.Setup(l => l.Logger.Log(testMessage)).Verifiable();
			_sut = new CloudWatchService() { LambdaContext = _lambdaContextMock.Object };

			// Act
			_sut.Log(testSeverity, testMessage);
			
			// Assert
			_lambdaContextMock.Verify(l => l.Logger.Log(expectedLogMessage), Times.Once());
		}

		#endregion

		#region LogLine

		[Fact]
		public void LogLineAppendsPrefixToDefaultLambdaLogger()
		{
			// Arrange
			string testMessage = "test-message";
			LogSeverity testSeverity = LogSeverity.Debug;
			string expectedLogMessage = $"[{testSeverity.Name()}] {testMessage}";
			_lambdaContextMock.Setup(l => l.Logger.LogLine(testMessage)).Verifiable();
			_sut = new CloudWatchService() { LambdaContext = _lambdaContextMock.Object };

			// Act
			_sut.LogLine(testSeverity, testMessage);
			
			// Assert
			_lambdaContextMock.Verify(l => l.Logger.LogLine(expectedLogMessage), Times.Once());
		}

		#endregion
	}
}
