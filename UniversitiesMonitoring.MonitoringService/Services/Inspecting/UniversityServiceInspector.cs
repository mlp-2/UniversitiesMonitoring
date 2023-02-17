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
        var nowStatus = await _serviceInspector.InspectServiceAsync(new Uri(_universityServiceEntity.Url));

        if (nowStatus != _isOnline)
        {
            reportBuilder.AddChangeState(_universityServiceEntity.ServiceId, nowStatus);
            _isOnline = nowStatus;
        }
    }
}