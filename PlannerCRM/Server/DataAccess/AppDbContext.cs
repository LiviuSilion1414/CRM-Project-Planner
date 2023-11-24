namespace PlannerCRM.Server.DataAccess;

public class AppDbContext: IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Employee>()
            .Property(em => em.Id)
            .ValueGeneratedOnAdd();

        modelBuilder
            .Entity<EmployeeSalary>()
            .Property(em => em.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(e => e.Employee)
            .WithMany(ea => ea.EmployeeActivity)
            .HasForeignKey(ei => ei.EmployeeId);

        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(a => a.Activity)
            .WithMany(ea => ea.EmployeeActivity)
            .HasForeignKey(ai => ai.ActivityId);
    }
    
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<FirmClient> Clients { get; set; }
    public DbSet<EmployeeActivity> EmployeeActivity { get; set; }
    public DbSet<WorkTimeRecord> WorkTimeRecords { get; set; }
    public DbSet<WorkOrderCost> WorkOrderCosts { get; set; }
    public DbSet<ClientWorkOrder> ClientWorkOrders { get; set; }
}