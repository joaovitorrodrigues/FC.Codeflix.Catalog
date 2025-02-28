using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FluentAssertions;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{
    [Collection(nameof(ListCategoriesApiTestFixture))]
    public class ListCategoriesApiTest
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

            foreach(CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                exampleItem.Name.Should().Be(outputItem.Name);
                exampleItem.Description.Should().Be(outputItem.Description);
                exampleItem.IsActive.Should().Be(outputItem.IsActive);
                exampleItem.CreatedAt.Should().Be(outputItem.CreatedAt);
            }

        }
    }
}
 