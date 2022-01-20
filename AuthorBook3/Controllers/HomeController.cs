using AuthorBook3.Models;
using AuthorBook3.Models.Others;
using AuthorBook3.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorBook3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IGenerelRepository genRepo { get; }

        public HomeController(ILogger<HomeController> logger, IGenerelRepository generelRepository)
        {
            _logger = logger;
            genRepo = generelRepository;
        }

        public IActionResult Index()
        {
            HomeIndexVM model = new HomeIndexVM()
            {
                Authors= genRepo.Authors(),
                AuthorsWithBooks= genRepo.AuthorsWithBooks(),
                AuthorsWithOutBooks= genRepo.AuthorsWithOutBooks(),
                BookTypes= genRepo.BookTypes(),
                BookTypesWithBooks= genRepo.BookTypesWithBooks(),
                BookTypesWithOutBooks= genRepo.BookTypesWithOutBooks(),
                Books= genRepo.Books(),
            }; 
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            string physicalPath = "wwwroot/pdfs/MyCV.pdf";
            byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);
            MemoryStream ms = new MemoryStream(pdfBytes);
            return new FileStreamResult(ms, "application/pdf");
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult NeedHelp()
        {
            return View();
        }
        
        public IActionResult Tools()
        {
            return View();
        }
        public IActionResult ScreenShots()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
