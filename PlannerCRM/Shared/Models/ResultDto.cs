using System.Net;

namespace PlannerCRM.Shared.Models;

public class ResultDto
{
    public Guid? id { get; set; }
    public string? message { get; set; }
    public MessageType messageType { get; set; }
    public HttpStatusCode statusCode { get; set; }
    public bool hasCompleted { get; set; }

    public object? data { get; set; }
}

public enum MessageType
{
    Success,
    Warning, 
    Error,
    Information
}
