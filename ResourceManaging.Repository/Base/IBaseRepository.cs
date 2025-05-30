namespace ResourceManaging.Repository.Base
{
    public interface IBaseRepository<TObj,TFilter, TUpdate > where TObj : class
    {
        Task<TObj> RetrieveByIdAsync(int id);
        Task<IEnumerable<TObj>> RetrieveAllAsync(string orderByColumn = null, bool ascending = true);
        Task<IEnumerable<TObj>> RetrieveByFilterAsync(TFilter filter);
        Task<int> CreateAsync(TObj entity);
        Task<bool> UpdateAsync(TUpdate entity);
        Task<bool> DeleteAsync(int id);
    }
}