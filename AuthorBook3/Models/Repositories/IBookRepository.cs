using AuthorBook3.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorBook3.Models.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooksForAuthor(int authorId);
        Task<List<Book>> GetBooksForbookType(int bookTypeId);
        Task<int> AddBookWithFile(BookCreateVM bookCreateVM);
    }
}
