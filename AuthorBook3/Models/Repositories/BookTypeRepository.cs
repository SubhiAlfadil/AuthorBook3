using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorBook3.Models.Repositories
{
    public class BookTypeRepository : IRepository<BookType>, IBookTypeRepository
    {
        private readonly DBContexta _db;

        public BookTypeRepository(DBContexta context)
        {
            _db = context;
        }
        public async Task<int> Add(BookType item)
        {
            try
            {
                item.CreateOn = DateTime.Now;
                _db.BookTypes.Add(item);
                await _db.SaveChangesAsync();
                return item.BookTypeId;
            }
            catch (System.Exception ex)
            {
                return -1;
            }
        }

        public bool AnyById(int id)
        {
            return _db.BookTypes.Any(a => a.BookTypeId == id);
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                var item = await _db.BookTypes.FindAsync(id);
                if (item == null) { return 1; }
                _db.BookTypes.Remove(item);
                await _db.SaveChangesAsync();

                return item.BookTypeId;
            }
            catch (System.Exception ex)
            {
                return -1;
            }
        }

        public async Task<int> Edit(BookType item)
        {
            if (item == null) { return -1; }
            try
            {
                _db.BookTypes.Update(item);
                await _db.SaveChangesAsync();
                return item.BookTypeId;
            }
            catch (System.Exception ex)
            {
                return -1;
            }
        }

        public async Task<List<BookType>> GetAll()
        {
            try
            {
                return await _db.BookTypes.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<BookType>> GetBookTypesWithBooks()
        {
            try
            {
                var ids = await _db.Books.Select(a => a.BookTypeId).ToListAsync();
                ids = ids.Distinct().ToList();
                var bookTypeList = new List<BookType>();
                foreach (var bookTypeId in ids)
                {
                    var bookType = await _db.BookTypes.Where(a => a.BookTypeId == bookTypeId).FirstOrDefaultAsync();
                    if (bookType != null)
                    {
                        bookTypeList.Add(bookType);
                    }
                }
                return bookTypeList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<BookType>> GetBookTypesWithOutBooks()
        {
            try
            {
                var ids = await _db.Books.Select(a => a.BookTypeId).ToListAsync();
                ids = ids.Distinct().ToList();
                var allIds = await _db.BookTypes.Select(a => a.BookTypeId).ToListAsync();
                allIds = allIds.Except(ids).ToList();
                var bookTypeList = new List<BookType>();
                foreach (var bookTypeId in allIds)
                {
                    var bookType = await _db.BookTypes.Where(a => a.BookTypeId == bookTypeId).FirstOrDefaultAsync();
                    if (bookType != null)
                    {
                        bookTypeList.Add(bookType);
                    }
                }
                return bookTypeList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BookType> GetById(int id)
        {
            return await _db.BookTypes.FindAsync(id);
        }
    }
}
