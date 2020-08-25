using System.Collections.Generic;

namespace EFCore.IoTDB
{
    public class OPPO_Service
    {
        /// <summary>
        /// 服务序号（主键）
        /// </summary>
        public int siid { get; set; }

        /// <summary>
        /// 服务英文名
        /// </summary>
        public string name1 { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 服务描述
        /// </summary>
        public string description { get; set; }
    }
}
