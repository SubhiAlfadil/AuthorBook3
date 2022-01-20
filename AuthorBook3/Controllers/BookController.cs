using AuthorBook3.Models;
using AuthorBook3.Models.Others;
using AuthorBook3.Models.Repositories;
using AuthorBook3.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorBook3.Controllers
{
    public class BookController : Controller
    {
        public IRepository<Book> _BookRep { get; }
        public IBookRepository _BookRep2 { get; }
        public IRepository<Author> _AuthorRep { get; }
        public IRepository<BookType> _BookTypeRep { get; }
        private IWebHostEnvironment webHostEnvironment { get; }

        public BookController(IRepository<Book> bookRepository, IRepository<Author> authorRepository,
            IRepository<BookType> BookTypeRepository, IBookRepository _BookRep2, IWebHostEnvironment webHostEnvironment)
        {
            _BookRep = bookRepository;
            this._BookRep2 = _BookRep2;
            _AuthorRep = authorRepository;
            _BookTypeRep = BookTypeRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Book
        public async Task<ActionResult> Index(int? currentYear, int? currentMonth)
        {
            int cYear = 0;
            int cMonth = 0;
            var books = await _BookRep.GetAll();
            // Filter By Year
            var sortedYList = books.GroupBy(a => a.CreateOn.Year).OrderByDescending(a => a.Key).ToList();
            var yearVMList = new List<YearVM>();
            foreach (var item in sortedYList)
            {
                int count = books.Where(a => a.CreateOn.Year.ToString() == item.Key.ToString()).Count();
                yearVMList.Add(new YearVM() { Count = count, Name = item.Key.ToString() });
            }

            if (currentYear != null)
            {
                cYear = currentYear.Value;
            }
            else
            {
                if (sortedYList.FirstOrDefault() != null) { cYear = sortedYList.FirstOrDefault().Key; }
            }
            books = books.Where(a => a.CreateOn.Year == cYear).ToList();
            // Filter By Month
            var sortedMList = books.GroupBy(a => a.CreateOn.Month).OrderByDescending(a => a.Key).ToList();
            var monthVMList = new List<MonthVM>();
            foreach (var item2 in sortedMList)
            {
                int count = books.Where(a => a.CreateOn.Month.ToString() == item2.FirstOrDefault().CreateOn.Month.ToString()).Count();
                monthVMList.Add(new MonthVM() { Count = count, Name = item2.Key.ToString() });
            }
            ViewBag.monthList = monthVMList;
            if (currentMonth != null)
            {
                cMonth = currentMonth.Value;
            }
            else
            {
                if (sortedMList.FirstOrDefault() != null) { cMonth = sortedMList.FirstOrDefault().Key; }
            }
            books = books.Where(a => a.CreateOn.Month == cMonth).ToList();

            ViewBag.yearList = yearVMList;
            ViewBag.currentYear = cYear;
            ViewBag.monthVMList = monthVMList;
            ViewBag.currentMonth = cMonth;
            foreach (var item in books)
            {
                item.Author = await _AuthorRep.GetById(item.AuthorId);
                item.BookType = await _BookTypeRep.GetById(item.BookTypeId);
            }
            return View(books);
        }

        // GET: Book/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Book book = await _BookRep.GetById(id);
            if (book == null) { return RedirectToAction(nameof(Index)); }
            return View(book);
        }



        // GET: Book/Download/5
        [Route("download")]
        public async Task<IActionResult> Download(int id)
        {
            Book book = await _BookRep.GetById(id);
            if (book == null) { return RedirectToAction(nameof(Index)); }
            var uploads = Path.Combine(webHostEnvironment.WebRootPath, "bookFiles");
            var filePath = Path.Combine(uploads, book.bookFile);
            if (!System.IO.File.Exists(filePath)) return NotFound();
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), book.bookFile);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        // GET: Book/Create
        public async Task<ActionResult> Create()
        {
            BookCreateVM model = new BookCreateVM()
            {
                Authors = await _AuthorRep.GetAll(),
                BookTypes = await _BookTypeRep.GetAll()
            };
            return View(model);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookCreateVM model)
        {
            try
            {
                if (await _BookRep2.AddBookWithFile(model) > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                goto Finnel;
            }
            catch
            {
                goto Finnel;
            }
        Finnel:
            model.Authors = await _AuthorRep.GetAll();
            model.BookTypes = await _BookTypeRep.GetAll();
            return View(model);
        }

        // GET: Book/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Book book = await _BookRep.GetById(id);
            if (book == null) { return RedirectToAction(nameof(Index)); }
            BookCreateVM model = new BookCreateVM()
            {
                Authors = await _AuthorRep.GetAll(),
                BookTypes = await _BookTypeRep.GetAll(),
                book = book
            };
            return View(model);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BookCreateVM model)
        {
            if (!_BookRep.AnyById(model.book.BookId)) { return RedirectToAction(nameof(Index)); }
            try
            {
                if (await _BookRep.Edit(model.book) > 0)
                {
                    return RedirectToAction(nameof(Details), new { id = model.book.BookId });
                }
                goto Finnel;
            }
            catch
            {
                goto Finnel;
            }
        Finnel:
            model.Authors = await _AuthorRep.GetAll();
            model.BookTypes = await _BookTypeRep.GetAll();
            return View(model);
        }

        // GET: Book/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (await _BookRep.Delete(id) > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        //// POST: Book/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AcceptVerbs(Name ="Delete")]
        //public async Task<ActionResult> ConDelete(int id)
        //{
        //    try
        //    {
        //        if (await _BookRep.Delete(id) > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //        return View();
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
