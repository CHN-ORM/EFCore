using System;
using System.Linq;
using System.Collections.Generic;
using EFCore.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using EFCore.IoTDB;
using System.Threading.Tasks;

namespace EFCore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            bool.TryParse("1", out var result);
            Console.WriteLine(result);
            return;

            await OutterRelation();
        }

        #region 外关联

        /// <summary>
        /// 外关联
        /// </summary>
        static async Task OutterRelation()
        {
            // 创建服务集合
            IServiceCollection services = new ServiceCollection();
            services.AddDbContextPool<IoTDbContext>(options =>
                  options.UseSqlite("Data Source=IoTDb.db"));
            //options.UseMySql("Server=localhost;port=3307;Database=IotDb;User=root;Password=357592895;"));

            // 创建服务提供程序
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<IoTDbContext>();

                await InitDbAsync(db);

                Console.WriteLine($"Gateway count: {db.OPPO_Gateway.Count()}");

                Console.WriteLine($"Device count: {db.OPPO_Device.Count()}");

                Console.WriteLine($"Property count: {db.OPPO_Property.Count()}");

                Console.WriteLine();

                Console.WriteLine($"设备表");
                var queryDev = db.OPPO_Device
                    .Include(d => d.DevicePropertis);

                foreach (var item in queryDev)
                {
                    Console.WriteLine($"Name: {item.dev_name}, Type: {item.DevicePropertis.First().Type.name}");
                }

                Console.WriteLine($"设备列表");
                var devs = db.OPPO_Device
                    .Include(d => d.DevicePropertis)
                    .ThenInclude(dp => dp.Property)
                    .ToList();

                foreach (var dev in devs)
                {
                    Console.WriteLine($"设备名：{dev.dev_name}, equip_no: {dev.equip_no}，typeid: {dev.typeid}, props: {dev.DevicePropertis.Count()}");
                    foreach (var prop in dev.DevicePropertis.OrderBy(dp => dp.siid))
                    {
                        Console.WriteLine($" 服务：{prop.Service.name}, 属性：{prop.Property.name}, siid: {prop.siid}, iid: {prop.iid}, type: {prop.Property.type}, point_no: {prop.point_no}, point_type: {prop.point_type}");
                    }
                }
            }
        }

        public class Prop
        {
            public OPPO_Device Device { get; set; }
            public OPPO_DeviceType Type { get; set; }
        }

        private const int EquipNo = 15114;
        private const int TypeId = 1004;

        private static async Task InitDbAsync(IoTDbContext db)
        {
            // 重新生成数据库
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            Console.WriteLine($"Database Created...");

            // 初始化数据库

            // 添加网关信息
            await db.OPPO_Gateway.AddRangeAsync(new List<OPPO_Gateway>
                {
                    new OPPO_Gateway
                    {
                        did = "1ETDhi3a",
                        pid = "15Lm",
                        vid = "3" ,
                        mac = "123456789012345678",
                        pin = "123",

                        root_cert = Consts.ServerRootCert,
                        product_cert = Consts.ProductCert,
                        dev_cert = Consts.DeviceCert,
                        dev_pri_key = Consts.DevicePrivateKey,
                    },
                });

            // 添加子设备
            await db.OPPO_Device.AddRangeAsync(new OPPO_Device
            {
                equip_no = EquipNo,
                did = "rtk3bjzm",
                pid = "rtaK",
                vid = "c0a80002",
                typeid = TypeId,
                dev_name = "路由器",
                dev_pub_key = "04D4B6B498323E078B5BBA19ADC2D290622E00C57C61F895C867E06C34DEF399CECC4580B58054CDE892201B2B6A36B414C535EEE127C0CEAEF51375707AB1B872",
            });

            await db.OPPO_DeviceType.AddRangeAsync(
                new OPPO_DeviceType { typeid = 1000, name1 = "coldHeatSourceSystem", name = "冷热源", },
                new OPPO_DeviceType { typeid = 1001, name1 = "vav", name = "空调末端", },
                new OPPO_DeviceType { typeid = 1002, name1 = "eaf", name = "排风机", },
                new OPPO_DeviceType { typeid = 1003, name1 = "fcu", name = "风机盘管", },
                new OPPO_DeviceType { typeid = 1004, name1 = "ahu", name = "AHU", });

            // 添加服务
            await db.OPPO_Service.AddRangeAsync(
                  new OPPO_Service { /*siid = 101000,*/ siid = 256000, name1 = "eaf", name = "排风机" }
                , new OPPO_Service { /*siid = 101001,*/ siid = 256256, name1 = "fcu", name = "风机盘管" }
                , new OPPO_Service { /*siid = 100102,*/ siid = 256512, name1 = "saf", name = "送风机" }
                , new OPPO_Service { /*siid = 101003,*/ siid = 256768, name1 = "ahu", name = "空气处理单元" }
                , new OPPO_Service { /*siid = 101004,*/ siid = 257024, name1 = "vav", name = "vav空调末端" }
                , new OPPO_Service { /*siid = 101005,*/ siid = 257280, name1 = "chiller", name = "冷却塔" }
                , new OPPO_Service { /*siid = 101006,*/ siid = 257536, name1 = "cwPump", name = "冷却泵", description = "Condenser Water Pump" }
                , new OPPO_Service { /*siid = 101007,*/ siid = 257792, name1 = "chwPump", name = "冷冻泵", description = "Chilled Water Pump" }
                , new OPPO_Service { /*siid = 101008,*/ siid = 258048, name1 = "valve", name = "阀门", description = "valve" }
                , new OPPO_Service { /*siid = 101009,*/ siid = 258304, name1 = "iceStorageTank", name = "蓄冰槽" }
                , new OPPO_Service { /*siid = 101010,*/ siid = 258560, name1 = "cwPipe", name = "冷却水管", description = "condenser water pipe" }
                , new OPPO_Service { /*siid = 101011,*/ siid = 258816, name1 = "chwPipe", name = "冷冻水管", description = "Chilled Water Pipe" }
                , new OPPO_Service { /*siid = 101012,*/ siid = 259072, name1 = "baseLoadChiller", name = "机载主机", description = "chiller" }
                , new OPPO_Service { /*siid = 100036,*/ siid = 9216, name1 = "tempHumSensor", name = "温湿度" }
                , new OPPO_Service { /*siid = 101013,*/ siid = 259328, name1 = "doubleModeChiller", name = "双工况主机" }
                , new OPPO_Service { /*siid = 101014,*/ siid = 259584, name1 = "glycolPump", name = "乙二醇泵" }
                , new OPPO_Service { /*siid = 101015,*/ siid = 259840, name1 = "coldHeatSourceSystem", name = "冷热源系统" }
                , new OPPO_Service { /*siid = 101016,*/ siid = 260096, name1 = "plateExchanger", name = "板换" }
                , new OPPO_Service { /*siid = 101017,*/ siid = 260352, name1 = "faf", name = "新风机" }
                , new OPPO_Service { /*siid = 101018,*/ siid = 260608, name1 = "raf", name = "回风机" }
                , new OPPO_Service { /*siid = 101019,*/ siid = 260864, name1 = "mfa", name = "手报", description = "manualFireAlarm" }
                , new OPPO_Service { /*siid = 101020,*/ siid = 261120, name1 = "ava", name = "声光报警", description = "audiableAndVisualAlarm" }
                , new OPPO_Service { /*siid = 101021,*/ siid = 261376, name1 = "fireHydrant", name = "消防栓", description = "Fire Hydrant" }
                , new OPPO_Service { /*siid = 101022,*/ siid = 261632, name1 = "broadcastingSys", name = "广播模块" }
                );

            // 添加属性
            await db.OPPO_Property.AddRangeAsync(
                // 排风机
                new OPPO_Property { pid = 201000, type = "bool", name1 = "mannulAutoSta", name = "故障状态", description = "Fault Status" }
                , new OPPO_Property { pid = 201001, type = "bool", name1 = "faultSta", name = "手自动状态", description = "Mannul&auto status" }
                , new OPPO_Property { pid = 201002, type = "bool", name1 = "pressureDiffSta", name = "压差状态", description = "Pressure differential Status" }
                , new OPPO_Property { pid = 201003, type = "uint32", name1 = "freqSetting", name = "频率控制", description = "Exhaust Fan Frequence Setting" }
                , new OPPO_Property { pid = 201004, type = "uint32", name1 = "freqFeedback", name = "频率反馈", description = "Exhaust Fan Frequence Feedback" }
                , new OPPO_Property { pid = 201005, type = "uint32", name1 = "vlvOpening", name = "阀门开度设定", description = "Valve Opening" }
                , new OPPO_Property { pid = 201006, type = "uint32", name1 = "vlvFeedback", name = "阀门开度反馈", description = "Valve Opening Feedback" }

                , new OPPO_Property { pid = 201007, type = "bool", name1 = "currentTemperature", name = "初效滤网状态", description = "Primary Filter Status" }
                , new OPPO_Property { pid = 201008, type = "bool", name1 = "mediumFilterSta", name = "中效滤网状态", description = "Medium Filter Status" }
                , new OPPO_Property { pid = 201009, type = "bool", name1 = "finalFilterSta", name = "终效滤网状态", description = "Final Filter Status" }
                , new OPPO_Property { pid = 201010, type = "bool", name1 = "elecPrecipitatorFault", name = "静电除尘器故障", description = "Final Filter Status" }
                , new OPPO_Property { pid = 201011, type = "bool", name1 = "elecPrecipitatorPower", name = "静电除尘器状态", description = "Final Filter Status" }
                , new OPPO_Property { pid = 201012, type = "uint32", name1 = "coolingVlvFeedback1", name = "冷水阀开度反馈1", description = "Cooling Valve Opening Feedback" }
                , new OPPO_Property { pid = 201013, type = "uint32", name1 = "coolingVlvFeedback2", name = "冷水阀开度反馈2", description = "Cooling Valve Opening Feedback" }
                , new OPPO_Property { pid = 201014, type = "uint32", name1 = "coolingVlvOpening1", name = "冷水阀开度调节1", description = "Cooling Valve Opening" }
                , new OPPO_Property { pid = 201015, type = "uint32", name1 = "coolingVlvOpening2", name = "冷水阀开度调节2", description = "Cooling Valve Opening" }
                , new OPPO_Property { pid = 201016, type = "uint32", name1 = "heatingVlvFeedback", name = "热水阀开度调节", description = "Heating Valve Opening Feedback" }
                , new OPPO_Property { pid = 201017, type = "uint32", name1 = "heatingVlvOpening", name = "热水阀开度反馈", description = "Heating Valve Opening" }
                , new OPPO_Property { pid = 201018, type = "bool", name1 = "humidifierPower", name = "加湿器启停状态", description = "Humidifier Power On / Off" }
                , new OPPO_Property { pid = 201019, type = "bool", name1 = "humidifiersFault", name = "加湿器故障状态", description = "Humidifier Fault" }
                , new OPPO_Property { pid = 201020, type = "bool", name1 = "elecHeatingFault1", name = "电加热故障状态1", description = "Electrical Heating Fault" }
                , new OPPO_Property { pid = 201021, type = "bool", name1 = "elecHeatingMannulAutoSta1", name = "电加热手自动状态1", description = "Electrical Heating Mannual Auto Status" }
                , new OPPO_Property { pid = 201022, type = "bool", name1 = "elecHeatingPower1", name = "电加热启停状态1", description = "Electrical Heating Power" }
                , new OPPO_Property { pid = 201023, type = "uint32", name1 = "elecHeatingTemp1", name = "电加热温度设定1", description = "Electrical Heating temperature" }
                , new OPPO_Property { pid = 201024, type = "bool", name1 = "elecHeatingFault2", name = "电加热故障状态2", description = "Humidifier Fault" }
                , new OPPO_Property { pid = 201025, type = "bool", name1 = "elecHeatingMannulAutoSta2", name = "电加热手自动状态2", description = "Electrical Heating Mannual Auto Status" }
                , new OPPO_Property { pid = 201026, type = "bool", name1 = "elecHeatingPower2", name = "电加热启停状态2", description = "Electrical Heating Power" }
                , new OPPO_Property { pid = 201027, type = "uint32", name1 = "elecHeatingTemp2", name = "电加热温度设定2", description = "Electrical Heating temperature" }
                , new OPPO_Property { pid = 201028, type = "bool", name1 = "elecHeatingPower3", name = "电加热启停状态3", description = "Electrical Heating Power" }
                , new OPPO_Property { pid = 201029, type = "bool", name1 = "elecHeatingFault3", name = "电加热故障状态3", description = "Humidifier Fault3" }

                , new OPPO_Property { pid = 201030, type = "uint32", name1 = "targetCo2", name = "二氧化碳浓度设定", description = "TargetCo2" }
                , new OPPO_Property { pid = 201031, type = "uint32", name1 = "currentPres1", name = "压力1", description = "Pressure1" }
                , new OPPO_Property { pid = 201032, type = "uint32", name1 = "currentPres2", name = "压力2", description = "Pressure2" }
                , new OPPO_Property { pid = 201033, type = "uint32", name1 = "targetPres1", name = "压力设定1", description = "Pressure1setting" }
                , new OPPO_Property { pid = 201034, type = "uint32", name1 = "targetPres2", name = "压力设定2", description = "Pressure2setting" }
                , new OPPO_Property { pid = 201035, type = "uint32", name1 = "minimumFreq", name = "频率最小值设定", description = "" }
                , new OPPO_Property { pid = 201036, type = "uint32", name1 = "maxHumidity", name = "湿度上限设定值", description = "" }
                , new OPPO_Property { pid = 201037, type = "uint32", name1 = "elecHeatingTempThreshold", name = "电加热报警温度设定值", description = "" }
                , new OPPO_Property { pid = 201038, type = "uint32", name1 = "minCoolingVlvTemp", name = "冷水阀最小温度设定值1", description = "" }
                , new OPPO_Property { pid = 201039, type = "bool", name1 = "highPresHumidification", name = "高压加湿控制", description = "" }
                , new OPPO_Property { pid = 201040, type = "uint32", name1 = "humidifierOpeningFeedback", name = "加湿器开度反馈", description = "" }
                , new OPPO_Property { pid = 201041, type = "uint32", name1 = "humidifierOpening", name = "加湿器开度控制", description = "" }
                , new OPPO_Property { pid = 201042, type = "uint32", name1 = "currentTemperature3", name = "送风温度3", description = "" }
                , new OPPO_Property { pid = 201043, type = "uint32", name1 = "maxVlvOpening", name = "阀开度最大值", description = "" }
                , new OPPO_Property { pid = 201044, type = "uint32", name1 = "airflowRate", name = "实际送风量", description = "" }
                , new OPPO_Property { pid = 201045, type = "uint32", name1 = "maxAirflowRate", name = "最大风量设定", description = "" }
                , new OPPO_Property { pid = 201046, type = "bool", name1 = "hlSta", name = "高液位状态", description = "" }
                , new OPPO_Property { pid = 201047, type = "bool", name1 = "llSta", name = "低液位状态", description = "" }
                , new OPPO_Property { pid = 201048, type = "bool", name1 = "vibrationSta", name = "震动状态", description = "" }
                , new OPPO_Property { pid = 201049, type = "bool", name1 = "devAvailable", name = "控制可用", description = "" }
                , new OPPO_Property { pid = 201050, type = "bool", name1 = "devAbnormalSta", name = "控制异常", description = "" }
                , new OPPO_Property { pid = 201051, type = "bool", name1 = "waterFlowSta", name = "水流状态", description = "" }
                , new OPPO_Property { pid = 201052, type = "uint32", name1 = "supplyWaterTemp", name = "供水温度", description = "" }
                , new OPPO_Property { pid = 201053, type = "uint32", name1 = "supplyWaterPres", name = "供水压力", description = "" }
                , new OPPO_Property { pid = 201054, type = "uint32", name1 = "returnWaterTemp", name = "回水温度", description = "" }
                , new OPPO_Property { pid = 201055, type = "uint32", name1 = "returnWaterPres", name = "回水压力", description = "" }
                , new OPPO_Property { pid = 201056, type = "uint32", name1 = "supplyWaterRate", name = "供水流量", description = "" }
                , new OPPO_Property { pid = 201057, type = "uint32", name1 = "returnWaterRate", name = "回水流量", description = "" }
                , new OPPO_Property { pid = 201058, type = "uint32", name1 = "targetsupplyWaterTemp", name = "供水温度设定", description = "" }
                , new OPPO_Property { pid = 201059, type = "uint32", name1 = "iceStorageRate", name = "畜冰槽蓄冰百分比", description = "" }
                , new OPPO_Property { pid = 201060, type = "uint32", name1 = "iceThickness", name = "畜冰槽蓄冰厚度", description = "" }
                , new OPPO_Property { pid = 201061, type = "uint32", name1 = "iceStorageCapacity", name = "蓄冰量", description = "" }
                , new OPPO_Property { pid = 201062, type = "uint32", name1 = "coolingCapacity", name = "冷量", description = "" }
                , new OPPO_Property { pid = 201063, type = "uint32", name1 = "evaSatPres", name = "主机蒸发器冷媒饱和压力", description = "evaporatorapproachpressure" }
                , new OPPO_Property { pid = 201064, type = "uint32", name1 = "evaSatTemp", name = "主机蒸发器冷媒饱和温度", description = "" }
                , new OPPO_Property { pid = 201065, type = "uint32", name1 = "conSatPres", name = "主机冷凝器冷媒饱和压力", description = "" }
                , new OPPO_Property { pid = 201066, type = "uint32", name1 = "conSatTemp", name = "主机冷凝器冷媒饱和温度", description = "" }
                , new OPPO_Property { pid = 201067, type = "uint32", name1 = "evaAppTemp", name = "主机蒸发器趋近温度", description = "" }
                , new OPPO_Property { pid = 201068, type = "uint32", name1 = "conAppTemp", name = "主机冷凝器趋近温度", description = "" }
                , new OPPO_Property { pid = 201069, type = "bool", name1 = "evaFlowSta", name = "主机蒸发器水流状态", description = "" }
                , new OPPO_Property { pid = 201070, type = "bool", name1 = "conFlowSta", name = "主机冷凝器水流状态", description = "" }
                , new OPPO_Property { pid = 201071, type = "uint32", name1 = "cop", name = "主机COP", description = "" }
                , new OPPO_Property { pid = 201072, type = "uint32", name1 = "runningLoad", name = "运行负荷", description = "" }
                , new OPPO_Property { pid = 201073, type = "uint32", name1 = "runningTime", name = "运行时间", description = "" }
                , new OPPO_Property { pid = 201074, type = "uint32", name1 = "ggOpening1", name = "主机导叶开度1", description = "guideglade" }
                , new OPPO_Property { pid = 201075, type = "uint32", name1 = "ggOpening2", name = "主机导叶开度2", description = "" }
                , new OPPO_Property { pid = 201076, type = "uint32", name1 = "ggOpening3", name = "主机导叶开度3", description = "" }
                , new OPPO_Property { pid = 201077, type = "uint32", name1 = "motorCoilTemp1", name = "主机马达线圈温度1", description = "" }
                , new OPPO_Property { pid = 201078, type = "uint32", name1 = "motorCoilTemp2", name = "主机马达线圈温度2", description = "" }
                , new OPPO_Property { pid = 201079, type = "uint32", name1 = "motorCoilTemp3", name = "主机马达线圈温度3", description = "" }
                , new OPPO_Property { pid = 201080, type = "uint32", name1 = "hVibrationValue", name = "主机横向震动值", description = "" }
                , new OPPO_Property { pid = 201081, type = "uint32", name1 = "vVibrationValue", name = "主机纵向震动值", description = "" }
                , new OPPO_Property { pid = 201082, type = "uint32", name1 = "ggOpening1Peer", name = "2#导叶开度", description = "guideglade" }
                , new OPPO_Property { pid = 201083, type = "uint32", name1 = "ggOpening2Peer", name = "2#导叶开度2", description = "" }
                , new OPPO_Property { pid = 201084, type = "uint32", name1 = "motorCoilTemp1Peer", name = "2#电机线圈温度", description = "" }
                , new OPPO_Property { pid = 201085, type = "uint32", name1 = "conAppTempPeer", name = "2#冷凝器趋近温差", description = "" }
                , new OPPO_Property { pid = 201086, type = "uint32", name1 = "eaTemp", name = "排气温度", description = "" }
                , new OPPO_Property { pid = 201087, type = "uint32", name1 = "eaTempPeer", name = "2#排气温度", description = "" }
                , new OPPO_Property { pid = 201088, type = "uint32", name1 = "averageCurrent", name = "平均线电流", description = "" }
                , new OPPO_Property { pid = 201089, type = "uint32", name1 = "averageCurrentPeer", name = "2#平均线电流", description = "" }
                , new OPPO_Property { pid = 201090, type = "uint32", name1 = "acOilPressure", name = "压缩机油压力(油压差)", description = "" }
                , new OPPO_Property { pid = 201091, type = "uint32", name1 = "acOilPressurePeer", name = "2#压缩机油压力(油压差)", description = "" }
                , new OPPO_Property { pid = 201092, type = "uint32", name1 = "acRunningTime", name = "1#压缩机运行时间", description = "" }
                , new OPPO_Property { pid = 201093, type = "uint32", name1 = "acRunningTimePeer", name = "2#压缩机运行时间", description = "" }
                , new OPPO_Property { pid = 201094, type = "uint32", name1 = "oilTemp", name = "1#油温", description = "" }
                , new OPPO_Property { pid = 201095, type = "uint32", name1 = "oilTempPeer", name = "2#油温", description = "" }
                , new OPPO_Property { pid = 201096, type = "uint32", name1 = "evaAppTempPeer", name = "2#蒸发器趋近温差", description = "" }
                , new OPPO_Property { pid = 201097, type = "uint32", name1 = "acStartUpNum", name = "1#主机压缩机启动次数", description = "" }
                , new OPPO_Property { pid = 201098, type = "uint32", name1 = "acStartUpNumPeer", name = "2#主机压缩机启动次数", description = "" }
                , new OPPO_Property { pid = 201099, type = "uint32", name1 = "chwSupplyWaterTemp", name = "冷冻出水温度", description = "" }
                , new OPPO_Property { pid = 201100, type = "uint32", name1 = "chwReturnWaterTemp", name = "冷冻进水温度", description = "" }
                , new OPPO_Property { pid = 201101, type = "uint32", name1 = "conSatTempPeer", name = "2#冷凝器冷媒温度", description = "" }
                , new OPPO_Property { pid = 201102, type = "uint32", name1 = "conSatPresPeer", name = "2#冷凝器冷媒压力", description = "" }
                , new OPPO_Property { pid = 201103, type = "uint32", name1 = "cwSupplyWaterTemp", name = "冷却出水温度", description = "" }
                , new OPPO_Property { pid = 201104, type = "uint32", name1 = "cwReturnWaterTemp", name = "冷却进水温度", description = "" }
                , new OPPO_Property { pid = 201105, type = "uint32", name1 = "runningLoadPeer", name = "2#运行电流百分比(冷机负荷)", description = "" }
                , new OPPO_Property { pid = 201106, type = "uint32", name1 = "evaSatTempPeer", name = "2#蒸发器冷媒温度", description = "" }
                , new OPPO_Property { pid = 201107, type = "uint32", name1 = "evaSatPresPeer", name = "2#蒸发器冷媒压力", description = "" }
                , new OPPO_Property { pid = 201108, type = "uint32", name1 = "l1Current", name = "1#主机L1电流", description = "" }
                , new OPPO_Property { pid = 201109, type = "uint32", name1 = "l1CurrentPeer", name = "2#主机L1电流", description = "" }
                , new OPPO_Property { pid = 201110, type = "uint32", name1 = "l2Current", name = "1#主机L2电流", description = "" }
                , new OPPO_Property { pid = 201111, type = "uint32", name1 = "l2CurrentPeer", name = "2#主机L2电流", description = "" }
                , new OPPO_Property { pid = 201112, type = "uint32", name1 = "l3Current", name = "1#主机L3电流", description = "" }
                , new OPPO_Property { pid = 201113, type = "uint32", name1 = "l3CurrentPeer", name = "2#主机L3电流", description = "" }
                , new OPPO_Property { pid = 201114, type = "bool", name1 = "conFlowStaPeer", name = "2#冷凝器水流状态", description = "" }
                , new OPPO_Property { pid = 201115, type = "bool", name1 = "evaFlowStaPeer", name = "2#蒸发器水流状态", description = "" }
                , new OPPO_Property { pid = 201116, type = "uint32", name1 = "autoTurnOnTemp", name = "主机自动启动温差设定值", description = "" }
                , new OPPO_Property { pid = 201117, type = "uint32", name1 = "autoTurnOffTemp", name = "主机自动停机温差设定值", description = "" }
                , new OPPO_Property { pid = 201118, type = "uint32", name1 = "cwTargetReturnTemp", name = "冷却水回水温度设定值", description = "" }
                , new OPPO_Property { pid = 201119, type = "uint32", name1 = "runningPower", name = "1#功率", description = "" }
                , new OPPO_Property { pid = 201120, type = "uint32", name1 = "runningPowerPeer", name = "2#功率", description = "" }
                , new OPPO_Property { pid = 201121, type = "uint32", name1 = "chwTargetSupplyTemp", name = "1#出水温度设定点（冷冻供水温度设定）", description = "" }
                , new OPPO_Property { pid = 201122, type = "uint32", name1 = "chwTargetSupplyTempPeer", name = "2#出水温度设定点（冷冻供水温度设定）", description = "" }
                , new OPPO_Property { pid = 201123, type = "uint32", name1 = "abVoltage", name = "1#主机AB相电压", description = "" }
                , new OPPO_Property { pid = 201124, type = "uint32", name1 = "abVoltagePeer", name = "2#主机AB相电压", description = "" }
                , new OPPO_Property { pid = 201125, type = "uint32", name1 = "bcVoltage", name = "1#主机BC相电压", description = "" }
                , new OPPO_Property { pid = 201126, type = "uint32", name1 = "bcVoltagePeer", name = "2#主机BC相电压", description = "" }
                , new OPPO_Property { pid = 201127, type = "uint32", name1 = "caVoltage", name = "1#主机CA相电压", description = "" }
                , new OPPO_Property { pid = 201128, type = "uint32", name1 = "caVoltagePeer", name = "2#主机CA相电压", description = "" }
                , new OPPO_Property { pid = 201129, type = "uint32", name1 = "maxRunningLoad", name = "1#最大电流百分比设定（机组负荷设定）", description = "" }
                , new OPPO_Property { pid = 201130, type = "uint32", name1 = "maxRunningLoadPeer", name = "2#最大电流百分比设定（机组负荷设定）", description = "" }
                , new OPPO_Property { pid = 201131, type = "uint32", name1 = "oilPumpPres", name = "1#油泵压力", description = "" }
                , new OPPO_Property { pid = 201132, type = "uint32", name1 = "oilPumpPresPeer", name = "2#油泵压力", description = "" }
                , new OPPO_Property { pid = 201133, type = "uint32", name1 = "oilTankPres", name = "1#油箱压力", description = "" }
                , new OPPO_Property { pid = 201134, type = "uint32", name1 = "oilTankPresPeer", name = "2#油箱压力", description = "" }
                , new OPPO_Property { pid = 201135, type = "uint32", name1 = "coldHeatSourceMode", name = "冷热源系统模式", description = "" }
                , new OPPO_Property { pid = 201136, type = "uint32", name1 = "currentTemperature2", name = "送风温度2", description = "" }
                , new OPPO_Property { pid = 201137, type = "bool", name1 = "alarmState", name = "报警状态", description = "" }

                , new OPPO_Property { pid = 200022, type = "bool", name1 = "power", name = "开关", description = "Power On/ Off" }
                , new OPPO_Property { pid = 200142, type = "int32", name1 = "currentTemperature", name = "当前温度", description = "Current Temperature" }
                , new OPPO_Property { pid = 200143, type = "int32", name1 = "targetTemperature", name = "目标温度", description = "Target Temperature" }
                , new OPPO_Property { pid = 200160, type = "uint32", name1 = "currentHumidity", name = "当前湿度", description = "Current Humidity" }
                , new OPPO_Property { pid = 200161, type = "uint32", name1 = "targetHumidity", name = "目标湿度", description = "Target Humidity" }
                , new OPPO_Property { pid = 200148, type = "uint32", name1 = "windSpeed", name = "风速", description = "Wind Speed" }
                , new OPPO_Property { pid = 200153, type = "uint32", name1 = "co2", name = "二氧化碳", description = "CO2" }
                , new OPPO_Property { pid = 200048, type = "uint32", name1 = "enthalpy", name = "温湿度焓值", description = "" }

                // Event
                , new OPPO_Property { pid = 401000, type = "bool", name1 = "startMfa", name = "手报报警", description = "" }
                , new OPPO_Property { pid = 401001, type = "bool", name1 = "invalidAlarm", name = "主机失效报警", description = "" }
                , new OPPO_Property { pid = 401002, type = "bool", name1 = "highTempAlarm", name = "电加热高温报警", description = "" }

                );

            // 添加服务属性
            await db.OPPO_DeviceProperty.AddRangeAsync(
                  // AHU
                  new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201014, iid = 8, point_no = 1, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201015, iid = 9, point_no = 2, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201037, iid = 25, point_no = 3, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201007, iid = 1, point_no = 4, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201008, iid = 2, point_no = 5, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 260608, pid = 201036, iid = 16, point_no = 6, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 401002, iid = 30, point_no = 7, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201022, iid = 16, point_no = 8, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201026, iid = 20, point_no = 9, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201028, iid = 22, point_no = 10, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201018, iid = 12, point_no = 11, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201019, iid = 13, point_no = 12, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 260352, pid = 201003, iid = 9, point_no = 13, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 260352, pid = 200160, iid = 7, point_no = 14, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 260352, pid = 200142, iid = 4, point_no = 15, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 260608, pid = 201005, iid = 14, point_no = 16, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 260608, pid = 200160, iid = 6, point_no = 17, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 260608, pid = 200161, iid = 7, point_no = 18, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 260608, pid = 200142, iid = 4, point_no = 19, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 260608, pid = 200143, iid = 5, point_no = 20, point_type = "C" }
                // TODO: Action处理
                //, new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201000, iid = 31, point_no = 21, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256512, pid = 200161, iid = 12, point_no = 22, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256512, pid = 200143, iid = 11, point_no = 23, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256512, pid = 200142, iid = 9, point_no = 24, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256512, pid = 201001, iid = 2, point_no = 25, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256512, pid = 200022, iid = 1, point_no = 26, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256512, pid = 201002, iid = 4, point_no = 27, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256512, pid = 201000, iid = 3, point_no = 28, point_type = "C" }
                // TODO：补充设备总启停
                //, new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256768, pid = 201000, iid = 15, point_no = 30, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256512, pid = 201003, iid = 7, point_no = 31, point_type = "C" }
                , new OPPO_DeviceProperty { equip_no = EquipNo, typeid = TypeId, siid = 256512, pid = 201004, iid = 8, point_no = 32, point_type = "C" }
                );

            await db.SaveChangesAsync();

        }

        #endregion

        #region 图书馆

        /// <summary>
        /// 图书馆
        /// </summary>
        static void Library()
        {
            // 初始化服务
            IServiceCollection services = new ServiceCollection();
            new Startup().ConfiguraServices(services);
            var sp = services.BuildServiceProvider();

            // 初始化数据库
            Init(sp);

            // 获取关系数据
            RetriveRelational(sp);
        }

        /// <summary>
        /// 加载关联数据
        /// </summary>
        static void RetriveRelational(IServiceProvider sp)
        {
            using (var scope = sp.CreateScope())
            using (var db = scope.ServiceProvider.GetService<BookDbContext>())
            {
                LoadLibraries(db);

                LoadBooks(db);
            }
        }

        /// <summary>
        /// 加载图书馆，及关联的数据
        /// </summary>
        private static void LoadLibraries(BookDbContext db)
        {
            var libQuery = db.Libraries
                .Include(lib => lib.Books);
            foreach (var library in libQuery)
            {
                System.Console.WriteLine($"LibraryId: {library.LibraryId}, BookCount: {library.Books.Count}, Name: {library.Name}");

                foreach (var book in library.Books)
                {
                    System.Console.WriteLine($" LibraryId: {book.LibraryId}, Book.Library: {book.Library.LibraryId}, BookId: {book.BookId}, Name: {book.Name}");
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 加载书本
        /// </summary>
        private static void LoadBooks(BookDbContext db)
        {
            var bookQuery = db.Books
                .Include(b => b.Library)
                .ToList();

            foreach (var book in db.Books)
            {
                Console.WriteLine($"BookId: {book.BookId}, LibraryName: {book.Library?.Name}, Name: {book.Name}");
            }
        }

        /// <summary>
        /// 初始化数据库，创建数据
        /// </summary>
        static void Init(IServiceProvider sp)
        {
            using (var scope = sp.CreateScope())
            using (var db = scope.ServiceProvider.GetService<BookDbContext>())
            {
                if (!db.Database.EnsureCreated())
                    return;

                db.Libraries.RemoveRange(db.Libraries);
                db.SaveChanges();

                var newLibrary1 = new Library
                {
                    Name = "国家图书馆",
                };

                newLibrary1.Books.AddRange(
                         new List<Book>
                         {
                            new Book { Name = "史记" },
                            new Book { Name = "资治通鉴" },
                         });

                db.Libraries.Add(newLibrary1);

                var newLibrary2 = new Library
                {
                    Name = "首都图书馆",
                };

                newLibrary2.Books.AddRange(
                    new List<Book>
                    {
                        new Book { Name = "论语" },
                        new Book { Name = "大学" },
                        new Book { Name = "中庸" },
                        new Book { Name = "孟子" },
                    });
                db.Libraries.Add(newLibrary2);

                db.SaveChanges();
            }

            #endregion

        }
    }
}
