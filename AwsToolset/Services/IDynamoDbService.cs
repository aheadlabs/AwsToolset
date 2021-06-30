using System.Collections.Generic;

namespace AwsToolset.Services
{
    public interface IDynamoDbService<TIn, TOut> where TIn : class where TOut : class
    {
        /// <summary>
        /// Contains the end point to work from. Only needed when pointing to a local service in a
        /// development environment. Pointing to a real DynamoDb placed in Amazon will not require this
        /// property to be feeded and it can be empty.
        /// </summary> 
        string EndPoint { get; set; }

        /// <summary>
        /// Contains the AWS region to work from. The payload must specify the string code of region (i.e: eu-west-1).
        /// </summary>
        /// <see>https://docs.aws.amazon.com/general/latest/gr/rande.html</see>
        string Region { set; }

        /// <summary>
        /// Contains a representation of the table that will be operated. The payload will be the
        /// string name of the target table.
        /// </summary>
        string Table { get; set; }

        /// <summary>
        /// Adds a new object to the DynamoDb Database, in the specified <c>Table</c>
        /// </summary>
        TOut Add(TIn inObject);

        /// <summary>
        /// Deletes an object from the DynamoDb Database, in the specified <c>Table</c>
        /// </summary>
        TOut Delete(int id);

        /// <summary>
        /// Updates an object from the DynamoDb Database, in the specified <c>Table</c>
        /// </summary>
        TOut Edit(TIn inObject);

        /// <summary>
        /// Gets a list of objects from the DynamoDb Database, in the specified <c>Table</c>
        /// </summary>
        IEnumerable<TOut> Get();

        /// <summary>
        /// Gets a single object from the DynamoDb Database, in the specified <c>Table</c>
        /// </summary>
        TOut Get(int id);
    }
}