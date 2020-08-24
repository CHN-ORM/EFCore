using System.Collections.Generic;

namespace EFCore.IoTDB
{
    /// <summary>
    /// 子设备信息
    /// </summary>
    public class OPPO_Device
    {
        public int equip_no { get; set; }
        public int typeid { get; set; }
        public string vid { get; set; }
        public string did { get; set; }
        public string pid { get; set; }
        public string cid { get; set; }
        public string dev_name { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string brand { get; set; }
        public string softver { get; set; }
        public string mac { get; set; }
        public string hardver { get; set; }
        public string sn { get; set; }
        public string parent_did { get; set; }
        public int? rssi { get; set; }
        public string bssid { get; set; }
        public string ip { get; set; }
        public string dev_time { get; set; }
        public ulong? utc { get; set; }
        public string timezone { get; set; }
        public string ssid { get; set; }
        public string dev_pub_key { get; set; }
        public bool online { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public OPPO_DeviceType Type { get; set; }
    }
}
