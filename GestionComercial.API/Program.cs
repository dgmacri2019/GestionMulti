using GestionComercial.Api.Notifications;
using GestionComercial.API.Helpers;
using GestionComercial.API.Hubs;
using GestionComercial.API.Notifications;
using GestionComercial.API.Notifications.Background;
using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Applications.Services;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar la configuración de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);   // detección de clientes colgados
    options.KeepAliveInterval = TimeSpan.FromSeconds(10);   // ping periódico
    options.MaximumParallelInvocationsPerClient = 10;            // defensivo
});

// Cola + dispatcher
builder.Services.AddSingleton<INotificationQueue, NotificationQueue>();
builder.Services.AddHostedService<NotificationDispatcher>();

// Inyección de dependencias
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IPriceListService, PriceListService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IMasterService, MasterService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClientsNotifier, SignalRClientsNotifier>();
builder.Services.AddScoped<IProvidersNotifier, SignalRProvidersNotifier>();
builder.Services.AddScoped<IArticlesNotifier, SignalRArticlesNotifier>();
builder.Services.AddScoped<IBoxAndBanksNotifier, SignalRBoxAndBanksNotifier>();
builder.Services.AddScoped<IBankParametersNotifier, SignalRBankParametersNotifier>();

//builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
// Configurar JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Agregar controladores
builder.Services.AddControllers();
builder.WebHost.UseIISIntegration();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Primero enrutamiento
app.UseRouting();

// Luego autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Finalmente, los endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ClientsHub>("/hubs/clients");
    endpoints.MapHub<ProvidersHub>("/hubs/providers");
    endpoints.MapHub<ArticlesHub>("/hubs/articles");
    endpoints.MapHub<BoxAndBanksHub>("/hubs/boxandbank");
    endpoints.MapHub<BankParametersHub>("/hubs/bankparameter");
});
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        GeneralResponse result = await SeedData.InitializeAsync(services);
        if (result.Success)
            logger.LogInformation("Finalizo SeedData");
        else
            logger.LogError(result.Message);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al ejecutar el SeedData");
    }
}

app.Run();
