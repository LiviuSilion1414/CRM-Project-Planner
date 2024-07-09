namespace PlannerCRM.Server.Mappers;

public static class WorkTimeRecordMapper
{
    public static WorkTimeRecord MapToWorkTimeRecord(this WorkTimeRecordFormDto dto)
    {
        return new WorkTimeRecord
        {
            Id = dto.Id,
            Date = dto.Date,
            Hours = dto.Hours
                ?? throw new ArgumentNullException(nameof(dto.Hours), ExceptionsMessages.NULL_ARG),
            TotalPrice = dto.TotalPrice + dto.Hours
                ?? throw new ArgumentNullException(nameof(dto.Hours), ExceptionsMessages.NULL_ARG),
            ActivityId = dto.ActivityId,
            EmployeeId = dto.EmployeeId,
            Employee = new(), 
            WorkOrderId = dto.WorkOrderId
        };
    }

    public static WorkTimeRecordViewDto MapToWorkTimeRecordViewDto(this WorkTimeRecord workTimeRecord)
    {
        return new WorkTimeRecordViewDto
        {
            Id = workTimeRecord.Id,
            Date = workTimeRecord.Date,
            Hours = workTimeRecord.Hours,
            TotalPrice = workTimeRecord.TotalPrice,
            ActivityId = workTimeRecord.ActivityId,
            WorkOrderId = workTimeRecord.WorkOrderId,
            EmployeeId = workTimeRecord.EmployeeId
        };
    }
}