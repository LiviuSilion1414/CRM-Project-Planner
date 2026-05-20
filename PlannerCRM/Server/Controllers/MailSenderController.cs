using AutoMapper;
using MailGunExamples;
using Microsoft.AspNetCore.Mvc;
using PlannerCRM.Server.Models;
using PlannerCRM.Server.System;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route(ApiUrl.MAIL_SENDER_CONTROLLER)]
public class MailSenderController(PlannerCrmContext context) : ControllerBase
{

    private readonly MailSender _repo = new();
    private readonly SystemLogHelper _systemLog = new(context);

    [HttpPost]
    [Route(ApiUrl.SEND_MAIL)]
    public async Task<ResultDto> SendMail()
    {
        try
        {
            var res = await _repo.Send();
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = true,
                message = "Operation completed",
                messageType = MessageType.Success,
                statusCode = HttpStatusCode.OK
            };
        } catch (Exception ex)
        {
            await _systemLog.WriteLog(ApiUrl.MAIL_SENDER_CONTROLLER + ApiUrl.SEND_MAIL, ex, User?.Identity, null);
            return new ResultDto()
            {
                id = null,
                data = null,
                hasCompleted = false,
                message = "Operation failed",
                messageType = MessageType.Error,
                statusCode = HttpStatusCode.NotFound
            };
        }
    }
}
