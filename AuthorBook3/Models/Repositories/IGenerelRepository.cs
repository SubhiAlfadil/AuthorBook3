using System.Threading.Tasks;

namespace AuthorBook3.Models.Repositories
{
    public interface IGenerelRepository
    {
         int Authors();
         int AuthorsWithBooks();
         int AuthorsWithOutBooks();
         int BookTypes();
         int BookTypesWithBooks();
         int BookTypesWithOutBooks();
         int Books();
    }
}
