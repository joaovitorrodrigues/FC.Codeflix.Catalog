using Bogus;
using FC.Codeflix.Catalog.Api;
using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FC.Codeflix.Catalog.EndToEndTests.Base
{
    public abstract class BaseFixture
    {
        protected Faker Faker { get; set; }
        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }

        public ApiClient ApiClient { get; set; }

        private readonly string _dbConnectionString;
        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebAppFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();
            ApiClient = new ApiClient(HttpClient);
            var configuration = (IConfiguration?)WebAppFactory.Services.GetService(typeof(IConfiguration));
            ArgumentNullException.ThrowIfNull(configuration);
            _dbConnectionString = configuration.GetConnectionString("CatalogDb")!;
        }


        public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var dbContext = new CodeflixCatalogDbContext(
                    new DbContextOptionsBuilder<CodeflixCatalogDbContext>().UseMySql(_dbConnectionString, ServerVersion.AutoDetect(_dbConnectionString))
                    .Options
                );
            return dbContext;
        }

        public void CleanPersistence()
        {
            var context = CreateDbContext();
            context.Database.EnsureDeleted();   
            context.Database.EnsureCreated();
        }
    }
}
