namespace PlannerCRM.Server.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : 
    IdentityDbContext<Employee, EmployeeRole, int>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<IdentityUser>().ToTable("Employees");

        modelBuilder.Entity<IdentityRole>().ToTable("EmployeeRoles");

        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("EmployeeUserRole");

        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("EmployeeClaim");

        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("EmployeeLogin");

        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("EmployeeRoleClaim");

        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("EmployeeToken");

        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(e => e.Employee)
            .WithMany(ea => ea.EmployeeActivity)
            .HasForeignKey(ei => ei.EmployeeId);

        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(a => a.Activity)
            .WithMany(ea => ea.EmployeeActivity)
            .HasForeignKey(ai => ai.ActivityId);

        modelBuilder.Entity<WorkTimeRecord>()
            .HasOne(wtr => wtr.Employee)
            .WithMany(em => em.WorkTimeRecords)
            .HasForeignKey(wtr => wtr.EmployeeId);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeRole> EmployeeRoles { get; set; }
    public DbSet<EmployeeActivity> EmployeeActivity { get; set; }
    public DbSet<EmployeeProfilePicture> ProfilePictures { get; set; }

    public DbSet<FirmClient> Clients { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<ClientWorkOrder> ClientWorkOrders { get; set; }
    public DbSet<WorkOrderCost> WorkOrderCosts { get; set; }
    public DbSet<WorkTimeRecord> WorkTimeRecords { get; set; }
}