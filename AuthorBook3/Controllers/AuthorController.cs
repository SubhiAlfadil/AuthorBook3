using AuthorBook3.Models;
using AuthorBook3.Models.Others;
using AuthorBook3.Models.Repositories;
using AuthorBook3.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorBook3.Controllers
{
    public class AuthorController : Controller
    {
        private IRepository<Author> _AuthorRep { get; }
        private IBookRepository _BookRep2 { get; }
        private IAuthorRepository _AuthorRep2 { get; }
        private IWebHostEnvironment webHostEnvironment { get; }

        public AuthorController(IRepository<Author> authorRepository, IBookRepository bookRepository, IAuthorRepository authorRep, IWebHostEnvironment webHostEnvironment)
        {
            _AuthorRep = authorRepository;
            _BookRep2 = bookRepository;
            _AuthorRep2 = authorRep;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Author
        public async Task<ActionResult> Index(int? currentYear, int? currentMonth,string WithBooks,string WithOutBooks)
        {
            int cYear = 0;
            int cMonth = 0;
            var authores = new List<Author>();
            if (!string.IsNullOrEmpty(WithBooks) && WithBooks.Equals("WithBooks")) {
                authores = await _AuthorRep2.GetAuthorsWithBooks();
                ViewBag.Title2 = " With Books";
                ViewBag.WithBooks = "WithBooks";
            }
            else if (!string.IsNullOrEmpty(WithOutBooks) && WithOutBooks.Equals("WithOutBooks"))
            {
                authores = await _AuthorRep2.GetAuthorsWithOutBooks();
                ViewBag.Title2 = " WithOut Books";
                ViewBag.WithOutBooks = "WithOutBooks";
            }
            else
            {
            authores = await _AuthorRep.GetAll();
            }
            // Filter By Year
            var sortedYList = authores.GroupBy(a => a.CreateOn.Year).OrderByDescending(a => a.Key).ToList();
            var yearVMList = new List<YearVM>();
            foreach (var item in sortedYList)
            {
                int count = authores.Where(a => a.CreateOn.Year.ToString() == item.Key.ToString()).Count();
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
            authores = authores.Where(a => a.CreateOn.Year == cYear).ToList();
            // Filter By Month
            var sortedMList = authores.GroupBy(a => a.CreateOn.Month).OrderByDescending(a => a.Key).ToList();
            var monthVMList = new List<MonthVM>();
            foreach (var item2 in sortedMList)
            {
                int count = authores.Where(a => a.CreateOn.Month.ToString() == item2.FirstOrDefault().CreateOn.Month.ToString()).Count();
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
            authores = authores.Where(a => a.CreateOn.Month == cMonth).ToList();

            ViewBag.yearList = yearVMList;
            ViewBag.currentYear = cYear;
            ViewBag.monthVMList = monthVMList;
            ViewBag.currentMonth = cMonth;

            foreach (var author in authores)
            { 
                foreach (var book in await _BookRep2.GetBooksForAuthor(author.AuthorId))
                {
                    author.Books.Add(book);
                }
            }

            return View(authores);
        }

        // GET: Author/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Author author = await _AuthorRep.GetById(id);
            if (author == null) { return RedirectToAction(nameof(Index)); }
            return View(author);
        }

        // GET: Author/Create
        public ActionResult Create()
        {
            return View(new AuthorCreateVM());
        }

        // POST: Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AuthorCreateVM model)
        {
            try
            {
                string uniqueFileName =await new AuthorRepository().UploadedFile(model, webHostEnvironment);
                Author author = new Author() { 
                    BirthDate=model.BirthDate, 
                    FullName=model.FullName, 
                    Number=model.Number, 
                    ProfilePicture= uniqueFileName
                };

                if (await _AuthorRep.Add(author) > 0)
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
            AuthorCreateVM authorEditVM = await _AuthorRep2.GetAuthorEditVM(id);
            if (authorEditVM == null) { return RedirectToAction(nameof(Index)); }
            return View(authorEditVM);
        }

        // POST: Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AuthorCreateVM authorEditVM)
        {
            if (!_AuthorRep.AnyById(authorEditVM.AuthorId)) { return RedirectToAction(nameof(Index)); }
            try
            {
                if (await _AuthorRep2.EditAuthor(authorEditVM) > 0)
                {
                    return RedirectToAction(nameof(Details), new { id = authorEditVM.AuthorId });
                }
                return View(authorEditVM);
            }
            catch
            {
                return View(authorEditVM);
            }
        }

        // GET: Author/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (await _AuthorRep.Delete(id) > 0)
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
        //        if (await _AuthorRep.Delete(id) > 0)
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
