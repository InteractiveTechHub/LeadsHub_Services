using DbUp;
using InteractiveLeads.Core.Enums;
using LeadsHub.Api.Services;
using LeadsHub.Core.Bac;
using LeadsHub.Core.Hubs;
using LeadsHub.Core.Identity.Models;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Services;
using LeadsHub.Core.Services.Chat;
using LeadsHub.Core.Utility;
using LeadsHub.Data;
using LeadsHub.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

string connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
SD.ConnectString = connectionString;
SD.WhatsAppAPIBase = configuration["WhatsAppBase"];

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("http://localhost", "http://localhost:4200", "http://tecnoprohub.com", "http://www.tecnoprohub.com", "https://tecnoprohub.com", "https://www.tecnoprohub.com")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
});

#region Database config
// Configuration for dapper and postgres.
builder.Services.AddTransient<IDbConnection>(config =>
  new NpgsqlConnection(connectionString));

// Configuration for Entity framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseNpgsql(connectionString));
#endregion

#region Authentication
// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/leadHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddSignInManager()
  .AddDefaultTokenProviders();
#endregion

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(45);
    options.HandshakeTimeout = TimeSpan.FromSeconds(60);
});

// Swagger and open api
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RabbitMQ Broker
//builder.Services.AddHostedService<MessageConsumer>();

// Services
builder.Services.AddScoped<JwtService>();
builder.Services.AddSingleton<IActiveChatManager, ActiveChatManager>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddSingleton<IDistributionService, DistributionService>();
builder.Services.AddSingleton<ISendMessageService, SendMessageService>();

// Bac
builder.Services.AddScoped<ICompanyBac, CompanyBac>();
builder.Services.AddScoped<IConsultantBac, ConsultantBac>();
builder.Services.AddSingleton<ILeadBrokerBac, LeadBrokerBac>();
builder.Services.AddScoped<ILeadManagerBac, LeadManagerBac>();
builder.Services.AddScoped<IIntegrationBac, IntegrationBac>();
builder.Services.AddSingleton<ITimelineBac, TimelineBac>();
builder.Services.AddSingleton<IWhatsAppBac, WhatsAppBac>();

// Repository
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IConsultantRepository, ConsultantRepository>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IIntegrationRepository, IntegrationRepository>();
builder.Services.AddSingleton<ILeadRepository, LeadRepository>();
builder.Services.AddSingleton<ILeadBrokerRepository, LeadBrokerRepository>();
builder.Services.AddScoped<ILeadManagerRepository, LeadManagerRepository>();
builder.Services.AddSingleton<ITimelineRepository, TimelineRepository>();
builder.Services.AddSingleton<IWhatsAppRepository, WhatsAppRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<LeadHub>("/leadhub");

await ApplyMigrationAsync();
ApplyUserAndRolesAsync();
ApplyScripts();

app.Run();

void ApplyScripts() {
    var updgrader = DeployChanges.To.PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(typeof(ApplicationDbContext).Assembly)
        .LogToConsole()
        .LogScriptOutput()
        .Build();

    var result = updgrader.PerformUpgrade();

    if (result.Successful)
    {
        //Here a should make the seed.
    }

    if (!result.Successful)
    {
        Environment.ExitCode = 3;
        Console.WriteLine(result.Error.Message);
    }
}

// Apply migration pending migrations
async Task ApplyMigrationAsync()
{
    using IServiceScope scope = app.Services.CreateScope();
    var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();    

    if (_db.Database.GetPendingMigrations().Any())
    {
        await _db.Database.MigrateAsync();
    }
}

async void ApplyUserAndRolesAsync()
{
    // Scope for creating default roles
    using (IServiceScope scope = app.Services.CreateScope())
    {
        RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = RolesEnum.List.Select(r => r.Name).ToArray();

        foreach (string role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Scope for creating default roles
    using (IServiceScope scope = app.Services.CreateScope())
    {
        UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string user = "sysadmin";
        string email = "sysadmin@admin.com";
        string password = "SysAdmin#1234";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            ApplicationUser appUser = new()
            {
                UserName = user,
                Email = email,
                EmailConfirmed = true,
            };

            IdentityResult result = await userManager.CreateAsync(appUser, password);

            if (result.Succeeded)
                result = await userManager.AddToRoleAsync(appUser, RolesEnum.SysAdmin.Name);
        }
    }
}
