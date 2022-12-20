using IdentityFoolingAround.Data.Context;
using IdentityFoolingAround.Data.Models;
using IdentityFoolingAround.Extensions;
using IdentityFoolingAround.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("No connection string found with key 'DefaultConnection'");
builder.Services.AddDbContextPool<ApplicationContext>(options => options.UseSqlite(connectionString));

// Setup Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Setup Identity
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddSignInManager()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationContext>();

// Setup authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.Cookie.Name = IdentityConstants.ApplicationScheme;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);

        // From IdentityCookieAuthenticationBuilderExtensions
        options.LoginPath = new PathString("/Account/Login");
        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
        };
    })
    .AddCookie(IdentityConstants.ExternalScheme, options =>
    {
        options.Cookie.Name = IdentityConstants.ExternalScheme;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    })
    .AddCookie(IdentityConstants.TwoFactorUserIdScheme, options =>
    {
        options.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToReturnUrl = _ => Task.CompletedTask
        };
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    })
    .AddGoogle(googleOptions =>
    {
        var settings = builder.Configuration.GetSettings<GoogleSettings>(GoogleSettings.SectionName);
        googleOptions.ClientId = settings.ClientId;
        googleOptions.ClientSecret = settings.ClientSecret;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityFoolingAround v1"));
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();