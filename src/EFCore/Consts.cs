﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore
{
    public static class Consts
    {

        /// <summary>
        /// 服务器根证书
        /// </summary>
        public const string ServerRootCert = @"-----BEGIN CERTIFICATE-----
MIICPTCCAeKgAwIBAgIJALOcMrrG/xidMAoGCCqGSM49BAMCMHExCzAJBgNVBAYT
AkNOMRIwEAYDVQQIDAlHdWFuZ2RvbmcxETAPBgNVBAcMCFNoZW56aGVuMQ0wCwYD
VQQKDARPUFBPMRkwFwYDVQQLDBBPUFBPIElvVCBSb290IENBMREwDwYDVQQDDAhv
cHBvLmNvbTAeFw0xOTEwMjEwNzM2MzBaFw0zOTEwMTYwNzM2MzBaMHExCzAJBgNV
BAYTAkNOMRIwEAYDVQQIDAlHdWFuZ2RvbmcxETAPBgNVBAcMCFNoZW56aGVuMQ0w
CwYDVQQKDARPUFBPMRkwFwYDVQQLDBBPUFBPIElvVCBSb290IENBMREwDwYDVQQD
DAhvcHBvLmNvbTBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABLeBklUaRqg6IhOz
JHxlmemgh45leQrkm+6t+WrLS2kLEAN9voCka+V58ETMliJ+ZPveUdhaHT3CV2h6
XLNQpn6jYzBhMB0GA1UdDgQWBBRAgBYzbkSp+FCO/SC8yI80NpYXATAfBgNVHSME
GDAWgBRAgBYzbkSp+FCO/SC8yI80NpYXATAPBgNVHRMBAf8EBTADAQH/MA4GA1Ud
DwEB/wQEAwICBDAKBggqhkjOPQQDAgNJADBGAiEAvo3OtK/jPmL9oTYtv94AZf/C
WFVGzSnxK4B/oRlBg38CIQDLCfVxyoQZ9SQ+dqFhtq9zuAsggFvBR53ZaHkWLvSb
Uw==
-----END CERTIFICATE-----";

        /// <summary>
        /// 产品证书
        /// </summary>
        public const string ProductCert = @"-----BEGIN CERTIFICATE-----
MIICDTCCAbKgAwIBAgIIAmg56nZrfAAwCgYIKoZIzj0EAwIwcDELMAkGA1UEBhMC
Q04xEjAQBgNVBAgMCUd1YW5nZG9uZzERMA8GA1UEBwwIU2hlbnpoZW4xDTALBgNV
BAoMBE9QUE8xFDASBgNVBAsMC0RldiBSb290IENBMRUwEwYDVQQDDAxkZXYub3Bw
by5jb20wHhcNMTgxMjMxMTYwMDAwWhcNMjgxMjMxMTYwMDAwWjBAMQ4wDAYDVQQD
DAVQMTVMbTENMAsGA1UECgwET1BQTzELMAkGA1UEBhMCQ04xEjAQBgNVBAgMCUd1
YW5nZG9uZzBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABDizGTB1DiaZrAaUng+7
qrN9mFUHG1bh3ni1eHL0GnF15v3znzJdNjPrH4Yb4YD4hDRBZ/n8xm2GsB0YroPM
+xOjZjBkMB0GA1UdDgQWBBQuP8GuQBjGFbUF2NYS78/UC+1/rjAfBgNVHSMEGDAW
gBTFNYGy2BwbHdf1rxc7+52m/yljpjASBgNVHRMBAf8ECDAGAQH/AgEAMA4GA1Ud
DwEB/wQEAwICBDAKBggqhkjOPQQDAgNJADBGAiEAgU83Uyzme78vs3n27m0qhmOh
jhYdKiBSCxsc2DCWIqgCIQChXMlNp1oH75woSBFDM7Sl7w9u15WUo8osT3iFpWdc
zw==
-----END CERTIFICATE-----
";

        /// <summary>
        /// 设备证书
        /// </summary>
        public const string DeviceCert = @"-----BEGIN CERTIFICATE-----
MIIBbzCCARWgAwIBAgIBAzAKBggqhkjOPQQDAjBAMQ4wDAYDVQQDDAVQMTVMbTEN
MAsGA1UECgwET1BQTzELMAkGA1UEBhMCQ04xEjAQBgNVBAgMCUd1YW5nZG9uZzAg
Fw0yMDA3MTYwNzE5MDRaGA8yMTIwMDYyMjA3MTkwNFowFDESMBAGA1UEAwwJRDFF
VERoaTNhMFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEg7yLzvDEsvPBb/hr6oo4
4tWUBA4IrnCHOsxloeGD+B92HarJdZ9EwJ8E/e0x2N73i1Warz0gGAaHDKh27wuu
O6MqMCgwDgYDVR0PAQH/BAQDAgWgMBYGA1UdJQEB/wQMMAoGCCsGAQUFBwMCMAoG
CCqGSM49BAMCA0gAMEUCIQDs6BjHtEinoVMPEfjDvkqceDm30WpwLyFb7TmDPyqR
oAIgBRV8Uqg/QC6OaTEpOpR+7zCjt70DOSWis5HkNuVrj0k=
-----END CERTIFICATE-----
";

        /// <summary>
        /// 设备私钥
        /// </summary>
        public const string DevicePrivateKey = @"-----BEGIN EC PRIVATE KEY-----
MHcCAQEEIBQztkDtRYExYjTFWwLUQeUap/1hUnhphs+Nskmz4dgvoAoGCCqGSM49
AwEHoUQDQgAEg7yLzvDEsvPBb/hr6oo44tWUBA4IrnCHOsxloeGD+B92HarJdZ9E
wJ8E/e0x2N73i1Warz0gGAaHDKh27wuuOw==
-----END EC PRIVATE KEY-----
";

    }
}
