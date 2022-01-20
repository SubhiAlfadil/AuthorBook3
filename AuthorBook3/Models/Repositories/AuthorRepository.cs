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
    public class AuthorRepository : IRepository<Author>, IAuthorRepository
    {
        private readonly DBContexta _db;
        private IWebHostEnvironment webHostEnvironment { get; }

        public AuthorRepository(DBContexta context, IWebHostEnvironment webHostEnvironment)
        {
            _db = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public AuthorRepository()
        {
        }
        public async Task<int> Add(Author item)
        {
            try
            {
                item.CreateOn = DateTime.Now;
                _db.Authors.Add(item);
                await _db.SaveChangesAsync();
                return item.AuthorId;
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
                var item = await _db.Authors.FindAsync(id);
                if (item == null) { return 1; }
                _db.Authors.Remove(item);
                await _db.SaveChangesAsync();
                // Delete All Books
                IList<Book> books = await _db.Books.Where(a => a.AuthorId == id).ToListAsync();
                foreach (var book in books)
                {
                    _db.Books.Remove(book);
                }
                await _db.SaveChangesAsync();
                return item.AuthorId;
            }
            catch (System.Exception ex)
            {
                return -1;
            }
        }
        public bool AnyById(int id)
        {
            return _db.Authors.Any(a => a.AuthorId == id);
        }
        public async Task<int> EditAuthor(AuthorCreateVM authorEditVM)
        {
            if (authorEditVM == null) { return -1; }
            try
            {
                string uniqueFileName = await UploadedFile(authorEditVM, webHostEnvironment);
                Author author = getAuthorFrom(authorEditVM, uniqueFileName);
                tryDeleteOldImage(authorEditVM);
                _db.Authors.Update(author);
                await _db.SaveChangesAsync();
                return authorEditVM.AuthorId;
            }
            catch (System.Exception ex)
            {
                return -1;
            }
        }

        private void tryDeleteOldImage(AuthorCreateVM authorEditVM)
        {
            try
            {
                if (!string.IsNullOrEmpty(authorEditVM.ProfilePicture) && authorEditVM.NewProfileImage != null)
                {
                    string oldFile = Path.Combine(webHostEnvironment.WebRootPath, "images", authorEditVM.ProfilePicture);
                    if ((System.IO.File.Exists(oldFile)))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private Author getAuthorFrom(AuthorCreateVM authorEditVM, string uniqueFileName)
        {
            try
            {
                Author author = new Author()
                {
                    AuthorId = authorEditVM.AuthorId,
                    BirthDate = authorEditVM.BirthDate,
                    CreateOn = authorEditVM.CreateOn,
                    FullName = authorEditVM.FullName,
                    Number = authorEditVM.Number,
                    ProfilePicture = uniqueFileName
                };
                return author;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Author>> GetAll()
        {
            try
            {
                return await _db.Authors.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Author> GetById(int id)
        {
            return await _db.Authors.FindAsync(id);
        }

        public async Task<AuthorCreateVM> GetAuthorEditVM(int AuthorId)
        {
            try
            {
                Author author = await GetById(AuthorId);
                AuthorCreateVM authorEditVM = new AuthorCreateVM()
                {
                    AuthorId = author.AuthorId,
                    BirthDate = author.BirthDate,
                    CreateOn = author.CreateOn,
                    FullName = author.FullName,
                    Number = author.Number,
                    ProfilePicture = author.ProfilePicture
                };
                return authorEditVM;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<int> Edit(Author item)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadedFile(AuthorCreateVM model, IWebHostEnvironment webHostEnvironment)
        {
            string uniqueFileName = null;
            if (model.NewProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.NewProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.NewProfileImage.CopyToAsync(fileStream);
                }
            }
            else
            {
                uniqueFileName = model.ProfilePicture;
            }
            return uniqueFileName;
        }

        public async Task<List<Author>> GetAuthorsWithBooks()
        {
            try
            {
                var ids = await _db.Books.Select(a => a.AuthorId).ToListAsync();
                ids = ids.Distinct().ToList();
                var authorList = new List<Author>();
                foreach (var authorId in ids)
                {
                    var author = await _db.Authors.Where(a => a.AuthorId == authorId).FirstOrDefaultAsync();
                    if (author != null)
                    {
                        authorList.Add(author);
                    }
                }
                return authorList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Author>> GetAuthorsWithOutBooks()
        {
            try
            {
                var ids = await _db.Books.Select(a => a.AuthorId).ToListAsync();
                ids = ids.Distinct().ToList();
                var allIds = await _db.Authors.Select(a => a.AuthorId).ToListAsync();
                allIds = allIds.Except(ids).ToList();    
                var authorList = new List<Author>();
                foreach (var authorId in allIds)
                {
                    var author = await _db.Authors.Where(a => a.AuthorId == authorId).FirstOrDefaultAsync();
                    if (author != null)
                    {
                        authorList.Add(author);
                    }
                }
                return authorList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
