﻿using System;
using System.Linq;
using System.Collections.Generic;
using EFCore.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using EFCore.IoTDB;
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
                  //options.UseSqlite("Data Source=IoTDb.db"));
                  options.UseMySql("Server=localhost;port=3307;Database=IotDb;User=root;Password=357592895;"));

            // 创建服务提供程序
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<IoTDbContext>();

                await InitDbAsync(db);

                Console.WriteLine($"Gateway count: {db.OPPO_Gateway.Count()}");
                //foreach (var item in db.Gateways)
                //{
                //    Console.WriteLine(item.vid);
                //}

                Console.WriteLine($"Device count: {db.OPPO_Device.Count()}");
                //foreach (var item in db.Devices)
                //{
                //    Console.WriteLine(item.vid);
                //}

                Console.WriteLine($"Property count: {db.OPPO_Property.Count()}");
                //foreach (var item in db.Properties)
                //{
                //    Console.WriteLine(item);
                //}

                Console.WriteLine($"ServiceProperty count: {db.OPPO_ServiceProperty.Count()}");
                //foreach (var item in db.ServiceProperties)
                //{
                //    Console.WriteLine(item);
                //}
                Console.WriteLine($"DeviceService count: {db.OPPO_DeviceService.Count()}");
                Console.WriteLine();

                Console.WriteLine($"设备表");
                var queryDev = db.OPPO_Device
                    .Include(d => d.Type);
                foreach (var item in queryDev)
                {
                    Console.WriteLine($"Name: {item.dev_name}, Type: {item.Type.name}");
                }

                Console.WriteLine($"服务属性表");
                var querySP = db.OPPO_ServiceProperty
                    .Include(sp => sp.Property)
                    .Include(sp => sp.Service)
                    .OrderBy(s => s.siid);
                foreach (var item in querySP)
                {
                    Console.WriteLine($"siid: {item.siid}, pid: {item.iid}, Service: {item.Service.name}, Property: {item.Property.name}");
                }
                Console.WriteLine();

                Console.WriteLine($"设备服务表");
                var queryDS = db.OPPO_DeviceService
                    .Include(ds => ds.Service)
                    .Include(ds => ds.DeviceType)
                    .OrderBy(ds => ds.typeid)
                    .ThenBy(ds => ds.siid);
                foreach (var item in queryDS)
                {
                    Console.WriteLine($"typeid: {item.typeid}, siid: {item.siid}, Name: {item.Service.name}, Type: {item.DeviceType.name}");
                }
                Console.WriteLine();

                // 获取设备的属性列表
                var props = db.OPPO_Device.Join<OPPO_Device, OPPO_DeviceType, int, Prop>(db.OPPO_DeviceType, d => d.typeid, d => d.typeid, (d, t) =>
                new Prop { Device = d, Type = t });

                foreach (var prop in props)
                {
                    Console.WriteLine($"设备名称: {prop.Device.dev_name}, 类型: {prop.Type.name}");
                }
            }

        }

        public class Prop
        {
            public OPPO_Device Device { get; set; }
            public OPPO_DeviceType Type { get; set; }
        }

        private static async Task InitDbAsync(IoTDbContext db)
        {
            // 重新生成数据库
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            Console.WriteLine($"Database Created...");

            // 初始化数据库

            // 添加网关信息
            await db.OPPO_Gateway.AddRangeAsync(new List<OPPO_Gateway>
                {
                    new OPPO_Gateway { pid = "1", vid = "v58x" },
                    new OPPO_Gateway { pid = "2", vid = "v53x" },
                    new OPPO_Gateway { pid = "3", vid = "v52x" },
                });

            // 添加子设备
            await db.OPPO_Device.AddRangeAsync(new OPPO_Device
            {
                equip_no = 1,
                did = "rtk3bjzm",
                pid = "rtaK",
                vid = "c0a80002",
                typeid = 1000,
                dev_name = "路由器",
                dev_pub_key = "04D4B6B498323E078B5BBA19ADC2D290622E00C57C61F895C867E06C34DEF399CECC4580B58054CDE892201B2B6A36B414C535EEE127C0CEAEF51375707AB1B872",
            });

            await db.OPPO_DeviceType.AddRangeAsync(
                new OPPO_DeviceType { typeid = 1000, name1 = "coldHeatSourceSystem", name = "冷热源", },
                new OPPO_DeviceType { typeid = 1001, name1 = "vav", name = "空调末端", },
                new OPPO_DeviceType { typeid = 1002, name1 = "eaf", name = "排风机", },
                new OPPO_DeviceType { typeid = 1003, name1 = "fcu", name = "风机盘管", },
                new OPPO_DeviceType { typeid = 1004, name1 = "ahu", name = "AHU", });

            // 添加服务
            await db.OPPO_Service.AddRangeAsync(
                new OPPO_Service { siid = 101000, name1 = "eaf", name = "排风机" }
                , new OPPO_Service { siid = 101001, name1 = "fcu", name = "风机盘管" }
                , new OPPO_Service { siid = 100102, name1 = "saf", name = "送风机" }
                , new OPPO_Service { siid = 101003, name1 = "ahu", name = "空气处理单元" }
                , new OPPO_Service { siid = 101004, name1 = "vav", name = "vav空调末端" }
                , new OPPO_Service { siid = 101005, name1 = "chiller", name = "冷却塔" }
                , new OPPO_Service { siid = 101006, name1 = "cwPump", name = "冷却泵", description = "Condenser Water Pump" }
                , new OPPO_Service { siid = 101007, name1 = "chwPump", name = "冷冻泵", description = "Chilled Water Pump" }
                , new OPPO_Service { siid = 101008, name1 = "valve", name = "阀门", description = "valve" }
                , new OPPO_Service { siid = 101009, name1 = "iceStorageTank", name = "蓄冰槽" }
                , new OPPO_Service { siid = 101010, name1 = "cwPipe", name = "冷却水管", description = "condenser water pipe" }
                , new OPPO_Service { siid = 101011, name1 = "chwPipe", name = "冷冻水管", description = "Chilled Water Pipe" }
                , new OPPO_Service { siid = 101012, name1 = "baseLoadChiller", name = "机载主机", description = "chiller" }
                , new OPPO_Service { siid = 100036, name1 = "tempHumSensor", name = "温湿度" }
                , new OPPO_Service { siid = 101013, name1 = "doubleModeChiller", name = "双工况主机" }
                , new OPPO_Service { siid = 101014, name1 = "glycolPump", name = "乙二醇泵" }
                , new OPPO_Service { siid = 101015, name1 = "coldHeatSourceSystem", name = "冷热源系统" }
                , new OPPO_Service { siid = 101016, name1 = "plateExchanger", name = "板换" }
                , new OPPO_Service { siid = 101017, name1 = "faf", name = "新风机" }
                , new OPPO_Service { siid = 101018, name1 = "raf", name = "回风机" }
                , new OPPO_Service { siid = 101019, name1 = "mfa", name = "手报", description = "manualFireAlarm" }
                , new OPPO_Service { siid = 101020, name1 = "ava", name = "声光报警", description = "audiableAndVisualAlarm" }
                , new OPPO_Service { siid = 101021, name1 = "fireHydrant", name = "消防栓", description = "Fire Hydrant" }
                , new OPPO_Service { siid = 101022, name1 = "broadcastingSys", name = "广播模块" }
                );

            // 添加属性
            await db.OPPO_Property.AddRangeAsync(
                // 排风机
                //new OPPO_Property { pid = , propid = "power", type = "int", point_no = 1, point_type = "C" }
                new OPPO_Property { pid = 201000, type = "bool", name1 = "mannulAutoSta", name = "故障状态", description = "Fault Status" }
                , new OPPO_Property { pid = 201001, type = "bool", name1 = "faultSta", name = "手自动状态", description = "Mannul&auto status" }
                , new OPPO_Property { pid = 201002, type = "bool", name1 = "pressureDiffSta", name = "压差状态", description = "Pressure differential Status" }
                , new OPPO_Property { pid = 201003, type = "uint32", name1 = "freqSetting", name = "频率控制", description = "Exhaust Fan Frequence Setting" }
                , new OPPO_Property { pid = 201004, type = "uint32", name1 = "freqFeedback", name = "频率反馈", description = "Exhaust Fan Frequence Feedback" }
                , new OPPO_Property { pid = 201005, type = "uint32", name1 = "vlvOpening", name = "阀门开度设定", description = "Valve Opening" }
                , new OPPO_Property { pid = 201006, type = "uint32", name1 = "vlvFeedback", name = "阀门开度反馈", description = "Valve Opening Feedback" }
                , new OPPO_Property { pid = 201007, type = "bool", name1 = "currentTemperature", name = "初效滤网状态", description = "Primary Filter Status" }
                , new OPPO_Property { pid = 201008, type = "bool", name1 = "mediumFilterSta", name = "中效滤网状态", description = "Medium Filter Status" }
                , new OPPO_Property { pid = 201009, type = "bool", name1 = "finalFilterSta", name = "终效滤网状态", description = "Final Filter Status" }

                );

            // 添加服务属性
            await db.OPPO_ServiceProperty.AddRangeAsync(
                // 排风机
                new OPPO_ServiceProperty { siid = 101000, pid = 201000, iid = 2, point_no = 2, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101000, pid = 201001, iid = 3, point_no = 3, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101000, pid = 201002, iid = 4, point_no = 4, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101000, pid = 201003, iid = 5, point_no = 5, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101000, pid = 201004, iid = 6, point_no = 6, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101000, pid = 201005, iid = 7, point_no = 7, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101000, pid = 201006, iid = 8, point_no = 8, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101000, pid = 201007, iid = 9, point_no = 9, point_type = "C" }
                // 风机盘管
                , new OPPO_ServiceProperty { siid = 101001, pid = 201000, iid = 2, point_no = 2, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101001, pid = 201001, iid = 3, point_no = 3, point_type = "C" }
                //, new OPPO_ServiceProperty { siid = 101001, pid = 201000, iid = 4, point_no = 4, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101001, pid = 201003, iid = 5, point_no = 5, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101001, pid = 201004, iid = 6, point_no = 6, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101001, pid = 201005, iid = 7, point_no = 7, point_type = "C" }
                , new OPPO_ServiceProperty { siid = 101001, pid = 201006, iid = 8, point_no = 8, point_type = "C" }
                );

            // 添加设备服务
            await db.OPPO_DeviceService.AddRangeAsync(
                new OPPO_DeviceService { typeid = 1000, siid = 101000 }
                );

            await db.SaveChangesAsync();

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
