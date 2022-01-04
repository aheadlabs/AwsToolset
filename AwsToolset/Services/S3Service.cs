using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AwsToolset.Enums;
using AwsToolset.Models.S3;
using DotnetToolset.ExtensionMethods;
using DotnetToolset.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Res = AwsToolset.Resources.Literals;

namespace AwsToolset.Services
{
	public class S3Service : IS3Service
	{
		private readonly ICloudWatchService _cloudWatchService;
		private readonly IRestService<MemoryStream> _restService;
		private IAmazonS3 _amazonS3;
		private RegionEndpoint _bucketRegion;

		public S3Service(ICloudWatchService cloudWatchService, IRestService<MemoryStream> restService)
		{
			_cloudWatchService = cloudWatchService;
			_restService = restService;
		}

		#region Public properties

		/// <inheritdoc />
		public S3 S3Settings { get; set; }

		/// <inheritdoc />
		public string BucketRegion
		{
            set
			{
				_bucketRegion = RegionEndpoint.GetBySystemName(value);

				// Create an S3 client that uses Newtonsoft.Json serialization
				_amazonS3 = new AmazonS3Client(_bucketRegion);
			}
		}

		/// <inheritdoc />
		public Uri BucketArn => new Uri($"arn:aws:s3:::{S3Settings.BucketName}");

		/// <inheritdoc />
		public Uri BucketUrl => new Uri($"https://{S3Settings.BucketName}.s3-{S3Settings.Region}.amazonaws.com");

		#endregion

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

        #region S3 objects

        /// <inheritdoc />
        public bool ObjectExists(string keyName) => GetBucketObjectsKeyNameList(o => o.Key == keyName).Any();

        /// <inheritdoc />
        public void UploadObject(string keyName, Stream objectData, bool overwrite = true, bool publiclyVisible = false,  List<Tag> tagSet = null)
        {
            TransferUtility fileTransferUtility = new TransferUtility(_amazonS3);

            try
            {
                if (!overwrite && ObjectExists(keyName))
				{
					throw new InvalidOperationException(Res.b_ObjectTryingToBeUploadedToBucketAlreadyExistsIQuit);
				}

				using (MemoryStream ms = new MemoryStream())
                {
                    // This Copy is necessary because DeflateStream has not length data and UploadAsync will complain
                    if (objectData is MemoryStream)
					{
						objectData.Position = 0;
					}

					objectData.CopyTo(ms);

                    // Upload
                    TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = S3Settings.BucketName,
                        Key = keyName,
                        TagSet = tagSet,
                        InputStream = ms,
                        CannedACL = publiclyVisible ? S3CannedACL.PublicRead : S3CannedACL.Private
                    };
                    fileTransferUtility.Upload(uploadRequest);
                }
            }
            catch (Exception ex)
            {
                if (ex is AmazonS3Exception)
                {
                    _logger.LogError(Res.p_ErrorWritingObjectToS3.ParseParameter(ex.Message));
                }
                else if (ex is InvalidOperationException)
                {
                    _logger.LogError(ex.Message);
                }
                else
                {
                    _logger.LogError(Res.p_UnknownErrorWritingObjectToS3.ParseParameter(ex.Message));
                }
                throw;
            }
        }

        /// <inheritdoc />
        public void UploadObject(string keyName, XDocument objectData, bool overwrite = true, bool publiclyVisible = false, List<Tag> tagSet = null)
        {
            // Create stream from XDocument
            MemoryStream ms = new MemoryStream();
            objectData.Save(ms);

            // Upload object
            UploadObject(keyName, ms, overwrite, publiclyVisible, tagSet);
        }

        /// <inheritdoc />
        public string UploadObjectFromUrl(string url, bool overwrite = true, bool publiclyVisible = false, List<Tag> tagSet = null, string prefix = null)
        {
            try
            {
                _restService.BaseUrl = url;
                MemoryStream data = _restService.Download(url);
                string fileName = Path.GetFileName(url);
                string keyNamePrefix = (prefix != null) ? $"{prefix}-" : string.Empty;
                string keyName = $"{keyNamePrefix}{S3Settings.BaseDocumentKeyName}{fileName}";
                UploadObject(keyName, data, overwrite, publiclyVisible ,tagSet);
                return keyName;
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(Res.p_ErrorWritingObjectToS3.ParseParameter(e.Message));
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(Res.p_UnknownErrorWritingObjectToS3.ParseParameter(e.Message));
                throw;
            }
        }

