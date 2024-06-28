namespace PlannerCRM.Server.Mappers;

public static class WorkTimeRecordMapper
{
    public async static Task<WorkTimeRecord> MapToWorkTimeRecord(this WorkTimeRecordFormDto dto, AppDbContext context) {
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
            Employee = context.Employees
                .Where(em => !em.IsDeleted && !em.IsArchived)
                .Single(e => e.Id == dto.EmployeeId),
            WorkOrderId = await context.WorkOrders
                .AnyAsync(wo => !wo.IsDeleted && !wo.IsCompleted)
                    ? dto.WorkOrderId
                    : throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_ADD)
        };
    }

    public static WorkTimeRecordViewDto MapToWorkTimeRecordViewDto(
        this WorkTimeRecord workTimeRecord, 
        AppDbContext context,
        int workOrderId, 
        int activityId, 
        string employeeId) 
    {
        return new WorkTimeRecordViewDto
        {
            Id = workTimeRecord.Id,
            Date = workTimeRecord.Date,
            Hours = context.WorkTimeRecords
                .Where(wtr =>
                    wtr.WorkOrderId == workOrderId &&
                    wtr.ActivityId == activityId &&
                    wtr.EmployeeId == employeeId)
                .Distinct()
                .Sum(wtrSum => wtrSum.Hours),
            TotalPrice = workTimeRecord.TotalPrice,
            ActivityId = workTimeRecord.ActivityId,
            WorkOrderId = workTimeRecord.WorkOrderId,
            EmployeeId = workTimeRecord.EmployeeId
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
            EmployeeId = workTimeRecord.EmployeeId
        };
    }
}