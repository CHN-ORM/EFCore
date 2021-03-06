﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.IoTDB
{
    public class OPPO_DeviceProperty
    {
        /// <summary>
        /// 设备号（复合主键）
        /// </summary>
        public int equip_no { get; set; }

        /// <summary>
        /// 设备类型id（复合主键）
        /// </summary>
        public int typeid { get; set; }

        /// <summary>
        /// 服务id（复合主键）
        /// </summary>
        public int siid { get; set; }

        /// <summary>
        /// 属性id（复合主键）
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// 属性iid
        /// </summary>
        public int iid { get; set; }

        /// <summary>
        /// 测点号
        /// </summary>
        public int point_no { get; set; }

        /// <summary>
        /// 测点类型（'C', 'X', 'S'）
        /// </summary>
        public string point_type { get; set; }

        /// <summary>
        /// 设备类型导航属性
        /// </summary>
        public OPPO_DeviceType Type { get; set; }

        /// <summary>
        /// 服务导航属性
        /// </summary>
        public OPPO_Service Service { get; set; }

        /// <summary>
        /// 属性导航属性
        /// </summary>
        public OPPO_Property Property { get; set; }

        /// <summary>
        /// 设备导航属性
        /// </summary>
        public OPPO_Device Device { get; set; }

    }
}
