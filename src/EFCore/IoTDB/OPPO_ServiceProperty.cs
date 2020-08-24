using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.IoTDB
{
    public class OPPO_ServiceProperty
    {
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
        /// 服务导航属性
        /// </summary>
        public OPPO_Service Service { get; set; }

        /// <summary>
        /// 属性导航属性
        /// </summary>
        public OPPO_Property Property { get; set; }
    }
}
