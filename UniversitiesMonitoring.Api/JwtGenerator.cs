using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

[assembly: InternalsVisibleTo("UniversitiesMonitoring.Api.Tests")]
namespace UniversitiesMonitoring.Api;

internal class JwtGenerator
{
    private readonly SymmetricSecurityKey _jwtSecret;

    /// <summary>
    /// Роль обычного пользователя
    /// </summary>
    public const string UserRole = "USER_ROLE";
    
    /// <summary>
    /// Роль администратора
    /// </summary>
    public const string AdminRole = "ADMIN_ROLE";
    
    public JwtGenerator(IConfiguration configuration) =>
        _jwtSecret = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(configuration["JwtSecret"]));

    /// <summary>
    /// Генерирует JWT токен, исходя из переданных данных
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="isRegularUser">True, если пользователь - не администратор</param>
    /// <param name="lifeTime">Время жизни токена в секундах</param>
    /// <returns>JWT токен</returns>
    public string GenerateTokenForUser(ulong userId, bool isRegularUser, int lifeTime, DateTime? initializingTime = null)
    {
        var now = initializingTime ?? DateTime.UtcNow;
        var claims = GenerateIdentity(userId, isRegularUser);
        
        var jwt = new JwtSecurityToken(
            issuer: "API_HOST",
            notBefore: now,
            claims: claims.Claims,
            expires: now.Add(TimeSpan.FromMinutes(lifeTime)),
            signingCredentials: new SigningCredentials(_jwtSecret, SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt!;
    }

    private ClaimsIdentity GenerateIdentity(ulong userId, bool isRegularUser)
    {
        var claims = new Claim[]
        {
            new("name", userId.ToString()),
            new("role", isRegularUser ? UserRole: AdminRole)
        };
        
        var claimsIdentity =
            new ClaimsIdentity(claims, "Token", "name",
                "role");

        return claimsIdentity;
    }
}