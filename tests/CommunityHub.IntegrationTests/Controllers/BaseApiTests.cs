using AutoMapper;
using CommunityHub.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityHub.IntegrationTests
{
    public abstract class BaseApiTests : IDisposable, IClassFixture<ApplicationStartup>
    {
        protected readonly ApplicationStartup Application;
        protected readonly HttpClient HttpClient;
        protected readonly ApplicationDbContext DbContext;

        protected readonly IServiceProvider ServiceProvider;
        protected readonly IMapper Mapper;

        protected BaseApiTests(ApplicationStartup application)
        {
            Application = application;
            HttpClient = Application.Client;
            ServiceProvider = Application.WebApplicationFactory.Services;

            Mapper = ServiceProvider.GetRequiredService<IMapper>();
            DbContext = ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }
    }
}
