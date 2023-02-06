using Microsoft.AspNetCore.Authorization;
using UniversitiesMonitoring.Api.Entities;
using UniversitiesMonitoring.Api.Services;
using UniversitiesMonitoring.Api.WebSocket;
using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Controllers;

[Route("/services/")]
internal class ServicesController : ControllerBase
{
    private readonly IServicesProvider _servicesProvider;
    private readonly IUsersProvider _usersProvider;
    private readonly IWebSocketUpdateStateNotifier _webSocketUpdateStateNotifier;

    private bool IsLocalHostRequest => Request.Host.Host == "localhost";
    
    public ServicesController(IServicesProvider servicesProvider, IUsersProvider usersProvider, IWebSocketUpdateStateNotifier webSocketUpdateStateNotifier)
    {
        _servicesProvider = servicesProvider;
        _usersProvider = usersProvider;
        _webSocketUpdateStateNotifier = webSocketUpdateStateNotifier;
    }
    
    [Authorize(Roles = JwtGenerator.UserRole)]
    [HttpPost("{id:long}/subscribe")]
    public async Task<IActionResult> SubscribeService([FromRoute] ulong id)
    {
        var service = await _servicesProvider.GetServiceAsync(id);
        var user = await _usersProvider.GetUserAsync(User.Identity!.Name!);
        
        if (service == null || user == null)
        {
            return BadRequest("Сервис или пользователь не найден");
        }

        await _servicesProvider.SubscribeUserAsync(user, service);
        return Ok();
    }

    [Authorize(Roles = JwtGenerator.UserRole)]
    [HttpDelete("{id:long}/unsubscribe")]
    public async Task<IActionResult> Unsubscribe([FromRoute] ulong id)
    {
        var service = await _servicesProvider.GetServiceAsync(id);
        var user = await _usersProvider.GetUserAsync(User.Identity!.Name!);
    
        if (service == null || user == null)
        {
            return BadRequest("Сервис или пользователь не найден");
        }

        await _servicesProvider.UnsubscribeUserAsync(user, service);
        return Ok();
    }

    [Authorize(Roles = JwtGenerator.UserRole)]
    [HttpPost("{id:long}/comment")]
    public async Task<IActionResult> Comment(
        [FromRoute] ulong id,
        [FromBody] Comment comment)
    {
        var service = await _servicesProvider.GetServiceAsync(id);
        var user = await _usersProvider.GetUserAsync(User.Identity!.Name!);
        
        if (service == null || user == null)
        {
            return BadRequest("Сервис или пользователь не найден");
        }

        await _servicesProvider.LeaveCommentAsync(service, user, comment);
        return Ok();
    }

    [Authorize(Roles = JwtGenerator.UserRole)]
    [HttpPost("{id:long}/report")]
    public async Task<IActionResult> Report(
        [FromRoute] ulong id,
        [FromBody] Report report)
    {
        var service = await _servicesProvider.GetServiceAsync(id);
        var user = await _usersProvider.GetUserAsync(User.Identity!.Name!);
        
        if (service == null || user == null)
        {
            return BadRequest("Сервис или пользователь не найден");
        }

        await _servicesProvider.CreateReportAsync(service, user, report);

        return Ok();
    }

    [HttpGet]
    public IActionResult GetAllServices(
        [FromQuery] bool loadUsers,
        [FromQuery] bool loadComments)
    {
        // Нужно, чтобы не сливать инфу
        loadUsers = loadUsers && IsLocalHostRequest;
        loadComments = loadComments && IsLocalHostRequest;
        
        var services = _servicesProvider.GetAllServices();

        return Ok(from service in services select new UniversityServiceEntity(service, loadUsers, loadComments));
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateService([FromBody] ChangeStateEntity[] updates)
    {
        if (!IsLocalHostRequest) return Ok();

        var updateSuccess = true;

        var servicesId = new ulong[updates.Length];

        for (var i = 0; i < updates.Length; i++)
        {
            var update = updates[i];
            servicesId[i] = updates[i].ServiceId;
            var service = await _servicesProvider.GetServiceAsync(update.ServiceId);

            if (service == null)
            {
                updateSuccess = updateSuccess && false;
                continue;
            }

            await _servicesProvider.UpdateServiceStateAsync(service, update.IsOnline, false);
        }

        await _webSocketUpdateStateNotifier.NotifyAsync(servicesId);
        
        if (updateSuccess) return Ok();
        else return BadRequest("Часть сервисов не найдены");
    }
}