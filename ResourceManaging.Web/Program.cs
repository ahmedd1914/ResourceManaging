using ResourceManaging.Repository;
using ResourceManaging.Repository.Interfaces.Resource;
using ResourceManaging.Repository.Interfaces.Reservation;
using ResourceManaging.Repository.Interfaces.Employee;
using ResourceManaging.Repository.Implementation.Resources;
using ResourceManaging.Repository.Implementation.Reservations;
using ResourceManaging.Repository.Implementation.Employees;
using ResourceManaging.Services.Interfaces;
using ResourceManaging.Services.Interfaces.Resource;
using ResourceManaging.Services.Interfaces.Reservation;
using ResourceManaging.Services.Interfaces.Authentication;
using ResourceManaging.Services.Interfaces.Employee;
using ResourceManaging.Services.Implementations;
using ResourceManaging.Services.Implementations.Resource;
using ResourceManaging.Services.Implementations.Reservation;
using ResourceManaging.Services.Implementations.Authentication;
using ResourceManaging.Services.Implementations.Employee;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
ConnectionFactory.SetConnectionString(connectionString);

// Register Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(sp => new EmployeeRepository(connectionString));
builder.Services.AddScoped<IResourceRepository, ResourceRepository>(sp => new ResourceRepository(connectionString));
builder.Services.AddScoped<IResourceTypeRepository, ResourceTypeRepository>(sp => new ResourceTypeRepository(connectionString));
builder.Services.AddScoped<IReservationRepository, ReservationRepository>(sp => new ReservationRepository(connectionString));
builder.Services.AddScoped<IReservationResourceRepository, ReservationResourceRepository>(sp => new ReservationResourceRepository(connectionString));
builder.Services.AddScoped<IReservationNotificationRepository, ReservationNotificationRepository>(sp => new ReservationNotificationRepository(connectionString));

// Register Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IResourceTypeService, ResourceTypeService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IReservationResourceService, ReservationResourceService>();
builder.Services.AddScoped<IReservationNotificationService, ReservationNotificationService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
