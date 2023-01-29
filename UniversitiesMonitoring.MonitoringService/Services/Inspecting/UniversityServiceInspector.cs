using System.Net;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.MonitoringService.Services.Inspecting;

internal class UniversityServiceInspector
{
    private readonly IServicesInspector _servicesInspector;
    private readonly UniversityService _universityServiceEntity;
    private bool _isOnline;

    
    public UniversityServiceInspector(IServicesInspector inspector, UniversityService serviceEntity)
    {
        _servicesInspector = inspector;
        _universityServiceEntity = serviceEntity;
        _isOnline = serviceEntity.UniversityServiceStateChanges.Last().IsOnline;
    }

    public async Task UpdateStateAsync(UpdateBuilder reportBuilder)
    {
        bool nowStatus = await _servicesInspector.InspectServiceAsync(
            new IPAddress(_universityServiceEntity.Ipaddress));
        
        if (nowStatus != _isOnline)
        {
            reportBuilder.AddChangeState(_universityServiceEntity.Id, nowStatus);
            _isOnline = nowStatus;
        }
    }
}