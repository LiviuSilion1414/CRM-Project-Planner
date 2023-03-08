using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlannerCRM.Server.Models;
using PlannerCRM.Server.DataAccess;
using PlannerCRM.Server.Services.Interfaces;
using PlannerCRM.Server.Services.ConcreteClasses;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration
            .GetConnectionString("ConnString")
                ?? throw new InvalidOperationException("ConnString not found!"))
);

builder.Services.AddScoped<IdentityDbContext, AppDbContext>();

builder.Services.AddScoped<IRepository<WorkOrder>, WorkOrderRepository>();
builder.Services.AddScoped<IRepository<WorkTimeRecord>, WorkTimeRecordRepository>();
builder.Services.AddScoped<IWorkTimeRecordRepository, WorkTimeRecordRepository>();
builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();
builder.Services.AddScoped<ICalculateService, CalculateService>();
builder.Services.AddScoped<IRepository<Activity>, ActivityRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
} else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
