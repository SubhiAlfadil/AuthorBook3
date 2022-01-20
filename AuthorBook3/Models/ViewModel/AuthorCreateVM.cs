using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthorBook3.Models.ViewModel
{
    public class AuthorCreateVM
    {
        public int AuthorId { get; set; }
        [Required]
        [Display(Name = "Author Name")]
        public string FullName { get; set; }
        [Required]

        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Required]

        [Display(Name = "Phone Number")]
        public string Number { get; set; }

        [Display(Name = "Create On")]
        public DateTime CreateOn { get; set; }

        public string ProfilePicture { get; set; }
        [Display(Name = "Profile Image")]
        public IFormFile NewProfileImage { get; set; }
    }
}
