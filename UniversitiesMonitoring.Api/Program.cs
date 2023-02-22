using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversitiesMonitoring.Api;
using UniversitiesMonitoring.Api.Services;
using UniversitiesMonitoring.Api.WebSocket;
using UniversityMonitoring.Data;
using UniversityMonitoring.Data.Models;

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET") ??
                                                                            builder.Configuration["JwtSecret"])),
        ValidateIssuerSigningKey = true
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UniversitiesMonitoringContext>();
    
    if (await dbContext.Moderators.CountAsync() == 0)
    {
        var moderator = new Moderator()
        {
            Id = 1,
            PasswordSha256hash = Sha256Computing.ComputeSha256("12345678")
        };

        await dbContext.Moderators.AddAsync(moderator);
        await dbContext.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
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

app.UseMiddleware<HandlingExceptionsMiddleware>();
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
