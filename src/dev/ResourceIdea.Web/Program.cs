using ResourceIdea.Web.Core.Handlers.Tasks;

using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddTransient<IClientsHandler, ClientsHandler>();
builder.Services.AddTransient<IEngagementHandler, EngagementHandler>();
builder.Services.AddTransient<ITaskHandler, TaskHandler>();

var app = builder.Build();

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

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
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
