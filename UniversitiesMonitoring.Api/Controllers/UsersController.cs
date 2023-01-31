using UniversitiesMonitoring.Api.Entities;

namespace UniversitiesMonitoring.Api.Controllers;

[ApiController]
[Route("/user/")]
internal class UsersController : ControllerBase
{
    [HttpPost("auth")]
    public Task<IActionResult> UserAuth([FromBody] AuthEntity auth) => throw new NotImplementedException();
    
    [HttpPost("register")]
    public Task<IActionResult> UserRegister([FromBody] AuthEntity auth) => throw new NotImplementedException();
    
    [HttpPut("telegram/update")]
    public Task<IActionResult> TelegramUpdate([FromBody] TelegramUpdateEntity update) => throw new NotImplementedException();
    
    [HttpPut("email/update")]
    public Task<IActionResult> EmailUpdate([FromBody] EmailUpdateEntity update) => throw new NotImplementedException();
}