using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using UniversitiesMonitoring.Module.Networking;

[assembly: InternalsVisibleTo("Swashbuckle.AspNetCore")]

namespace UniversitiesMonitoring.Module.Controllers;

[ApiController]
public class TestController : ControllerBase
{
    private readonly TestProvider _testProvider;
    private readonly LocationProvider _locationProvider;

    public TestController(TestProvider testProvider, 
        LocationProvider locationProvider)
    {
        _testProvider = testProvider;
        _locationProvider = locationProvider;
    }

    [HttpGet("/test")]
    public async Task<IActionResult> Test(
        [FromQuery] Uri url) => Ok(await _testProvider.TestAsync(url));
}