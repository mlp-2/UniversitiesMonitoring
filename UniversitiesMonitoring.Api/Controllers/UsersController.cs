using Microsoft.AspNetCore.Authorization;
using UniversitiesMonitoring.Api.Entities;
using UniversitiesMonitoring.Api.Services;
using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UsersController : ControllerBase
{
    private readonly IUsersProvider _usersProvider;
    private readonly JwtGenerator _jwtGenerator;

    public UsersController(IUsersProvider usersProvider, JwtGenerator jwtGenerator)
    {
        _usersProvider = usersProvider;
        _jwtGenerator = jwtGenerator;
    }
    
    [HttpPost("auth")]
    public IActionResult UserAuth([FromBody] AuthEntity auth)
    {
        var user = _usersProvider.GetUser(auth.Username);

        if (user == null)
        {
            return BadRequest("Некорректное имя пользователя или пароль");
        }
        
        var passwordHash = Sha256Computing.ComputeSha256(auth.Password);
        
        if (!user.PasswordSha256hash.IsSequenceEquals(passwordHash))
        {
            return BadRequest("Некорректное имя пользователя или пароль");
        }
        
        var token = _jwtGenerator.GenerateTokenForUser(user.Id, true);

        Response.Cookies.Append("auth", token);
        
        return Ok(new
        {
            jwt = token
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> UserRegister([FromBody] AuthEntity auth)
    {
        var user = _usersProvider.GetUser(auth.Username);

        if (user != null)
        {
            return BadRequest("Пользователь с таким именем уже существует");
        }

        var result = await _usersProvider.CreateUserAsync(auth.Username, auth.Password); 
        
        if (result == null)
        {
            return BadRequest("Пользователь с таким именем уже существует");
        }

        return Ok(new {
            jwt = _jwtGenerator.GenerateTokenForUser(result.Id, true)
        });
    }

    [Authorize(Roles = JwtGenerator.UserRole)]
    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        var user = await _usersProvider.GetUserAsync(User.Identity!.Name!);

        if (user == null) return BadRequest();
        
        return Ok(new UserEntity(user));
    }
    
    [Authorize(Roles = JwtGenerator.UserRole)]
    [HttpPut("email/update")]
    public async Task<IActionResult> EmailUpdate([FromBody] EmailUpdateEntity update)
    {
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            return BadRequest();
        }

        var isSuccess = await _usersProvider.ModifyUserAsync(ulong.Parse(User.Identity!.Name!), CreateModifyEmailAction(update));

        if (!isSuccess)
        {
            return BadRequest("Некорректные данные");
        }

        return Ok();
    }

    private Action<User> CreateModifyEmailAction(EmailUpdateEntity update) => user =>
    {
        user.Email = update.Email;
        user.SendEmailNotification = update.CanSend;
    };
}
