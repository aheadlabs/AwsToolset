using Amazon;
using Amazon.S3.Model;
using AwsToolset.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}
