using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using PlannerCRM.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController
    (
        IMapper mapper, 
        AppDbContext context,
        IConfiguration config
    ) 
    : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IConfiguration _config = config;
    //private readonly Models.Common.AppSettings _appSettings;
    private readonly AppDbContext _context = context;

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<CurrentUser?>> Login(EmployeeLoginDto dto)
    {

        var guid = Guid.NewGuid();
        var model = _mapper.Map<EmployeeLogin>(dto);
        var foundEmployee = await _context.Employees
                                          .Include(x => x.EmployeeRoles)
                                          .SingleOrDefaultAsync(em => 
                                            em.Email.Contains(dto.EmailOrUsername) ||
                                            em.Name.Contains(dto.EmailOrUsername));
        byte[] salt1 = new byte[128 / 8];
        string cryptedPwd1 = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: dto.Password,
                                                                        salt: salt1,
                                                                        prf: KeyDerivationPrf.HMACSHA256,
                                                                        iterationCount: 10000,
                                                                        numBytesRequested: 256 / 8));
        if (foundEmployee is not null)
        {
            byte[] salt = new byte[128 / 8];
            string cryptedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: dto.Password,
                                                                            salt: salt,
                                                                            prf: KeyDerivationPrf.HMACSHA256,
                                                                            iterationCount: 10000,
                                                                            numBytesRequested: 256 / 8));
            if (cryptedPwd != foundEmployee.PasswordHash)
            {
                return BadRequest(new());
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var appSettings = _config.GetSection("AppSettings").Get<Models.Common.AppSettings>();

            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);



            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, foundEmployee.Name),
                    new Claim(ClaimTypes.Email, foundEmployee.Email),
                    new Claim(ClaimTypes.NameIdentifier, foundEmployee.Guid.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenAsString = tokenHandler.WriteToken(token);

            var foundLoginData = await _context.EmployeeLoginData.OrderBy(em => em.LastSeen.Day) 
                                                                 .Where(e => e.EmployeeId == foundEmployee.Guid)
                                                                 .Include(e => e.Employee)
                                                                 .ToListAsync();

            await _context.EmployeeLoginData.AddAsync(new()
            {
                EmployeeId = foundEmployee.Guid,
                LastSeen = DateTime.UtcNow,
                Token = tokenAsString
            });

            await _context.SaveChangesAsync();

            List<Claim> newClaims = foundEmployee.EmployeeRoles
                                                 .Select(x => new Claim(ClaimTypes.Role, x.RoleName.ToString()))
                                                 .ToList();

            return Ok(new CurrentUser
                {
                    Guid = foundEmployee.Guid,
                    Email = foundEmployee.Email,
                    IsAuthenticated = true,
                    Name = foundEmployee.Name,
                    Token = tokenAsString,
                    Roles = foundEmployee.EmployeeRoles.Select(x => x.ToString()).ToList(),
                    ClaimsOk = newClaims,
                    Claims = []
                }
            );
        } 
        else
        {
            return NotFound("User not found");
        }
    }
}
