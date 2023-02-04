using UniversitiesMonitoring.Api.Services;
using UniversitiesMonitoring.Api.WebSocket;
using UniversityMonitoring.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataContext(builder.Configuration);

builder.Services.AddScoped<IUsersProvider, UsersProvider>()
    .AddScoped<IServicesProvider, ServicesProvider>()
    .AddScoped<IModeratorsProvider, ModeratorsProvider>()
    .AddSingleton<IWebSocketUpdateStateNotifier, WebSocketUpdateStateNotifier>();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
