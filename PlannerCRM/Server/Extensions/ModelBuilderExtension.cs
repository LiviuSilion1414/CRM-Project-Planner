using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PlannerCRM.Server.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyUtcDateTimeConversion(this ModelBuilder modelBuilder)
    {
        var utcConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(), //when saving
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc) //when reading
        );

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var dateTimeProperties = entityType.ClrType
                .GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime));

            foreach (var property in dateTimeProperties)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(property.Name)
                    .HasConversion(utcConverter);
            }
        }
    }

    public static void ConfigureEnums(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Roles>();
    }

    public static void ConfigureRelationships(this ModelBuilder modelBuilder)
    {
        ConfigurePrimaryKeys(modelBuilder);
        ConfigureFirmClientWorkOrders(modelBuilder);
        ConfigureWorkOrderActivities(modelBuilder);
        ConfigureEmployeeActivities(modelBuilder);
        ConfigureEmployeesRoles(modelBuilder);
    }

    private static void ConfigurePrimaryKeys(ModelBuilder modelBuilder)
    {
        #region Activity
        modelBuilder.Entity<Activity>()
            .HasKey(x=> x.Id);

        modelBuilder.Entity<Activity>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region Employee
        modelBuilder.Entity<Employee>()
            .HasKey(x=> x.Id);

        modelBuilder.Entity<Employee>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region FirmClient
        modelBuilder.Entity<FirmClient>()
            .HasKey(x=> x.Id);

        modelBuilder.Entity<FirmClient>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region Role
        modelBuilder.Entity<Role>()
            .HasKey(x=> x.Id);

        modelBuilder.Entity<Role>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region WorkOrder
        modelBuilder.Entity<WorkOrder>()
            .HasKey(x=> x.Id);

        modelBuilder.Entity<WorkOrder>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region ClientWorkOrder
        modelBuilder.Entity<ClientWorkOrder>()
            .HasKey(x=> x.Id);

        modelBuilder.Entity<ClientWorkOrder>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region EmployeeActivity
        modelBuilder.Entity<EmployeeActivity>()
            .HasKey(x=> x.Id);

        modelBuilder.Entity<EmployeeActivity>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region EmployeeRole
        modelBuilder.Entity<EmployeeRole>()
            .HasKey(x=> x.Id);

        modelBuilder.Entity<EmployeeRole>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion

        #region WorkOrderActivity
        modelBuilder.Entity<WorkOrderActivity>()
            .HasKey(x=> x.Id);

        modelBuilder.Entity<WorkOrderActivity>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        #endregion
    }

    private static void ConfigureFirmClientWorkOrders(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FirmClient>()
            .HasMany(c => c.WorkOrders)
            .WithOne(w => w.FirmClient)
            .HasForeignKey(w => w.FirmClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureWorkOrderActivities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkOrder>()
            .HasMany(w => w.Activities)
            .WithOne(a => a.WorkOrder)
            .HasForeignKey(a => a.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureEmployeeActivities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(ea => ea.Employee)
            .WithMany(e => e.EmployeeActivities)
            .HasForeignKey(ea => ea.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmployeeActivity>()
            .HasOne(ea => ea.Activity)
            .WithMany(a => a.EmployeeActivities)
            .HasForeignKey(ea => ea.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureEmployeesRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeRole>()
            .HasOne(er => er.Employee)
            .WithMany(e => e.EmployeeRoles)
            .HasForeignKey(er => er.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmployeeRole>()
            .HasOne(er => er.Role)
            .WithMany(e => e.EmployeeRoles)
            .HasForeignKey(er => er.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}