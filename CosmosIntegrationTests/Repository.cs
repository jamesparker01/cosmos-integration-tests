using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace CosmosIntegrationTests
{
    public class Repository
    {
        private Container _container;
        public Repository()
        {
            //https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
            //https://github.com/Azure/azure-cosmos-dotnet-v3/blob/master/Microsoft.Azure.Cosmos.Samples/Usage/ItemManagement/Program.cs
            var client = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            //client.CreateDatabaseIfNotExistsAsync
            _container = client.GetContainer("databaseId", "containerId");
        }

        public async Task Create(Page page)
        {
            var pageDto = new PageDto().FromDomain(page);
            
            var response = await _container.CreateItemAsync(pageDto, new PartitionKey(page.Id.ToString()));
        }

        public async Task<Page> Read(Guid id)
        {
            var response = await _container.ReadItemAsync<PageDto>(
                id.ToString(),
                new PartitionKey(id.ToString()));

            return response.Resource.ToDomain();
        }
    }
}
