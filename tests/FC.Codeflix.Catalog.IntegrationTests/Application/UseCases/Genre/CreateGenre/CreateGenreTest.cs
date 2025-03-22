
namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Genre.CreateGenre
{
    [Collection(nameof(CreateGenreTestFixture))]
    public class CreateGenreTest
    {
        private readonly CreateGenreTestFixture _fixture;
        public CreateGenreTest(CreateGenreTestFixture fixture)
        {
            _fixture = fixture;
        }

    }
}
