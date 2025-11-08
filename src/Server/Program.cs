using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using EastSeat.ResourceIdea.Server.Data;
using EastSeat.ResourceIdea.Shared.Models;
using EastSeat.ResourceIdea.Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var env = builder.Environment;
var configuration = builder.Configuration;
configuration.AddEnvironmentVariables();

// PostgreSQL connection from env vars
var pgHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
var pgDb = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "resourceidea";
var pgUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
var pgPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgres";
var connectionString = $"Host={pgHost};Database={pgDb};Username={pgUser};Password={pgPassword};";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 6;
    opts.Password.RequireDigit = true;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

var jwtIssuer = configuration["JWT_ISSUER"] ?? "ResourceIdea";
var jwtAudience = configuration["JWT_AUDIENCE"] ?? "ResourceIdea.Client";
var jwtKey = configuration["JWT_KEY"] ?? "ChangeThisDevKey1234567890"; // dev only
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = signingKey
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminOrManager", policy => policy.RequireRole("Admin", "Manager"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("[Migrations] Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Migrations] Error applying migrations: {ex.Message}");
    }
}

// Optional seeding (controlled by env vars to avoid hard-coded accounts)
var runSeed = Environment.GetEnvironmentVariable("RESOURCEIDEA_RUN_SEED");
var seedFile = Environment.GetEnvironmentVariable("RESOURCEIDEA_SEED_USERS_FILE") ?? "./config/seeds/sample-users.json";
var defaultPassword = Environment.GetEnvironmentVariable("RESOURCEIDEA_DEFAULT_PASSWORD") ?? "Pass@word1";
if (string.Equals(runSeed, "true", StringComparison.OrdinalIgnoreCase))
{
    try
    {
        await UserSeeder.SeedAsync(app.Services, seedFile, defaultPassword);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Seed] Error: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Auth endpoints
app.MapPost("/api/auth/register", async (RegisterRequest req, UserManager<IdentityUser> userManager) =>
{
    var user = new IdentityUser { UserName = req.Email, Email = req.Email };
    var result = await userManager.CreateAsync(user, req.Password);
    if (!result.Succeeded)
    {
        return Results.BadRequest(result.Errors.Select(e => e.Description));
    }
    return Results.Ok();
});

app.MapPost("/api/auth/login", async (LoginRequest req, UserManager<IdentityUser> userManager) =>
{
    var user = await userManager.FindByEmailAsync(req.Email);
    if (user == null) return Results.Unauthorized();
    if (!await userManager.CheckPasswordAsync(user, req.Password)) return Results.Unauthorized();
    var roles = await userManager.GetRolesAsync(user);

    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };
    claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

    var token = new JwtSecurityToken(
        issuer: jwtIssuer,
        audience: jwtAudience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(8),
        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
    );
    var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new AuthResponse
    {
        AccessToken = accessToken,
        ExpiresUtc = token.ValidTo,
        Email = user.Email ?? string.Empty,
        Roles = roles.ToArray()
    });
});

app.MapPost("/api/auth/forgot-password", async (ForgotPasswordRequest req, UserManager<IdentityUser> userManager) =>
{
    var user = await userManager.FindByEmailAsync(req.Email);
    if (user == null) return Results.Ok(); // Do not reveal
    var token = await userManager.GeneratePasswordResetTokenAsync(user);
    // TODO: Replace with email provider; log for now.
    Console.WriteLine($"Password reset token for {req.Email}: {token}");
    return Results.Ok();
});

app.MapPost("/api/auth/reset-password", async (ResetPasswordRequest req, UserManager<IdentityUser> userManager) =>
{
    var user = await userManager.FindByEmailAsync(req.Email);
    if (user == null) return Results.BadRequest("Invalid user");
    var result = await userManager.ResetPasswordAsync(user, req.Token, req.NewPassword);
    if (!result.Succeeded) return Results.BadRequest(result.Errors.Select(e => e.Description));
    return Results.Ok();
});

// Clients CRUD
app.MapGet("/api/clients", async (ApplicationDbContext db) =>
    await db.Clients.AsNoTracking().OrderBy(c => c.Name).ToListAsync());

app.MapGet("/api/clients/{id:guid}", async (Guid id, ApplicationDbContext db) =>
    await db.Clients.FindAsync(id) is Client c ? Results.Ok(c) : Results.NotFound());

app.MapPost("/api/clients", async (ClientCreateDto dto, ApplicationDbContext db) =>
{
    var entity = new Client { Name = dto.Name, Code = dto.Code };
    db.Clients.Add(entity);
    await db.SaveChangesAsync();
    return Results.Created($"/api/clients/{entity.Id}", entity);
}).RequireAuthorization("RequireAdminOrManager");

app.MapPut("/api/clients/{id:guid}", async (Guid id, ClientUpdateDto dto, ApplicationDbContext db) =>
{
    var entity = await db.Clients.FindAsync(id);
    if (entity == null) return Results.NotFound();
    entity.Name = dto.Name;
    entity.Code = dto.Code;
    entity.Status = dto.Status;
    await db.SaveChangesAsync();
    return Results.Ok(entity);
}).RequireAuthorization("RequireAdminOrManager");

app.MapDelete("/api/clients/{id:guid}", async (Guid id, ApplicationDbContext db) =>
{
    var entity = await db.Clients.FindAsync(id);
    if (entity == null) return Results.NotFound();
    db.Clients.Remove(entity);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization("RequireAdminOrManager");

app.Run();
