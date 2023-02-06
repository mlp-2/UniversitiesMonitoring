using UniversitiesMonitoring.Api.Entities;

namespace UniversitiesMonitoring.Api.Controllers;

[Route("/moderator/")]
internal class ModeratorController : ControllerBase
{
    [HttpPost("auth")]
    public Task<IActionResult> ModeratorAuth([FromBody] ModeratorAuthEntity auth) => throw new NotImplementedException();
    
    [HttpGet("reports")]
    public Task<IActionResult> GetReports() => throw new NotImplementedException();
    
    [HttpPost("reports/{id:long}/accept")]
    public Task<IActionResult> AcceptReport([FromRoute] ulong id) => throw new NotImplementedException();
    
    [HttpPost("reports/{id:long}/deny")]
    public Task<IActionResult> DenyReport([FromRoute] ulong id) => throw new NotImplementedException();
}