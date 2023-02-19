using Moq;
using UniversitiesMonitoring.Api.Entities;
using UniversitiesMonitoring.Api.Services;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Tests;

public class ServicesProviderTests
{
    [Fact]
    public async Task Service_Getting_Must_Not_Be_Null()
    {
        var dataProvider = CreateProviderMock();
        var testService = new UniversityService();

        dataProvider.Setup(x => x.UniversityServices.FindAsync(It.IsAny<ulong>()))
            .ReturnsAsync(testService);
        
        var servicesProvider = CreateServicesProvider(dataProvider);
        var result = await servicesProvider.GetServiceAsync(13123124512342);
        
        Assert.Equal(testService, result);
    }
    
    [Fact]
    public async Task Service_Getting_Must_Be_Null()
    {
        var dataProvider = CreateProviderMock();

        dataProvider.Setup(x => x.UniversityServices.FindAsync(It.IsAny<ulong>()))
            .ReturnsAsync((UniversityService?)null);
        
        var servicesProvider = CreateServicesProvider(dataProvider);
        var result = await servicesProvider.GetServiceAsync(13123124512342);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task User_Subscribe_To_Service()
    {
        var dataProvider = CreateProviderMock();
        
        dataProvider.Setup(x => x.Subscribes.AddAsync(It.IsAny<UserSubscribeToService>()));
        dataProvider.Setup(x => x.SaveChangesAsync());
        
        var servicesProvider = CreateServicesProvider(dataProvider);

        await servicesProvider.SubscribeUserAsync(new User(), new UniversityService());
        
        dataProvider.Verify(x => x.Subscribes.AddAsync(It.IsAny<UserSubscribeToService>()), Times.Once);
        dataProvider.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Unsubscribe_User_From_Service_Must_Success()
    {
        var dataProvider = CreateProviderMock();
        var testUser = new User();
        var testService = new UniversityService();
        var testSubscribeReference = new UserSubscribeToService()
        {
            User = testUser,
            Service = testService
        };

        dataProvider.Setup(x => x.Subscribes.ExecuteSql(It.IsAny<string>())).Returns(new[] {testSubscribeReference});

        var servicesProvider = CreateServicesProvider(dataProvider);

        await servicesProvider.UnsubscribeUserAsync(testUser, testService);
        
        dataProvider.Verify(x => x.Subscribes.ExecuteSql(It.IsAny<string>()), Times.Once);
        dataProvider.Verify(x => x.Subscribes.Remove(testSubscribeReference), Times.Once);
        dataProvider.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Unsubscribe_User_From_Service_Must_Throw_Exception()
    {
        var dataProvider = CreateProviderMock();

        dataProvider.Setup(x => x.Subscribes.ExecuteSql(It.IsAny<string>()))
            .Returns(Array.Empty<UserSubscribeToService>());

        var servicesProvider = CreateServicesProvider(dataProvider);
        await Assert.ThrowsAsync<InvalidOperationException>(() => servicesProvider.UnsubscribeUserAsync(new User(), new UniversityService()));
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task Update_Service_State(bool isOnline, bool force)
    {
        var dataProvider = CreateProviderMock();
        var servicesProvider = CreateServicesProvider(dataProvider);
        
        dataProvider.Setup(x => x.UniversityServiceStateChange.AddAsync(It.IsAny<UniversityServiceStateChange>()));
        dataProvider.Setup(x => x.SaveChangesAsync());
        
        await servicesProvider.UpdateServiceStateAsync(new UniversityService(), isOnline, force);
        
        dataProvider.Verify(x => x.UniversityServiceStateChange.AddAsync(It.IsAny<UniversityServiceStateChange>()), Times.Once);

        if (force)
        {
            dataProvider.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }

    [Fact]
    public async Task Leave_Comment()
    {
        var dataProvider = CreateProviderMock();
        
        dataProvider.Setup(x => x.Rates.AddAsync(It.IsAny<UserRateOfService>()));
        dataProvider.Setup(x => x.SaveChangesAsync());
        
        var servicesProvider = CreateServicesProvider(dataProvider);

        await servicesProvider.LeaveCommentAsync(new UniversityService(), new User(), new Comment(5, "Awesome!"));
        
        dataProvider.Verify(x => x.Rates.AddAsync(It.IsAny<UserRateOfService>()), Times.Once);
        dataProvider.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Create_Report()
    {
        var dataProvider = CreateProviderMock();

        dataProvider.Setup(x => x.Reports.AddAsync(It.IsAny<UniversityServiceReport>()));
        dataProvider.Setup(x => x.SaveChangesAsync());
        
        var servicesProvider = CreateServicesProvider(dataProvider);

        await servicesProvider.CreateReportAsync(new UniversityService(), new User(), new Report("Smthg went wrong", false, 0x0));
        
        dataProvider.Verify(x => x.Reports.AddAsync(It.IsAny<UniversityServiceReport>()), Times.Once);
        dataProvider.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Get_Report()
    {
        var dataProvider = CreateProviderMock();

        dataProvider.Setup(x => x.Reports.FindAsync(It.IsAny<ulong>())).ReturnsAsync(new UniversityServiceReport());
        
        var servicesProvider = CreateServicesProvider(dataProvider);

        Assert.NotNull(await servicesProvider.GetReportAsync(0x0)); 
    }

    [Fact]
    public async Task Delete_Report()
    {
        var dataProvider = CreateProviderMock();

        dataProvider.Setup(x => x.Reports.Remove(It.IsAny<UniversityServiceReport>()));
        dataProvider.Setup(x => x.SaveChangesAsync());
        
        var servicesProvider = CreateServicesProvider(dataProvider);

        await servicesProvider.DeleteReportAsync(It.IsAny<UniversityServiceReport>());
        
        dataProvider.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
    
    private Mock<IDataProvider> CreateProviderMock() => new();

    private ServicesProvider CreateServicesProvider(Mock<IDataProvider> dataProviderMock) => new(dataProviderMock.Object);
}