using Newtonsoft.Json;
using System;

namespace CosmosIntegrationTests
{
    public class PageDto
    {
        [JsonProperty("id")]
        public Guid Id {get; set;}
        [JsonProperty("title")]
        public string Title {get; set;}
        [JsonProperty("slug")]
        public string Slug {get; set;}

        public Page ToDomain()
        {
            return new Page
            {
                Id = Id,
                Slug = Slug,
                Title = Title
            };
        }

        public PageDto FromDomain(Page page)
        {
            return new PageDto
            {
                Id = page.Id,
                Slug = page.Slug,
                Title = page.Title
            };
        }
    }
}
