using Humanizer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.JSInterop.Infrastructure;
using PlannerCRM.Server.Models;
using System.Security.Cryptography;
using static PlannerCRM.Shared.Dtos.DtoShared;

namespace PlannerCRM.Server.Repositories;

public class EmployeeRepository(PlannerCrmContext context, IMapper mapper)
{
    private readonly PlannerCrmContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task Insert(EmployeeDto dto)
    {
        try
        {
            var model = _mapper.Map<Employee>(dto);

            if (!await _context.Employees.AnyAsync(em => em.Username.ToLower().Trim().Equals(dto.username.ToLower().Trim())))
            {
                if (string.IsNullOrEmpty(dto.newPassword))
                {
                    dto.newPassword = GeneratePassword(8);
                }

                model.PasswordHash = CryptPassword(dto.newPassword);
                model.LastSeen = DateTime.Now;
                model.CreationDate = DateTime.Now;

                await _context.Employees.AddAsync(model);
                await _context.SaveChangesAsync();

                var userRole = new EmployeesRole()
                {
                    FkIdRole = Guid.Parse(BaseRoles.User.GetDescription()),
                    FkIdEmployee = model.Id,
                    IsRemoveable = false
                };

                await _context.EmployeesRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();
            }
        } catch (Exception)
        {
            throw;
        }
    }

    private static string CryptPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        string cryptedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password,
                                                                        salt: salt,
                                                                        prf: KeyDerivationPrf.HMACSHA256,
                                                                        iterationCount: 10000,
                                                                        numBytesRequested: 256 / 8));
        return cryptedPwd;
    }

    static string GeneratePassword(int length)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+";
        char[] password = new char[length];

        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            for (int i = 0 ; i < length ; i++)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                password[i] = validChars[(int)(num % (uint)validChars.Length)];
            }
        }

        return new string(password);
    }

    public async Task Update(EmployeeDto dto)
    {
        try
        {
            var model = await _context.Employees
                                      .FirstOrDefaultAsync(x => x.Id == (Guid)dto.id);

            if (model == null) return;

            model.Name = dto.name;
            model.Surname = dto.surname;
            model.Username = dto.username;
            model.IsRemoveable = (bool)dto.isRemoveable;

            if (!string.IsNullOrEmpty(dto.newPassword))
            {
                model.PasswordHash = CryptPassword(dto.newPassword);
            }

            _context.Update(model);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {
            throw;
        }
    }


    public async Task Delete(EmployeeDto dto)
    {
        try
        {
            var employee = await _context.Employees
                                         .Include(e => e.EmployeeActivities)
                                         .Include(e => e.EmployeesRoles)
                                         .Include(e => e.WorkTimes)
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
            var itemsList = await _context.Employees
                                         .AsNoTracking()
                                         .AsSplitQuery()
                                         .Include(a => a.EmployeeActivities)
                                         .Include(a => a.EmployeesRoles)
                                         .Where(a => idList.Contains(a.Id))
                                         .ToListAsync();

            _context.RemoveRange(itemsList);

            await _context.SaveChangesAsync();
        } catch (Exception)
        {

            throw;
        }
    }

    public async Task<EmployeeDto> Get(EmployeeFilterDto filter)
    {
        try
        {
            var employee = await _context.Employees
                                         .AsNoTracking()
                                         .AsSplitQuery()
                                         .Include(e => e.EmployeeActivities)
                                         .Include(e => e.EmployeesRoles).ThenInclude(x => x.FkIdRoleNavigation).ThenInclude(x => x.FkIdGroupRoleNavigation)
                                         .FirstAsync(e => e.Id == filter.employeeId);

            return _mapper.Map<EmployeeDto>(employee);
        } catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeDto>> List(EmployeeFilterDto filter)
    {
        try
        {
            var employees = await _context.Employees
                                          .AsNoTracking()
                                          .AsSplitQuery()
                                          .OrderBy(e => e.Id)
                                          .Include(e => e.EmployeesRoles)
                                          .Include(e => e.EmployeesRoles).ThenInclude(x => x.FkIdRoleNavigation)
                                          .Where(x => ((string.IsNullOrEmpty(filter.name) || x.Name.ToLower().Trim().Contains(filter.name))) &&
                                                      ((string.IsNullOrEmpty(filter.surname) || x.Surname.ToLower().Trim().Contains(filter.surname))) &&
                                                      ((string.IsNullOrEmpty(filter.username) || x.Username.ToLower().Trim().Contains(filter.username))) &&
                                                      ((filter.roleId == null || filter.roleId == Guid.Empty) || (x.EmployeesRoles.Any(y => y.FkIdRole == filter.roleId))))
                                          .ToListAsync();

            return _mapper.Map<List<EmployeeDto>>(employees);
        } catch (Exception)
        {
            throw;
        }
    }
}