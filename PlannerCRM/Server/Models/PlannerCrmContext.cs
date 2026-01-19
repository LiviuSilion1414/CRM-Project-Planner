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

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuRole> MenuRoles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<WorkOrder> WorkOrders { get; set; }

    public virtual DbSet<WorkTime> WorkTimes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Activtities");

            entity.HasIndex(e => e.Id, "IX_Activtities");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())", "DF_Activities_Id");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.HexColor).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkIdWorkOrderNavigation).WithMany(p => p.Activities)
                .HasForeignKey(d => d.FkIdWorkOrder)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_WorkOrders_Activities");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())", "DF_Employees_Id");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.LastSeen).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Surname).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<EmployeeActivity>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())", "DF_EmployeeActivities_Id");

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
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())", "DF_EmployeesRoles_Id");

            entity.HasOne(d => d.FkIdEmployeeNavigation).WithMany(p => p.EmployeesRoles)
                .HasForeignKey(d => d.FkIdEmployee)
                .HasConstraintName("FK_EmployeesRoles_Employees");

            entity.HasOne(d => d.FkIdRoleNavigation).WithMany(p => p.EmployeesRoles)
                .HasForeignKey(d => d.FkIdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeesRoles_Roles");
        });

        modelBuilder.Entity<FirmClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FirmClient");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())", "DF_FirmClients_Id");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FiscalCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.VatNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menu");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())", "DF_Menu_id")
                .HasColumnName("id");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .HasColumnName("icon");
            entity.Property(e => e.IsDropdown).HasColumnName("isDropdown");
            entity.Property(e => e.Path)
                .HasMaxLength(50)
                .HasColumnName("path");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<MenuRole>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())", "DF_MenuRoles_id")
                .HasColumnName("id");
            entity.Property(e => e.FkIdMenu).HasColumnName("fkIdMenu");
            entity.Property(e => e.FkIdRole).HasColumnName("fkIdRole");

            entity.HasOne(d => d.FkIdMenuNavigation).WithMany(p => p.MenuRoles)
                .HasForeignKey(d => d.FkIdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuRoles_Menu");

            entity.HasOne(d => d.FkIdRoleNavigation).WithMany(p => p.MenuRoles)
                .HasForeignKey(d => d.FkIdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuRoles_Roles");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())", "DF_Roles_Id");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())", "DF_Settings_id")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsAdvanced).HasColumnName("isAdvanced");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())", "DF_WorkOrders_Id");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkIdFirmClientNavigation).WithMany(p => p.WorkOrders)
                .HasForeignKey(d => d.FkIdFirmClient)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_WorkOrders_FirmClients");
        });

        modelBuilder.Entity<WorkTime>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())", "DF_WorkTimes_Id");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkIdActivityNavigation).WithMany(p => p.WorkTimes)
                .HasForeignKey(d => d.FkIdActivity)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Activities_WorkTimes");

            entity.HasOne(d => d.FkIdEmployeeNavigation).WithMany(p => p.WorkTimes)
                .HasForeignKey(d => d.FkIdEmployee)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_WorkTimes_Employees");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
