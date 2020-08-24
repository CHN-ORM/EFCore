using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.IoTDB
{
    public class IoTDbContext : DbContext
    {
        #region 公共属性

        /// <summary>
        /// 网关信息
        /// </summary>
        public DbSet<OPPO_Gateway> OPPO_Gateway { get; set; }

        /// <summary>
        /// 子设备信息
        /// </summary>
        public DbSet<OPPO_Device> OPPO_Device { get; set; }
        
        /// <summary>
        /// 子设备类型
        /// </summary>
        public DbSet<OPPO_DeviceType> OPPO_DeviceType { get; set; }

        /// <summary>
        /// 设备服务信息
        /// </summary>
        public DbSet<OPPO_Service> OPPO_Service { get; set; }

        /// <summary>
        /// 服务属性信息
        /// </summary>
        public DbSet<OPPO_Property> OPPO_Property { get; set; }

        /// <summary>
        /// 设备服务关系（多对多）
        /// </summary>
        public DbSet<OPPO_DeviceService> OPPO_DeviceService { get; set; }

        /// <summary>
        /// /服务属性关系（多对多）
        /// </summary>
        public DbSet<OPPO_ServiceProperty> OPPO_ServiceProperty { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="options"></param>
        public IoTDbContext(DbContextOptions<IoTDbContext> options)
            : base(options) { }

        #endregion

        #region 重写方法

        /// <summary>
        /// 模型创建方法
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 自定义

            // 网关
            modelBuilder.Entity<OPPO_Gateway>()
                .HasKey(g => g.pid);

            // 子设备
            var deviceBuilder = modelBuilder.Entity<OPPO_Device>();
            deviceBuilder.HasKey(d => d.vid);
            deviceBuilder
                .HasOne(d => d.Type)
                .WithMany(t => t.Devices);

            // 设备类型
            modelBuilder.Entity<OPPO_DeviceType>()
                .HasKey(t => t.typeid);

            // 设备服务
            modelBuilder.Entity<OPPO_Service>()
                .HasKey(s => s.siid);

            // 服务属性
            modelBuilder.Entity<OPPO_Property>()
                .HasKey(s =>  s.pid);

            // 子设备服务关系配置
            var subDeviceServiceBuilder = modelBuilder.Entity<OPPO_DeviceService>();
            subDeviceServiceBuilder.HasKey(s => new
                {
                    s.typeid,
                    s.siid,
                });
            subDeviceServiceBuilder
                .HasOne(m => m.Service)
                .WithMany(s => s.DeviceServices);
            subDeviceServiceBuilder
                .HasOne(s => s.DeviceType)
                .WithMany(s => s.DeviceServices);

            // 服务属性关系配置
            var servicePropertyBuilder = modelBuilder.Entity<OPPO_ServiceProperty>();
            servicePropertyBuilder.HasKey(sp => new
            {
                sp.siid,
                sp.pid,
            });
            servicePropertyBuilder
                .HasOne(sp => sp.Service)
                .WithMany(s => s.ServiceProperties);
            servicePropertyBuilder
                .HasOne(sp => sp.Property)
                .WithMany(p => p.ServiceProperties);
        }

        #endregion
    }
}
