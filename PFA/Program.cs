using Arebis.Core.AspNet.Mvc.Localization;
using Arebis.Core.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PFA.Data;
using PFA.JobModel;
using PFA.Localize;
using PFA.Repository.Interface;
using PFA.Repository.Service;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<JobPostDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

builder.Services.AddDbContext<ApplicationContext>(option =>
    {
        option.UseSqlServer(builder.Configuration.GetConnectionString("conn"));

    });


builder.Services.AddDbContext<PFA.Data.Localize.LocalizeDbContext>(optionsAction: options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("conn")));
builder.Services.AddTransient<ILocalizationSource, DbContextLocalizationSource>();


builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services
    .AddLocalizationFromSource(builder.Configuration, options =>
    {
        options.Domains = new string[] { "PFA" };
        options.CacheFileName = "LocalizationCache.json";
        options.UseOnlyReviewedLocalizationValues = false;
        options.AllowLocalizeFormat = true;
    });

builder.Services.AddModelBindingLocalizationFromSource();

builder.Services.AddControllers(config =>
{
    config.Filters.Add<ModelStateLocalizationFilter>();
});


builder.Services.ConfigureApplicationCookie(option =>
    {
        option.AccessDeniedPath = "/Account/AccessDenied";
        option.LogoutPath = "/Account/Logout";

        option.AccessDeniedPath = "/Account/Login";
        option.LogoutPath = "/Account/Login";
        option.LogoutPath = "/Account/Logout";
        option.Cookie.Name = "PFA";
        option.ExpireTimeSpan = TimeSpan.FromDays(30);
        option.SlidingExpiration = true;

    });


// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalizationFromSource();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var supportedCultures =
    new[] { "en", "ne", "he" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
