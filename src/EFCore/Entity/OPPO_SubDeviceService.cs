using System.Collections.Generic;

namespace EFCore.Entity
{
    public class OPPO_SubDeviceService
    {
        /// <summary>
        /// 子设备id
        /// </summary>
        public string vid { get; set; }

        /// <summary>
        /// 服务id
        /// </summary>
        public int siid { get; set; }

        /// <summary>
        /// 导航-子设备
        /// </summary>
        public OPPO_SubDevice SubDevice { get; set; }

        /// <summary>
        /// 导肮-服务
        /// </summary>
        public OPPO_Service Service { get; set; }
    }
}
