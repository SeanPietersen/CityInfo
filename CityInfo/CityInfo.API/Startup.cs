using AutoMapper;
using CityInfo.Application.Contract;
using CityInfo.Application.Profiles;
using CityInfo.Infrastructure;
using CityInfo.Infrastructure.DbContexts;
using CityInfo.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CityInfo.API
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Startup
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Startup(IConfiguration configuration)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Configuration = configuration;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IConfiguration Configuration { get; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        // This method gets called by the runtime. Use this method to add services to the container.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddControllers(options =>
           {
               options.ReturnHttpNotAcceptable = true;
           }).AddNewtonsoftJson()
           .AddXmlDataContractSerializerFormatters();

            services.AddSwaggerGen(c => {
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "CityInfo.API", Version = "v1" });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                c.IncludeXmlComments(xmlCommentsFullPath);

                c.AddSecurityDefinition("CityInfoApiBearerAuth", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "Input a valid token to access this API"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "CityInfoApiBearerAuth"
                            }
                        }, new List<string>()
                    }
                });
            });

            services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif

            services.AddSingleton<ICitiesDataStore, CitiesDataStore>();

            services.AddTransient<IPointOfInterestContract, PointOfInterestContract>();

            services.AddTransient<ICityContract, CityContract>();

            services.AddTransient<IAuthenticationContract, AuthenticationContract>();

            services.AddDbContext<CityInfoContext>(dbContextOptions => dbContextOptions.UseSqlite(
                Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

            services.AddScoped<ICityInfoRepository, CityInfoRepository>();

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CityProfile());
                mc.AddProfile(new PointOfInterestProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Authentication:Issuer"],
                        ValidAudience = Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(Configuration["Authentication:SecretForKey"]))
                    };

                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBeFromCapeTown", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("city", "Cape Town");
                });
            });

            services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CityInfo.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
