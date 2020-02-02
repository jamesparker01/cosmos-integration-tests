using AutoFixture.NUnit3;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CosmosIntegrationTests
{
    public class Tests
    {
        [Test, AutoData]
        public async Task end_to_end(
            Repository repository,
            Page page)
        {
            // given
            await repository.Create(page);

            // when
            var readResponse = await repository.Read(page.Id);

            // then
            Assert.That(readResponse.Id, Is.EqualTo(page.Id));
            Assert.That(readResponse.Slug, Is.EqualTo(page.Slug));
            Assert.That(readResponse.Title, Is.EqualTo(page.Title));
        }
    }
}
