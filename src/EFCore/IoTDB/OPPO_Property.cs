using System.Collections.Generic;

namespace EFCore.IoTDB
{
    /// <summary>
    /// 属性
    /// </summary>
    public class OPPO_Property
    {
        /// <summary>
        /// 属性序号（主键）
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// 属性英文名
        /// </summary>
        public string name1 { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 属性描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 服务属性
        /// </summary>
        public List<OPPO_ServiceProperty> ServiceProperties { get; set; }
    }
}
