using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTest
    {
        private readonly UpdateCategoryApiTestFixture _fixture;

        public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
        => _fixture = fixture;

        [Fact(DisplayName =nameof(UpdateCategory))]
        [Trait("End2End/API", "Category/Update - Endpoints")]
        public async void UpdateCategory()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var exampleCategory = exampleCategoriesList[10];
            var input = _fixture.getExampleInput(exampleCategory.Id);

            var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>(
                "/categories",
                input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)input.IsActive!);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().NotBeSameDateAs(default);

            var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        }

    }
}
