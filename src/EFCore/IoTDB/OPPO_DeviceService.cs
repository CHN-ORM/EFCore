using System.Collections.Generic;

namespace EFCore.IoTDB
{
    public class OPPO_DeviceService
    {
        /// <summary>
        /// 设备类型id（主键）
        /// </summary>
        public int typeid { get; set; }

        /// <summary>
        /// 服务id
        /// </summary>
        public int siid { get; set; }

        /// <summary>
        /// 导航-子设备类型
        /// </summary>
        public OPPO_DeviceType DeviceType { get; set; }

        /// <summary>
        /// 导肮-服务
        /// </summary>
        public OPPO_Service Service { get; set; }
    }
}
