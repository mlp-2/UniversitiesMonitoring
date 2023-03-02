using System.Collections;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using UniversitiesMonitoring.Api.Services;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Tests;

public class UptimeTests
{
    [Theory]
    [ClassData(typeof(UptimeTestData))]
    public void CalculateUptime(UniversityServiceStateChange[] stateChanges, double expectedUptime)
    {
        var dataProviderMock = new Mock<IDataProvider>();
        var memCacheMock = new Mock<IMemoryCache>();
        var cacheEntryMock = new Mock<ICacheEntry>();
        object uptimeDataMock = new UptimeData();
        
        memCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out uptimeDataMock)).Returns(false);
        memCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);
        
        foreach (var change in stateChanges)
        {
            change.ServiceId = 0;
        }
        
        dataProviderMock.Setup(x => x.UniversityServiceStateChange.GetlAll()).Returns(stateChanges.AsQueryable());

        var servicesProvider = new ServicesProvider(dataProviderMock.Object, memCacheMock.Object);
        var realUptime = servicesProvider.GetServiceUptime(0);
        
        memCacheMock.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Once);
        
        Assert.Equal(expectedUptime, realUptime);
    }
}

internal class UptimeTestData : IEnumerable<object[]>
{
    /// <inheritdoc />
    public IEnumerator<object[]> GetEnumerator()
    {
        var utcNow = DateTime.UtcNow;
        
        yield return new object[]
        {
            new []
            {
                new UniversityServiceStateChange()
                {
                    IsOnline = true,
                    ChangedAt = utcNow.AddDays(-2)
                },
                new UniversityServiceStateChange()
                {
                    IsOnline = false,
                    ChangedAt = utcNow.AddDays(-1)
                },
                new UniversityServiceStateChange()
                {
                    IsOnline = true,
                    ChangedAt = utcNow
                },
            },
            0.5d
        };
        
        yield return new object[]
        {
            new []
            {
                new UniversityServiceStateChange()
                {
                    IsOnline = true,
                    ChangedAt = utcNow.AddDays(-2)
                },
                new UniversityServiceStateChange()
                {
                    IsOnline = true,
                    ChangedAt = utcNow
                },
                new UniversityServiceStateChange()
                {
                    IsOnline = false,
                    ChangedAt = utcNow.AddDays(-1)
                },
            },
            0.5d
        };
        
        yield return new object[]
        {
            new []
            {
                new UniversityServiceStateChange()
                {
                    IsOnline = true,
                    ChangedAt = utcNow.AddDays(-5)
                },
                new UniversityServiceStateChange()
                {
                    IsOnline = false,
                    ChangedAt = utcNow.AddDays(-4)
                },
                new UniversityServiceStateChange()
                {
                    IsOnline = true,
                    ChangedAt = utcNow.AddDays(-2)
                },
                new UniversityServiceStateChange()
                {
                    IsOnline = false,
                    ChangedAt = utcNow.AddDays(-1)
                },
                new UniversityServiceStateChange()
                {
                    IsOnline = true,
                    ChangedAt = utcNow
                },
            },
            0.4d
        };
        
        yield return new object[]
        {
            new []
            {
                new UniversityServiceStateChange()
                {
                    IsOnline = true,
                    ChangedAt = utcNow.AddDays(-5)
                },
                new UniversityServiceStateChange()
                {
                    IsOnline = false,
                    ChangedAt = utcNow.AddDays(-1)
                },
                new UniversityServiceStateChange()
                {
                    IsOnline = true,
                    ChangedAt = utcNow
                },
            },
            0.8d
        };
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}