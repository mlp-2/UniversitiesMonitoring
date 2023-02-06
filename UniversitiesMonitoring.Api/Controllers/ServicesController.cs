using UniversitiesMonitoring.Api.Entities;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.Api.Controllers;

[Route("/services/")]
internal class ServicesController : ControllerBase
{
    [HttpPost("{id:long}/subscribe")]
    public Task<IActionResult> SubscribeService([FromRoute] ulong id) => throw new NotImplementedException();

    [HttpDelete("{id:long}/unsubscribe")]
    public Task<IActionResult> Unsubscribe([FromRoute] ulong id) => throw new NotImplementedException();

    [HttpPost("{id:long}/comment")]
    public Task<IActionResult> Comment(
        [FromRoute] ulong id,
        [FromBody] Comment comment) => throw new NotImplementedException();
    
    [HttpPost("{id:long}/report")]
    public Task<IActionResult> Report(
        [FromRoute] ulong id,
        [FromBody] Report report) => throw new NotImplementedException();

    [HttpGet]
    public Task<IActionResult> GetAllServices(
        [FromQuery] bool loadUsers,
        [FromQuery] bool loadComments) => throw new NotImplementedException();

    [HttpPut("update")]
    public Task<IActionResult> UpdateService([FromBody] ChangeStateEntity[] update) => throw new NotImplementedException();
}