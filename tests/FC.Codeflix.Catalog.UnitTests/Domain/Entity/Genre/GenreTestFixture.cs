using FC.Codeflix.Catalog.UnitTests.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Genre
{
    [CollectionDefinition(nameof(GenreTestFixture))]
    public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture> { }
    public class GenreTestFixture : BaseFixture
    {
        public string GetValidName()
            => Faker.Commerce.Categories(1)[0];

        public DomainEntity.Genre GetValidGenre(bool isActive = true,
            List<Guid>? categoriesIdsList = null)
        {
            var genre = new DomainEntity.Genre(GetValidName(), isActive);
            if(categoriesIdsList is not null)
            {
                foreach (var categoryId in categoriesIdsList)
                {
                    genre.AddCategory(categoryId);
                }
            }

            return genre;
        }

        public List<Guid> GetCategoriesId(int quantity = 1)
        {
            var categoriesId = new List<Guid>();
            for (int i = 0; i < quantity; i++)
            {
                categoriesId.Add(Guid.NewGuid());
            }
            return categoriesId;
        }

    }
}
