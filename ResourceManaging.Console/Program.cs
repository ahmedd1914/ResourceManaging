//using ResourceManaging.Models;
//using ResourceManaging.Services.Interfaces.Employee;
//using ResourceManaging.Services.Interfaces.Resource;
//using ResourceManaging.Services.Interfaces.Reservation;
//using ResourceManaging.Services.Interfaces.Authentication;
//using ResourceManaging.Services.Implementations.Employee;
//using ResourceManaging.Services.Implementations.Resource;
//using ResourceManaging.Services.Implementations.Reservation;
//using ResourceManaging.Services.Implementations.Authentication;
//using ResourceManaging.Repository.Implementation.Employees;
//using ResourceManaging.Repository.Implementation.Resources;
//using ResourceManaging.Repository.Implementation.Reservations;
//using ResourceManaging.Repository.Interfaces.Employee;
//using ResourceManaging.Repository.Interfaces.Resource;
//using ResourceManaging.Repository.Interfaces.Reservation;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using ResourceManaging.Repository;
//using ResourceManaging.Services.DTOs.Employee;
//using ResourceManaging.Services.DTOs.Resource;
//using ResourceManaging.Services.DTOs.Reservation;
//using ResourceManaging.Services.DTOs.Authentication;

//class Program
//{
//    private static readonly Random _random = new Random();
//    private static List<int> _createdEmployeeIds = new List<int>();
//    private static List<int> _createdResourceTypeIds = new List<int>();
//    private static List<int> _createdResourceIds = new List<int>();
//    private static List<int> _createdReservationIds = new List<int>();

//    static async Task Main(string[] args)
//    {
//        Console.WriteLine("Resource Management System - Service Tests");
//        Console.WriteLine("----------------------------------------");

//        var services = ConfigureServices();
//        var serviceProvider = services.BuildServiceProvider();

//        try
//        {
//            await TestAuthenticationService(serviceProvider);
//            await TestEmployeeService(serviceProvider);
//            await TestResourceService(serviceProvider);
//            await TestReservationService(serviceProvider);
//            await TestReservationResourceService(serviceProvider);
//            await TestReservationNotificationService(serviceProvider);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"\nError occurred: {ex.Message}");
//            Console.WriteLine($"Stack trace: {ex.StackTrace}");
//        }
//        finally
//        {
//            await CleanupTestData(serviceProvider);
//        }

//        Console.WriteLine("\nPress any key to exit...");
//        Console.ReadKey();
//    }

//    static IServiceCollection ConfigureServices()
//    {
//        var services = new ServiceCollection();
//        var configuration = new ConfigurationBuilder()
//            .SetBasePath(Directory.GetCurrentDirectory())
//            .AddJsonFile("appsettings.json")
//            .Build();

//        string connectionString = configuration.GetConnectionString("DefaultConnection");
//        ConnectionFactory.SetConnectionString(connectionString);

//        // Register Repositories
//        services.AddScoped<IEmployeeRepository, EmployeeRepository>(sp => new EmployeeRepository(connectionString));
//        services.AddScoped<IResourceRepository, ResourceRepository>(sp => new ResourceRepository(connectionString));
//        services.AddScoped<IResourceTypeRepository, ResourceTypeRepository>(sp => new ResourceTypeRepository(connectionString));
//        services.AddScoped<IReservationRepository, ReservationRepository>(sp => new ReservationRepository(connectionString));
//        services.AddScoped<IReservationResourceRepository, ReservationResourceRepository>(sp => new ReservationResourceRepository(connectionString));
//        services.AddScoped<IReservationNotificationRepository, ReservationNotificationRepository>(sp => new ReservationNotificationRepository(connectionString));

//        // Register Services
//        services.AddScoped<IAuthenticationService, AuthenticationService>();
//        services.AddScoped<IEmployeeService, EmployeeService>();
//        services.AddScoped<IResourceService, ResourceService>();
//        services.AddScoped<IResourceTypeService, ResourceTypeService>();
//        services.AddScoped<IReservationService, ReservationService>();
//        services.AddScoped<IReservationResourceService, ReservationResourceService>();
//        services.AddScoped<IReservationNotificationService, ReservationNotificationService>();

//        return services;
//    }

//    private static string GenerateRandomString(int length)
//    {
//        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
//        return new string(Enumerable.Repeat(chars, length)
//            .Select(s => s[_random.Next(s.Length)]).ToArray());
//    }

