using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorBook3.Models.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<int> Add(T item);
        Task<int> Edit(T item);
        Task<int> Delete(int id);
        bool AnyById(int id);

    }
}
