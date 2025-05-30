namespace ResourceManaging.Repository.Helpers
{
    public class Filter
    {
        private readonly Dictionary<string, object> _parameters;

        public Filter()
        {
            _parameters = new Dictionary<string, object>();
        }

        public void AddParameter(string key, object value)
        {
            _parameters.Add(key, value);
        }

        public bool HasParameter(string key)
        {
            return _parameters.ContainsKey(key);
        }

        public T GetParameter<T>(string key)
        {
            return (T)_parameters[key];
        }
        public IEnumerable<string> GetParameters()
        {
            return _parameters.Keys;
        }
        public void Clear()
        {
            _parameters.Clear();
        }
        
        public int Count => _parameters.Count;
    }
}