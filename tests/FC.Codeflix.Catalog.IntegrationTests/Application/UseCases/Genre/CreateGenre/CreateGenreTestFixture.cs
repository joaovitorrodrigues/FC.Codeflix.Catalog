using FC.Codeflix.Catalog.IntegrationTests.Base;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Genre.CreateGenre
{
    [CollectionDefinition(nameof(CreateGenreTestFixture))]
    public class CreateGenreTestFixtureCollection : ICollectionFixture<CreateGenreTestFixture> { }
    public class CreateGenreTestFixture : BaseFixture
    {
    }
}
