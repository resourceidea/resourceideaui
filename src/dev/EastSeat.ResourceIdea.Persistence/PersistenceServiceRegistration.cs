using System.Text;
using System.Text.Json;

using EastSeat.ResourceIdea.Application.Contracts.Identity;
using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Models;
using EastSeat.ResourceIdea.Persistence.Models;
using EastSeat.ResourceIdea.Persistence.Repositories;
using EastSeat.ResourceIdea.Persistence.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EastSeat.ResourceIdea.Persistence;

/// <summary>
/// Persistent services registration.
/// </summary>
public static class PersistenceServiceRegistration
{
    /// <summary>
    /// Add data persistence services to the service collection.
    /// </summary>
    /// <param name="services">Services collection.</param>
    /// <param name="configuration">App configurations.</param>
    /// <returns>Registered services collection.</returns>
    public static IServiceCollection AddApiPersistentServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.RegisterAuthIdentityServices(configuration);

        // TODO: Move the JWT setting to configuration store.
        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(o => {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"] ?? string.Empty))
                    };

                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c => {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context => {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize("401 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context => {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonSerializer.Serialize("403 Not authorized");
                            return context.Response.WriteAsync(result);
                        }
                    };
                });

        services.RegisterAppServices();

        return services;
    }

    /// <summary>
    /// Register persistence services to the services collection.
    /// </summary>
    /// <param name="services">Services collection.</param>
    /// <param name="configuration">App configuration.</param>
    /// <returns>Services collection.</returns>
    public static IServiceCollection AddWebPersistentServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.RegisterAuthIdentityServices(configuration);
        services.RegisterAppServices();

        return services;
    }

    private static void RegisterAppServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

        services.AddScoped<IAssetAssignmentRepository, AssetAssignmentRepository>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IEmployeeAssignmentRepository, EmployeeAssignmentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEngagementRepository, EngagementRepository>();
        services.AddScoped<IJobPositionRepository, JobPositionRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
    }

    private static void RegisterAuthIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ResourceIdeaDbContext>(
            options => options.UseSqlServer(connectionString: configuration.GetConnectionString("DefaultConnectionString"),
                                            sqlServerOptionsAction: b => b.MigrationsAssembly(typeof(ResourceIdeaDbContext).Assembly.FullName))
        );
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddIdentity<ApplicationUser, IdentityRole>(options => {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedEmail = false;

            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ResourceIdeaDbContext>()
        .AddDefaultTokenProviders();

        services.AddTransient<IResourceIdeaAuthenticationService, ResourceIdeaAuthenticationService>();
    }
}