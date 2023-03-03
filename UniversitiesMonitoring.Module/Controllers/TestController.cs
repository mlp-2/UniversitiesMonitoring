using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using UniversitiesMonitoring.Module.Networking;

[assembly: InternalsVisibleTo("Swashbuckle.AspNetCore")]

namespace UniversitiesMonitoring.Module.Controllers;

[ApiController]
public class TestController : ControllerBase
{
    private readonly TestProvider _testProvider;

    public TestController(TestProvider testProvider)
    {
        _testProvider = testProvider;
    }

    [HttpGet("/test")]
    public async Task<IActionResult> Test(
        [FromQuery] Uri url) => Ok(await _testProvider.TestAsync(url));
}