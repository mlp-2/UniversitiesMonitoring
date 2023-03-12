namespace UniversitiesMonitoring.Module;

public class LocationProvider
{
    private readonly IConfiguration _configuration;

    public LocationProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Location => Environment.GetEnvironmentVariable("LOCATION_NAME") ??
                              _configuration["LocationName"] ??
                              "Неизвестная локация";
}