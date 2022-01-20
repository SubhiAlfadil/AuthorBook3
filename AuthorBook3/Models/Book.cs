using System;
using System.ComponentModel.DataAnnotations;

namespace AuthorBook3.Models
{
    public class Book
    {

        [Display(Name="Book No.")]
        public int BookId { get; set; }
        public string Title { get; set; }
        [Display(Name="Book Des.")]
        public string Description { get; set; }

        [Display(Name = "Create On")]
        public DateTime CreateOn { get; set; }
        // www.root//bookFile
        public string bookFile { get; set; }

        [Display(Name = "Author Name")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [Display(Name = "Book Type Name")]
        public int BookTypeId { get; set; }
        public BookType BookType { get; set; }
    }
}
