using System;
using System.Collections.Concurrent;
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
using Microsoft.OpenApi.Models;
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
            services.AddMvcCore()
                .AddApiExplorer();
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Directory.GetCurrentDirectory()));
            services.AddSingleton<IImageParameterFixer, DefaultImageParameterFixer>();
            services.AddEntityFrameworkNpgsql();
            services.AddAuthorization();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = configuration["AuthServer:ApiName"];
                    //options.InboundJwtClaimTypeMap["sub"] = AbpClaimTypes.UserId;
                    //options.InboundJwtClaimTypeMap["role"] = AbpClaimTypes.Role;
                    //options.InboundJwtClaimTypeMap["email"] = AbpClaimTypes.Email;
                    //options.InboundJwtClaimTypeMap["email_verified"] = AbpClaimTypes.EmailVerified;
                    //options.InboundJwtClaimTypeMap["phone_number"] = AbpClaimTypes.PhoneNumber;
                    //options.InboundJwtClaimTypeMap["phone_number_verified"] = AbpClaimTypes.PhoneNumberVerified;
                    //options.InboundJwtClaimTypeMap["name"] = AbpClaimTypes.UserName;
                });
            services.AddImageService(option =>
                {
                    option.Filters = new[] {".jpg", ".jpeg", ".gif", ".png", ".bmp"};
                })
                .UseEntityFrameworkStore(builder =>
                {
                    //builder.UseSqlServer("Server=localhost; Database=ImageDb; Trusted_Connection=True;");
                    builder.UseNpgsql(configuration.GetConnectionString("Default"));
                    //builder.UseMySql(configuration.GetConnectionString("Mysql"));
                });
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        //.WithOrigins(
                        //    configuration["CorsOrigins"]
                        //        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        //        .ToArray()
                        //)
                        .AllowAnyOrigin()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    //.AllowCredentials();
                });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "图片资源API", Version = "v1"});
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ImageApi.xml"));
                c.OperationFilter<SwaggerFileUploadFilter>();
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                var apiSecurityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                                {Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
                        },
                        new List<string>()
                    }
                };
                c.AddSecurityRequirement(apiSecurityRequirement);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(DefaultCorsPolicyName);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
            app.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "图片资源API"); });
        }
    }
}
