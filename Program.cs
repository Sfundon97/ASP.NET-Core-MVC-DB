using ASPNETCore_DB.Data;
using ASPNETCore_DB.Interfaces;
using ASPNETCore_DB.Models;
using ASPNETCore_DB.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SQLiteDBContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<LoginDBContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LoginDBContext>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IExceptionFilter, CustomExceptionFilter>();
builder.Services.AddScoped<IDBInitializer, DBInitializerRepo>();
builder.Services.AddScoped<IStudent, StudentRepo>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthentication();;
app.UseAuthorization();

app.UseStaticFiles();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Student" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}




app.Run();
