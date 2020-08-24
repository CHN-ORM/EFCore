using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCore
{
    public class Startup
    {
        public void ConfiguraServices(IServiceCollection services)
        {
            services.AddDbContextPool<BookDbContext>(options =>
            {
                ILoggerFactory loggerFactory = LoggerFactory.Create(
                    builder =>
                    {
                        builder
                        //.AddFilter((category, level) =>
                        //    level == LogLevel.Information
                        //    && category.EndsWith("Connection", StringComparison.Ordinal))
                        .AddConsole();
                    }
                );
                options
                    .UseSqlite("Data Source=books.db")
                    //.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=books;Integrated Security=True")
                    //.UseInMemoryDatabase("books")
                    .UseLoggerFactory(loggerFactory);
            });
        }
    }
}
