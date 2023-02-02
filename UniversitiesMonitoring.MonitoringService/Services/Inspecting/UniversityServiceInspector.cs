using System.Net;
using UniversityMonitoring.Data.Entities;
namespace UniversitiesMonitoring.MonitoringService.Services.Inspecting;

internal class UniversityServiceInspector
{
    private readonly IServicesInspector _servicesInspector;
    private readonly UniversityServiceEntity _universityServiceEntity;
    private bool _isOnline;

    
    public UniversityServiceInspector(IServicesInspector inspector, UniversityServiceEntity serviceEntity)
    {
        _servicesInspector = inspector;
        _universityServiceEntity = serviceEntity;
        _isOnline = _universityServiceEntity.IsOnline;
    }

    public async Task UpdateStateAsync(UpdateBuilder reportBuilder)
    {
        var nowStatus = await _servicesInspector.InspectServiceAsync(IPAddress.Parse(_universityServiceEntity.IpAddress));
        
        if (nowStatus != _isOnline)
        {
            reportBuilder.AddChangeState(_universityServiceEntity.ServiceId, nowStatus);
            _isOnline = nowStatus;
        }
    }
}