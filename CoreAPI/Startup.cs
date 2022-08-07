using System;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CoreAPIDemo.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Infrastructure.Context;
using WebAPI.Core.Interfaces;
using WebAPI.Infrastructure.Repository;
using WebAPI.Extensions;
using WebAPI.Infrastructure.Auth;
using WebAPI.Infrastructure.Services;
using NLog;
using System.IO;
using WebAPI.Infrastructure.Common;
using WebAPI.Core.Interfaces.Repositories;

namespace CoreAPIDemo
{
    public class Startup
    {
        private const string SecretKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var context = new CustomAssemblyLoadContext();
            services.AddControllers();
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDBContext>(option => option.UseMySQL(connectionString));
            services.AddSwaggerExtension();
            services.AddCors(op =>
            {
                if (CurrentEnvironment.EnvironmentName != "Production")
                {
                    op.AddPolicy("CorsPolicy", builder =>
                   builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials());
                }
                else
                {
                    op.AddPolicy("CorsPolicy", builder =>
                   builder.WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
                }
            });
            // jwt wire  up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                options.secretKey = SecretKey;
                options.PreviewTokenLinkValidTime = Convert.ToInt32(jwtAppSettingOptions[nameof(JwtIssuerOptions.PreviewTokenLinkValidTime)]);
                options.EmailLinkValidTime = Convert.ToInt32(jwtAppSettingOptions[nameof(JwtIssuerOptions.EmailLinkValidTime)]);
                options.JWTWithoutUserKey = jwtAppSettingOptions[nameof(JwtIssuerOptions.JWTWithoutUserKey)];
                options.JWTWithoutPasswordKey = jwtAppSettingOptions[nameof(JwtIssuerOptions.JWTWithoutPasswordKey)];
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.Configure<ImagesSettings>(Configuration.GetSection("ImagesSettings"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IShopsRepository, ShopsRepository>();
            services.AddScoped<IShopsService, ShopsService>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers()
               .ConfigureApiBehaviorOptions(options =>
               {
                   options.InvalidModelStateResponseFactory = context =>
                   {
                       var result = new BadRequestObjectResult(context.ModelState);

                       result.ContentTypes.Add(MediaTypeNames.Application.Json);
                       result.ContentTypes.Add(MediaTypeNames.Application.Xml);

                       return result;
                   };
               });

            services.AddSingleton<ILoggerService, LoggerService>();

            // Now register our services with Autofac container.
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Result")).SingleInstance();

            builder.Populate(services);
            var container = builder.Build();
            // Create the IServiceProvider based on the container.
            services.AddMemoryCache();

            services.AddRouting();

            // This is required for HTTP client integration
            services.AddHttpClient();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.ConfigureExceptionMiddleware();
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();


            if (env.IsProduction())
            {
                app.UseSentryTracing();
            }
            if (env.IsStaging())
            {
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("X-Evironment", "Staging");
                    await next();
                });
            }


            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
