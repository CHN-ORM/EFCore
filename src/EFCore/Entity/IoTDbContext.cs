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
        public DbSet<OPPO_SubDevice> SubDevices { get; set; }
        public DbSet<OPPO_ServiceProperty> PointProperties { get; set; }
        public DbSet<OPPO_Service> Services { get; set; }
        public DbSet<OPPO_SubDeviceService> SubDeviceServices { get; set; }


        public IoTDbContext(DbContextOptions<IoTDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 自定义

            // 网关
            modelBuilder.Entity<OPPO_Gateway>()
                .HasKey(g => g.pid);

            // 子设备
            modelBuilder.Entity<OPPO_SubDevice>()
                .HasKey(p => p.vid);

            // 设备服务
            modelBuilder.Entity<OPPO_Service>()
                .HasKey(s => new
                {
                    s.siid,
                });

            // 服务属性
            modelBuilder.Entity<OPPO_ServiceProperty>()
                .HasKey(s => new
                {
                    s.siid,
                    s.iid,
                });


            // 子设备服务关系配置
            var subDeviceServiceBuilder = modelBuilder.Entity<OPPO_SubDeviceService>();
            subDeviceServiceBuilder.HasKey(s => new
                {
                    s.vid,
                    s.siid,
                });
            subDeviceServiceBuilder
                .HasOne(m => m.Service)
                .WithMany(s => s.SubDeviceServices);
            subDeviceServiceBuilder
                .HasOne(s => s.SubDevice)
                .WithMany(s => s.SubDeviceServices);

        }
    }
}
