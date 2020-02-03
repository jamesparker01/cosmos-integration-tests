using Microsoft.Azure.Cosmos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosIntegrationTests
{
    public class Repository
    {
        private readonly Container _container;
        private const string AccountEndpoint = "https://localhost:8082";
        private const string AccountKey =
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public Repository()
        {
            //https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
            //https://github.com/Azure/azure-cosmos-dotnet-v3/blob/master/Microsoft.Azure.Cosmos.Samples/Usage/ItemManagement/Program.cs
            var client = new CosmosClient(AccountEndpoint, AccountKey);
            _container = client.GetContainer("databaseId", "containerId");
        }

        public async Task Create(Page page)
        {
            var pageDto = new PageDto().FromDomain(page);
            
            var response = await _container.CreateItemAsync(pageDto, new PartitionKey(page.Id.ToString()));
        }

        public async Task<Page> Read(Guid id)
        {
            try
            {
                 var response = await _container.ReadItemAsync<PageDto>(
                    id.ToString(),
                    new PartitionKey(id.ToString()));

                return response.Resource.ToDomain();
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<Page> ReadBySlug(string slug)
        {
            var query = new QueryDefinition(
                    "select * from pages p where p.slug = @Slug")
                .WithParameter("@Slug", slug);

            var resultSet = _container.GetItemQueryIterator<PageDto>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    MaxItemCount = 1
                });

            while (resultSet.HasMoreResults)
            {
                var response = await resultSet.ReadNextAsync();
                return response.First().ToDomain();
            }

            return null;
        }

        public async Task Upsert(Page page)
        {
             var response = await _container.UpsertItemAsync(
                new PageDto().FromDomain(page),
                partitionKey: new PartitionKey(page.Id.ToString()));
        }

        public async Task Delete(Guid id)
        {
             await _container.DeleteItemAsync<PageDto>(
                 id.ToString(),
                new PartitionKey(id.ToString()));
        }
    }
}
