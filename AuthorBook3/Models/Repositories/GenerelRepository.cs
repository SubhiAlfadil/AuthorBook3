using System.Linq;
using System.Threading.Tasks;

namespace AuthorBook3.Models.Repositories
{
    public class GenerelRepository : IGenerelRepository
    {
        private readonly DBContexta _db;

        public GenerelRepository(DBContexta dB)
        {
            _db = dB;
        }

        public int Authors()
        {
            return _db.Authors.Count();
        }

        public int AuthorsWithBooks()
        {
            var ids = _db.Books.Select(a => a.AuthorId).ToList();
            return _db.Authors.Where(a => ids.Any(id => a.AuthorId == id)).Count();
        }

        public int AuthorsWithOutBooks()
        {
            var ids = _db.Books.Select(a => a.AuthorId).ToList();
            int count = 0;
            foreach (var authorId in _db.Authors.Select(a => a.AuthorId).ToList())
            {
                bool found = true;
                foreach (var id in ids)
                {
                    if (id == authorId)
                    {
                        found = false;
                        break;
                    }
                }
                if (found) { count++; }
            }
            return count;
        }

        public int Books()
        {
            return _db.Books.Count();
        }

        public int BookTypes()
        {
            return _db.BookTypes.Count();
        }

        public int BookTypesWithBooks()
        {
            var ids = _db.Books.Select(a => a.BookTypeId).ToList();
            return _db.BookTypes.Where(a => ids.Any(id => a.BookTypeId == id)).Count();
        }

        public int BookTypesWithOutBooks()
        {
            var ids = _db.Books.Select(a => a.BookTypeId).ToList();
            int count = 0;
            foreach (var bookTypeId in _db.BookTypes.Select(a => a.BookTypeId).ToList())
            {
                bool found = true;
                foreach (var id in ids)
                {
                    if (id == bookTypeId)
                    {
                        found = false;
                        break;
                    }
                }
                if (found) { count++; }
            }
            return count;
        }
    }
}
