using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthorBook3.Models.ViewModel
{
    public class BookCreateVM
    {
        public Book book { get; set; }
        public List<Author> Authors { get; set; }
        public List<BookType> BookTypes { get; set; }
        [Display(Name = "Book File")]
        public IFormFile NewBookFile { get; set; }
    }
}
