using CommunityHub.Core.Constants;
using CommunityHub.Core.Factory;
using CommunityHub.UI;
using CommunityHub.UI.Middleware;
using CommunityHub.UI.Services;
using CommunityHub.UI.Services.Account;
using CommunityHub.UI.Services.Admin;
using CommunityHub.UI.Services.CommunityHub.UI.Services;
using CommunityHub.UI.Services.Registration;
using CommunityHub.UI.Services.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

// Add services to the container
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var appSettings = builder.Configuration.GetSection("AppSettings");
var clientUrl = appSettings.GetValue<string>("ClientUrl");
var appUrl = appSettings.GetValue<string>("AppUrl");

builder.Services.Configure<AppSettings>(appSettings);
builder.Services.AddHttpClient<BaseService>((sp, client) =>
{
    var appSettings = sp.GetRequiredService<IOptions<AppSettings>>();
    client.BaseAddress = new Uri(appSettings.Value.ClientUrl);
});

builder.Services.AddScoped<BaseService>((sp) =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var appSettings = sp.GetRequiredService<IOptions<AppSettings>>();
    var requestSender = sp.GetRequiredService<IHttpRequestSender>();

    return new BaseService(httpClient, requestSender, appSettings);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IResponseFactory, ResponseFactory>();
builder.Services.AddScoped<IHttpRequestSender, HttpRequestSender>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICookieStorage, CookieStorage>();
builder.Services.AddScoped<ICookieReaderService, CookieReaderService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(1);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Error/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthenticatedUser", policy =>
        policy.RequireAuthenticatedUser());

    options.AddPolicy("AdminUser", policy =>
        policy.RequireRole("admin", "superadmin"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(UiRoute.Error.GeneralError);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseMiddleware<RedirectToLoginMiddleware>();

app.UseAuthorization();
app.UseMiddleware<NoCacheMiddleware>();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
).WithStaticAssets();

app.UseStatusCodePagesWithReExecute(UiRoute.Error.PageNotFound);

app.Run();

