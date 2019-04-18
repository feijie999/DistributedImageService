using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Core;
using ImageCore.Extensions;
using ImageCore.Persistence.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.Swagger;

namespace ImageApi
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "public";

        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _hostingEnvironment = env;
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Directory.GetCurrentDirectory()));
            services.AddSingleton<IImageParameterFixer, DefaultImageParameterFixer>();
            services.AddEntityFrameworkNpgsql();
            services.AddImageService(option =>
                {
                    option.Filters = new[] {".jpg", ".jpeg", ".gif", ".png", ".bmp"};
                })
                .UseEntityFrameworkStore(builder =>
                {
                    //builder.UseSqlServer("Server=localhost; Database=ImageDb; Trusted_Connection=True;");
                    //builder.UseNpgsql(_appConfiguration.GetConnectionString("Default"));
                    builder.UseMySql(configuration.GetConnectionString("Mysql"));
                });
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "图片资源API", Version = "v1"});
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ImageApi.xml"));
                c.OperationFilter<SwaggerFileUploadFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(DefaultCorsPolicyName);
            app.UseMvcWithDefaultRoute();
            app.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "图片资源API"); });
        }
    }
}
