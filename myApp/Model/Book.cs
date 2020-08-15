namespace myApp.Model
{
    public class Book
    {
        public int BookId { get; set; }     
        public string Name { get; set; } 
        public decimal Price { get; set; }
        public string Author { get; set; }

        public int LibraryId { get; set; }
        public Library Library { get; set; }
    }
}