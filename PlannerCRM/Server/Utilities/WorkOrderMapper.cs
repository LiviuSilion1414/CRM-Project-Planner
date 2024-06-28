namespace PlannerCRM.Server.Utilities;

public static class WorkOrderMapper
{
    public static WorkOrder MapToWorkOrder(this WorkOrderFormDto dto, AppDbContext context) {
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
            ClientId = dto.ClientId,
            Client = context.Clients
                .Single(cl => cl.Id == dto.ClientId)
        };
    }

    public static WorkOrderFormDto MapToWorkOrderFormDto(this WorkOrder workOrder, AppDbContext context) {
        return new WorkOrderFormDto
        {
            Id = workOrder.Id,
            Name = workOrder.Name,
            StartDate = workOrder.StartDate,
            FinishDate = workOrder.FinishDate,
            ClientId = workOrder.ClientId,
            IsOnEdit = true,
            ClientName = context.Clients
                .Single(cl => cl.Id == workOrder.ClientId)
                .Name,
        };
    }

    public static WorkOrderSelectDto MapToWorkOrderSelectDto(this WorkOrder workOrder, AppDbContext context) {
        return new WorkOrderSelectDto
        {
            Id = workOrder.Id,
            Name = workOrder.Name,
            ClientName = context.Clients
                .Single(cl => cl.Id == workOrder.ClientId)
                .Name
        };
    } 

    public static ClientWorkOrder MapToClientWorkOrder(this WorkOrder workOrder) {
        return new ClientWorkOrder
        {
            WorkOrderId = workOrder.Id,
            ClientId = workOrder.ClientId,
        };
    }

    public static WorkOrderDeleteDto MapToWorkOrderDeleteDto(this WorkOrder workOrder, AppDbContext context) {
        return new WorkOrderDeleteDto
        {
            Id = workOrder.Id,
            Name = workOrder.Name,
            StartDate = workOrder.StartDate,
            FinishDate = workOrder.FinishDate,
            ClientName = context.Clients
                .Single(cl => cl.Id == workOrder.ClientId)
                .Name
        };
    }

    public static WorkOrderViewDto MapToWorkOrderViewDto(this WorkOrder workOrder, AppDbContext context) {
        return new WorkOrderViewDto
        {
            Id = workOrder.Id,
            Name = workOrder.Name,
            StartDate = workOrder.StartDate,
            FinishDate = workOrder.FinishDate,
            IsCompleted = workOrder.IsCompleted,
            IsDeleted = workOrder.IsDeleted
        };
    }
}