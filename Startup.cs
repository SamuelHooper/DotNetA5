using ApplicationTemplate.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApplicationTemplate
{
    internal class Startup
    {
        public IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddFile("app.log");
            });

            // Add code here to register any interface and concrete service groups
            services.AddSingleton<IMainService, MainService>();

            return services.BuildServiceProvider();
        }
    }
}
