using Microsoft.Extensions.Configuration;
using Moq;
using Xunit.Abstractions;

namespace UniversitiesMonitoring.Api.Tests;

public class JwtGeneratorTests
{
    private readonly ITestOutputHelper _logger;

    public JwtGeneratorTests(ITestOutputHelper logger)
    {
        _logger = logger;
    }
    
    [Theory]
    [InlineData(
        2048L, 
        false, 
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMjA0OCIsInJvbGUiOiJBRE1JTl9ST0xFIiwibmJmIjoxMTIzMjY0ODAwLCJleHAiOjExMjM3NTYzMjAsImlzcyI6IkFQSV9IT1NUIn0.8CHAG0nIDlbRxDwle9R1WRb3YCfGIk_7TtXpUjWZ9wc")]
    [InlineData(
        2048L, 
        true, 
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMjA0OCIsInJvbGUiOiJVU0VSX1JPTEUiLCJuYmYiOjExMjMyNjQ4MDAsImV4cCI6MTEyMzc1NjMyMCwiaXNzIjoiQVBJX0hPU1QifQ.goDDhGZ2KYIM16XmEEXaQeEhpGxMhkon1i6gb-qwSZ0")]
    public void Test(ulong userId, bool isRegularUser, string expectedJwt)
    {
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x[It.IsAny<string>()]).Returns("very-big-256-secret");

        var jwtGenerator = new JwtGenerator(configurationMock.Object);

        var token = jwtGenerator.GenerateTokenForUser(userId, isRegularUser, 8192, new DateTime(2005, 8, 5, 18, 0, 0, DateTimeKind.Utc));
        
        _logger.WriteLine($"JWT: {token}");
        
        Assert.Equal(expectedJwt, token);
    }
}