//    private static string GenerateRandomEmail()
//    {
//        return $"{GenerateRandomString(8)}@{GenerateRandomString(5)}.com";
//    }

//    static async Task TestAuthenticationService(IServiceProvider serviceProvider)
//    {
//        Console.WriteLine("\n=== Testing Authentication Service ===");
//        var authService = serviceProvider.GetRequiredService<IAuthenticationService>();

//        var username = GenerateRandomString(8);
//        var registerRequest = new RegisterRequest
//        {
//            Username = username,
//            Email = GenerateRandomEmail(),
//            Password = "Test123!",
//            FullName = $"{GenerateRandomString(5)} {GenerateRandomString(8)}",
//            DateOfBirth = DateTime.Now.AddYears(-_random.Next(20, 60))
//        };

//        //var registerResponse = await authService.RegisterAsync(registerRequest);
//        //Console.WriteLine($"Registration {(registerResponse ? "successful" : "failed")}");

//        var loginRequest = new LoginRequest
//        {
//            Username = username,
//            Password = "Test123!"
//        };

//        var loginResponse = await authService.LoginAsync(loginRequest);
//        Console.WriteLine($"Login {(loginResponse.Success ? "successful" : "failed")}: {loginResponse.Message}");
//    }

//    static async Task TestEmployeeService(IServiceProvider serviceProvider)
//    {
//        Console.WriteLine("\n=== Testing Employee Service ===");
//        var employeeService = serviceProvider.GetRequiredService<IEmployeeService>();

//        // Test Create
//        var createRequest = new CreateEmployeeRequest
//        {
//            Username = GenerateRandomString(8),
//            Email = GenerateRandomEmail(),
//            Password = "Pass123!",
//            FullName = $"{GenerateRandomString(5)} {GenerateRandomString(8)}",
//            DateOfBirth = DateTime.Now.AddYears(-_random.Next(20, 60))
//        };

//        var createResponse = await employeeService.CreateEmployeeAsync(createRequest);
//        Console.WriteLine($"Employee creation {(createResponse.Success ? "successful" : "failed")}: {createResponse.Message}");

//        if (createResponse.Success)
//        {
//            _createdEmployeeIds.Add(createResponse.Employee.EmployeeId);

//            // Test Get
//            var getResponse = await employeeService.GetEmployeeByIdAsync(createResponse.Employee.EmployeeId);
//            Console.WriteLine($"Retrieved employee: {getResponse.Employee.FullName}");

//            // Test filtering by username
//            var filterByUsername = new EmployeeFilter { Username = createRequest.Username };
//            var filteredByUsername = await employeeService.GetEmployeesByFilterAsync(filterByUsername);
//            Console.WriteLine($"Employees with username {createRequest.Username}: {filteredByUsername.TotalCount}");

//            // Test filtering by email
//            var filterByEmail = new EmployeeFilter { Email = createRequest.Email };
//            var filteredByEmail = await employeeService.GetEmployeesByFilterAsync(filterByEmail);
//            Console.WriteLine($"Employees with email {createRequest.Email}: {filteredByEmail.TotalCount}");

//            // Test Update
//            var updateRequest = new UpdateEmployeeRequest
//            {
//                EmployeeId = createResponse.Employee.EmployeeId,
//                FullName = $"Updated {GenerateRandomString(8)}",
//                Email = GenerateRandomEmail(),
//                DateOfBirth = DateTime.Now.AddYears(-_random.Next(20, 60))
//            };

//            var updateResponse = await employeeService.UpdateEmployeeAsync(updateRequest);
//            Console.WriteLine($"Employee update {(updateResponse ? "successful" : "failed")}");

//            if (updateResponse)
//            {
//                var updatedEmployeeResponse = await employeeService.GetEmployeeByIdAsync(createResponse.Employee.EmployeeId);
//                Console.WriteLine($"Updated employee: {updatedEmployeeResponse.Employee.FullName}");
//            }

//            // Test List
//            var listResponse = await employeeService.GetEmployeesByFilterAsync(new EmployeeFilter());
//            Console.WriteLine($"Total employees: {listResponse.TotalCount}");
//        }
//    }

//    static async Task TestResourceService(IServiceProvider serviceProvider)
//    {
//        Console.WriteLine("\n=== Testing Resource Service ===");
//        var resourceService = serviceProvider.GetRequiredService<IResourceService>();
//        var resourceTypeService = serviceProvider.GetRequiredService<IResourceTypeService>();

