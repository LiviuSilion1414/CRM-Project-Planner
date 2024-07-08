
namespace PlannerCRM.Server.Extensions;

public static class DbSeederExtension
{
    public static async Task SeedDataAsync(this IApplicationBuilder app) {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (context.Users.Any()) {
            return;
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<EmployeeRole>>();

        await AddRolesAsync(roleManager);
        await AddAccountManagerAsync(userManager);

        await context.SaveChangesAsync();
    }

    private static async Task AddAccountManagerAsync(UserManager<Employee> userManager) {
        var accountManager = new Employee {
            Id = AccountManagerData.Id,
            Email = AccountManagerData.EMAIL,
            EmailConfirmed = true,
            UserName = AccountManagerData.EMAIL,
            Password = AccountManagerData.PASSWORD,
            StartDate = DateTime.Now,
            BirthDay = DateTime.Now,
            NumericCode = AccountManagerData.NUMERIC_CODE,
            CurrentHourlyRate = 10,
            PhoneNumber = AccountManagerData.PHONE_NUMBER,
            NormalizedEmail = AccountManagerData.EMAIL.ToUpper(),
            FirstName = AccountManagerData.FIRST_NAME,
            LastName = AccountManagerData.LAST_NAME,
            FullName = AccountManagerData.FULL_NAME,
        };

        await userManager.CreateAsync(accountManager, AccountManagerData.PASSWORD);
        await userManager.AddToRoleAsync(accountManager, nameof(Roles.ACCOUNT_MANAGER));
    }

    private static async Task AddRolesAsync(RoleManager<EmployeeRole> roleManager) {
        await roleManager.CreateAsync(
            new EmployeeRole {
                Name = nameof(Roles.ACCOUNT_MANAGER)
            }
        );

        foreach (var role in Enum.GetValues(typeof(Roles)))
        {
            await roleManager.CreateAsync(
                new EmployeeRole {
                    Name = role.ToString()
                }
            );
        }
    }
}

class AccountManagerData
{
    public const int Id = 1;
    public const string EMAIL = "account.manager@gmail.com";
    public const string PASSWORD = "Qwerty123";
    public const string FULL_NAME = "Account Manager";
    public const string FIRST_NAME = "Account";
    public const string LAST_NAME = "Manager";
    public const string NUMERIC_CODE = "CCMNGR01S10D987K";
    public const string PHONE_NUMBER = "3519115581";
}