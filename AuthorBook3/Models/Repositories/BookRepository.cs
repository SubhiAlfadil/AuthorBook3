using AuthorBook3.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorBook3.Models.Repositories
{
    public class BookRepository : IRepository<Book> , IBookRepository
    {
        private readonly DBContexta _db;
        private IWebHostEnvironment webHostEnvironment { get; }

        public BookRepository(DBContexta context, IWebHostEnvironment webHostEnvironment)
        {
            _db = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public BookRepository()
        {
        }
        public async Task<int> Add(Book item)
        {
            try
            {
                item.CreateOn = DateTime.Now;
                _db.Books.Add(item);
                await _db.SaveChangesAsync();
                return item.BookId;
            }
            catch (System.Exception ex)
            {
                return -1;
            }
        }
        public async Task<int> Delete(int id)
        {
            try
            {
                var item = await _db.Books.FindAsync(id);
                if (item == null) { 
                    return 1; 
                }
                _db.Books.Remove(item);
                await _db.SaveChangesAsync();
                return item.BookId;
            }
            catch (System.Exception ex)
            {
                return -1;
            }
        }
        public bool AnyById(int id)
        {
            return _db.Books.Any(a => a.BookId == id);
        }
        public async Task<int> Edit(Book item)
        {
            if (item == null) { return -1; }
            try
            {
                _db.Books.Update(item);
                await _db.SaveChangesAsync();
                return item.BookId;
            }
            catch (System.Exception ex)
            {
                return -1;
            }
        }

        public async Task<List<Book>> GetAll()
        {
            try
            {
                return await _db.Books.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Book> GetById(int id)
        {
            return await _db.Books.Include(a=>a.Author).Include(a=>a.BookType).FirstOrDefaultAsync(a=>a.BookId == id);
        }

        public async Task<List<Book>> GetBooksForAuthor(int authorId)
        {
            try
            {
            return await _db.Books.Where(a => a.AuthorId == authorId).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Book>> GetBooksForbookType(int bookTypeId)
        {
            try
            {
                return await _db.Books.Where(a => a.BookTypeId == bookTypeId).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> AddBookWithFile(BookCreateVM bookCreateVM)
        {
            try
            {
                bookCreateVM.book.CreateOn = DateTime.Now;
                bookCreateVM.book.bookFile =await UploadedFile(bookCreateVM, webHostEnvironment);
                _db.Books.Add(bookCreateVM.book);
                await _db.SaveChangesAsync();
                
                return bookCreateVM.book.BookId;
            }
            catch (System.Exception ex)
            {
                return -1;
            }
        }

        public async Task<string> UploadedFile(BookCreateVM model, IWebHostEnvironment webHostEnvironment)
        {
            string uniqueFileName = null;
            if (model.NewBookFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "bookFiles");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.NewBookFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.NewBookFile.CopyToAsync(fileStream);
                }
            }
            else
            {
                uniqueFileName = null;
            }
            return uniqueFileName;
        }

    }
}
