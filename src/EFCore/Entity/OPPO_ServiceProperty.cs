namespace EFCore.Entity
{
    /// <summary>
    /// 测点属性映射
    /// </summary>
    public class OPPO_ServiceProperty
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public int siid { get; set; }

        /// <summary>
        /// 属性Id
        /// </summary>
        public int iid { get; set; }

        /// <summary>
        /// 测点号
        /// </summary>
        public int point_no { get; set; }

        /// <summary>
        /// 测点类型, 'C','X','S'
        /// </summary>
        public string point_type { get; set; }
    }
}