//        // Test Resource Type
//        var typeRequest = new CreateResourceTypeRequest
//        {
//            Name = $"Resource Type {GenerateRandomString(5)}",
//            Description = $"Description for {GenerateRandomString(10)}"
//        };

//        var typeResponse = await resourceTypeService.CreateResourceTypeAsync(typeRequest);
//        Console.WriteLine($"Resource type creation {(typeResponse.Success ? "successful" : "failed")}: {typeResponse.Message}");

//        if (typeResponse.Success)
//        {
//            _createdResourceTypeIds.Add(typeResponse.ResourceType.ResourceTypeId);

//            // Test Resource
//            var createRequest = new CreateResourceRequest
//            {
//                Name = $"Resource {GenerateRandomString(5)}",
//                ResourceTypeId = typeResponse.ResourceType.ResourceTypeId,
//                Capacity = _random.Next(1, 100),
//                IsActive = true
//            };

//            var createResponse = await resourceService.CreateResourceAsync(createRequest);
//            Console.WriteLine($"Resource creation {(createResponse.Success ? "successful" : "failed")}: {createResponse.Message}");

//            if (createResponse.Success)
//            {
//                _createdResourceIds.Add(createResponse.Resource.ResourceId);

//                // Test Get
//                var getResponse = await resourceService.GetResourceByIdAsync(createResponse.Resource.ResourceId);
//                Console.WriteLine($"Retrieved resource: {getResponse.Resource.Name}");

//                // Test filtering by resource type
//                var filterByType = new ResourceFilter();
//                filterByType.AddResourceTypeFilter(typeResponse.ResourceType.ResourceTypeId);
//                var filteredByType = await resourceService.GetResourcesByFilterAsync(filterByType);
//                Console.WriteLine($"Resources with type {typeResponse.ResourceType.ResourceTypeId}: {filteredByType.TotalCount}");

//                // Test filtering by name
//                var filterByName = new ResourceFilter();
//                filterByName.AddNameFilter(createRequest.Name);
//                var filteredByName = await resourceService.GetResourcesByFilterAsync(filterByName);
//                Console.WriteLine($"Resources with name {createRequest.Name}: {filteredByName.TotalCount}");

//                // Test Update
//                var updateRequest = new UpdateResourceRequest
//                {
//                    ResourceId = createResponse.Resource.ResourceId,
//                    ResourceTypeId = typeResponse.ResourceType.ResourceTypeId,
//                    Name = $"Updated {GenerateRandomString(8)}",
//                    Capacity = _random.Next(1, 100),
//                    IsActive = true
//                };

//                var updateResponse = await resourceService.UpdateResourceAsync(updateRequest);
//                Console.WriteLine($"Resource update {(updateResponse ? "successful" : "failed")}");

//                if (updateResponse)
//                {
//                    var getUpdatedResponse = await resourceService.GetResourceByIdAsync(createResponse.Resource.ResourceId);
//                    Console.WriteLine($"Updated resource: {getUpdatedResponse.Resource.Name}");
//                }

//                // Test List
//                var listResponse = await resourceService.GetResourcesByFilterAsync(new ResourceFilter());
//                Console.WriteLine($"Total resources: {listResponse.TotalCount}");
//            }
//        }
//    }

//    static async Task TestReservationService(IServiceProvider serviceProvider)
//    {
//        Console.WriteLine("\n=== Testing Reservation Service ===");
//        var reservationService = serviceProvider.GetRequiredService<IReservationService>();
//        var employeeService = serviceProvider.GetRequiredService<IEmployeeService>();
//        var resourceService = serviceProvider.GetRequiredService<IResourceService>();

//        // Get an employee and resource for the reservation
//        var employees = await employeeService.GetEmployeesByFilterAsync(new EmployeeFilter());
//        var resources = await resourceService.GetResourcesByFilterAsync(new ResourceFilter());

//        if (employees.Employees.Any() && resources.Resources.Any())
//        {
//            var employee = employees.Employees.First();
//            var resource = resources.Resources.First();

//            // Test Create
//            var createRequest = new CreateReservationRequest
//            {
//                ReservorId = employee.EmployeeId,
//                StartTime = DateTime.Now.AddHours(_random.Next(1, 24)),
//                EndTime = DateTime.Now.AddHours(_random.Next(25, 48)),
//                Purpose = $"Meeting {GenerateRandomString(5)}",
//                Participants = _random.Next(1, 20)
//            };

