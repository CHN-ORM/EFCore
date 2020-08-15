using System.Collections.Generic;

namespace myApp.Model
{
    public class Library
    {
        public int LibraryId { get; set; }
        public string Name { get; set; }

        public List<Book> Books { get; } = new List<Book>();
    }
}