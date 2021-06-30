using Amazon;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace AwsToolset.Services
{
    public class DynamoDbService<TIn, TOut> : IDynamoDbService<TIn, TOut> where TIn : class
        where TOut: class

    {
        private AmazonDynamoDBClient _client;
        private AmazonDynamoDBConfig _config;
        private RegionEndpoint _regionEndpoint;
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
                // Create the DynamoDb config
                _config = new AmazonDynamoDBConfig { ServiceURL = _endPoint, RegionEndpoint = _regionEndpoint ?? RegionEndpoint.EUWest1 };
                // Use the config to feed the client
                _client = new AmazonDynamoDBClient(_config);
            }
        }

        /// <inheritdoc />
        public string Region
        {
            set => _regionEndpoint = RegionEndpoint.GetBySystemName(value);
        }

        /// <inheritdoc />
        public string Table { get => _tableEnvironment.TableName; set => _tableEnvironment = Amazon.DynamoDBv2.DocumentModel.Table.LoadTable(_client, value); }

        #endregion

        #region DynamoDb Operations

        /// <inheritdoc />
        public TOut Add(TIn inObject)
        {
            string jsonText = JsonSerializer.Serialize(inObject);
            var item = Document.FromJson(jsonText);

            var putItemConfig = new PutItemOperationConfig
            {
                ReturnValues = ReturnValues.AllNewAttributes
            }; 

            Document operationResult = _tableEnvironment.PutItemAsync(item, putItemConfig).Result;

            TOut result = JsonSerializer.Deserialize<TOut>(operationResult.ToJson());

            return result;
        }

        /// <inheritdoc />
        public TOut Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TOut Edit(TIn inObject)
        {
            string jsonText = JsonSerializer.Serialize(inObject);
            var item = Document.FromJson(jsonText);

            var updateItemConfig = new UpdateItemOperationConfig
            {
                ReturnValues = ReturnValues.AllNewAttributes
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
