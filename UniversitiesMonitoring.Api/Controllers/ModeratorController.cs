using Microsoft.AspNetCore.Authorization;
using UniversitiesMonitoring.Api.Entities;
using UniversitiesMonitoring.Api.Services;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Controllers;

[ApiController]
[Route("api/moderator")]
public class ModeratorController : ControllerBase
{
    private readonly IModeratorsProvider _moderatorsProvider;
    private readonly IServicesProvider _servicesProvider;
    private readonly IDataProvider _dataProvider;
    private readonly JwtGenerator _jwtGenerator;
    private readonly IModulesProvider _modulesProvider;

    public ModeratorController(IModeratorsProvider moderatorsProvider,
        IServicesProvider servicesProvider,
        IDataProvider dataProvider,
        JwtGenerator jwtGenerator, IModulesProvider modulesProvider)
    {
        _moderatorsProvider = moderatorsProvider;
        _servicesProvider = servicesProvider;
        _dataProvider = dataProvider;
        _jwtGenerator = jwtGenerator;
        _modulesProvider = modulesProvider;
    }

    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpGet("test")]
    public IActionResult TestToken() => Ok();
    
    [HttpPost("auth")]
    public async Task<IActionResult> ModeratorAuth([FromBody] ModeratorAuthEntity auth)
    {
        var moderator = await _moderatorsProvider.GetModeratorAsync(auth.Id);

        if (moderator == null)
        {
            return BadRequest("Некорректное имя пользователя или пароль");
        }
        
        var passwordHash = Sha256Computing.ComputeSha256(auth.Password);

        if (!moderator.PasswordSha256hash.IsSequenceEquals(passwordHash))
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

    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpGet("reports")]
    public IActionResult GetReports() => Ok(from report in _servicesProvider.GetAllReports() 
        select new ReportEntity(report));

    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpPost("reports/{id:long}/accept")]
    public async Task<IActionResult> AcceptReport([FromRoute] ulong id)
    {
        var report = await _servicesProvider.GetReportAsync(id);

        if (report == null)
        {
            return BadRequest("Репорт не найден");
        }

        await _moderatorsProvider.AcceptReportAsync(report);
        await _servicesProvider.SolveReportAsync(report);
        return Ok();
    }

    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpPost("reports/{id:long}/deny")]
    public async Task<IActionResult> DenyReport([FromRoute] ulong id)
    {
        var report = await _servicesProvider.GetReportAsync(id);

        if (report == null)
        {
            return BadRequest("Репорт не найден");
        }
        
        await _servicesProvider.SolveReportAsync(report);
        return Ok();
    }

    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpPost("universities/add")]
    public async Task<IActionResult> AddUniversity(
        [FromQuery] string name)
    {
        try
        {
            var university = new University()
            {
                Name = name
            };
            
            await _dataProvider.Universities.AddAsync(university);
            await _dataProvider.SaveChangesAsync();
            return Ok(university.Id);
        }
        catch
        {
            return BadRequest("ВУЗ с таким названием уже существует");
        }
    }
    
    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpDelete("universities/{id:long}/delete")]
    public async Task<IActionResult> DeleteUniversity(ulong id)
    {
        var university = await _dataProvider.Universities.FindAsync(id);

        if (university == null) return BadRequest();
        
        _dataProvider.Universities.Remove(university);
        await _dataProvider.SaveChangesAsync();

        return Ok();
    }

    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpPut("universities/{id:long}/rename")]
    public async Task<IActionResult> RenameUniversity(
        [FromRoute] ulong id,
        [FromQuery] string newName)
    {
        try
        {
            var university = await _dataProvider.Universities.FindAsync(id);

            if (university == null) return BadRequest();

            university.Name = newName;
            await _dataProvider.SaveChangesAsync();

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpPost("services/add")]
    public async Task<IActionResult> AddService(
        [FromBody] AddServiceEntity serviceEntity)
    {
        try
        {
            var university = await _servicesProvider.GetUniversityAsync(serviceEntity.UniversityId);

            if (university == null)
            {
                return BadRequest("Такого ВУЗа не существует в системе");
            }

            if (!Uri.IsWellFormedUriString(serviceEntity.Url, UriKind.Absolute))
            {
                return BadRequest("Некорректная ссылка");
            }
            
            var service = new UniversityService()
            {
                Name = serviceEntity.Name,
                University = university,
                Url = serviceEntity.Url
            };

            await _dataProvider.UniversityServices.AddAsync(service);
            await _dataProvider.SaveChangesAsync();
            return Ok(service.Id);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpDelete("services/{id:long}/delete")]
    public async Task<IActionResult> DeleteService(ulong id)
    {
        var service = await _servicesProvider.GetServiceAsync(id);
        if (service == null) return BadRequest();

        _dataProvider.UniversityServices.Remove(service);
        await _dataProvider.SaveChangesAsync();

        return Ok();
    }
    
    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpPut("services/{id:long}/rename")]
    public async Task<IActionResult> RenameService(
        [FromRoute] ulong id,
        [FromQuery] string newName)
    {
        try
        {
            var service = await _servicesProvider.GetServiceAsync(id);
            if (service == null) return BadRequest();

            service.Name = newName;
            await _dataProvider.SaveChangesAsync();

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpPut("services/{id:long}/change-url")]
    public async Task<IActionResult> ChangeUrlOfService(
        [FromRoute] ulong id,
        [FromQuery] string newUrl)
    {
        try
        {
            var service = await _servicesProvider.GetServiceAsync(id);
            if (service == null) return BadRequest();

            service.Url = newUrl;
            await _dataProvider.SaveChangesAsync();

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpGet("modules")]
    public IActionResult GetModules() => Ok(_modulesProvider.GetModulesAsync());
    
    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpPost("modules")]
    public async Task<IActionResult> CreateModule(
        [FromQuery] string url)
    {
        var moduleData = await _modulesProvider.CreateModuleAsync(url); 
        
        return Ok(new ModuleEntity(moduleData.Item1, moduleData.Item2));
    }

    [Authorize(Roles = JwtGenerator.AdminRole)]
    [HttpDelete("modules/{id:long}")]
    public async Task<IActionResult> DeleteModule([FromRoute] ulong id)
    {
        await _modulesProvider.DeleteModuleAsync(id);
        return Ok();
    }
}