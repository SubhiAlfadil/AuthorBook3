using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthorBook3.Models
{
    public class BookType
    {
        public BookType()
        {
            Books = new HashSet<Book>();
        }
        [Display(Name = "Type No.")]
        public int BookTypeId { get; set; }
        [Required]
        [Display(Name = "Type Name")]
        public string Name { get; set; }
        [Display(Name = "Book Type Des.")]
        public string Description { get; set; }
        [Display(Name = "Create On")]
        public DateTime CreateOn { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
