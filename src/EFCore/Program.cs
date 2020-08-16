using System;
using System.Linq;
using System.Collections.Generic;
using EFCore.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // 初始化服务
            IServiceCollection services = new ServiceCollection();
            new Startup().ConfiguraServices(services);
            var sp = services.BuildServiceProvider();

            // 初始化数据库
            Init(sp);

            // 获取关系数据
            RetriveRelational(sp);
        }

        #region 关系映射

        /// <summary>
        /// 加载关联数据
        /// </summary>
        static void RetriveRelational(IServiceProvider sp)
        {
            using (var scope = sp.CreateScope())
            using (var db = scope.ServiceProvider.GetService<BookDbContext>())
            {
                LoadLibraries(db);

                LoadBooks(db);
            }
        }

        /// <summary>
        /// 加载图书馆，及关联的数据
        /// </summary>
        private static void LoadLibraries(BookDbContext db)
        {
            var libQuery = db.Libraries
                .Include(lib => lib.Books);
            foreach (var library in libQuery)
            {
                System.Console.WriteLine($"LibraryId: {library.LibraryId}, BookCount: {library.Books.Count}, Name: {library.Name}");

                foreach (var book in library.Books)
                {
                    System.Console.WriteLine($" LibraryId: {book.LibraryId}, Book.Library: {book.Library.LibraryId}, BookId: {book.BookId}, Name: {book.Name}");
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 加载书本
        /// </summary>
        private static void LoadBooks(BookDbContext db)
        {
            var bookQuery = db.Books
                .Include(b => b.Library)
                .ToList();

            foreach (var book in db.Books)
            {
                Console.WriteLine($"BookId: {book.BookId}, LibraryName: {book.Library?.Name}, Name: {book.Name}");
            }
        }

        /// <summary>
        /// 初始化数据库，创建数据
        /// </summary>
        static void Init(IServiceProvider sp)
        {
            using (var scope = sp.CreateScope())
            using (var db = scope.ServiceProvider.GetService<BookDbContext>())
            {
                if (!db.Database.EnsureCreated())
                    return;

                db.Libraries.RemoveRange(db.Libraries);
                db.SaveChanges();

                var newLibrary1 = new Library
                {
                    Name = "国家图书馆",
                };

                newLibrary1.Books.AddRange(
                         new List<Book>
                         {
                            new Book { Name = "史记" },
                            new Book { Name = "资治通鉴" },
                         });

                db.Libraries.Add(newLibrary1);

                var newLibrary2 = new Library
                {
                    Name = "首都图书馆",
                };

                newLibrary2.Books.AddRange(
                    new List<Book>
                    {
                        new Book { Name = "论语" },
                        new Book { Name = "大学" },
                        new Book { Name = "中庸" },
                        new Book { Name = "孟子" },
                    });
                db.Libraries.Add(newLibrary2);

                db.SaveChanges();
            }

            #endregion

        }
    }
}
