namespace PlannerCRM.Server.Utilities;

public class DtoValidatorService
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DtoValidatorService(AppDbContext context, UserManager<IdentityUser> userManager) {
        _context = context;
        _userManager = userManager;
    }

    public void ValidateEmployee(EmployeeFormDto dto, OperationType operation, out bool isValid) {
        CheckDtoHealth(dto, out isValid);

        if (dto.Role == ConstantValues.ADMIN_ROLE || dto.Email == ConstantValues.ADMIN_EMAIL) {
            throw new DuplicateElementException(message: ExceptionsMessages.NOT_ASSEGNABLE_ROLE);
        }

        var employeeIsAlreadyPresent = _context.Employees
            .Any(em => 
                EF.Functions.ILike(em.Email, $"%{dto.Email}%"));        
        
        var userIsAlreadyPresent = _userManager.Users
            .Any(user => user.Email == dto.Email);

        if (operation == OperationType.ADD) {
            if (employeeIsAlreadyPresent || userIsAlreadyPresent) {
                throw new DuplicateElementException(message: ExceptionsMessages.OBJECT_ALREADY_PRESENT);
            }
        } else {
            if (!employeeIsAlreadyPresent && !userIsAlreadyPresent) {
                throw new KeyNotFoundException(message: ExceptionsMessages.OBJECT_NOT_FOUND);
            }
        }
        
        isValid = true;
    }

    public void ValidateWorkOrder(WorkOrderFormDto dto, OperationType operation, out bool isValid) {
        CheckDtoHealth(dto, out isValid);
        
        var isAlreadyPresent = _context.WorkOrders
            .Any(wo=> ((!wo.IsCompleted) || (!wo.IsDeleted)) && wo.Id == dto.Id);

        if (operation == OperationType.ADD) {
            if (isAlreadyPresent) {
                throw new DuplicateElementException(message: ExceptionsMessages.OBJECT_ALREADY_PRESENT);
            }
        } else {
            if (!isAlreadyPresent) {
                throw new KeyNotFoundException(message: ExceptionsMessages.OBJECT_NOT_FOUND);
            }
        }

        isValid = true;
    }

    public void ValidateActivity(ActivityFormDto dto, OperationType operation, out bool isValid) {
        CheckDtoHealth(dto, out isValid);

        var isAlreadyPresent = _context.Activities
            .Any(ac => ac.Id == dto.Id);

        if (isAlreadyPresent) {
            throw new DuplicateElementException(ExceptionsMessages.OBJECT_ALREADY_PRESENT);
        }

        if (operation == OperationType.ADD) {
            if (isAlreadyPresent) {
                throw new DuplicateElementException(message: ExceptionsMessages.OBJECT_ALREADY_PRESENT);
            }
        } else {
            if (!isAlreadyPresent) {
                throw new KeyNotFoundException(message: ExceptionsMessages.OBJECT_NOT_FOUND);
            }
        }

        if (!dto.EmployeeActivity.Any()) {
            throw new NullReferenceException(ExceptionsMessages.NULL_PROP);
        }
    }

    public void ValidateWorkTime(WorkTimeRecordFormDto dto, out bool isValid) => CheckDtoHealth(dto, out isValid);

    public async Task<Employee> ValidateDeleteEmployeeAsync(int id) {
        return await _context.Employees
            .SingleOrDefaultAsync(em => em.Id == id) ??
                throw new KeyNotFoundException(ExceptionsMessages.IMPOSSIBLE_DELETE);
    }

    public async Task<IdentityUser> ValidateDeleteUserAsync(string email) {
        if (string.IsNullOrEmpty(email)) {
            throw new ArgumentNullException(email, ExceptionsMessages.NULL_OBJECT);
        }

        return await _userManager.FindByEmailAsync(email) 
            ?? throw new KeyNotFoundException(ExceptionsMessages.OBJECT_NOT_FOUND);
    }

    public async Task<WorkOrder> ValidateDeleteWorkOrderAsync(int id) {
        var hasRelationships = await _context.EmployeeActivity
			.AnyAsync(ea  => ea.Activity.WorkOrderId == id);

        return await _context.WorkOrders
			.SingleOrDefaultAsync(w => w.Id == id) 
                ?? throw new KeyNotFoundException(ExceptionsMessages.IMPOSSIBLE_DELETE);
                
    }

    public async Task<Activity> ValidateDeleteActivityAsync(int id) {
        return await _context.Activities
            .SingleOrDefaultAsync(ac => ac.Id == id)
                ?? throw new InvalidOperationException(ExceptionsMessages.IMPOSSIBLE_DELETE);
    }

    private static void CheckDtoHealth(object dto, out bool isValid) {
        isValid = false;

        if (dto is null) {
            throw new ArgumentNullException(paramName: nameof(dto), message: ExceptionsMessages.NULL_OBJECT); 
        }

        if (dto.GetType() != typeof(EmployeeFormDto)) {
            throw new TypeMismatchException(message: ExceptionsMessages.TYPE_MISMATCH);
        }
        
        var propertyName = string.Empty;
        var hasPropertiesNull = dto
            .GetType()
            .GetProperties()
            .Any(prop => {
                if (prop.GetValue(dto) is null) {
                    propertyName = prop.Name;
                    return true;
                }

                return false;
            });

        if (hasPropertiesNull) {
            throw new ArgumentNullException(paramName: propertyName, message: ExceptionsMessages.NULL_ARG);
        }

        isValid = true;
    }
}