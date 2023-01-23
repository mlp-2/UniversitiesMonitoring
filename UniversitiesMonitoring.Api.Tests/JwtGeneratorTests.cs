using Microsoft.Extensions.Configuration;
using Moq;

namespace UniversitiesMonitoring.Api.Tests;

public class JwtGeneratorTests
{
    [Theory]
    [InlineData(
        2048L, 
        false, 
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMjA0OCIsInJvbGUiOiJBRE1JTl9ST0xFIiwibmJmIjoxMTIzMjUwNDAwLCJleHAiOjExMjM3NDE5MjAsImlzcyI6IkFQSV9IT1NUIn0.igq0ITDEhZZrniKWjcqrEYqvaB3YXTy0VRCQj9utUco")]
    [InlineData(
        2048L, 
        true, 
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMjA0OCIsInJvbGUiOiJVU0VSX1JPTEUiLCJuYmYiOjExMjMyNTA0MDAsImV4cCI6MTEyMzc0MTkyMCwiaXNzIjoiQVBJX0hPU1QifQ.IIhHBjcCXpIXq2HR1t3_HEaO5rhApx64kiCIR4WgVlw")]
    public void Test(ulong userId, bool isRegularUser, string expectedJwt)
    {
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x[It.IsAny<string>()]).Returns("very-big-256-secret");

        var jwtGenerator = new JwtGenerator(configurationMock.Object);

        var token = jwtGenerator.GenerateTokenForUser(userId, isRegularUser, 8192, new DateTime(2005, 8, 5, 18, 0, 0, DateTimeKind.Utc));
        
        Assert.Equal(expectedJwt, token);
    }
}