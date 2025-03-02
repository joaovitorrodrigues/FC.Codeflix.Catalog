using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FluentAssertions;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{
    [Collection(nameof(ListCategoriesApiTestFixture))]
    public class ListCategoriesApiTest : IDisposable
    {
        private readonly ListCategoriesApiTestFixture _fixture;

        public ListCategoriesApiTest(ListCategoriesApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName =nameof(ListCategoriesAndTotalByDefault))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async void ListCategoriesAndTotalByDefault()
        {
            int defaultPerPage = 15;

            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);


            var (response, output) = await _fixture.ApiClient
                .Get<ListCategoriesOutput>($"/categories");


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Items.Should().HaveCount(defaultPerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.CurrentPage.Should().Be(1);
            output.PerPage.Should().Be(defaultPerPage);

            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                exampleItem.Name.Should().Be(outputItem.Name);
                exampleItem.Description.Should().Be(outputItem.Description);
                exampleItem.IsActive.Should().Be(outputItem.IsActive);
                exampleItem.CreatedAt.Should().Be(outputItem.CreatedAt);
            }

        }

        [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async void ListCategoriesAndTotal()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);

            var input = new ListCategoriesInput
            {
                Page = 1,
                PerPage = 5
            };

            var (response, output) = await _fixture.ApiClient
                .Get<ListCategoriesOutput>($"/categories", input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Items.Should().HaveCount(input.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.CurrentPage.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                exampleItem.Name.Should().Be(outputItem.Name);
                exampleItem.Description.Should().Be(outputItem.Description);
                exampleItem.IsActive.Should().Be(outputItem.IsActive);
                exampleItem.CreatedAt.Should().Be(outputItem.CreatedAt);
            }

        }

        [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async void ItemsEmptyWhenPersistenceEmpty()
        {
            var (response, output) = await _fixture.ApiClient
                .Get<ListCategoriesOutput>($"/categories");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Items.Should().HaveCount(0);
            output.Total.Should().Be(0);
        }

        public void Dispose()
        => _fixture.CleanPersistence();
    }
}
 