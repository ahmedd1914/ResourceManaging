namespace ResourceManaging.Repository.Helpers
{
    public class Update
    {
        private readonly Dictionary<string, object> _updates;

        public Update()
        {
            _updates = new Dictionary<string, object>();
        }

        public void AddUpdate(string key, object value)
        {
            _updates.Add(key, value);
        }

        public bool HasUpdate(string key)
        {
            return _updates.ContainsKey(key);
        }

        public T GetUpdate<T>(string key)
        {
            return (T)_updates[key];
        }

        public IEnumerable<string> GetUpdateKeys()
        {
            return _updates.Keys;
        }

        public IEnumerable<string> GetUpdateKeysExcluding(string keyToExclude)
        {
            return _updates.Keys.Where(k => k != keyToExclude);
        }

        public IEnumerable<T> GetUpdates<T>()
        {
            return _updates.Values.Cast<T>();
        }

        public void Clear()
        {
            _updates.Clear();
        }
        
        public int Count => _updates.Count;
    }
}