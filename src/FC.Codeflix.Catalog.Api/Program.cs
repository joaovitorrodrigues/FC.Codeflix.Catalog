using FC.Codeflix.Catalog.Api.Configurations;

namespace FC.Codeflix.Catalog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services
                .AddAndConfigureControllers()
                .AddUseCases()
                .AddAppConnections();


            var app = builder.Build();
            app.UseDocumentation();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
