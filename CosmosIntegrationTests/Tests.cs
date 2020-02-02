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
            Page page, 
            Page updatedPage)
        {
            // given
            await repository.Create(page);

            // when
            var readResponse = await repository.Read(page.Id);

            // then
            Assert.That(readResponse.Id, Is.EqualTo(page.Id));
            Assert.That(readResponse.Slug, Is.EqualTo(page.Slug));
            Assert.That(readResponse.Title, Is.EqualTo(page.Title));

            // when
            updatedPage.Id = page.Id;
            await repository.Upsert(updatedPage);
            var updatedReadResponse = await repository.Read(page.Id);

            // then
            Assert.That(updatedReadResponse.Id, Is.EqualTo(updatedPage.Id));
            Assert.That(updatedReadResponse.Slug, Is.EqualTo(updatedPage.Slug));
            Assert.That(updatedReadResponse.Title, Is.EqualTo(updatedPage.Title));

            // when
            await repository.Delete(updatedPage.Id);

            // then
            var deletedPage = await repository.Read(updatedPage.Id);
            Assert.That(deletedPage, Is.Null);
        }
    }
}
