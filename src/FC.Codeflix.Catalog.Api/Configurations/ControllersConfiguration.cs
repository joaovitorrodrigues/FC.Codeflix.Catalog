﻿using FC.Codeflix.Catalog.Api.Filters;

namespace FC.Codeflix.Catalog.Api.Configurations
{
    public static class ControllersConfiguration
    {
        public static IServiceCollection AddAndConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add(typeof(ApiGlobalExceptionFilter)));
            services.AddDocumentaion();

            return services;
        }

        private static IServiceCollection AddDocumentaion(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        public static WebApplication UseDocumentation(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    }
}
