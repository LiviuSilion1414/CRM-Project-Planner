namespace PlannerCRM.Server.Utilities;

public static class FirmClientMapper
{
    public static FirmClient MapToFirmClient(this ClientFormDto dto) {
        return new FirmClient
        {
            Id = dto.Id,
            Name = dto.Name,
            VatNumber = dto.VatNumber
        };
    }

    public static ClientFormDto MapToClientFormDto(this FirmClient client) {
        return new ClientFormDto
        {
            Id = client.Id,
            Name = client.Name,
            VatNumber = client.VatNumber,
            //WorkOrderId = client.WorkOrderId
        };
    }

    public static ClientViewDto MapToClientViewDto(this FirmClient client) {
        return new ClientViewDto
        {
            Id = client.Id,
            Name = client.Name,
            VatNumber = client.VatNumber
        };
    }
    
    public static ClientDeleteDto MapToClientDeleteDto(this FirmClient client) {
        return new ClientDeleteDto
        {
            Id = client.Id,
            Name = client.Name,
            VatNumber = client.VatNumber,
            // WorkOrderId = client.WorkOrderId
        };
    }
}