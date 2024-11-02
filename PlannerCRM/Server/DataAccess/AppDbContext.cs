using PlannerCRM.Server.Extensions;
using PlannerCRM.Server.Models.Entities;

namespace PlannerCRM.Server.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) :
    IdentityDbContext<
        Employee, EmployeeRole, int,
        EmployeeUserClaim, EmployeeUserRole, EmployeeUserLogin,
        EmployeeRoleClaim, EmployeeUserToken>
    (options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureEnums();
        modelBuilder.ConfigureRelationships();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<EmployeeActivity> EmployeeActivities { get; set; }
    public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }

    public DbSet<FirmClient> Clients { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<ClientWorkOrder> ClientWorkOrders { get; set; }
    public DbSet<WorkOrderCost> WorkOrderCosts { get; set; }
    public DbSet<WorkTimeRecord> WorkTimeRecords { get; set; }
}