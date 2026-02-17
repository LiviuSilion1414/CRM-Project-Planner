using Humanizer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.JSInterop.Infrastructure;
using PlannerCRM.Server.Models;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Server.Repositories;

public class EmployeesActivitiesRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(EmployeeActivityDto dto)
    {
        try
        {
            var model = _mapper.Map<EmployeeActivity>(dto);

            await _context.EmployeeActivities.AddAsync(model);
            await _context.SaveChangesAsync();
        } 
        catch (Exception)
        {
            throw;
        }
    }

    public async Task Update(EmployeeActivityDto dto)
    {
        try
        {
            var model = await _context.EmployeeActivities
                                      .FirstOrDefaultAsync(x => x.Id == (Guid)dto.id);

            if (model == null) return;

            model.FkIdEmployee = (Guid)dto.fkIdEmployee;
            model.FkIdActivity = (Guid)dto.fkIdActivity;

            _context.Update(model);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {
            throw;
        }
    }


    public async Task Delete(EmployeeActivityDto dto)
    {
        try
        {
            var employee = await _context.EmployeeActivities
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
            var itemsList = await _context.EmployeeActivities
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


    public async Task<EmployeeActivityDto> Get(EmployeeActivityFilterDto filter)
    {
        try
        {
            var employee = await _context.EmployeeActivities
                                         .AsNoTracking()
                                         .AsSplitQuery()
                                         .Include(e => e.FkIdEmployeeNavigation)
                                         .Include(e => e.FkIdActivityNavigation)
                                         .FirstAsync(e => e.Id == filter.id);

            return _mapper.Map<EmployeeActivityDto>(employee);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeActivityDto>> List(EmployeeActivityFilterDto filter)
    {
        try
        {
            var employees = await _context.EmployeeActivities
                                          .AsNoTracking()
                                          .AsSplitQuery()
                                          .OrderBy(e => e.Id)
                                          .Include(e => e.FkIdEmployeeNavigation)
                                          .Include(e => e.FkIdActivityNavigation)
                                          .Where(x => (filter.activityId == null || filter.activityId == Guid.Empty) || (x.FkIdActivity == (Guid)filter.activityId) &&
                                                      (filter.employeeId == null || filter.employeeId == Guid.Empty) || (x.FkIdEmployee == filter.employeeId))
                                          .ToListAsync();

            return _mapper.Map<List<EmployeeActivityDto>>(employees);
        } catch (Exception)
        {
            throw;
        }
    }
}