using System.Collections;
using Moq;
using UniversitiesMonitoring.Api.Services;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Tests;

public class UsersProviderTests
{
    [Fact]
    public async Task Get_User()
    {
        var userRef = new User();
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.Users.FindAsync(It.IsAny<ulong>())).ReturnsAsync(userRef);
        var usersProvider = new UsersProvider(dataProviderMock.Object);

        Assert.Equal(userRef, await usersProvider.GetUserAsync(0x0));
    }

    [Theory]
    [ClassData(typeof(ModifyUserTestData))]
    public async Task Modify_User_Must_Be_Success(Action<User> modifyAction, Func<User, bool> checkAction)
    {
        var userRef = new User()
        {
            Id = 0,
            Username = "DenVot",
            PasswordSha256hash = new byte[] {0x0, 0x1, 0xf},
            Email = null,
            SendEmailNotification = false,
        };

        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.Users.FindAsync(It.IsAny<ulong>())).ReturnsAsync(userRef);
        dataProviderMock.Setup(x => x.SaveChangesAsync());

        var usersProvider = new UsersProvider(dataProviderMock.Object);

        var result = await usersProvider.ModifyUserAsync(0x0, modifyAction);

        Assert.True(result);
        Assert.True(checkAction(userRef));
    }
}

public class ModifyUserTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new Action<User>(user => user.Username = "NewName"),
            new Func<User, bool>(user => user.Username == "NewName")
        };

        yield return new object[]
        {
            new Action<User>(user => user.SendEmailNotification = true),
            new Func<User, bool>(user => user.SendEmailNotification)
        };

        yield return new object[]
        {
            new Action<User>(user => user.Email = "awesome@email.com"),
            new Func<User, bool>(user => user.Email == "awesome@email.com")
        };

        yield return new object[]
        {
            new Action<User>(user => user.Email = "awesome@email.com"),
            new Func<User, bool>(user => user.Email == "awesome@email.com")
        };

        var newSha256 = Array.Empty<byte>();

        yield return new object[]
        {
            new Action<User>(user => user.PasswordSha256hash = newSha256),
            new Func<User, bool>(user => user.PasswordSha256hash == newSha256)
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}