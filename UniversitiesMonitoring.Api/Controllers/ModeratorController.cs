using Microsoft.AspNetCore.Authorization;
using UniversitiesMonitoring.Api.Entities;
using UniversitiesMonitoring.Api.Services;

namespace UniversitiesMonitoring.Api.Controllers;

[ApiController]
[Route("api/moderator")]
public class ModeratorController : ControllerBase
{
    private readonly IModeratorsProvider _moderatorsProvider;
    private readonly IServicesProvider _servicesProvider;
    private readonly JwtGenerator _jwtGenerator;

    public ModeratorController(IModeratorsProvider moderatorsProvider, IServicesProvider servicesProvider, JwtGenerator jwtGenerator)
    {
        _moderatorsProvider = moderatorsProvider;
        _servicesProvider = servicesProvider;
        _jwtGenerator = jwtGenerator;
    }
    
    [HttpPost("auth")]
    public async Task<IActionResult> ModeratorAuth([FromBody] ModeratorAuthEntity auth)
    {
        var moderator = await _moderatorsProvider.GetModeratorAsync(auth.Id);

        if (moderator == null)
        {
            return BadRequest("Некорректное имя пользователя или пароль");
        }
        
        var passwordHash = Sha256Computing.ComputeSha256(auth.Password);

        if (moderator.PasswordSha256hash != passwordHash)
        {
            return BadRequest("Некорректное имя пользователя или пароль");
        }

        var token = _jwtGenerator.GenerateTokenForUser(moderator.Id, false);

        Response.Cookies.Append("auth", token);
        
        return Ok(new
        {
            jwt = token
        });
    }

    [Authorize(JwtGenerator.AdminRole)]
    [HttpGet("reports")]
    public IActionResult GetReports() => Ok(from report in _servicesProvider.GetAllReports() 
        select new Report(report.Content, report.IsOnline, report.ServiceId));

    [Authorize(JwtGenerator.AdminRole)]
    [HttpPost("reports/{id:long}/accept")]
    public async Task<IActionResult> AcceptReport([FromRoute] ulong id)
    {
        var report = await _servicesProvider.GetReportAsync(id);

        if (report == null)
        {
            return BadRequest("Репорт не найден");
        }

        var service = await _servicesProvider.GetServiceAsync(report.ServiceId);

        await _servicesProvider.UpdateServiceStateAsync(service!, report.IsOnline, true);
        await _servicesProvider.DeleteReportAsync(report);
        return Ok();
    }

    [Authorize(JwtGenerator.AdminRole)]
    [HttpPost("reports/{id:long}/deny")]
    public async Task<IActionResult> DenyReport([FromRoute] ulong id)
    {
        var report = await _servicesProvider.GetReportAsync(id);

        if (report == null)
        {
            return BadRequest("Репорт не найден");
        }
        
        await _servicesProvider.DeleteReportAsync(report);
        return Ok();
    }
}