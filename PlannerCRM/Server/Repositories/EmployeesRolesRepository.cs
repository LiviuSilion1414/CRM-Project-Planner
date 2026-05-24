using Humanizer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.JSInterop.Infrastructure;
using PlannerCRM.Server.Models;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Server.Repositories;

public class EmployeesRolesRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(EmployeesRolesDto dto)
    {
        try
        {
            var model = _mapper.Map<EmployeesRole>(dto);

            await _context.EmployeesRoles.AddAsync(model);
            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task Update(EmployeesRolesDto dto)
    {
        try
        {
            var model = await _context.EmployeesRoles
                                      .FirstOrDefaultAsync(x => x.Id == (Guid)dto.id);

            if (model == null) return;

            model.FkIdEmployee = dto.fkIdEmployee;
            model.FkIdRole = dto.fkIdRole;

            _context.Update(model);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {
            throw;
        }
    }


    public async Task Delete(EmployeesRolesDto dto)
    {
        try
        {
            var employee = await _context.EmployeesRoles
                                         .FirstOrDefaultAsync(e => e.Id == dto.id);

            _context.Remove(employee);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteMultiple(List<Guid?> idList)
    {
        try
        {
            var itemsList = await _context.EmployeesRoles
                                         .AsNoTracking()
                                         .AsSplitQuery()
                                         .Where(a => idList.Contains(a.Id))
                                         .ToListAsync();

            _context.RemoveRange(itemsList);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {

            throw;
        }
    }


    public async Task<EmployeesRolesDto> Get(EmployeesRolesFilterDto filter)
    {
        try
        {
            var employee = await _context.EmployeesRoles
                                         .AsNoTracking()
                                         .AsSplitQuery()
                                         .Include(e => e.FkIdEmployeeNavigation)
                                         .Include(e => e.FkIdRoleNavigation)
                                         .FirstAsync(e => e.Id == filter.employeeId);

            return _mapper.Map<EmployeesRolesDto>(employee);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeesRolesDto>> List(EmployeesRolesFilterDto filter)
    {
        try
        {
            var employees = await _context.EmployeesRoles
                                          .AsNoTracking()
                                          .AsSplitQuery()
                                          .OrderBy(e => e.Id)
                                          .Include(e => e.FkIdEmployeeNavigation)
                                          .Include(e => e.FkIdRoleNavigation).ThenInclude(x => x.FkIdGroupRoleNavigation)
                                          .Where(x => (filter.roleId == null || filter.roleId == Guid.Empty) || (x.FkIdRole == (Guid)filter.roleId) &&
                                                      (filter.employeeId == null || filter.employeeId == Guid.Empty) || (x.FkIdEmployee == filter.employeeId))
                                          .ToListAsync();

            return _mapper.Map<List<EmployeesRolesDto>>(employees);
        } catch (Exception)
        {
            throw;
        }
    }
}