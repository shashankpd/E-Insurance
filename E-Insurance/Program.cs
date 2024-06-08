using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IRegistrationBusinessLogic, RegistrationBusinessLigic>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

// Policy creation
builder.Services.AddScoped<IPolicyCreationBL, PolicyCreationBL>();
builder.Services.AddScoped<IPolicyCreationService, PolicyCreationService>();

// Policy purchase
builder.Services.AddScoped<ICustomerPolicyPurchaseBL, CustomerPolicyPurchaseBL>();
builder.Services.AddScoped<ICustomerPolicyPurchaseService, CustomerPolicyPurchaseService>();

// Payment
builder.Services.AddScoped<IPaymentProcessBL, PaymentProcessBL>();
builder.Services.AddScoped<IPaymentProcessService, PaymentProcessService>();

/*builder.Services.AddScoped<IAgentCommission, AgentCommissionService>(); // Correct spelling
builder.Services.AddScoped<IAgentComissionBl, AgentComissionServiceBl>(); // Corrected interface name
*/
builder.Services.AddControllers();

// Add NLog Logger
var logpath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
NLog.GlobalDiagnosticsContext.Set("LogDirectory", logpath);
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();
builder.Services.AddSingleton<NLog.ILogger>(NLog.LogManager.GetCurrentClassLogger());

// Adding JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Management", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

    // Require JWT tokens to be passed on requests 
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDistributedMemoryCache();

// JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