//            var createResponse = await reservationService.CreateReservationAsync(createRequest);
//            Console.WriteLine($"Reservation creation {(createResponse.Success ? "successful" : "failed")}: {createResponse.Message}");

//            if (createResponse.Success)
//            {
//                _createdReservationIds.Add(createResponse.Reservation.ReservationId);

//                // Test Get
//                var getResponse = await reservationService.GetReservationByIdAsync(createResponse.Reservation.ReservationId);
//                Console.WriteLine($"Retrieved reservation: {getResponse.Reservation.Purpose}");

//                // Test filtering by purpose
//                var filterByPurpose = new ReservationFilter();
//                filterByPurpose.AddParameter("Purpose", createRequest.Purpose);
//                var filteredByPurpose = await reservationService.GetReservationsByFilterAsync(filterByPurpose);
//                Console.WriteLine($"Reservations with purpose '{createRequest.Purpose}': {filteredByPurpose.TotalCount}");

//                // Test filtering by participant count
//                var filterByParticipants = new ReservationFilter();
//                filterByParticipants.AddParameter("Participants", createRequest.Participants);
//                var filteredByParticipants = await reservationService.GetReservationsByFilterAsync(filterByParticipants);
//                Console.WriteLine($"Reservations with {createRequest.Participants} participants: {filteredByParticipants.TotalCount}");

//                // Test Update
//                var updateRequest = new UpdateReservationRequest
//                {
//                    ReservationId = createResponse.Reservation.ReservationId,
//                    StartTime = DateTime.Now.AddHours(_random.Next(1, 24)),
//                    EndTime = DateTime.Now.AddHours(_random.Next(25, 48)),
//                    Purpose = $"Updated Meeting {GenerateRandomString(5)}",
//                    Participants = _random.Next(1, 20)
//                };

//                var updateResponse = await reservationService.UpdateReservationAsync(updateRequest);
//                Console.WriteLine($"Reservation update {(updateResponse ? "successful" : "failed")}");

//                if (updateResponse)
//                {
//                    var getUpdatedResponse = await reservationService.GetReservationByIdAsync(createResponse.Reservation.ReservationId);
//                    Console.WriteLine($"Updated reservation: {getUpdatedResponse.Reservation.Purpose}");
//                }

//                // Test List
//                var listResponse = await reservationService.GetReservationsByFilterAsync(new ReservationFilter());
//                Console.WriteLine($"Total reservations: {listResponse.TotalCount}");
//            }
//        }
//    }

//    static async Task TestReservationResourceService(IServiceProvider serviceProvider)
//    {
//        Console.WriteLine("\n=== Testing Reservation Resource Service ===");
//        var reservationResourceService = serviceProvider.GetRequiredService<IReservationResourceService>();

//        if (_createdReservationIds.Any() && _createdResourceIds.Any())
//        {
//            var reservationId = _createdReservationIds.First();
//            var resourceId = _createdResourceIds.First();

//            // Test Add Resource to Reservation
//            var addRequest = new AddResourceToReservationRequest
//            {
//                ReservationId = reservationId,
//                ResourceId = resourceId
//            };

//            var addResponse = await reservationResourceService.AddResourceToReservationAsync(addRequest);
//            Console.WriteLine($"Add resource to reservation {(addResponse.Success ? "successful" : "failed")}: {addResponse.Message}");

//            // Test Get Resources by Reservation
//            var resourcesResponse = await reservationResourceService.GetResourcesByReservationIdAsync(reservationId);
//            Console.WriteLine($"Resources in reservation: {resourcesResponse.TotalCount}");

//            // Test Get Reservations by Resource
//            var reservationsResponse = await reservationResourceService.GetReservationsByResourceIdAsync(resourceId);
//            Console.WriteLine($"Reservations for resource: {reservationsResponse.TotalCount}");

//            // Test Remove Resource from Reservation
//            var removeRequest = new RemoveResourceFromReservationRequest
//            {
//                ReservationId = reservationId,
//                ResourceId = resourceId
//            };

//            var removeResponse = await reservationResourceService.RemoveResourceFromReservationAsync(removeRequest);
//            Console.WriteLine($"Remove resource from reservation {(removeResponse ? "successful" : "failed")}");
//        }
//    }

//    static async Task TestReservationNotificationService(IServiceProvider serviceProvider)
//    {
//        Console.WriteLine("\n=== Testing Reservation Notification Service ===");
//        var notificationService = serviceProvider.GetRequiredService<IReservationNotificationService>();

