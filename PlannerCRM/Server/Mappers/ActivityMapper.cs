using PlannerCRM.Server.Models.Entities;

namespace PlannerCRM.Server.Mappers;

public static class ActivityMapper
{
    public static Activity MapToActivity(this ActivityFormDto dto)
    {
        return new Activity
        {
            Id = dto.Id,
            Name = dto.Name,
            StartDate = dto?.StartDate
                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            FinishDate = dto.FinishDate
                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            WorkOrderId = dto.WorkOrderId
                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            EmployeeActivity = dto.EmployeeActivity
                .Select(ea => ea.MapToEmployeeActivity(dto.Id))
                .ToHashSet()
        };
    }

    public static ActivityFormDto MapToActivityFormDto(this Activity activity)
    {
        return new ActivityFormDto
        {
            Id = activity.Id,
            Name = activity.Name,
            StartDate = activity.StartDate,
            FinishDate = activity.FinishDate,
            WorkOrderId = activity.WorkOrderId,
            ClientName = string.Empty,
            EmployeeActivity = [],
            ViewEmployeeActivity = activity.EmployeeActivity
                .Select(ea => ea.MapToEmployeeActivityDto())
                .ToHashSet()
        };
    }

    public static EmployeeActivity MapToEmployeeActivity(this EmployeeActivityDto dto, int activityId)
    {
        return new EmployeeActivity
        {
            Id = dto.Id,
            EmployeeId = dto.EmployeeId,
            ActivityId = activityId,
        };
    }

    public static ActivitySelectDto MapToActivitySelectDto(this Activity activity)
    {
        return new ActivitySelectDto
        {
            Id = activity.Id,
            Name = activity.Name,
            StartDate = activity.StartDate,
            FinishDate = activity.FinishDate,
            WorkOrderId = activity.WorkOrderId
        };
    }

    public static ActivityViewDto MapToActivityViewDto(this Activity activity)
    {
        return new ActivityViewDto
        {
            Id = activity.Id,
            Name = activity.Name,
            StartDate = activity.StartDate,
            FinishDate = activity.FinishDate,
            WorkOrderId = activity.WorkOrderId,
            ClientName = string.Empty,
            WorkOrderName = string.Empty,
            EmployeeActivity = activity.EmployeeActivity
                .Select(ea => ea.MapToEmployeeActivityDto())
                .ToHashSet()
        };
    }

    public static ActivityViewDto MapToActivityViewDto(this Activity activity, string clientName, string workOrderName)
    {
        var mappedActivity = activity.MapToActivityViewDto();
        mappedActivity.ClientName = clientName;
        mappedActivity.WorkOrderName = workOrderName;

        return mappedActivity;
    }

    public static ActivityDeleteDto MapToActivityDeleteDto(this Activity activity)
    {
        return new ActivityDeleteDto
        {
            Id = activity.Id,
            Name = activity.Name,
            StartDate = activity.StartDate,
            FinishDate = activity.FinishDate,
            WorkOrderId = activity.WorkOrderId,
            Client = new(),
            Employees = []
        };
    }

}