using System.Collections.Generic;

namespace EFCore.Entity
{
    public class OPPO_Service
    {
        public int siid { get; set; }
        public string service_name { get; set; }

        public List<OPPO_SubDeviceService> SubDeviceServices { get; set; }

        public List<OPPO_ServiceProperty> Properties { get; set; }
    }
}
