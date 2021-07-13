using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace AwsToolset.Services
{
	public class DynamoDbService<TIn, TOut> : IDynamoDbService<TIn, TOut> where TIn : class
        where TOut: class

    {
        private AmazonDynamoDBClient _client;
        private Table _tableEnvironment;
        private string _endPoint;

        private readonly ICloudWatchService _cloudWatchService;

        public DynamoDbService(ICloudWatchService cloudWatchService)
        {
            _cloudWatchService = cloudWatchService;
        }

        #region Public properties

        /// <inheritdoc />
        public string EndPoint
        {
            get => _endPoint;
            set
            {
                _endPoint = value;
                
                // Use the config to feed the client
                _client = new AmazonDynamoDBClient(new AmazonDynamoDBConfig { ServiceURL = _endPoint });
            }
        }

        /// <inheritdoc />
        public string Region
        {
            set =>
                // Use the config to feed the client
                _client = new AmazonDynamoDBClient(new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(value) });
        }

        /// <inheritdoc />
        public string Table
        {
            get => _tableEnvironment.TableName; 
            set => _tableEnvironment = Amazon.DynamoDBv2.DocumentModel.Table.LoadTable(_client, value);
        }

        #endregion

        #region DynamoDb Operations

        /// <inheritdoc />
        public TOut Add(TIn inObject)
        {
            string jsonText = JsonSerializer.Serialize(inObject);
            var item = Document.FromJson(jsonText);

            var putItemConfig = new UpdateItemOperationConfig
            {
                ReturnValues = ReturnValues.AllNewAttributes
            }; 

            Document operationResult = _tableEnvironment.UpdateItemAsync(item, putItemConfig).Result;

            TOut result = JsonSerializer.Deserialize<TOut>(operationResult.ToJson());

            return result;
        }

        /// <inheritdoc />
        public TOut Delete(int id)
        {
            var deleteItemConfig = new DeleteItemOperationConfig
            {
                ReturnValues = ReturnValues.AllOldAttributes
            };
            
            Document operationResult = _tableEnvironment.DeleteItemAsync(id, deleteItemConfig).Result;

            TOut result = JsonSerializer.Deserialize<TOut>(operationResult.ToJson());

            return result;
        }

        /// <inheritdoc />
        public TOut Edit(TIn inObject)
        {
            string jsonText = JsonSerializer.Serialize(inObject);
            var item = Document.FromJson(jsonText);

            var updateItemConfig = new UpdateItemOperationConfig
            {
                ReturnValues = ReturnValues.UpdatedNewAttributes
            }; 

            Document operationResult = _tableEnvironment.UpdateItemAsync(item, updateItemConfig).Result;
            

            TOut result = JsonSerializer.Deserialize<TOut>(operationResult.ToJson());
            

            return result;
        }

        /// <inheritdoc />
        public IEnumerable<TOut> Get()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TOut Get(int id)
        {
            var getItemConfig = new GetItemOperationConfig
            {
                ConsistentRead = true
            };
            
            Document operationResult = _tableEnvironment.GetItemAsync(id, getItemConfig).Result;

            TOut result = JsonSerializer.Deserialize<TOut>(operationResult.ToJson());

            return result;
        }

        #endregion
    }
}
