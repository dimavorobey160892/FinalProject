using Microsoft.EntityFrameworkCore;
using AutoStoreLib;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyFinalProject.Services;
using MyFinalProject.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/User/Login");
builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("autoStoreDb") ?? throw new InvalidOperationException("Connection string 'autoStoreDb' not found.");
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
