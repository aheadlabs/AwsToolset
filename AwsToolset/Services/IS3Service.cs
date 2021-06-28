using Amazon;
using Amazon.S3.Model;
using AwsToolset.Enums;
using AwsToolset.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AwsToolset.Services
{
	public interface IS3Service
	{
		#region Properties

		/// <summary>
		/// Contains the AWS S3 settings
		/// </summary>
		S3 S3Settings { get; set; }

		/// <summary>
		/// Determines the region of the S3 
		/// </summary>
		RegionEndpoint BucketRegion { get; set; }

		/// <summary>
		/// Gets the bucket ARN (Amazon Resource Name)
		/// </summary>
		/// <returns>Bucket ARN</returns>
		Uri BucketArn { get; }

		/// <summary>
		/// Gets the bucket URL 
		/// </summary>
		/// <returns>Bucket URL</returns>
		Uri BucketUrl { get; }

        #endregion

        #region S3 buckets

        /// <summary>
        /// Gets a list of the objects in the bucket
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<S3Object>> GetBucketObjectListAsync();

        /// <summary>
        /// Gets a list of the objects in the bucket matching a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<S3Object> GetBucketObjectListAsync(Func<S3Object, bool> predicate);

        /// <summary>
        /// Gets a list of the object keyName names in the bucket
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetBucketObjectsKeyNameList();

        /// <summary>
        /// Gets a list of the object keyName names in the bucket, matching a predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<string> GetBucketObjectsKeyNameList(Func<S3Object, bool> predicate);

        #endregion S3 buckets

        #region S3 objects

        /// <summary>
        /// Checks if an object exists in the bucket
        /// </summary>
        /// <param name="keyName">Key name to be checked</param>
        /// <returns>True if the object exists in the bucket</returns>
        bool ObjectExists(string keyName);

        /// <summary>
        /// Uploads a generic Stream to a S3 bucket attaching a tag set
        /// </summary>
        /// <param name="keyName">Key name of the object in S3</param>
        /// <param name="objectData"></param>
        /// <param name="overwrite">If true (by default), the file will be overwritten or a new version will be created if versioning is enabled in the bucket</param>
        /// <param name="tagSet">List of tags to be attached to the object</param>
        /// <returns>Response from AWS</returns>
        void UploadObject(string keyName, Stream objectData, bool overwrite = true, List<Tag> tagSet = null);

        /// <summary>
        /// Uploads an XML file to a S3 bucket attaching a tag set
        /// </summary>
        /// <param name="keyName">Key name of the object in S3</param>
        /// <param name="objectData"></param>
        /// <param name="overwrite">If true (by default), the file will be overwritten or a new version will be created if versioning is enabled in the bucket</param>
        /// <param name="tagSet">List of tags to be attached to the object</param>
        /// <returns>Response from AWS</returns>
        void UploadObject(string keyName, XDocument objectData, bool overwrite = true, List<Tag> tagSet = null);

        /// <summary>
        /// Puts an object in an S3 bucket directly from a URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="overwrite">If true (by default), the file will be overwritten or a new version will be created if versioning is enabled in the bucket</param>
        /// <param name="tagSet">List of tags to be attached to the object</param>
        /// <param name="prefix">Prefix for the keyName of the object</param>
        /// <returns>Key name of the uploaded object</returns>
        string UploadObjectFromUrl(string url, bool overwrite = true, List<Tag> tagSet = null, string prefix = null);

        /// <summary>
        /// Gets a file from S3 and returns it as a Stream
        /// </summary>
        /// <param name="bucketName">Bucket name where the object will be get from</param>
        /// <param name="objectKey">Key name of the object in S3</param>
        /// <returns>Object Stream for the object retrieved</returns>
        Stream GetFileAsStream(string bucketName, string objectKey);

        /// <summary>
        /// Deletes a file from a S3 bucket
        /// </summary>
        /// <param name="bucketName">Bucket name where the object will be deleted from</param>
        /// <param name="keyName">Key name of the object in S3</param>
        /// <returns></returns>
        void DeleteObjectAsync(string bucketName, string keyName);

        /// <summary>
        /// Deletes a file from a S3 bucket
        /// </summary>
        /// <param name="deleteObjectRequest"></param>
        void DeleteObjectAsync(DeleteObjectRequest deleteObjectRequest);

        /// <summary>
        /// Gets an S3 object
        /// </summary>
        /// <param name="key">S3 object keyName</param>
        /// <returns>S3 object</returns>
        Task<GetObjectResponse> GetObjectAsync(string key);

        /// <summary>
        /// Gets an S3 object
        /// </summary>
        /// <param name="getObjectRequest">GetObjectRequest</param>
        /// <returns></returns>
        Task<GetObjectResponse> GetObjectAsync(GetObjectRequest getObjectRequest);

        /// <summary>
        /// Gets the object ARN (Amazon Resource Name) from the object keyName
        /// </summary>
        /// <param name="objectKey">Object keyName</param>
        /// <returns>Object ARN</returns>
        string GetObjectArn(string objectKey);

        /// <summary>
        /// Gets the object URL from the object keyName
        /// </summary>
        /// <param name="objectKey"></param>
        /// <returns></returns>
        Uri GetObjectUrl(string objectKey);

        #endregion

        #region Tag and tag sets

        /// <summary>
        /// Gets all the tags of a S3 object
        /// </summary>
        /// <param name="keyName">S3 object keyName name</param>
        /// <param name="predicate">Predicate for adjusting the resulting list</param>
        /// <returns>Structure that contains a list of tags</returns>
        Task<List<Tag>> GetObjectTagsAsync(string keyName, Func<Tag, bool> predicate = null);

        /// <summary>
        /// Sets all the tags of the S3 object
        /// </summary>
        /// <param name="keyName">S3 object keyName</param>
        /// <param name="tagSet">List of tags to be put in the object</param>
        /// <returns>Tag list</returns>
        Task<HttpStatusCode> SetObjectTagsAsync(string keyName, List<Tag> tagSet);

        /// <summary>
        /// Adds tags to the S3 object preserving the existing ones
        /// </summary>
        /// <param name="keyName">S3 object keyName</param>
        /// <param name="tagSet">List of tags to be put in the object</param>
        /// <param name="existingTagsAction">Determines what action to take if tags exist in the object</param>
        /// <returns>Tag list</returns>
        Task<HttpStatusCode> AddObjectTagsAsync(string keyName, List<Tag> tagSet, ExistingTagsAction existingTagsAction = ExistingTagsAction.Overwrite);

        ///// <summary>
        ///// Checks if a state tag is already in a tag set
        ///// </summary>
        ///// <param name="tags">Tag set to be checked</param>
        ///// <param name="tag">State tag to check</param>
        ///// <returns>True if the tag is in the tag set</returns>
        //bool IsStateTagInTagSet(Tagging tags, StateTag tag);

        ///// <summary>
        ///// Checks if a tag set contains the processed state tag
        ///// </summary>
        ///// <param name="tagSet">Tag set to be checked</param>
        ///// <returns>True if the processed state tag is found</returns>
        //bool IsS3ObjectTaggedAsProcessed(Tagging tagSet);

        ///// <summary>
        ///// Checks if a tag set contains the pending state tag
        ///// </summary>
        ///// <param name="tagSet">Tag set to be checked</param>
        ///// <returns>True if the pending state tag is found</returns>
        //bool IsS3ObjectTaggedAsPending(Tagging tagSet);

        ///// <summary>
        ///// Checks if the S3 object has a date tag
        ///// </summary>
        ///// <param name="tagSet">Tag set to be checked</param>
        ///// <returns>True if date tag is found</returns>
        //bool IsS3ObjectDateTagged(Tagging tagSet);

        ///// <summary>
        ///// Creates a tag set with 2 tags: state and date
        ///// </summary>
        ///// <param name="stateTag">State tag to be added</param>
        ///// <param name="dateTime">Date to be added as a date tag</param>
        ///// <returns>Tag set created</returns>
        //Tagging CreateTagSet(StateTag stateTag, DateTime dateTime);

        ///// <summary>
        ///// Merges 2 tag sets
        ///// </summary>
        ///// <param name="tagSet1">First tag set</param>
        ///// <param name="tagSet2">Second tag set</param>
        ///// <param name="preference">Indicates which tag set takes preference</param>
        ///// <returns></returns>
        //Tagging MergeTagSets(Tagging tagSet1, Tagging tagSet2, TagSetPreference preference = TagSetPreference.Last);

        ///// <summary>
        ///// Tags S3 objects as "processed backend"
        ///// </summary>
        ///// <param name="processedObjects">Objects processed in S3 and the backend</param>
        ///// <returns>Responses for the addition of tags to S3 objects</returns>
        //IEnumerable<PutObjectTaggingResponse> TagS3ObjectsAsProcessedBackend(IEnumerable<ProcessedObject> processedObjects);

        #endregion
    }
}
