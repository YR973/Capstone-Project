
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.SqlServer;
using Test;
using Python.Runtime;

var builder = WebApplication.CreateBuilder(args);
//Runtime.PythonDLL = @"C:\inetpub\wwwroot\Python311\python311.dll";
Runtime.PythonDLL = @"C:\Users\yfroo\Documents\GitHub\hello\Capstone-Project\Test\Python311\python311.dll";


builder.Services.AddDbContext<ProductContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddDbContext<UserContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddDbContext<OrderContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add DbContext service


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

app.Run();