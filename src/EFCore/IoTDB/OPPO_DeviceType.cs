using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.IoTDB
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public class OPPO_DeviceType
    {
        /// <summary>
        /// 类型id（主键）
        /// </summary>
        public int typeid { get; set; }

        /// <summary>
        /// 类型名称（英文）
        /// </summary>
        public string name1 { get; set; }

        /// <summary>
        /// 类型名称（中文）
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 服务列表
        /// </summary>
        public IEnumerable<OPPO_DeviceProperty> DevicePropertis { get; set; }
    }
}
