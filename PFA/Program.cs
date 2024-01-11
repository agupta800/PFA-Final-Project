using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PFA.Data;
using PFA.Repository.Interface;
using PFA.Repository.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationContext>(option =>
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("conn"));

});

builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender,EmailSender>();
builder.Services.ConfigureApplicationCookie(option =>
    {
        option.AccessDeniedPath = "/Account/Login";
        option.LogoutPath = "/Account/Login";
        option.LogoutPath= "/Account/Logout";
        option.Cookie.Name = "PFA";
        option.ExpireTimeSpan = TimeSpan.FromDays(30);
        option.SlidingExpiration = true;

    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
