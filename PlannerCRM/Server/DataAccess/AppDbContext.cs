using PlannerCRM.Server.Extensions;

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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConvertToUtcDateTime();

        modelBuilder.Entity<EmployeeSalary>()
            .HasIndex(ems => ems.EmployeeId)
            .IsUnique();
        
        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(e => e.Employee)
            .WithMany(ea => ea.EmployeeActivity)
            .HasForeignKey(ei => ei.EmployeeId);

        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(a => a.Activity)
            .WithMany(ea => ea.EmployeeActivity)
            .HasForeignKey(ai => ai.ActivityId);

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