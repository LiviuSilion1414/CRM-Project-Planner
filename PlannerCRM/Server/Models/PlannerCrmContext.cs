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

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeActivity> EmployeeActivities { get; set; }

    public virtual DbSet<EmployeesRole> EmployeesRoles { get; set; }

    public virtual DbSet<FirmClient> FirmClients { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<WorkOrder> WorkOrders { get; set; }

    public virtual DbSet<WorkTime> WorkTimes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Activtities");

            entity.HasIndex(e => e.Id, "IX_Activtities");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkIdWorkOrderNavigation).WithMany(p => p.Activities)
                .HasForeignKey(d => d.FkIdWorkOrder)
                .HasConstraintName("FK_WorkOrders_Activities");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.LastSeen).HasColumnType("datetime");
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
                .HasMaxLength(50);
        });

        modelBuilder.Entity<EmployeeActivity>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.FkIdActivityNavigation).WithMany(p => p.EmployeeActivities)
                .HasForeignKey(d => d.FkIdActivity)
                .HasConstraintName("FK_EmployeeActivities_Activities");

            entity.HasOne(d => d.FkIdEmployeeNavigation).WithMany(p => p.EmployeeActivities)
                .HasForeignKey(d => d.FkIdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeActivities_Employees");
        });

        modelBuilder.Entity<EmployeesRole>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

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
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.VatNumber)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(2000);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkIdFirmClientNavigation).WithMany(p => p.WorkOrders)
                .HasForeignKey(d => d.FkIdFirmClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkOrders_FirmClients");
        });

        modelBuilder.Entity<WorkTime>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkIdActivityNavigation).WithMany(p => p.WorkTimes)
                .HasForeignKey(d => d.FkIdActivity)
                .HasConstraintName("FK_Activities_WorkTimes");

            entity.HasOne(d => d.FkIdEmployeeNavigation).WithMany(p => p.WorkTimes)
                .HasForeignKey(d => d.FkIdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkTimes_Employees");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
