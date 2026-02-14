using Azure.Core;
using Humanizer;
using PlannerCRM.Server.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using static PlannerCRM.Shared.Dtos.DtoShared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlannerCRM.Server.System;

public class SystemLogHelper(PlannerCrmContext context)
{
    public PlannerCrmContext _context = context;
    public async Task WriteLog(string endpoint, Exception ex, IIdentity identity, object request)
    {
        SystemLog systemLog = new()
        {
            Date = DateTime.Now,
            Endpoint = endpoint,
            Reason = ex.Message,
            Stacktrace = ex.StackTrace,
            Username = ((ClaimsIdentity)identity).Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Name).Value,
            Request = JsonSerializer.Serialize(request),
            FkIdProject = Guid.Parse(Projects.Api.GetDescription())
        };

        await _context.SystemLogs.AddAsync(systemLog);
        await _context.SaveChangesAsync();
    }
}
