using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Suggession.Services;

namespace Suggession.Installer
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Code omitted for brevity
            services.AddSignalR();
            // configure DI for application services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountTypeService, AccountTypeService>();


            services.AddScoped<IMailingService, MailingService>();
           

            services.AddScoped<IAccountGroupPeriodService, AccountGroupPeriodService>();
            services.AddScoped<IAccountGroupService, AccountGroupService>();
            

       
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IContributionService, ContributionService>();
            services.AddScoped<IOCService, OCService>();
            services.AddScoped<IAccountGroupAccountService, AccountGroupAccountService>();
            services.AddScoped<IOCNewService, OCNewService>();
            services.AddScoped<ITabService, TabService>();
            services.AddScoped<IIdeaService, IdeaService>();
        }
    }
}
