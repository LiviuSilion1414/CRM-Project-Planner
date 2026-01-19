using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using PlannerCRM.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlannerCRM.Server.Controllers;

[ApiController]
[Route(ApiUrl.ACCOUNT_CONTROLLER)]
public class AccountController(IMapper mapper, PlannerCrmContext context, IConfiguration config) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IConfiguration _config = config;
    private readonly PlannerCrmContext _context = context;

    [AllowAnonymous]
    [HttpPost]
    [Route(ApiUrl.ACCOUNT_LOGIN)]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var foundEmployee = await _context.Employees
                                          .Include(x => x.EmployeesRoles)
                                          .ThenInclude(x => x.FkIdRoleNavigation).ThenInclude(x => x.MenuRoles).ThenInclude(x => x.FkIdMenuNavigation)
                                          .SingleOrDefaultAsync(em => em.Username.Contains(dto.username));

        if (foundEmployee is not null)
        {
            byte[] salt = new byte[128 / 8];
            string cryptedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: dto.password,
                                                                            salt: salt,
                                                                            prf: KeyDerivationPrf.HMACSHA256,
                                                                            iterationCount: 10000,
                                                                            numBytesRequested: 256 / 8));
            if (cryptedPwd != foundEmployee.PasswordHash)
            {
                return BadRequest(new());
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            var appSettings = _config.GetSection("AppSettings").Get<ServerAppSettings>();

            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(CustomClaimTypes.Name, foundEmployee.Name),
                    new Claim(CustomClaimTypes.Email, foundEmployee.Username),
                    new Claim(CustomClaimTypes.Guid, foundEmployee.Id.ToString()),
                    new Claim(CustomClaimTypes.IsAuthenticated, true.ToString()),
                }),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            foreach (var employeeRole in foundEmployee.EmployeesRoles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(CustomClaimTypes.Role, employeeRole.FkIdRoleNavigation.Name));
                foreach (var menu in employeeRole.FkIdRoleNavigation.MenuRoles.Select(x => x.FkIdMenuNavigation))
                {
                    tokenDescriptor.Subject.AddClaim(new Claim(CustomClaimTypes.Menu, menu.Path));
                }

            }

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenAsString = tokenHandler.WriteToken(token);

            foundEmployee.LastSeen = DateTime.Now;
            //foundEmployee.Token = tokenAsString;

            await _context.SaveChangesAsync();

            return Ok(
                new ResultDto
                {
                    data = tokenAsString,
                    id = foundEmployee.Id,
                    hasCompleted = true,
                    message = "Logged in",
                    messageType = MessageType.Success,
                    statusCode = HttpStatusCode.OK,

                }
            );
        } else
        {
            return NotFound("User not found");
        }
    }
}
