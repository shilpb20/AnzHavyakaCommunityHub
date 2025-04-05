using CommunityHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CommunityHub.IntegrationTests
{
    public abstract class BaseTests : IDisposable
    {
        protected ServiceProvider ServiceProvider { get; private set; }
        protected ApplicationDbContext Context { get; private set; }

        protected BaseTests()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            AddServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            Context = ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        protected abstract void AddServices(IServiceCollection services);

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
            ServiceProvider.Dispose();
        }
    }
}
