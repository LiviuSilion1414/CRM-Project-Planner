using PlannerCRM.Server.Models.Entities;

namespace PlannerCRM.Server.Mappers;

public static class WorkOrderMapper
{
    public static WorkOrder MapToWorkOrder(this WorkOrderFormDto dto)
    {
        return new WorkOrder
        {
            Id = dto.Id,
            Name = dto.Name,
            StartDate = dto.StartDate
                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            FinishDate = dto.FinishDate
                ?? throw new NullReferenceException(ExceptionsMessages.NULL_ARG),
            IsDeleted = false,
            IsCompleted = false,
            FirmClientId = dto.FirmClientId,
            Client = new()
        };
    }

    public static WorkOrderFormDto MapToWorkOrderFormDto(this WorkOrder workOrder)
    {
        return new WorkOrderFormDto
        {
            Id = workOrder.Id,
            Name = workOrder.Name,
            StartDate = workOrder.StartDate,
            FinishDate = workOrder.FinishDate,
            FirmClientId = workOrder.FirmClientId,
            IsOnEdit = true,
            ClientName = string.Empty
        };
    }

    public static WorkOrderSelectDto MapToWorkOrderSelectDto(this WorkOrder workOrder)
    {
        return new WorkOrderSelectDto
        {
            Id = workOrder.Id,
            Name = workOrder.Name,
            Client = new()
        };
    }

    public static ClientWorkOrder MapToClientWorkOrder(this WorkOrder workOrder)
    {
        return new ClientWorkOrder
        {
            WorkOrderId = workOrder.Id,
            FirmClientId = workOrder.FirmClientId,
        };
    }

    public static WorkOrderDeleteDto MapToWorkOrderDeleteDto(this WorkOrder workOrder)
    {
        return new WorkOrderDeleteDto
        {
            Id = workOrder.Id,
            Name = workOrder.Name,
            StartDate = workOrder.StartDate,
            FinishDate = workOrder.FinishDate,
            Client = workOrder.Client.MapToClientViewDto()
        };
    }

    public static WorkOrderViewDto MapToWorkOrderViewDto(this WorkOrder workOrder)
    {
        return new WorkOrderViewDto
        {
            Id = workOrder.Id,
            Name = workOrder.Name,
            StartDate = workOrder.StartDate,
            FinishDate = workOrder.FinishDate,
            IsCompleted = workOrder.IsCompleted,
            IsDeleted = workOrder.IsDeleted,
            Client = workOrder.Client.MapToClientViewDto()
        };
    }
}