using AppComponents.Email;
using AppComponents.Repository.EFCore;
using AppComponents.TemplateEngine;
using CommunityHub.Api.Data;
using CommunityHub.Api.Middleware;
using CommunityHub.Core.Factory;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.EmailService;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Models.Registration;
using CommunityHub.Infrastructure.Services;
using CommunityHub.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//App settings
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);

//Database
var appSettings = appSettingsSection.Get<AppSettings>();
bool useInMemoryDb = appSettings.TransactionSettings.UseInMemoryDatabase;

if (useInMemoryDb)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("TestDB"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}


//Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password policy
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 6;

    // Sign-in options
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(24);
});

//Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),

            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Name
        };
    });


builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Email
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
builder.Services.AddEmailService(emailSettings);

builder.Services.AddScoped<IModelTemplateEngine>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<IModelTemplateEngine>>();
    return new ModelTemplateEngine(logger, "{{", "}}");
});

builder.Services.AddScoped<IAppMailService, AppMailService>();


// Repositories and other services
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransactionManager<ApplicationDbContext>();
builder.Services.AddRepository<UserInfo, ApplicationDbContext>();
builder.Services.AddRepository<SpouseInfo, ApplicationDbContext>();
builder.Services.AddRepository<Child, ApplicationDbContext>();
builder.Services.AddRepository<ContactForm, ApplicationDbContext>();
builder.Services.AddRepository<RegistrationRequest, ApplicationDbContext>();

builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserAccountManagementService, UserAccountManagementService>();
builder.Services.AddScoped<ISpouseService, SpouseService>();
builder.Services.AddScoped<IChildService, ChildService>();
builder.Services.AddScoped<IUserInfoValidationService, UserInfoValidatorService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddScoped<IResponseFactory, ResponseFactory>();
builder.Services.AddScoped<ICookieWriterService, CookieWriterService>();

// Controllers, API setup, and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\nExample: Bearer eyJhbGciOiJIUzI..."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAntiforgery();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthenticatedUser", policy =>
        policy.RequireAuthenticatedUser());

    options.AddPolicy("AdminUser", policy =>
        policy.RequireRole("admin", "superadmin"));
});

// Configure the app to explicitly use HTTP/HTTPS ports
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

var app = builder.Build();



// Seed roles
var serviceProvider = app.Services.CreateScope().ServiceProvider;
var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
await DataSeeder.SeedRolesAsync(serviceProvider, userManager);




if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});

app.UseAntiforgery();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<JwtAuthenticationMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();
