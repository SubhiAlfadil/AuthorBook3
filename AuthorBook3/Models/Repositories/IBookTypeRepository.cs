using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorBook3.Models.Repositories
{
    public interface IBookTypeRepository
    {
        Task<List<BookType>> GetBookTypesWithBooks();
        Task<List<BookType>> GetBookTypesWithOutBooks();
    }
}
