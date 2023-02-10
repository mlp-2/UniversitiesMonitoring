using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UniversitiesMonitoring.Api;
using UniversitiesMonitoring.Api.Services;
using UniversitiesMonitoring.Api.WebSocket;
using UniversityMonitoring.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataContext(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUsersProvider, UsersProvider>()
    .AddScoped<IServicesProvider, ServicesProvider>()
    .AddScoped<IModeratorsProvider, ModeratorsProvider>()
    .AddSingleton<IWebSocketUpdateStateNotifier, WebSocketUpdateStateNotifier>()
    .AddSingleton<JwtGenerator>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = "API_HOST",
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSecret"])),
        ValidateIssuerSigningKey = true
    };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseMiddleware<HandlingExceptionsMiddleware>();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "/swagger";
    });
}

app.UseHttpsRedirection();
app.UseWebSockets();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    "default",
    "/{controller}/{action}");

app.MapFallbackToFile("index.html");

app.Run();
