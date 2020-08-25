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
        /// /设备属性关系（多对多）
        /// </summary>
        public DbSet<OPPO_DeviceProperty> OPPO_DeviceProperty { get; set; }

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
                .HasOne(d => d.Type);

            // 设备类型
            var typeBuilder = modelBuilder.Entity<OPPO_DeviceType>();
            typeBuilder.HasKey(t => t.typeid);
            typeBuilder
                .HasMany(t => t.DevicePropertis)
                .WithOne(dp => dp.Type);

            // 设备服务
            modelBuilder.Entity<OPPO_Service>()
                .HasKey(s => s.siid);

            // 服务属性
            modelBuilder.Entity<OPPO_Property>()
                .HasKey(s =>  s.pid);

            var dpBuilder = modelBuilder.Entity<OPPO_DeviceProperty>();
            dpBuilder.HasKey(dp => new { dp.typeid, dp.siid, dp.pid });
            dpBuilder
                .HasOne(dp => dp.Property)
                .WithMany(p => p.DeviceProperties);
            dpBuilder.HasOne(dp => dp.Service);
        }

        #endregion
    }
}