        /// <inheritdoc />
        public Stream GetFileAsStream(string bucketName, string objectKey)
        {
            try
            {
                TransferUtility fileTransferUtility = new TransferUtility(_amazonS3);
                return fileTransferUtility.OpenStream(bucketName, objectKey);
            }
            catch (Exception ex)
            {
                throw new AggregateException(
                    Res.p_CouldNotGetObjectFromBucketError.ParseParameters(new object[]
                    {
                        objectKey, bucketName, ex.Message
                    }));
            }
        }

        /// <inheritdoc />
        public void DeleteObjectAsync(string bucketName, string keyName)
        {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = keyName
            };
            DeleteObjectAsync(deleteObjectRequest);
        }

        public async void DeleteObjectAsync(DeleteObjectRequest deleteObjectRequest)
        {
            await _amazonS3.DeleteObjectAsync(deleteObjectRequest);
            _logger.LogError(Res.p_ObjectWithKeyWasDeletedInBucket.ParseParameters(new object[] { deleteObjectRequest.Key, deleteObjectRequest.BucketName }));
        }

        /// <inheritdoc />
        public async Task<GetObjectResponse> GetObjectAsync(string key) => await _amazonS3.GetObjectAsync(S3Settings.BucketName, key);

        /// <inheritdoc />
        public async Task<GetObjectResponse> GetObjectAsync(GetObjectRequest getObjectRequest) => await _amazonS3.GetObjectAsync(getObjectRequest);

        /// <inheritdoc />
        public string GetObjectArn(string objectKey) => $"{BucketArn}/{objectKey}";
        /// <inheritdoc />
        public Uri GetObjectUrl(string objectKey) => new Uri($"{BucketUrl}{objectKey}");

        #endregion

        #region Tags and tag sets

        /// <inheritdoc />
        public async Task<List<Tag>> GetObjectTagsAsync(string keyName, Func<Tag, bool> predicate = null)
        {
            GetObjectTaggingRequest objectTaggingRequest = new GetObjectTaggingRequest
            {
                BucketName = S3Settings.BucketName,
                Key = keyName
            };

            GetObjectTaggingResponse objectTaggingResponse = await _amazonS3.GetObjectTaggingAsync(objectTaggingRequest);

            return (predicate == null) ? objectTaggingResponse.Tagging : objectTaggingResponse.Tagging.Where(predicate).ToList();
        }

        /// <inheritdoc />
        public async Task<HttpStatusCode> SetObjectTagsAsync(string keyName, List<Tag> tagSet)
        {
            PutObjectTaggingRequest putObjectTaggingRequest = new PutObjectTaggingRequest
            {
                BucketName = S3Settings.BucketName,
                Key = keyName,
                Tagging = new Tagging { TagSet = tagSet }
            };

            PutObjectTaggingResponse putObjectTaggingResponse = await _amazonS3.PutObjectTaggingAsync(putObjectTaggingRequest);
            return putObjectTaggingResponse.HttpStatusCode;
        }

        /// <inheritdoc />
        public async Task<HttpStatusCode> AddObjectTagsAsync(string keyName, List<Tag> tagSet, ExistingTagsAction existingTagsAction = ExistingTagsAction.Overwrite)
        {
            // Get existing tags
            List<Tag> tags = await GetObjectTagsAsync(keyName);

            #region Add passed tags

            if (existingTagsAction == ExistingTagsAction.Duplicate)
            {
                tags.AddRange(tagSet);
            }

            if (existingTagsAction == ExistingTagsAction.Overwrite)
            {
                foreach (Tag tag in tagSet)
                {
                    if (tags.Exists(t => t.Key == tag.Key))
                    {
                        IEnumerable<Tag> existingTags = tags.Where(t => t.Key == tag.Key);
                        tags.ForEach(delegate (Tag existingTag)
                        {
                            existingTag.Value = tag.Value;
                        });
                    }
                    else
                    {
                        tags.Add(tag);
                    }
                }
            }

            #endregion Add passed tags

            // Set object tags
            return await SetObjectTagsAsync(keyName, tagSet);
        }

        #endregion Tags and tag sets
    }
}
