using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PlannerCRM.Server.Models2;

public partial class DevPlannerCrmContext : DbContext
{
    public DevPlannerCrmContext()
    {
    }

    public DevPlannerCrmContext(DbContextOptions<DevPlannerCrmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<ClientWorkOrder> ClientWorkOrders { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeActivity> EmployeeActivities { get; set; }

    public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }

    public virtual DbSet<EmployeeWorkTime> EmployeeWorkTimes { get; set; }

    public virtual DbSet<FirmClient> FirmClients { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<WorkOrder> WorkOrders { get; set; }

    public virtual DbSet<WorkOrderActivity> WorkOrderActivities { get; set; }

    public virtual DbSet<WorkTime> WorkTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("roles", new[] { "account_manager", "operation_manager", "project_manager", "senior_developer", "junior_developer" });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasIndex(e => e.WorkOrderId, "IX_Activities_WorkOrderId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.Activities).HasForeignKey(d => d.WorkOrderId);

            entity.HasMany(d => d.Employees).WithMany(p => p.Activities)
                .UsingEntity<Dictionary<string, object>>(
                    "ActivityEmployee",
                    r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeesId"),
                    l => l.HasOne<Activity>().WithMany().HasForeignKey("ActivitiesId"),
                    j =>
                    {
                        j.HasKey("ActivitiesId", "EmployeesId");
                        j.ToTable("ActivityEmployee");
                        j.HasIndex(new[] { "EmployeesId" }, "IX_ActivityEmployee_EmployeesId");
                    });
        });

        modelBuilder.Entity<ClientWorkOrder>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsRemoveable).HasDefaultValue(true);
        });

        modelBuilder.Entity<EmployeeActivity>(entity =>
        {
            entity.HasIndex(e => e.ActivityId, "IX_EmployeeActivities_ActivityId");

            entity.HasIndex(e => e.EmployeeId, "IX_EmployeeActivities_EmployeeId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Activity).WithMany(p => p.EmployeeActivities).HasForeignKey(d => d.ActivityId);

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeActivities).HasForeignKey(d => d.EmployeeId);
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.HasIndex(e => e.EmployeeId, "IX_EmployeeRoles_EmployeeId");

            entity.HasIndex(e => e.RoleId, "IX_EmployeeRoles_RoleId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeRoles).HasForeignKey(d => d.EmployeeId);

            entity.HasOne(d => d.Role).WithMany(p => p.EmployeeRoles).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<EmployeeWorkTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EmployeeWorkTimes_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Activity).WithMany(p => p.EmployeeWorkTimes)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ActivityId");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeWorkTimes)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmployeeId");

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.EmployeeWorkTimes)
                .HasForeignKey(d => d.WorkOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkOrderId");

            entity.HasOne(d => d.WorkTime).WithMany(p => p.EmployeeWorkTimes)
                .HasForeignKey(d => d.WorkTimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkTimeId");
        });

        modelBuilder.Entity<FirmClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Clients");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsRemoveable).HasDefaultValue(true);
        });

        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.HasIndex(e => e.FirmClientId, "IX_WorkOrders_FirmClientId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.FirmClient).WithMany(p => p.WorkOrders)
                .HasForeignKey(d => d.FirmClientId)
                .HasConstraintName("FK_WorkOrders_Clients_FirmClientId");
        });

        modelBuilder.Entity<WorkOrderActivity>(entity =>
        {
            entity.HasIndex(e => e.ActivityId, "IX_WorkOrderActivities_ActivityId");

            entity.HasIndex(e => e.WorkOrderId, "IX_WorkOrderActivities_WorkOrderId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Activity).WithMany(p => p.WorkOrderActivities).HasForeignKey(d => d.ActivityId);

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.WorkOrderActivities).HasForeignKey(d => d.WorkOrderId);
        });

        modelBuilder.Entity<WorkTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("WorkTime_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).IsRequired();

            entity.HasOne(d => d.Activity).WithMany(p => p.WorkTimes)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ActivityId");

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkTimes)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EmployeeId");

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.WorkTimes)
                .HasForeignKey(d => d.WorkOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WorkOrderId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
