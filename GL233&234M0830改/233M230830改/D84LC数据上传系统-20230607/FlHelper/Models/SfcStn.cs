using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlHelper.Models
{
   public class SfcStn
    {
        /// <summary>
        /// 密鑰，必填參數
        /// </summary>
        public string hashCode { get; set; }
        /// <summary>
        /// 時間+設備管制編號（yyyymmdd-hhmmss-fff-設備管制編號）,時間精確至毫秒，設備管制編號按最長位數計算，如不足最長位則在設備編號前補0，必填參數,值為上料確認接口回傳的requestId值
        /// </summary>
        public string requestId { get; set; }
        /// <summary>
        /// 默認為空: "";數據重傳:"REUPLOAD=Y";參數不作記錄:"UNSAVE=Y"
        /// </summary>
        public string resv1 { get; set; }
        /// <summary>
        /// 產品訊息列表
        /// </summary>
        public ProductInfo productInfo { get; set; }
        public EquipmentInfo equipmentInfo { get; set; }
        public RecipeInfo recipeInfo { get; set; }
        public AppleReturnInfo appleReturnInfo { get; set; }
    }

    public class AppleReturnInfo
    {
    }

    public class RecipeInfo
    {
        /// <summary>
        /// 產品掃描開始時間：YYYYMMDDHHmmss(24H)，必填參數
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 產品下料時間：YYYYMMDDHHmmss(24H)，必填參數
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        /// 產品加工穴位
        /// </summary>
        public string cavity { get; set; }
        /// <summary>
        /// 檢測結果，可填入"PASS"、"FAIL"、"PL"(焊接拋料)、"PL-001"(BandDispense拋料)、"PL-002"(BGtoBandAssy拋料) 、 "pass"、"fail"
        /// </summary>
        public string judgement { get; set; }
        /// <summary>
        /// 濕度
        /// </summary>
        public string humidity { get; set; }
        /// <summary>
        /// 溫度
        /// </summary>
        public string temperature { get; set; }
        /// <summary>
        /// 開始加工時間
        /// </summary>
        public string processStartTime { get; set; }
        /// <summary>
        /// 加工結束時間
        /// </summary>
        public string processEndTime { get; set; }
        /// <summary>
        /// 等待時間
        /// </summary>
        public string waitCt { get; set; }
        /// <summary>
        /// 作業時間
        /// </summary>
        public string ct { get; set; }
        /// <summary>
        /// 拋料原因，若參數過長則截取前100位存儲
        /// </summary>
        public string tossingItem { get; set; }
        /// <summary>
        /// OEE報錯代碼清單
        /// </summary>
        public ErrorInfo[] errorInfo { get; set; }
        /// <summary>
        /// 參數列表
        /// </summary>
        public ParaInfo[] paraInfo { get; set; }
        
    }

    public class ParaInfo
    {
        /// <summary>
        /// 焊接區域
        /// </summary>

        public string area { get; set; }
        /// <summary>
        /// 開始焊接時間
        /// </summary>

        public string sTime { get; set; }
        /// <summary>
        /// 焊接完成時間
        /// </summary>

        public string eTime { get; set; }
        /// <summary>
        /// 功率設置上限
        /// </summary>

        public string powerUsl { get; set; }
        /// <summary>
        /// 功率設置下限
        /// </summary>

        public string powerLsl { get; set; }
        /// <summary>
        /// 脉衝波形
        /// </summary>

        public string pusleWaveform { get; set; }
        /// <summary>
        /// 填充間隙
        /// </summary>

        public string hatch { get; set; }
        /// <summary>
        /// 填充角度
        /// </summary>

        public string angel { get; set; }
        /// <summary>
        /// 搖擺振幅
        /// </summary>

        public string swingAmplitude { get; set; }
        /// <summary>
        /// 搖擺頻率
        /// </summary>

        public string swingFreq { get; set; }
        /// <summary>
        /// 電流
        /// </summary>

        public string current { get; set; }
        /// <summary>
        /// 空跳速度
        /// </summary>

        public string jumpingSpeed { get; set; }
        /// <summary>
        /// 空跳延時
        /// </summary>

        public string jumpingDelay { get; set; }
        /// <summary>
        /// 攝像頭焊接間隙
        /// </summary>

        public string trimWasherGap { get; set; }
        /// <summary>
        /// 掃描延時
        /// </summary>

        public string scanningDelay { get; set; }
        /// <summary>
        /// Scan掃描層波形
        /// </summary>

        public string scanParametersWaveform { get; set; }
        /// <summary>
        /// Scan掃描層焊點大小
        /// </summary>

        public string scanSpotSize { get; set; }
        /// <summary>
        /// Scan掃描層設定功率
        /// </summary>

        public string scanPower { get; set; }
        /// <summary>
        /// Scan掃描層脉衝能量
        /// </summary>

        public string scanPulseEnergy { get; set; }
        /// <summary>
        /// Scan掃描層頻率
        /// </summary>

        public string scanFrequency { get; set; }
        /// <summary>
        /// Scan掃描層焊接速度
        /// </summary>

        public string scanLinearSpeed { get; set; }
        /// <summary>
        /// Scan掃描層填充類型
        /// </summary>

        public string scanFillingPattern { get; set; }
        /// <summary>
        /// Scan掃描層電流
        /// </summary>

        public string scanCurrnet { get; set; }
        /// <summary>
        ///Scan掃描層空跳速度
        /// </summary>

        public string scanJumpingSpeed { get; set; }
        /// <summary>
        /// Scan掃描層空跳延時
        /// </summary>

        public string scanJumpingDelay { get; set; }
        /// <summary>
        /// Scan掃描層掃描延時
        /// </summary>

        public string scanScanningDelay { get; set; }
        /// <summary>
        /// 焊接類型
        /// </summary>

        public string patternType { get; set; }
        /// <summary>
        /// Laser焊接層參數波形
        /// </summary>

        public string parametersWaveform { get; set; }
        /// <summary>
        /// Laser焊接層焊點尺寸
        /// </summary>

        public string spotSize { get; set; }

        /// <summary>
        ///Laser焊接層功率
        /// </summary>

        public string power { get; set; }
        /// <summary>
        /// Laser焊接層脈衝能量
        /// </summary>

        public string pulseEnergy { get; set; }
        /// <summary>
        /// Laser焊接層頻率
        /// </summary>

        public string frequency { get; set; }
        /// <summary>
        /// Laser焊接層線性速度
        /// </summary>

        public string linearSpeed { get; set; }
        /// <summary>
        /// Laser焊接層填充方式
        /// </summary>

        public string fillingPattern { get; set; }
        /// <summary>
        /// CCD版本
        /// </summary>

        public string ccd { get; set; }
        /// <summary>
        /// 普雷斯特檢測結果
        /// </summary>

        public string precitecResult { get; set; }
        /// <summary>
        /// 普雷斯特檢測等級
        /// </summary>

        public string precitecGrading { get; set; }
        /// <summary>
        /// 普雷斯特數值
        /// </summary>

        public string precitecValue { get; set; }
        /// <summary>
        /// 普雷斯特檢測版本
        /// </summary>

        public string precitecProcessRev { get; set; }
        /// <summary>
        /// 預扭
        /// </summary>

        public string preTorque { get; set; }
        /// <summary>
        /// 預推
        /// </summary>

        public string prePush { get; set; }
        /// <summary>
        /// 起點半徑
        /// </summary>

        public string startingRadius { get; set; }
        /// <summary>
        /// 終點半徑
        /// </summary>

        public string endingRadius { get; set; }
        /// <summary>
        /// 內圈圈數
        /// </summary>

        public string innerRingQty { get; set; }
        /// <summary>
        /// 外圈圈數
        /// </summary>

        public string outerRingQty { get; set; }
        /// <summary>
        /// 螺旋
        /// </summary>

        public string sprialPitch { get; set; }


        /// <summary>
        /// 內環第一段波時間
        /// </summary>

        public string a1stFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第一段波功率 a1stFiberCoreWavePower
        /// </summary>

        public string a1stSettingInnerLASEREnergyPercentage { get; set; }
    //    /// <summary>
    //    /// 外環第一段波時間
    //    /// </summary>

    //    public string a1stOuterRingWaveTimePeriod { get; set; }
    ///// <summary>
    //    /// 外環第一段波功率 a1stOuterRingWavePower
    ///// </summary>

    //    public string a1stSettingOuterLASEREnergyPercentage { get; set; }

        /// <summary>
        /// 內環第二段波時間
        /// </summary>

        public string a2ndFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第二段波功率 a2ndFiberCoreWavePower
        /// </summary>

        public string a2ndSettingInnerLASEREnergyPercentage { get; set; }
        /// <summary>
        /// 外環第二段波時間
        /// </summary>

        //public string a2ndOuterRingWaveTimePeriod { get; set; }
        ///// <summary>
        ///// 外環第二段波功率
        ///// </summary>

        //public string a2ndSettingOuterLASEREnergyPercentage { get; set; }

        /// <summary>
        /// 內環第三段波時間
        /// </summary>

        public string a3rdFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第三段波功率
        /// </summary>

        public string a3rdSettingInnerLASEREnergyPercentage { get; set; }
        /// <summary>
        /// 外環第三段波時間
        /// </summary>

        //public string a3rdOuterRingWaveTimePeriod { get; set; }
        ///// <summary>
        ///// 外環第三段波功率
        ///// </summary>

        //public string a3rdSettingOuterLASEREnergyPercentage { get; set; }

        /// <summary>
        /// 內環第四段波時間
        /// </summary>

        public string a4thFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第四段波功率
        /// </summary>

        public string a4thSettingInnerLASEREnergyPercentage { get; set; }
        /// <summary>
        /// 外環第四段波時間
        /// </summary>

        //public string a4thOuterRingWaveTimePeriod { get; set; }
        ///// <summary>
        ///// 外環第四段波功率
        ///// </summary>

        //public string a4thSettingOuterLASEREnergyPercentage { get; set; }

        /// <summary>
        /// 內環第五段波時間
        /// </summary>

        public string a5thFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第五段波功率
        /// </summary>

        public string a5thSettingInnerLASEREnergyPercentage { get; set; }
        /// <summary>
        /// 外環第五段波時間
        /// </summary>

        //public string a5thOuterRingWaveTimePeriod { get; set; }
        ///// <summary>
        ///// 外環第五段波功率
        ///// </summary>

        //public string a5thSettingOuterLASEREnergyPercentage { get; set; }

        /// <summary>
        /// 內環第六段波時間
        /// </summary>

        public string a6thFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第六段波功率
        /// </summary>

        public string a6thSettingInnerLASEREnergyPercentage { get; set; }
        /// <summary>
        /// 外環第六段波時間
        /// </summary>

        //public string a6thOuterRingWaveTimePeriod { get; set; }
        /// <summary>
        /// 外環第六段波功率
        /// </summary>

        //0624 updated a6th

        /// <summary>
        /// 外环功率
        /// </summary>
        public string SettingOuterLASEREnergyPercentage { get; set; }

        //0624Added
        public string a1stPulseWidth { set; get; }
        public string a2ndPulseWidth { set; get; }
        public string a3rdPulseWidth { set; get; }
        public string a4thPulseWidth { set; get; }
        public string a5thPulseWidth { set; get; }
        public string a6thPulseWidth { set; get; }

        /// <summary>
        /// 標準速度(mm/s)
        /// </summary>

        public string Speed { get; set; }
        /// <summary>
        /// (mm)
        /// </summary>

        public string Position { get; set; }
        /// <summary>
        /// 擺動幅度(mm)
        /// </summary>


        public string WobbleAmplitude { get; set; }
    /// <summary>
    /// 擺動頻率(HZ)
    /// </summary>

    public string WobbleFrequency { get; set; }
        /// <summary>
        /// 波形模式
        /// </summary>

        public string waveMode { get; set; }
        /// <summary>
        /// 起點半徑
        /// </summary>

        public string scanStartingRadius { get; set; }

        /// <summary>
        /// 終點半徑
        /// </summary>

        public string scanEndingRadius { get; set; }
        /// <summary>
        /// 內圈圈數
        /// </summary>

        public string scanInnerRingQty { get; set; }
        /// <summary>
        /// 外圈圈數
        /// </summary>

        public string scanOuterRingQty { get; set; }
        /// <summary>
        /// 螺旋間距
        /// </summary>

        public string scanSprialPitch { get; set; }
        /// <summary>
        /// 脉衝輪廓
        /// </summary>

        public string pulseProfile { get; set; }
      
    }

    
    public class ErrorInfo
    {
        /// <summary>
        /// OEE報錯代碼
        /// </summary>

        public string errorCode { get; set; }
    }

    public class EquipmentInfo
    {
        /// <summary>
        /// 設備IP，必填參數
        /// </summary>
        public string equipmentIp { get; set; }
        /// <summary>
        /// 設備類型，L代表鐳射機台，必填參數
        /// </summary>
        public string equipmentType { get; set; }
        /// <summary>
        /// 設備管制編號，必填參數
        /// </summary>
        public string equipmentNo { get; set; }
        /// <summary>
        /// 供應商名稱
        /// </summary>
        public string vendorId { get; set; }
        /// <summary>
        /// 設備程序版本，必填參數
        /// </summary>
        public string processRevs { get; set; }
        /// <summary>
        /// 軟件名稱，必填參數
        /// </summary>
        public string softwareName { get; set; }
        /// <summary>
        /// 軟件版本，必填參數
        /// </summary>
        public string softwareVersion { get; set; }
        /// <summary>
        /// 圖檔名稱
        /// </summary>
        public string patternName { get; set; }
        /// <summary>
        /// 圖檔版本
        /// </summary>
        public string patternVersion { get; set; }
        /// <summary>
        /// CCD軟件版本
        /// </summary>
        public string ccdVersion { get; set; }
        /// <summary>
        /// CCD引導軟件版本
        /// </summary>
        public string ccdGuideVersion { get; set; }
        /// <summary>
        /// CCD複檢軟件版本
        /// </summary>
        public string ccdInspectionVersion { get; set; }
        /// <summary>
        /// CCD複檢軟件名稱
        /// </summary>
        public string ccdInspectionName { get; set; }
       
    }

    public class ProductInfo
    {
        /// <summary>
        /// 產品條碼，必填參數
        /// </summary>
        public string barCode { get; set; }
        /// <summary>
        /// 產品類型，必填參數（BP/BAND/SP，根據實際加工的產品類型填入默認值）
        /// </summary>
        public string barCodeType { get; set; }
        /// <summary>
        /// 產品出貨碼，上料確認返回對應值
        /// </summary>
        public string clientCode { get; set; }
        /// <summary>
        /// 填入工站名稱，必填參數
        /// </summary>
        public string station { get; set; }
        /// <summary>
        /// 執行單,必填參數
        /// </summary>
        public string billNo { get; set; }
        /// <summary>
        /// 機種
        /// </summary>
        public string product { set; get; }
        /// <summary>
        /// 階段
        /// </summary>
        public string phase { set; get; }
        /// <summary>
        /// Config
        /// </summary>
        public string config { set; get; }
        /// <summary>
        /// 客戶線別
        /// </summary>
        public string lineName { set; get; }
        /// <summary>
        /// 客戶設備編號
        /// </summary>
        public string stationId { set; get; }
        public string stationString { set; get; }
        /// <summary>
        /// 綁定訊息列表
        /// </summary>
        public BindCode[] bindCode { get; set; }
    }

    public class BindCode
    {
        /// <summary>
        /// 產品條碼
        /// </summary>
        public string codeSn { get; set; }
        /// <summary>
        /// 條碼類型
        /// </summary>
        public string codeSnType { get; set; }
        /// <summary>
        /// 是否可取代主碼過站,1是不取代主碼過站,2是取代主碼過站
        /// </summary>
        public string replace { get; set; }
    }
}
