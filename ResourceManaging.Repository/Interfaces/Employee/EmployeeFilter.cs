using ResourceManaging.Repository.Helpers;

namespace ResourceManaging.Repository.Interfaces.Employee
{
    public class EmployeeFilter
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int Count => (Username != null ? 1 : 0) + (Email != null ? 1 : 0);

        public IEnumerable<string> GetParameters()
        {
            if (Username != null) yield return "Username";
            if (Email != null) yield return "Email";
        }

        public T GetParameter<T>(string paramName)
        {
            return paramName switch
            {
                "Username" => (T)(object)Username,
                "Email" => (T)(object)Email,
                _ => default
            };
        }
    }
} 