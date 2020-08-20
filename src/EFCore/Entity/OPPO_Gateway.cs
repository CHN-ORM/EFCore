namespace EFCore.Entity
{
    /// <summary>
    /// 网关信息
    /// </summary>
    public class OPPO_Gateway
    {
        public string did { get; set; }
        public string pid { get; set; }
        public string cid { get; set; }
        public string vid { get; set; }
        public string mac { get; set; }
        public string model { get; set; }
        public string manufacturer { get; set; }
        public string brand { get; set; }
        public string sn { get; set; }
        public string root_cert { get; set; }
        public string product_cert { get; set; }
        public string dev_cert { get; set; }
        public string dev_pri_key { get; set; }
        public string dev_pub_key { get; set; }
        public string ecdh_pri_key { get; set; }
        public string pin { get; set; }
        public string dev_name { get; set; }
        public string bind_status { get; set; }
        public string connect_type { get; set; }
        public string config_type { get; set; }
        public string soft_ver { get; set; }
        public string hard_ver { get; set; }
        public string parent_did { get; set; }
        public string ssid { get; set; }
        public string psk { get; set; }
        public string bssid { get; set; }
        public string rssi { get; set; }
        public string ip { get; set; }
        public string netmask { get; set; }
        public string gateway { get; set; }
        public string dev_time { get; set; }
        public string timezone { get; set; }
        public string logitude { get; set; }
        public string latitude { get; set; }
        public string cloud_url { get; set; }
        public string access_url { get; set; }
        public string homeid { get; set; }
        public string homeaddress { get; set; }
        public string homesign { get; set; }
        public string homepubkey { get; set; }
    }
}
