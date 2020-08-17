using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.Entity
{
    public class IoTDbContext : DbContext
    {
        public DbSet<OPPO_Gateway> Gateways { get; set; }


        public IoTDbContext(DbContextOptions<IoTDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 自定义
            modelBuilder.Entity<OPPO_Gateway>()
                .HasKey(nameof(OPPO_Gateway.vid));
                //.HasNoKey();
        }
    }
}
