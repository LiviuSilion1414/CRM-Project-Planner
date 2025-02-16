namespace PlannerCRM.Server.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        modelBuilder.ApplyUtcDateTimeConversion();
        modelBuilder.ConfigureEnums();
        modelBuilder.ConfigureRelationships();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Activity> Activities { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<FirmClient> Clients { get; set; }
    public DbSet<Salary> Salaries { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<WorkOrderCost> WorkOrderCosts { get; set; }
    public DbSet<WorkTime> WorkTimes { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DbSet<EmployeeLoginData> EmployeeLoginData { get; set; }
    public DbSet<ActivityWorkTime> ActivityWorkTimes { get; set; }
    public DbSet<ClientWorkOrderCost> ClientWorkOrderCosts { get; set; }
    public DbSet<ClientWorkOrder> ClientWorkOrders { get; set; }
    public DbSet<EmployeeActivity> EmployeeActivities { get; set; }
    public DbSet<EmployeeRole> EmployeeRoles { get; set; }
    public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
    public DbSet<EmployeeWorkTime> EmployeeWorkTimes { get; set; }
    public DbSet<WorkOrderActivity> WorkOrderActivities { get; set; }
}