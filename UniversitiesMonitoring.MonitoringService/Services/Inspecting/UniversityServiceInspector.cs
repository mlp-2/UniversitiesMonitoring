using System.Net;
using UniversityMonitoring.Data.Entities;
namespace UniversitiesMonitoring.MonitoringService.Services.Inspecting;

internal class UniversityServiceInspector
{
    private readonly IServiceInspector _serviceInspector;
    private readonly UniversityServiceEntity _universityServiceEntity;
    private bool _isOnline;

    public UniversityServiceInspector(IServiceInspector inspector, UniversityServiceEntity serviceEntity)
    {
        _serviceInspector = inspector;
        _universityServiceEntity = serviceEntity;
        _isOnline = _universityServiceEntity.IsOnline;
    }

    public async Task UpdateStateAsync(UpdateBuilder reportBuilder)
    {
        var nowStatus = await _serviceInspector.InspectServiceAsync(IPAddress.Parse(_universityServiceEntity.IpAddress));
        var lockObj = new object();
        
        if (nowStatus != _isOnline)
        {
            lock (lockObj)
            {
                reportBuilder.AddChangeState(_universityServiceEntity.ServiceId, nowStatus);    
            }
            _isOnline = nowStatus;
        }
    }
}