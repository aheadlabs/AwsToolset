using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AwsToolset.Enums;
using AwsToolset.Models;
using DotnetToolset.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Res = AwsToolset.Resources.Literals;

namespace AwsToolset.Services
{
	public class S3Service : IS3Service
	{
		private readonly ICloudWatchService _cloudWatchService;
		private IAmazonS3 _amazonS3;
		private RegionEndpoint _bucketRegion;

		#region Properties

		/// <inheritdoc />
		public S3 S3Settings { get; set; }

		/// <inheritdoc />
		public RegionEndpoint BucketRegion
		{
			get => _bucketRegion;
			set
			{
				_bucketRegion = value;

				// Create an S3 client that uses Newtonsoft.Json serialization
				_amazonS3 = new AmazonS3Client(_bucketRegion);
			}
		}

		/// <inheritdoc />
		public Uri BucketArn => new Uri($"arn:aws:s3:::{S3Settings.BucketName}");

		/// <inheritdoc />
		public Uri BucketUrl => new Uri($"https://{S3Settings.BucketName}.s3-{S3Settings.Region}.amazonaws.com");

		#endregion

		public S3Service(ICloudWatchService cloudWatchService)
		{
			_cloudWatchService = cloudWatchService;
		}

		#region S3 buckets

		/// <inheritdoc />
		public async Task<IEnumerable<S3Object>> GetBucketObjectListAsync()
		{
			try
			{
				ListObjectsV2Request request = new ListObjectsV2Request { BucketName = S3Settings.BucketName };

				ListObjectsV2Response response = await _amazonS3.ListObjectsV2Async(request);

				return response.S3Objects;
			}
			catch (AmazonS3Exception amazonS3Exception)
			{
				_cloudWatchService.LogLine(LogSeverity.Error,
					Res.p_ErrorListingObjectsOfBucket.ParseParameter(S3Settings.BucketName));
				_cloudWatchService.LogLine(LogSeverity.Error, amazonS3Exception.Message);
				throw;
			}
		}

		/// <inheritdoc />
		public IEnumerable<S3Object> GetBucketObjectListAsync(Func<S3Object, bool> predicate) => GetBucketObjectListAsync().Result.Where(predicate);

		/// <inheritdoc />
		public IEnumerable<string> GetBucketObjectsKeyNameList() => GetBucketObjectListAsync().Result.Select(o => o.Key);

		/// <inheritdoc />
		public IEnumerable<string> GetBucketObjectsKeyNameList(Func<S3Object, bool> predicate) => GetBucketObjectListAsync(predicate).Select(o => o.Key);

		#endregion
	}
}
