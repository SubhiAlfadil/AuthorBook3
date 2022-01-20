using AuthorBook3.Models;
using AuthorBook3.Models.Others;
using AuthorBook3.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorBook3.Controllers
{
    public class BookTypeController : Controller
    {
        private IRepository<Author> _AuthorRep { get; }
        private IRepository<Book> _BookRep { get; }
        private IBookRepository _BookRep2 { get; }
        private IRepository<BookType> _BookTypeRep { get; }
        private IBookTypeRepository _BookTypeRep2 { get; }

        public BookTypeController(IRepository<Author> authorRepository, IRepository<Book> bookRepository
            , IRepository<BookType> bookTypeRepository, IBookRepository _BookRep2, IBookTypeRepository _BookTypeRep2)
        {
            _AuthorRep = authorRepository;
            _BookRep = bookRepository;
            _BookTypeRep = bookTypeRepository;
            this._BookRep2 = _BookRep2;
            this._BookTypeRep2 = _BookTypeRep2;
        }

        // GET: Author
        public async Task<ActionResult> Index(int? currentYear, int? currentMonth, string WithBooks, string WithOutBooks)
        {
            int cYear = 0;
            int cMonth = 0;
            var bookTypes = new List<BookType>();
            if (!string.IsNullOrEmpty(WithBooks) && WithBooks.Equals("WithBooks"))
            {
                bookTypes = await _BookTypeRep2.GetBookTypesWithBooks();
                ViewBag.Title2 = " With Books";
                ViewBag.WithBooks = "WithBooks";
            }
            else if (!string.IsNullOrEmpty(WithOutBooks) && WithOutBooks.Equals("WithOutBooks"))
            {
                bookTypes = await _BookTypeRep2.GetBookTypesWithOutBooks();
                ViewBag.Title2 = " WithOut Books";
                ViewBag.WithOutBooks = "WithOutBooks";
            }
            else
            {
                bookTypes = await _BookTypeRep.GetAll();
            }

            // Filter By Year
            var sortedYList = bookTypes.GroupBy(a => a.CreateOn.Year).OrderByDescending(a => a.Key).ToList();
            var yearVMList = new List<YearVM>();
            foreach (var item in sortedYList)
            {
                int count = bookTypes.Where(a => a.CreateOn.Year.ToString() == item.Key.ToString()).Count();
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
            bookTypes = bookTypes.Where(a => a.CreateOn.Year == cYear).ToList();
            // Filter By Month
            var sortedMList = bookTypes.GroupBy(a => a.CreateOn.Month).OrderByDescending(a => a.Key).ToList();
            var monthVMList = new List<MonthVM>();
            foreach (var item2 in sortedMList)
            {
                int count = bookTypes.Where(a => a.CreateOn.Month.ToString() == item2.FirstOrDefault().CreateOn.Month.ToString()).Count();
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
            bookTypes = bookTypes.Where(a => a.CreateOn.Month == cMonth).ToList();

            ViewBag.yearList = yearVMList;
            ViewBag.currentYear = cYear;
            ViewBag.monthVMList = monthVMList;
            ViewBag.currentMonth = cMonth;

            foreach (var bookType in bookTypes)
            {
                foreach (var book in await _BookRep2.GetBooksForbookType(bookType.BookTypeId))
                {
                    bookType.Books.Add(book);
                }
            }

            return View(bookTypes);
        }

        // GET: Author/Details/5
        public async Task<ActionResult> Details(int id)
        {
            BookType bookType = await _BookTypeRep.GetById(id);
            if (bookType == null) { return RedirectToAction(nameof(Index)); }
            return View(bookType);
        }

        // GET: Author/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookType bookType)
        {
            try
            {
                if (await _BookTypeRep.Add(bookType) > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Author/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            BookType bookType = await _BookTypeRep.GetById(id);
            if (bookType == null) { return RedirectToAction(nameof(Index)); }
            return View(bookType);
        }

        // POST: Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BookType bookType)
        {
            if (!_BookTypeRep.AnyById(bookType.BookTypeId)) { return RedirectToAction(nameof(Index)); }
            try
            {
                if (await _BookTypeRep.Edit(bookType) > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(bookType);
            }
            catch
            {
                return View(bookType);
            }
        }

        // GET: Author/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (await _BookTypeRep.Delete(id) > 0)
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

        //// POST: Author/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AcceptVerbs(Name ="Delete")]
        //public async Task<ActionResult> ConDelete(int id)
        //{
        //    try
        //    {
        //        if (await _BookTyperRep.Delete(id) > 0)
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
