using System.Net;

namespace PlannerCRM.Shared.Models;

public class ResultDto
{
    public Guid  Guid { get; set; }
    public string Message { get; set; }
    public MessageType MessageType { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool HasCompleted { get; set; }

    public object Data { get; set; }
}

public enum MessageType
{
    Success,
    Warning, 
    Error,
    Information
}
