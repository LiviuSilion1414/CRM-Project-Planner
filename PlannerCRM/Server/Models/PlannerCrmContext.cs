using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PlannerCRM.Server.Models;

public partial class PlannerCrmContext : DbContext
{
    public PlannerCrmContext()
    {
    }

    public PlannerCrmContext(DbContextOptions<PlannerCrmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivitiesWorkTime> ActivitiesWorkTimes { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeActivity> EmployeeActivities { get; set; }

    public virtual DbSet<EmployeeWorkTime> EmployeeWorkTimes { get; set; }

    public virtual DbSet<EmployeesRole> EmployeesRoles { get; set; }

    public virtual DbSet<FirmClient> FirmClients { get; set; }

    public virtual DbSet<FirmClientsWorkOrder> FirmClientsWorkOrders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<WorkOrder> WorkOrders { get; set; }

    public virtual DbSet<WorkOrdersActivity> WorkOrdersActivities { get; set; }

    public virtual DbSet<WorkTime> WorkTimes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivitiesWorkTime>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.FkIdEmployeeNavigation).WithMany(p => p.ActivitiesWorkTimes)
                .HasForeignKey(d => d.FkIdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivitiesWorkTimes_Employees");

            entity.HasOne(d => d.FkIdFirmClientNavigation).WithMany(p => p.ActivitiesWorkTimes)
                .HasForeignKey(d => d.FkIdFirmClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivitiesWorkTimes_FirmClients");

            entity.HasOne(d => d.FkIdWorkTimeNavigation).WithMany(p => p.ActivitiesWorkTimes)
                .HasForeignKey(d => d.FkIdWorkTime)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivitiesWorkTimes_WorkTimes");
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Activtities");

            entity.HasIndex(e => e.Id, "IX_Activtities");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.FkIdFirmClientNavigation).WithMany(p => p.Activities)
                .HasForeignKey(d => d.FkIdFirmClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activities_FirmClients");

            entity.HasOne(d => d.FkIdWorkOrderNavigation).WithMany(p => p.Activities)
                .HasForeignKey(d => d.FkIdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activities_WorkOrders");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.Surname)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<EmployeeActivity>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.FkIdFirmClientNavigation).WithMany(p => p.EmployeeActivities)
                .HasForeignKey(d => d.FkIdFirmClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeActivities_FirmClients");
        });

        modelBuilder.Entity<EmployeeWorkTime>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.FkIdEmployeeNavigation).WithMany(p => p.EmployeeWorkTimes)
                .HasForeignKey(d => d.FkIdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeWorkTimes_Employees");

            entity.HasOne(d => d.FkIdFirmClientNavigation).WithMany(p => p.EmployeeWorkTimes)
                .HasForeignKey(d => d.FkIdFirmClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeWorkTimes_FirmClients");

            entity.HasOne(d => d.FkIdWorkTimeNavigation).WithMany(p => p.EmployeeWorkTimes)
                .HasForeignKey(d => d.FkIdWorkTime)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeWorkTimes_WorkTimes");
        });

        modelBuilder.Entity<EmployeesRole>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.FkIdEmployeeNavigation).WithMany(p => p.EmployeesRoles)
                .HasForeignKey(d => d.FkIdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeesRoles_Employees");

            entity.HasOne(d => d.FkIdRoleNavigation).WithMany(p => p.EmployeesRoles)
                .HasForeignKey(d => d.FkIdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeesRoles_Roles");
        });

        modelBuilder.Entity<FirmClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FirmClient");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.VatNumber)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<FirmClientsWorkOrder>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.FkIdFirmClientNavigation).WithMany(p => p.FirmClientsWorkOrders)
                .HasForeignKey(d => d.FkIdFirmClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FirmClientsWorkOrders_FirmClients");

            entity.HasOne(d => d.FkIdWorkOrderNavigation).WithMany(p => p.FirmClientsWorkOrders)
                .HasForeignKey(d => d.FkIdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FirmClientsWorkOrders_WorkOrders");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(2000);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.FkIdFirmClientNavigation).WithMany(p => p.WorkOrders)
                .HasForeignKey(d => d.FkIdFirmClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkOrders_FirmClients");
        });

        modelBuilder.Entity<WorkOrdersActivity>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.FkIdFirmClientNavigation).WithMany(p => p.WorkOrdersActivities)
                .HasForeignKey(d => d.FkIdFirmClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkOrdersActivities_FirmClients");
        });

        modelBuilder.Entity<WorkTime>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.FkIdActivityNavigation).WithMany(p => p.WorkTimes)
                .HasForeignKey(d => d.FkIdActivity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activities_WorkTimes");

            entity.HasOne(d => d.FkIdWorkOrderNavigation).WithMany(p => p.WorkTimes)
                .HasForeignKey(d => d.FkIdWorkOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkOrders_WorkTimes");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
