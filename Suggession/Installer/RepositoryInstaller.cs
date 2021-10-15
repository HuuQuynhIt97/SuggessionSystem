using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Suggession.Data;

namespace Suggession.Installer
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        }
    }
}
