using Moq;
using UniversitiesMonitoring.Api.Services;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Tests;

public class ModeratorsProviderTests
{
    [Fact]
    public async Task Get_Moderator()
    {
        var modRef = new Moderator();
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.Moderators.FindAsync(It.IsAny<ulong>())).ReturnsAsync(modRef);
        var modProvider = new ModeratorsProvider(dataProviderMock.Object);
        
        Assert.Equal(modRef, await modProvider.GetModeratorAsync(0x0));
    }
}