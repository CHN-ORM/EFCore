using System;
using System.Linq;
using System.Collections.Generic;
using EFCore.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using EFCore.Entity;
using System.Threading.Tasks;

namespace EFCore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await OutterRelation();
        }

        #region 外关联

        /// <summary>
        /// 外关联
        /// </summary>
        static async Task OutterRelation()
        {
            // 创建服务集合
            IServiceCollection services = new ServiceCollection();
            services.AddDbContextPool<IoTDbContext>(options =>
                options.UseSqlite("Data Source=IoTDb.db"));

            // 创建服务提供程序
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<IoTDbContext>();
                
                // 重新生成数据库
                await db.Database.EnsureDeletedAsync();
                await db.Database.EnsureCreatedAsync();

                // 初始化数据库

                // 添加网关信息
                await db.Gateways.AddRangeAsync(new List<OPPO_Gateway>
                {
                    new OPPO_Gateway { vid = "v58x" },
                    new OPPO_Gateway { vid = "v53x" },
                    new OPPO_Gateway { vid = "v52x" },
                });

                // 添加子设备
                await db.SubDevices.AddRangeAsync(
                    new OPPO_SubDevice
                    {
                        equip_no = 1,
                        did = "rtk3bjzm",
                        pid = "rtaK",
                        vid = "c0a80002",
                        dev_name = "路由器",
                        dev_pub_key = "04D4B6B498323E078B5BBA19ADC2D290622E00C57C61F895C867E06C34DEF399CECC4580B58054CDE892201B2B6A36B414C535EEE127C0CEAEF51375707AB1B872",
                    });

                // 添加属性对应关系
                await db.PointProperties.AddRangeAsync(
                    new OPPO_ServiceProperty { siid = 19456, iid = 1, point_no = 1, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 2, point_no = 2, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 3, point_no = 3, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 4, point_no = 4, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 5, point_no = 5, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 6, point_no = 6, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 7, point_no = 7, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 8, point_no = 8, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 9, point_no = 9, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 10, point_no = 10, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 11, point_no = 11, point_type = "C" },
                    new OPPO_ServiceProperty { siid = 19456, iid = 12, point_no = 12, point_type = "C" }
                    );

                await db.SaveChangesAsync();

                foreach (var item in db.Gateways)
                {
                    Console.WriteLine(item.vid);
                }

                foreach (var item in db.SubDevices)
                {
                    Console.WriteLine(item.vid);
                }

                foreach (var item in db.PointProperties)
                {
                    Console.WriteLine(item);
                }

            }

        }

        #endregion

        #region 图书馆

        /// <summary>
        /// 图书馆
        /// </summary>
        static void Library()
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
