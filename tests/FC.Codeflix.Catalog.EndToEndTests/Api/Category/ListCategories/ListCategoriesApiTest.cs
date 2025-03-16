using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FluentAssertions;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.EndToEndTests.Extensions.DateTime;
using FC.Codeflix.Catalog.EndToEndTests.Models;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{

    [Collection(nameof(ListCategoriesApiTestFixture))]
    public class ListCategoriesApiTest : IDisposable
    {
        private readonly ListCategoriesApiTestFixture _fixture;

        public ListCategoriesApiTest(ListCategoriesApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ListCategoriesAndTotalByDefault()
        {
            int defaultPerPage = 15;

            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);


            var (response, output) = await _fixture.ApiClient
                .Get<TestApiResponseList<CategoryModelOutput>>($"/categories");


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Meta.Should().NotBeNull();
            output.Data.Should().HaveCount(defaultPerPage);

            output.Meta.Total.Should().Be(exampleCategoriesList.Count);
            output.Meta.CurrentPage.Should().Be(1);
            output.Meta.PerPage.Should().Be(defaultPerPage);

            foreach (CategoryModelOutput outputItem in output.Data)
            {
                var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                exampleItem.Name.Should().Be(outputItem.Name);
                exampleItem.Description.Should().Be(outputItem.Description);
                exampleItem.IsActive.Should().Be(outputItem.IsActive);
                exampleItem.CreatedAt.TrimMilliseconds().Should().Be(outputItem.CreatedAt.TrimMilliseconds());
            }

        }

        [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ListCategoriesAndTotal()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);

            var input = new ListCategoriesInput
            {
                Page = 1,
                PerPage = 5
            };

            var (response, output) = await _fixture.ApiClient
                .Get<TestApiResponseList<CategoryModelOutput>>($"/categories", input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Data.Should().HaveCount(input.PerPage);
            output.Meta.Total.Should().Be(exampleCategoriesList.Count);
            output.Meta.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            foreach (CategoryModelOutput outputItem in output.Data)
            {
                var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                exampleItem.Name.Should().Be(outputItem.Name);
                exampleItem.Description.Should().Be(outputItem.Description);
                exampleItem.IsActive.Should().Be(outputItem.IsActive);
                exampleItem.CreatedAt.TrimMilliseconds().Should().Be(outputItem.CreatedAt.TrimMilliseconds());
            }

        }

        [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ItemsEmptyWhenPersistenceEmpty()
        {
            var (response, output) = await _fixture.ApiClient
                .Get<TestApiResponseList<CategoryModelOutput>>($"/categories");

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Data.Should().HaveCount(0);
            output.Meta.Total.Should().Be(0);
        }

        [Theory(DisplayName = (nameof(ListPaginated)))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task ListPaginated(
           int quantityCategoriesToGenerate,
           int page,
           int perPage,
           int expectedQuantityItems
       )
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategoriesToGenerate);
            await _fixture.Persistence.InsertList(exampleCategoriesList);

            var input = new ListCategoriesInput
            {
                Page = page,
                PerPage = perPage
            };

            var (response, output) = await _fixture.ApiClient
                .Get<TestApiResponseList<CategoryModelOutput>>($"/categories", input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Data.Should().HaveCount(expectedQuantityItems);
            output.Meta.Total.Should().Be(exampleCategoriesList.Count);
            output.Meta.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            foreach (CategoryModelOutput outputItem in output.Data)
            {
                var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                exampleItem.Name.Should().Be(outputItem.Name);
                exampleItem.Description.Should().Be(outputItem.Description);
                exampleItem.IsActive.Should().Be(outputItem.IsActive);
                exampleItem.CreatedAt.TrimMilliseconds().Should().Be(outputItem.CreatedAt.TrimMilliseconds());
            }
        }

        [Theory(DisplayName = nameof(SearchByText))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData("Action", 1, 5, 1, 1)]
        [InlineData("Horror", 1, 5, 3, 3)]
        [InlineData("Horror", 2, 5, 0, 3)]
        [InlineData("Sci-fi", 1, 5, 4, 4)]
        [InlineData("Sci-fi", 1, 2, 2, 4)]
        [InlineData("Sci-fi", 2, 3, 1, 4)]
        [InlineData("Sci-fi Other", 1, 3, 0, 0)]
        [InlineData("Robots", 1, 5, 2, 2)]
        public async Task SearchByText(string search,
          int page,
          int perPage,
          int expectedQuantityItemsReturned,
          int expectedQuantityTotalItems
       )
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesListWithNames(new List<string>() {
                "Action",
                "Horror",
                "Horror - Based on Real Facts",
                "Horror - Robots",
                "Drama",
                "Sci-fi IA",
                "Sci-fi Space",
                "Sci-fi Robots",
                "Sci-fi Future"
            });

            await _fixture.Persistence.InsertList(exampleCategoriesList);

            var input = new ListCategoriesInput(page, perPage, search);

            var (response, output) = await _fixture.ApiClient
                .Get<TestApiResponseList<CategoryModelOutput>>($"/categories", input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Data.Should().HaveCount(expectedQuantityItemsReturned);
            output.Meta.Total.Should().Be(expectedQuantityTotalItems);
            output.Meta.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            foreach (CategoryModelOutput outputItem in output.Data)
            {
                var exampleItem = exampleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                exampleItem.Name.Should().Be(outputItem.Name);
                exampleItem.Description.Should().Be(outputItem.Description);
                exampleItem.IsActive.Should().Be(outputItem.IsActive);
                exampleItem.CreatedAt.TrimMilliseconds().Should().Be(outputItem.CreatedAt.TrimMilliseconds());
            }
        }

        [Theory(DisplayName = nameof(SearchOrdered))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("id", "asc")]
        [InlineData("id", "desc")]
        public async Task SearchOrdered(
       string orderBy,
       string order
       )
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var useCaseOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var input = new ListCategoriesInput(1, 20, "", orderBy, useCaseOrder);

            var (response, output) = await _fixture.ApiClient
                .Get<TestApiResponseList<CategoryModelOutput>>($"/categories", input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Data.Should().HaveCount(exampleCategoriesList.Count);
            output.Meta.Total.Should().Be(exampleCategoriesList.Count);
            output.Meta.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);


            var expectedOrderedList = _fixture.CloneCategoriesListOrdered(exampleCategoriesList, orderBy, useCaseOrder);

            for (int i = 0; i < expectedOrderedList.Count; i++)
            {
                var expectedItem = expectedOrderedList[i];
                var outputItem = output.Data[i];

                expectedItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(expectedItem.Name);
                outputItem.Id.Should().Be(expectedItem.Id);
                outputItem.Description.Should().Be(expectedItem.Description);
                outputItem.IsActive.Should().Be(expectedItem.IsActive);
                outputItem.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());
            }
        }

        [Theory(DisplayName = nameof(SearchOrderedDate))]
        [InlineData("CreatedAt", "asc")]
        [InlineData("CreatedAt", "desc")]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task SearchOrderedDate(
      string orderBy,
      string order
      )
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var useCaseOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var input = new ListCategoriesInput(1, 20, "", orderBy, useCaseOrder);

            var (response, output) = await _fixture.ApiClient
                .Get<TestApiResponseList<CategoryModelOutput>>($"/categories", input);


            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Data.Should().HaveCount(exampleCategoriesList.Count);
            output.Meta!.Total.Should().Be(exampleCategoriesList.Count);
            output.Meta.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);


            DateTime? lastDate = null;

            foreach (var outputItem in output.Data)
            {
                var exampleItem = exampleCategoriesList
                    .FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMilliseconds().Should().Be(
                    exampleItem.CreatedAt.TrimMilliseconds()
                );
                if (lastDate != null)
                {
                    if (order == "asc")
                        Assert.True(outputItem.CreatedAt >= lastDate);
                    else
                        Assert.True(outputItem.CreatedAt <= lastDate);
                }
                lastDate = outputItem.CreatedAt;
            }
        }
        public void Dispose()
        => _fixture.CleanPersistence();


    }
}
