using UniversitiesMonitoring.Module.Networking;
using UniversitiesMonitoring.Module.Networking.Testing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddSingleton<IEnumerable<ITestStrategy>>(
        new[] { (ITestStrategy) new HeadStrategy(), new PingStrategy() })
    .AddSingleton<TestProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();