using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace 卓汇数据追溯系统
{
    public class ProductConfig
    {

        #region 系统参数

        public string URL;
        public string site;
        public string resource;
        public string operation;

        public string BaozhanUrl { set; get; }
        public string ShangLiaoUrl { set; get; }
        public string TraceUrl { set; get; }
        public string GuoZhanUrl { set; get; }

        public string TupianUrl { set; get; }

        public string GuoZhanOpen { set; get; }
        public string TupianOpen { set; get; }
        /// <summary>
        /// 上料确认左右键OpenTime开启/屏蔽
        /// </summary>
        public string OpenTimeLROpen { set; get; }
        public string equipmentNo { set; get; }//设备编号
        public string station { set; get; }//工站名称
        public string barCodeType { set; get; }//2D码类型
        public string Trace_ip { set; get; }//Trace IP
        public string version { set; get; }
        public string Trace_line_id { set; get; }//机台line_id
        public string Trace_station_id { set; get; }//机台station_id
        public string productName { set; get; }//234M

        //焊接参数
        public string power;
        public string frequency;
        public string waveform;
        public string laser_speed;
        public string jump_speed;
        public string jump_delay;
        public string position_delay;
        public string pulse_profile;
        public string laser_height;

        //各级权限密码
        public string Operator_pwd;
        public string Technician_pwd;
        public string Administrator_pwd;
        #endregion

    }

    public class IniProductFile
    {
        #region 初始化
        private string _path;
        private IniOperation _iniOperation;
        private ProductConfig _productconfig;

        #endregion

        #region Property
        public ProductConfig productconfig
        {
            get { return _productconfig; }
            set { _productconfig = value; }
        }

        #endregion

        public IniProductFile(string path)
        {
            this._path = path;
            _iniOperation = new IniOperation(_path);
            _productconfig = new ProductConfig();
            ReadProductConfigSection();
        }

        public void ReadProductConfigSection()
        {
            string sectionName = "ProductConfig";
            _productconfig.URL = _iniOperation.ReadValue(sectionName, "URL");
            _productconfig.site = _iniOperation.ReadValue(sectionName, "site");
            _productconfig.resource = _iniOperation.ReadValue(sectionName, "resource");
            _productconfig.operation = _iniOperation.ReadValue(sectionName, "operation");

            _productconfig.BaozhanUrl = _iniOperation.ReadValue(sectionName, "BaozhanUrl");
            _productconfig.ShangLiaoUrl = _iniOperation.ReadValue(sectionName, "ShangLiaoUrl");
            _productconfig.TraceUrl = _iniOperation.ReadValue(sectionName, "TraceUrl");
            _productconfig.GuoZhanUrl = _iniOperation.ReadValue(sectionName, "GuoZhanUrl");
            _productconfig.TupianUrl= _iniOperation.ReadValue(sectionName, "TupianUrl");
            _productconfig.GuoZhanOpen = _iniOperation.ReadValue(sectionName, "GuoZhanOpen");
            _productconfig.TupianOpen = _iniOperation.ReadValue(sectionName, "TupianOpen");
            _productconfig.OpenTimeLROpen = _iniOperation.ReadValue(sectionName, "OpenTimeLROpen");

            _productconfig.equipmentNo = _iniOperation.ReadValue(sectionName, "equipmentNo");
            _productconfig.station = _iniOperation.ReadValue(sectionName, "station");
            _productconfig.barCodeType = _iniOperation.ReadValue(sectionName, "barCodeType");
            _productconfig.Trace_ip = _iniOperation.ReadValue(sectionName, "Trace_ip");
            _productconfig.version= _iniOperation.ReadValue(sectionName, "version");
            _productconfig.Trace_line_id = _iniOperation.ReadValue(sectionName, "Trace_line_id");
            _productconfig.Trace_station_id = _iniOperation.ReadValue(sectionName, "Trace_station_id");
            _productconfig.productName = _iniOperation.ReadValue(sectionName, "productName");

            _productconfig.power = _iniOperation.ReadValue(sectionName, "power");
            _productconfig.frequency = _iniOperation.ReadValue(sectionName, "frequency");
            _productconfig.waveform = _iniOperation.ReadValue(sectionName, "waveform");
            _productconfig.laser_speed = _iniOperation.ReadValue(sectionName, "laser_speed");
            _productconfig.jump_speed = _iniOperation.ReadValue(sectionName, "jump_speed");
            _productconfig.jump_delay = _iniOperation.ReadValue(sectionName, "jump_delay");
            _productconfig.position_delay = _iniOperation.ReadValue(sectionName, "position_delay");
            _productconfig.pulse_profile = _iniOperation.ReadValue(sectionName, "pulse_profile");
            _productconfig.laser_height = _iniOperation.ReadValue(sectionName, "laser_height");

            string sectionName3 = "PassWord";
            _productconfig.Operator_pwd = _iniOperation.ReadValue(sectionName3, "Operator_pwd");
            _productconfig.Technician_pwd = _iniOperation.ReadValue(sectionName3, "Technician_pwd");
            _productconfig.Administrator_pwd = _iniOperation.ReadValue(sectionName3, "Administrator_pwd");
        }

        public void WriteProductConfigSection()
        {
            string sectionName = "ProductConfig";
            _iniOperation.WriteValue(sectionName, "URL", _productconfig.URL);
            _iniOperation.WriteValue(sectionName, "site", _productconfig.site);
            _iniOperation.WriteValue(sectionName, "resource", _productconfig.resource);
            _iniOperation.WriteValue(sectionName, "operation", _productconfig.operation);

            _iniOperation.WriteValue(sectionName, "BaozhanUrl", _productconfig.BaozhanUrl);
            _iniOperation.WriteValue(sectionName, "ShangLiaoUrl", _productconfig.ShangLiaoUrl);
            _iniOperation.WriteValue(sectionName, "TraceUrl", _productconfig.TraceUrl);
            _iniOperation.WriteValue(sectionName, "GuoZhanUrl", _productconfig.GuoZhanUrl);
            _iniOperation.WriteValue(sectionName, "TupianUrl", _productconfig.TupianUrl);
            _iniOperation.WriteValue(sectionName, "GuoZhanOpen", _productconfig.GuoZhanOpen);
            _iniOperation.WriteValue(sectionName, "TupianOpen", _productconfig.TupianOpen);
            _iniOperation.WriteValue(sectionName, "OpenTimeLROpen", _productconfig.OpenTimeLROpen);

            _iniOperation.WriteValue(sectionName, "equipmentNo", _productconfig.equipmentNo);
            _iniOperation.WriteValue(sectionName, "station", _productconfig.station);
            _iniOperation.WriteValue(sectionName, "barCodeType", _productconfig.barCodeType);
            _iniOperation.WriteValue(sectionName, "Trace_ip", _productconfig.Trace_ip);
            _iniOperation.WriteValue(sectionName, "version", _productconfig.version);
            _iniOperation.WriteValue(sectionName, "Trace_line_id", _productconfig.Trace_line_id);
            _iniOperation.WriteValue(sectionName, "Trace_station_id", _productconfig.Trace_station_id);
            _iniOperation.WriteValue(sectionName, "productName", _productconfig.productName);

            _iniOperation.WriteValue(sectionName, "power", _productconfig.power);
            _iniOperation.WriteValue(sectionName, "frequency", _productconfig.frequency);
            _iniOperation.WriteValue(sectionName, "waveform", _productconfig.waveform);
            _iniOperation.WriteValue(sectionName, "laser_speed", _productconfig.laser_speed);
            _iniOperation.WriteValue(sectionName, "jump_speed", _productconfig.jump_speed);
            _iniOperation.WriteValue(sectionName, "jump_delay", _productconfig.jump_delay);
            _iniOperation.WriteValue(sectionName, "position_delay", _productconfig.position_delay);
            _iniOperation.WriteValue(sectionName, "pulse_profile", _productconfig.pulse_profile);
            _iniOperation.WriteValue(sectionName, "laser_height", _productconfig.laser_height);

            string sectionName2 = "PassWord";
            _iniOperation.WriteValue(sectionName2, "Operator_pwd", _productconfig.Operator_pwd);
            _iniOperation.WriteValue(sectionName2, "Technician_pwd", _productconfig.Technician_pwd);
            _iniOperation.WriteValue(sectionName2, "Administrator_pwd", _productconfig.Administrator_pwd);
        }

        public void WriteProductnumSection()
        {

        }
    }
}
