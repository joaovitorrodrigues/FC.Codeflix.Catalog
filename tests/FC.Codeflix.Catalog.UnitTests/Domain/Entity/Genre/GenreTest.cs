using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using EntityDomain = FC.Codeflix.Catalog.Domain.Entity;
namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Genre
{
    [Collection(nameof(GenreTestFixture))]
    public class GenreTest
    {
        private readonly GenreTestFixture _fixture;

        public GenreTest(GenreTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Genre - Aggregates")]
        public void Instantiate()
        {
            var genreName = _fixture.GetValidName();
            var genre = new EntityDomain.Genre(genreName);

            genre.Should().NotBeNull();
            genre.Name.Should().Be(genreName);
            genre.IsActive.Should().BeTrue();
            genre.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }


        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Genre - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            var genreName = _fixture.GetValidName();
            var genre = new EntityDomain.Genre(genreName, isActive);

            genre.Should().NotBeNull();
            genre.Name.Should().Be(genreName);
            genre.IsActive.Should().Be(isActive);
            genre.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Theory(DisplayName = nameof(Activate))]
        [Trait("Domain", "Genre - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void Activate(bool isActive)
        {
            var genre = _fixture.GetValidGenre(isActive);

            genre.Activate();

            genre.Should().NotBeNull();
            genre.IsActive.Should().BeTrue();
            genre.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Theory(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Genre - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void Deactivate(bool isActive)
        {
            var genre = _fixture.GetValidGenre(isActive);

            genre.Deactivate();

            genre.Should().NotBeNull();
            genre.IsActive.Should().BeFalse();
            genre.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact(DisplayName =nameof(Update))]
        [Trait("Domain", "Genre - Aggregates")]
        public void Update()
        {
            var newName = _fixture.GetValidName();
            var genre = _fixture.GetValidGenre();
            var oldIsActive = genre.IsActive;

            genre.Update(newName);

            genre.Should().NotBeNull();
            genre.Name.Should().Be(newName);
            genre.IsActive.Should().Be(oldIsActive);
            genre.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Theory(DisplayName = nameof(InstantiateThrowWhenNameEmpty))]
        [Trait("Domain", "Genre - Aggregates")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void InstantiateThrowWhenNameEmpty(string? name)
        {
            var action = () =>  new EntityDomain.Genre(name!);

            action.Should().Throw<EntityValidationException>().WithMessage("Name should not be null or empty");
                       
        }

        [Theory(DisplayName = nameof(UpdateThrowWhenNameIsEmpty))]
        [Trait("Domain", "Genre - Aggregates")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void UpdateThrowWhenNameIsEmpty(string? name)
        {
            var genre = _fixture.GetValidGenre();
            var oldIsActive = genre.IsActive;

            var action = () => genre.Update(name);

            action.Should().Throw<EntityValidationException>().WithMessage("Name should not be null or empty");
            
        }

    }
}
