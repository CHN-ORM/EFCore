using System;
using System.Linq;
using System.Collections.Generic;
using myApp.Model;

namespace myApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BookDbContext())
            {
                db.Database.EnsureCreated();

                // var newLibrary = new Library
                // {
                //     Name = "国家图书馆",
                // };
                
                // newLibrary.Books.AddRange(
                //     new List<Book>
                //     {
                //         new Book { Name = "史记" },
                //         new Book { Name = "资治通鉴" },
                //     });
                // db.Libraries.Add(newLibrary);
                // db.SaveChanges();

                foreach (var library in db.Libraries)
                {
                    System.Console.WriteLine($"LibraryId: {library.LibraryId}, Name: {library.Name}, BookCount: {library.Books.Count}");
                    
                    foreach (var book in library.Books)
                    {
                        System.Console.WriteLine($" LibraryId: {book.LibraryId}, BookId: {book.BookId}, Name: {book.Name}");
                    }
                }
                
                foreach (var book in db.Books)
                {
                    System.Console.WriteLine($" LibraryId: {book.LibraryId}, BookId: {book.BookId}, Name: {book.Name}");
                }
            }
        }
    }
}
