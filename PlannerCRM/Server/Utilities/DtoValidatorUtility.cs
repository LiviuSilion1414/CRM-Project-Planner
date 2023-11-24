namespace PlannerCRM.Server.Utilities;

public class DtoValidatorUtillity
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public DtoValidatorUtillity(AppDbContext context, UserManager<IdentityUser> userManager) {
        _dbContext = context;
        _userManager = userManager;
    }

    public async Task<bool> ValidateEmployeeAsync(EmployeeFormDto dto, OperationType operation) {
        var isValid = CheckDtoHealth(dto);

        if (isValid) {
            if (dto.Role == ConstantValues.ADMIN_ROLE || dto.Email == ConstantValues.ADMIN_EMAIL) {
                throw new DuplicateElementException(ExceptionsMessages.NOT_ASSEGNABLE_ROLE);
            }

            var isEmployeeAlreadyPresent = await _userManager.FindByIdAsync(dto.Id);
            var isUserAlreadyPresent = await _dbContext.Employees.AnyAsync(em => em.Id == dto.Id);

            if (operation == OperationType.ADD) {
                if (isEmployeeAlreadyPresent is not null || isUserAlreadyPresent) {
                    throw new DuplicateElementException(ExceptionsMessages.OBJECT_ALREADY_PRESENT);
                }
            } 
            
            if (operation == OperationType.EDIT) {
                if (isEmployeeAlreadyPresent is null || !isUserAlreadyPresent) {
                    throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
                }
            }

            return true;
        }
        
        return false;
    }

    public async Task<bool> ValidateClientAsync(ClientFormDto dto, OperationType operation) {
        var isValid = CheckDtoHealth(dto);

        if (isValid) {
            var clientIsAlreadyPresent = await _dbContext.Clients
                .AnyAsync(em => em.Id == dto.Id);        
            
            if (operation == OperationType.ADD) {
                if (clientIsAlreadyPresent) {
                    throw new DuplicateElementException(ExceptionsMessages.OBJECT_ALREADY_PRESENT);
                }
            } 
            
            if (operation == OperationType.EDIT) {
                if (!clientIsAlreadyPresent) {
                    throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
                }
            }

            return true;
        }
        
        return false;
    }

    public async Task<bool> ValidateWorkOrderAsync(WorkOrderFormDto dto, OperationType operation) {
        var isValid = CheckDtoHealth(dto);
        
        if (isValid) {

            var isAlreadyPresent = await _dbContext.WorkOrders
                .AnyAsync(wo=> ((!wo.IsCompleted) || (!wo.IsDeleted)) && wo.Id == dto.Id);

            if (operation == OperationType.ADD) {
                if (!await _dbContext.Clients.AnyAsync()) {
                    throw new UpdateRowSourceException(ExceptionsMessages.IMPOSSIBLE_ADD);
                }
                if (isAlreadyPresent) {
                    throw new DuplicateElementException(ExceptionsMessages.OBJECT_ALREADY_PRESENT);
                }
            } 

            if (operation == OperationType.EDIT) {
                if (!isAlreadyPresent) {
                    throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
                }
            }

            return true;
        }

        return false;
    }

    public async Task<bool> ValidateActivityAsync(ActivityFormDto dto, OperationType operation) {
        var isValid = CheckDtoHealth(dto);

        if (isValid) {
            var isAlreadyPresent = await _dbContext.Activities
                .AnyAsync(ac => ac.Id == dto.Id);

            if (operation == OperationType.ADD) {
                if (isAlreadyPresent) {
                    throw new DuplicateElementException(ExceptionsMessages.OBJECT_ALREADY_PRESENT);
                }
            } 
            
            if (operation == OperationType.EDIT) {
                if (!isAlreadyPresent) {
                    throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
                }
            }

            if (dto.ViewEmployeeActivity is null) {
                throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
            }

            return true;
        }

        return false;
    }

    public bool ValidateWorkTime(WorkTimeRecordFormDto dto) => 
        CheckDtoHealth(dto);

    public async Task<bool> ValidateDeleteEmployeeAsync(string userId) {
        var employeeExists = await _dbContext.Employees
            .SingleOrDefaultAsync(em => em.Id == userId) ??
                throw new KeyNotFoundException(ExceptionsMessages.IMPOSSIBLE_DELETE);

        var userExists = await _userManager.FindByIdAsync(userId) 
            ?? throw new KeyNotFoundException(ExceptionsMessages.IMPOSSIBLE_DELETE);

        return employeeExists is not null && userExists is not null;
    }

    public async Task<FirmClient> ValidateDeleteClientAsync(int id) {
        return await _dbContext.Clients
            .SingleOrDefaultAsync(cl => cl.Id == id) ??
                throw new KeyNotFoundException(ExceptionsMessages.IMPOSSIBLE_DELETE);
    }

    public async Task<WorkOrder> ValidateDeleteWorkOrderAsync(int id) {
        var hasRelationships = await _dbContext.EmployeeActivity
			.AnyAsync(ea  => ea.Activity.WorkOrderId == id);

        return await _dbContext.WorkOrders
			.SingleOrDefaultAsync(w => w.Id == id) 
                ?? throw new KeyNotFoundException(ExceptionsMessages.IMPOSSIBLE_DELETE);     
    }

    public async Task<Activity> ValidateDeleteActivityAsync(int id) {
        return await _dbContext.Activities
            .SingleOrDefaultAsync(ac => ac.Id == id)
                ?? throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_DELETE);
    }

    private static bool CheckForNullProperties(object dto) {
        return dto
            .GetType()
            .GetProperties()
            .Any(prop => prop.GetValue(dto) is null);
    }

    private static bool CheckDtoHealth(object dto) {
        if (dto is null) {
            return false;   
        }
        
        if (CheckForNullProperties(dto)) {
            return false;
        }

        return true;
    }
}