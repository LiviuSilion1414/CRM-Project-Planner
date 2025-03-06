using Humanizer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PdfSharp.Pdf.Filters;

namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository(AppDbContext context, IMapper mapper)
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(EmployeeDto dto)
    {
        try
        {
            var model = _mapper.Map<Employee>(dto);

            if ((await _context.Employees.SingleOrDefaultAsync(em => em.Email == dto.Email)) == null)
            {
                byte[] salt = new byte[128 / 8];
                string cryptedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: dto.Password,
                                                                                salt: salt,
                                                                                prf: KeyDerivationPrf.HMACSHA256,
                                                                                iterationCount: 10000,
                                                                                numBytesRequested: 256 / 8));

                model.PasswordHash = cryptedPwd;

                await _context.Employees.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task Update(EmployeeDto dto)
    {
        try
        {
            var model = _mapper.Map<Employee>(dto);

            var existingModel = await _context.Employees.SingleAsync(em => em.Id == model.Id);
            existingModel = model;

            _context.Update(existingModel);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task Delete(EmployeeFilterDto filter)
    {
        try
        {
            var employee = await _context.Employees
                                         .Include(e => e.Activities)
                                         .Include(e => e.EmployeeRoles)
                                         .SingleAsync(e => e.Id == filter.EmployeeId);

            _context.Remove(employee);

            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EmployeeDto> Get(EmployeeFilterDto filter)
    {
        try
        {
            var employee = await _context.Employees
                                         .Include(e => e.Activities)
                                         .Include(e => e.EmployeeRoles)
                                         .SingleAsync(e => e.Id == filter.EmployeeId);

            return _mapper.Map<EmployeeDto>(employee);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeDto>> List(EmployeeFilterDto filter)
    {
        try
        {
            var employees = await _context.Employees
                                          .OrderBy(e => e.Id)
                                          .Include(e => e.EmployeeRoles)
                                          .Include(e => e.Activities)
                                          .Include(e => e.EmployeeRoles)
                                          .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(employees);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeDto>> Search(EmployeeFilterDto filter)
    {
        try
        {
            var foundEmployee = await _context.Employees
                                              .Where(em => EF.Functions.ILike(em.Name, $"{filter.SearchQuery}"))
                                              .Include(e => e.Activities)
                                              .Include(e => e.EmployeeRoles)
                                              .Where(em =>
                                                  EF.Functions.ILike(em.Name, $"{filter.SearchQuery}") ||
                                                  EF.Functions.ILike(em.Email, $"{filter.SearchQuery}"))
                                              .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(foundEmployee);
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ActivityDto>> FindAssociatedActivitiesByEmployeeId(EmployeeFilterDto filter)
    {
        try
        {
            var foundActivities = await _context.Activities
                                                .Include(ac => ac.EmployeeActivities)
                                                .Include(ac => ac.Employees)
                                                .Where(ac => ac.Employees.Any(em => em.Id == filter.EmployeeId))
                                                .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(foundActivities);
        } 
        catch (Exception)
        {
            throw;
        }
    }
}