using AuthorBook3.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorBook3.Models.Repositories
{
    public interface IAuthorRepository
    {
        Task<AuthorCreateVM> GetAuthorEditVM(int AuthorId);
        Task<int> EditAuthor(AuthorCreateVM authorEditVM);
        Task<List<Author>> GetAuthorsWithBooks();
        Task<List<Author>> GetAuthorsWithOutBooks();
    }
}