//        if (_createdReservationIds.Any())
//        {
//            var reservationId = _createdReservationIds.First();

//            // Test Create Notification
//            var createRequest = new CreateReservationNotificationRequest
//            {
//                ReservationId = reservationId,
//                NotificationType = 'I', // Information
//                Message = $"Test notification {GenerateRandomString(10)}"
//            };

//            var createResponse = await notificationService.CreateNotificationAsync(createRequest);
//            Console.WriteLine($"Notification creation {(createResponse.Success ? "successful" : "failed")}: {createResponse.Message}");

//            if (createResponse.Success)
//            {
//                // Test Get Notifications by Reservation
//                var notificationsResponse = await notificationService.GetNotificationsByReservationIdAsync(reservationId);
//                Console.WriteLine($"Notifications for reservation: {notificationsResponse.TotalCount}");

//                // Test filtering by notification type
//                var filterByTypeNotif = new ResourceManaging.Repository.Interfaces.Reservation.ReservationNotificationFilter();
//                filterByTypeNotif.AddNotificationTypeFilter(createRequest.NotificationType);
//                var filteredByTypeNotif = await notificationService.GetNotificationsByFilterAsync(filterByTypeNotif);
//                Console.WriteLine($"Notifications with type '{createRequest.NotificationType}': {filteredByTypeNotif.TotalCount}");

//                // Test filtering by read status
//                var filterByRead = new ResourceManaging.Repository.Interfaces.Reservation.ReservationNotificationFilter();
//                filterByRead.AddReadStatusFilter(true);
//                var filteredByRead = await notificationService.GetNotificationsByFilterAsync(filterByRead);
//                Console.WriteLine($"Notifications with read status 'true': {filteredByRead.TotalCount}");

//                // Test Mark as Read
//                var markAsReadResponse = await notificationService.MarkNotificationAsReadAsync(createResponse.Notification.NotificationId);
//                Console.WriteLine($"Mark notification as read {(markAsReadResponse.Success ? "successful" : "failed")}: {markAsReadResponse.Message}");
//            }
//        }
//    }

//    static async Task CleanupTestData(IServiceProvider serviceProvider)
//    {
//        Console.WriteLine("\n=== Cleaning up test data ===");
//        var reservationService = serviceProvider.GetRequiredService<IReservationService>();
//        var employeeService = serviceProvider.GetRequiredService<IEmployeeService>();
//        var resourceService = serviceProvider.GetRequiredService<IResourceService>();
//        var resourceTypeService = serviceProvider.GetRequiredService<IResourceTypeService>();
//        var notificationService = serviceProvider.GetRequiredService<IReservationNotificationService>();

//        // Delete notifications for each reservation first
//        foreach (var reservationId in _createdReservationIds)
//        {
//            var notificationsResponse = await notificationService.GetNotificationsByReservationIdAsync(reservationId);
//            if (notificationsResponse.Notifications != null)
//            {
//                foreach (var notification in notificationsResponse.Notifications)
//                {
//                    await notificationService.DeleteNotificationAsync(notification.NotificationId);
//                }
//            }
//        }

//        // Delete reservations
//        foreach (var reservationId in _createdReservationIds)
//        {
//            await reservationService.DeleteReservationAsync(reservationId);
//        }
//        Console.WriteLine($"Deleted {_createdReservationIds.Count} reservations");

//        // Delete resources
//        foreach (var resourceId in _createdResourceIds)
//        {
//            await resourceService.DeleteResourceAsync(resourceId);
//        }
//        Console.WriteLine($"Deleted {_createdResourceIds.Count} resources");

//        // Delete resource types
//        foreach (var typeId in _createdResourceTypeIds)
//        {
//            await resourceTypeService.DeleteResourceTypeAsync(typeId);
//        }
//        Console.WriteLine($"Deleted {_createdResourceTypeIds.Count} resource types");

//        // Delete employees
//        foreach (var employeeId in _createdEmployeeIds)
//        {
//            await employeeService.DeleteEmployeeAsync(employeeId);
//        }
//        Console.WriteLine($"Deleted {_createdEmployeeIds.Count} employees");
//    }
//}
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Resource Management System - Console Application");
        Console.WriteLine("-----------------------------------------------");
        // Here you can call methods to test your services
        // For example:
        // await TestAuthenticationService();
        // await TestEmployeeService();
        // await TestResourceService();
        // await TestReservationService();
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}