using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.Filters;
using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    o.Password.RequireDigit = true; o.Password.RequiredLength = 8;
    o.Password.RequireNonAlphanumeric = false; o.Password.RequireUppercase = false;
    o.Lockout.MaxFailedAccessAttempts = 5;
    o.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication()
    .AddGoogle(o =>
    {
        o.ClientId     = builder.Configuration["Authentication:Google:ClientId"] ?? "";
        o.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
    });

builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath = "/Account/Login"; o.LogoutPath = "/Account/Logout";
    o.AccessDeniedPath = "/Account/AccessDenied";
    o.ExpireTimeSpan = TimeSpan.FromHours(8); o.SlidingExpiration = true;
});

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("AdminOnly",       p => p.RequireRole("Admin"));
    o.AddPolicy("AdminOrWarehouse",p => p.RequireRole("Admin","WarehouseStaff"));
    o.AddPolicy("SalesOrAbove",    p => p.RequireRole("Admin","WarehouseStaff","SalesStaff"));
});

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IAuditService,      AuditService>();
builder.Services.AddScoped<IEmailService,      EmailService>();
builder.Services.AddScoped<ICategoryService,   CategoryService>();
builder.Services.AddScoped<ISupplierService,   SupplierService>();
builder.Services.AddScoped<IProductService,    ProductService>();
builder.Services.AddScoped<IImportService,     ImportService>();
builder.Services.AddScoped<IExportService,     ExportService>();
builder.Services.AddScoped<ISePayService,      SePayService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<DynamicPermissionFilter>();

builder.Services.AddControllersWithViews();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.HttpOnly = true; o.Cookie.IsEssential = true;
});
builder.Services.AddHttpClient();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    await SeedAsync(scope.ServiceProvider);

if (!app.Environment.IsDevelopment())
{ app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Store}/{action=Index}/{id?}");

app.Run();

static async Task SeedAsync(IServiceProvider svc)
{
    var rm = svc.GetRequiredService<RoleManager<IdentityRole>>();
    var um = svc.GetRequiredService<UserManager<ApplicationUser>>();
    foreach (var r in new[]{"Admin","WarehouseStaff","SalesStaff"})
        if (!await rm.RoleExistsAsync(r)) await rm.CreateAsync(new IdentityRole(r));

    const string email = "vuongducanh1608@gmail.com";
    var admin = await um.FindByEmailAsync(email);
    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = email, Email = email,
            FullName = "Vuong Doan Duc Anh",
            EmailConfirmed = true, IsActive = true
        };
        await um.CreateAsync(admin, "Admin@12345");
        await um.AddToRoleAsync(admin, "Admin");
    }
}
