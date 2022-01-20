using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthorBook3.Models
{
    public class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }
        [Display(Name="Author No.")]
        public int AuthorId { get; set; }
        [Required]
        [Display(Name="Author Name")]
        public string FullName { get; set; }
        [Required]

        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Required]

        [Display(Name = "Phone Number")]
        public string Number { get; set; }

        [Display(Name = "Create On")]
        public DateTime CreateOn { get; set; }

        [Required(ErrorMessage = "Please choose profile image")]
        public string ProfilePicture { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
