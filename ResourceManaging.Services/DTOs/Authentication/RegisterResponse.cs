namespace ResourceManaging.Services.DTOs.Authentication
{
    public class RegisterResponse
    {
         public bool Success { get; set; }
        public string Message { get; set; }
        public int EmployeeId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
