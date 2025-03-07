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
public class AccountController(IMapper mapper, AppDbContext context, IConfiguration config) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IConfiguration _config = config;
    private readonly AppDbContext _context = context;

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var foundEmployee = await _context.Employees
                                          .Include(x => x.EmployeeRoles)
                                            .ThenInclude(x => x.Role)
                                          .SingleOrDefaultAsync(em => em.Email.Contains(dto.EmailOrUsername) ||
                                                                      em.Name.Contains(dto.EmailOrUsername));

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

            var appSettings = _config.GetSection("AppSettings").Get<Models.Common.ServerAppSettings>();

            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(CustomClaimTypes.Name, foundEmployee.Name),
                    new Claim(CustomClaimTypes.Email, foundEmployee.Email),
                    new Claim(CustomClaimTypes.Guid, foundEmployee.Id.ToString()),
                    new Claim(CustomClaimTypes.IsAuthenticated, true.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            foreach (var item in foundEmployee.EmployeeRoles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(CustomClaimTypes.Role, item.Role.RoleName));
            }

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenAsString = tokenHandler.WriteToken(token);

            foundEmployee.LastSeen = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(
                new ResultDto 
                {
                    Data = tokenAsString,
                    Guid = foundEmployee.Id,
                    HasCompleted = true,
                    Message = "Logged in",
                    MessageType = MessageType.Success,
                    StatusCode = HttpStatusCode.OK,
                    
                }
            );
        } 
        else
        {
            return NotFound("User not found");
        }
    }
}
