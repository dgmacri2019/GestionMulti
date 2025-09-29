using Afip.PublicServices.Interfaces;
using Afip.PublicServices.Services;
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
using GestionComercial.Domain.Traslates;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Reports.PublicServices.Interfaces;
using Reports.PublicServices.Services;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; // también sobre HTTPS
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest; // o Optimal
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest; // o Optimal
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar la configuración de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de Identity
builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        // Configuración de contraseñas
        options.Password.RequireDigit = false;             // Números obligatorios
        options.Password.RequireLowercase = false;         // Minúsculas obligatorias
        options.Password.RequireUppercase = false;         // Mayúsculas obligatorias
        options.Password.RequireNonAlphanumeric = false;   // Símbolos obligatorios
        options.Password.RequiredLength = 3;               // Mínimo de caracteres
        options.Password.RequiredUniqueChars = 1;          // Caracteres únicos

        // Configuración de bloqueo (opcional)
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        // Configuración de usuarios
        options.User.RequireUniqueEmail = false;
    })
    .AddErrorDescriber<SpanishIdentityErrorDescriber>() // 👈 clave
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
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IMasterService, MasterService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IParameterService, ParameterService>();
builder.Services.AddScoped<IMasterClassService, MasterClassService>();
builder.Services.AddScoped<IWSFEHomologacionService, WSFEHomologacionService>();
builder.Services.AddScoped<ILoginCMSHomologacionService, LoginCMSHomologacionService>();
builder.Services.AddScoped<IInvoiceReport, InvoiceReport>();



builder.Services.AddScoped<IClientsNotifier, SignalRClientsNotifier>();
builder.Services.AddScoped<IProvidersNotifier, SignalRProvidersNotifier>();
builder.Services.AddScoped<IArticlesNotifier, SignalRArticlesNotifier>();
builder.Services.AddScoped<IBoxAndBanksNotifier, SignalRBoxAndBanksNotifier>();
builder.Services.AddScoped<IBankParametersNotifier, SignalRBankParametersNotifier>();
builder.Services.AddScoped<ISalesNotifier, SignalRSalesNotifier>();
builder.Services.AddScoped<IParametersNotifier, SignalRParametersNotifier>();
builder.Services.AddScoped<IMasterClassNotifier, SignalRMasterClassNotifier>();
builder.Services.AddScoped<IUsersNotifier, SignalRUsersNotifier>();

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

app.UseResponseCompression();

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
    endpoints.MapHub<SalesHub>("/hubs/sales");
    endpoints.MapHub<GeneralParametersHub>("/hubs/generalparameter");
    endpoints.MapHub<MasterClassHub>("/hubs/masterclass");
    endpoints.MapHub<UsersHub>("/hubs/users");
});
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // 👇 Esto crea la base de datos si no existe y aplica todas las migraciones
        context.Database.Migrate();

        // 👇 Luego corrés tu seed
        GeneralResponse result = await SeedData.InitializeAsync(services);

        if (result.Success)
            logger.LogInformation("Finalizó SeedData");
        else
            logger.LogError(result.Message);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al ejecutar migraciones o SeedData");
    }
}

app.Run();
