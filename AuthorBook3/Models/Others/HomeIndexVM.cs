namespace AuthorBook3.Models.Others
{
    public class HomeIndexVM
    {
        public int Authors { get; set; }
        public int AuthorsWithBooks { get; set; }
        public int AuthorsWithOutBooks { get; set; }
        public int BookTypes { get; set; }
        public int BookTypesWithBooks { get; set; }
        public int BookTypesWithOutBooks { get; set; }
        public int Books { get; set; }
    }
}