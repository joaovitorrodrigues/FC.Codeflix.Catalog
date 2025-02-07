using FC.Codeflix.Catalog.Domain.Entity;
using Moq;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategories
{
    [Collection(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTest
    {
        private readonly ListCategoriesTestFixture _fixture;

        public ListCategoriesTest(ListCategoriesTestFixture fixture)
        => _fixture = fixture;

        [Fact(DisplayName = nameof(List))]
        [Trait("Application", "ListCategories - Use Cases")]
        public async Task List()
        {
            var categoriesExampleList = _fixture.GetCategoriesList();
            var repositoryMock = _fixture.GetRepositoryMock();
            var input = new ListCategoriesInput(
                page: 2,
                perPage: 15,
                search: "search-example",
                sort: "name",
                dir: SearchOrder.Asc
            );


            repositoryMock.Setup(x => x.Search(It.Is<SearchInput>(
                searchInput.Page == input.page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir), It.IsAny<CancellationToken>()
            )).ReturnsAsync(new OutputSearch<Category>(
                currentPage: input.page,
                perPage: input.perPage,
                Items: (IReadOnlyList<Category>)categoriesExampleList,
                total: 70
            ));

            var useCase = new ListCategories(repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();

            
        }
    }
}
