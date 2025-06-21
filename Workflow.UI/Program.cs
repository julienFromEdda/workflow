using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Workflow.Application.Seeders;
using Workflow.Application.Services;
using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces;
using Workflow.Persistence;
using Workflow.UI.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<WFContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WFContext>();

builder.Services.AddIdentity<Utilisateur, Role>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<WFContext>()
    .AddTokenProvider<DataProtectorTokenProvider<Utilisateur>>(TokenOptions.DefaultProvider)
    .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

builder.Services.AddScoped<InitialDataSeeder>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IDossierService, DossierService>();
builder.Services.AddScoped<ISeanceService, SeanceService>();
builder.Services.AddScoped<IPOJService, POJService>();
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IActionDossierService, ActionDossierService>();
builder.Services.AddScoped<IServiceService, ServiceService>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPermissionPolicies();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetRequiredService<InitialDataSeeder>();
    await seeder.SeedDataAsync();

    var roleManager = services.GetRequiredService<RoleManager<Role>>();
    await PermissionSeeder.SeedRolePermissionsAsync(roleManager);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
