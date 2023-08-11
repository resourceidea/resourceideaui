using ResourceIdea.Web.Core.Handlers.Engagements;
using ResourceIdea.Web.Core.Handlers.Tasks;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Debug)
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting up the ResourceIdea web application");

    var builder = WebApplication.CreateBuilder(args);
    
    builder.Host.UseSerilog();

    builder.Services.AddRazorPages();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ResourceIdeaDBContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = true;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ResourceIdeaDBContext>();
    builder.Services.ConfigureApplicationCookie(options => {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
    });

    builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
    builder.Services.AddScoped<IClientsHandler, ClientsHandler>();
    builder.Services.AddScoped<IEngagementService, EngagementService>();
    builder.Services.AddScoped<ITaskService, TaskService>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (!app.Environment.IsDevelopment())
    {
        //app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    else
    {
        //app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
    }

    app.UseExceptionHandler(exceptionHandlerApp => {
        exceptionHandlerApp.Run(async context => {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = Text.Html;

            await context.Response.WriteAsync(StringConstants.HTML_ERROR);

            var feature = context.Features.Get<IExceptionHandlerPathFeature>();
            if (feature?.Error is MissingSubscriptionCodeException)
            {
                context.Response.Redirect("/logout");
            }
        });
    });

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapRazorPages();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var userManager = services.GetService<UserManager<ApplicationUser>>();
    var roleManager = services.GetService<RoleManager<IdentityRole>>();
    app.SeedAdminUser(userManager!, roleManager!);

    app.UseCheckSubscriptionMiddleware();

    app.Run();
}
catch(Exception ex)
{
    Log.Error(ex, "An error occurred while starting the application");
}
finally
{
    await Log.CloseAndFlushAsync();
}
