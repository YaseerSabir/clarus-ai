using ClarusAI.Web.Components;
using ClarusAI.Business.Services;
using ClarusAI.Core.Interfaces;
using ClarusAI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Configure Azure Key Vault if available
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CLARUSAI_KEYVAULT_URL")))
{
    var keyVaultUrl = Environment.GetEnvironmentVariable("CLARUSAI_KEYVAULT_URL");
    // Azure Key Vault configuration for production deployment
    // builder.Configuration.AddAzureKeyVault(keyVaultUrl!, new DefaultAzureCredential());
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add HTTPS enforcement
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

// Configure JWT Authentication
var jwtSecret = Environment.GetEnvironmentVariable("CLARUSAI_JWT_SECRET");

if (string.IsNullOrEmpty(jwtSecret))
{
    if (builder.Environment.IsDevelopment())
    {
        // Use a development-only secret key
        jwtSecret = "Development-Secret-Key-32-Characters-Long-For-Testing-Only";
    }
    else
    {
        throw new InvalidOperationException("JWT secret key is required. Set CLARUSAI_JWT_SECRET environment variable.");
    }
}

if (!string.IsNullOrEmpty(jwtSecret))
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"] ?? "ClarusAI",
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"] ?? "ClarusAI-Users",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
}

builder.Services.AddAuthorization();

// Configure Database
var connectionString = Environment.GetEnvironmentVariable("CLARUSAI_CONNECTION_STRING") 
                      ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<ClarusAIDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Register services
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add security headers
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; font-src 'self' data:;");
    
    if (!app.Environment.IsDevelopment())
    {
        context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
    }
    
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
