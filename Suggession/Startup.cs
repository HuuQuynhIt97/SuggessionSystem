using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Suggession.Helpers;
using System.IO;
using System;
using Suggession.Helpers.SignalR;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Suggession.Helpers.AutoMapper;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Suggession.Data;
using Suggession._Services.Services;
using Suggession._Repositories.Interface;

namespace Suggession
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpContextAccessor();
            services.AddControllers()
             .AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
             });
            var connetionString = Configuration.GetConnectionString("DefaultConnection");
            // Configure DbContext with Scoped lifetime   
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(connetionString);
            });

            services.AddAutoMapper(typeof(Startup))
                .AddScoped<IMapper>(sp =>
                {
                    return new Mapper(AutoMapperConfig.RegisterMappings());
                })
                .AddSingleton(AutoMapperConfig.RegisterMappings());

            //Swagger
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Score KPI", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0]}
                };
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
            services.AddCors();


            services.AddSignalR();

            //Repository

            services.AddScoped<IAccountGroupAccountRepository, AccountGroupAccountRepository>();
            services.AddScoped<IAccountGroupRepository, AccountGroupRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IIdeaRepository, IdeaRepository>();
            services.AddScoped<IIdeaHistoryRepository, IdeaHistoryRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<ITabRepository, TabRepository>();
            services.AddScoped<IUploadFileRepository, UploadFileRepository>();

            //Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountGroupService, AccountGroupService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IAccountGroupAccountService, AccountGroupAccountService>();
            services.AddScoped<ITabService, TabService>();
            services.AddScoped<IIdeaService, IdeaService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin());
            app.UseSwagger();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SuggessionHub>("/suggession-hub");
            });
            //app.UseSpaStaticFiles();
            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = @"wwwroot/ClientApp";
            //    //if (env.IsDevelopment())
            //    //{
            //    //    spa.Options.SourcePath = @"../dmr-app";
            //    //    spa.UseAngularCliServer(npmScript: "start");
            //    //}
            //});
        }
    }
}
