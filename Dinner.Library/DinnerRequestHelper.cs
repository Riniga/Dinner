using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Dinner.Library;


public class DinnerRequestHelper
{
    private static string partitionKeyPath = "/id";
    private static CosmosData data = new CosmosData();

    public static async Task<List<DinnerRequest>> GetDinnerRequestsAsync()
    {
        var queryDefinition = new QueryDefinition("SELECT * FROM " + data.dinnerRequestContainerId); 
        var container = await GetContainer();
        var queryResultSetIterator = container.GetItemQueryIterator<DinnerRequest>(queryDefinition);
        List<DinnerRequest> dinnerRequests= new List<DinnerRequest>();
        
        while (queryResultSetIterator.HasMoreResults)
        {
            var currentResultSet = await queryResultSetIterator.ReadNextAsync();
            dinnerRequests.AddRange(currentResultSet);
        }
        return dinnerRequests;
    }

    public static async Task<DinnerRequest> GetDinnerRequestAsync(string id)
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM DinnerRequest WHERE DinnerRequest.id='" + id+"'");
            var container = await GetContainer();
            var queryResultSetIterator = container.GetItemQueryIterator<DinnerRequest>(queryDefinition);
            List<DinnerRequest> users = new List<DinnerRequest>();

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                users.AddRange(currentResultSet);
            }
            return users.FirstOrDefault();
        }


    public static async Task CreateDinnerRequestAsync(DinnerRequest dinnerRequest)
    {
        var container = await GetContainer();
        await container.CreateItemAsync<DinnerRequest>(dinnerRequest, new PartitionKey(dinnerRequest.Id.ToString()));
    }
    internal static async Task DeleteDinnerRequestAsync(string id)
    {
        var container = await GetContainer();
        await container.DeleteItemAsync<DinnerRequest>(id, new PartitionKey(id));
    }
    public static async Task UpdateDinnerRequestAsync(DinnerRequest dinnerRequest)
    {
        var container = await GetContainer();
        await container.ReplaceItemAsync<DinnerRequest>(dinnerRequest, dinnerRequest.Id.ToString(), new PartitionKey(dinnerRequest.Id.ToString()));
    }

    private static async Task<Container> GetContainer()
    {
        var cosmosClient = new CosmosClient(data.EndpointUrl, data.PrimaryKey);
        Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(data.databaseId);
        Container container = await database.CreateContainerIfNotExistsAsync(data.dinnerRequestContainerId, partitionKeyPath);
        return container;
    }
}