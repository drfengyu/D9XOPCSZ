using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 卓汇数据追溯系统
{
    class PassStn
    {
        public string hashCode { get; set; }
        public string requestId { get; set; }
        public string resv1 { get; set; }
        public ProductInfo productInfo { get; set; }
        public EquipmentInfo equipmentInfo { get; set; }
        public RecipeInfo recipeInfo { get; set; }
        public AppleReturnInfo appleReturnInfo { get; set; }


    }

    class BindCode1
    {
        public string codeSn { get; set; }
        public string codeSnType { get; set; }
        public string replace { get; set; }
    }
    class ProductInfo
    {
        public string barCode { get; set; }
        public string barCodeType { get; set; }
        public string station { get; set; }
        public string billNo { get; set; }
        public BindCode1[] bindCode { get; set; }
    }
    class EquipmentInfo
    {
        public string equipmentIp { get; set; }
        public string equipmentType { get; set; }
        public string equipmentNo { get; set; }
        public string vendorId { get; set; }
        public string processRevs { get; set; }

    }
    class RecipeInfo
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string cavity { get; set; }
        public string judgement { get; set; }
        public string humidity { get; set; }
        public string temperature { get; set; }
        public ParaInfo[] paraInfo { get; set; }
    }

    public class heightModel {
        public string TestHeightZ { set; get; }
        public string Difference { set; get; }
        public string ReferenceV { set; get; }
    }
   public class ParaInfo
    {
      public ParaInfo(FlHelper.Models.ParaInfo item,Dictionary<string,heightModel> HeightsResult=null) {
            /// <summary>
                            /// 焊接區域
                            /// </summary>

                           area=item.area;
                           if (HeightsResult!=null)
                           {
                               if (HeightsResult.ContainsKey(item.area))
                               {
                                   var hgmodel = new heightModel();
                                   HeightsResult.TryGetValue(item.area, out hgmodel);
                                   if (hgmodel != null)
                                   {
                                       this.TestHeightZ = hgmodel.TestHeightZ;
                                       this.Difference = hgmodel.Difference;
                                       this.ReferenceV = hgmodel.ReferenceV;
                                   }
                                   else
                                   {
                                       this.TestHeightZ = "";
                                       this.Difference = "";
                                       this.ReferenceV = "";
                                   }
                               }
                           }
                            /// <summary>
                            /// 開始焊接時間
                            /// </summary>

                            sTime =item.sTime;
                            /// <summary>
                            /// 焊接完成時間
                            /// </summary>

                            eTime = item.eTime;
                            /// <summary>
                            /// 功率設置上限
                            /// </summary>

                            powerUsl = item.powerUsl;
                            /// <summary>
                            /// 功率設置下限
                            /// </summary>

                           powerLsl = item.powerLsl;
                            /// <summary>
                            /// 脉衝波形
                            /// </summary>

                            pusleWaveform = item.pusleWaveform;
                            /// <summary>
                            /// 填充間隙 gai5
                            /// </summary>

                            hatch = item.hatch;
                            /// <summary>
                            /// 填充角度
                            /// </summary>

                            angel = item.angel;
                            /// <summary>
                            /// 搖擺振幅
                            /// </summary>

                            swingAmplitude = item.swingAmplitude;
                            /// <summary>
                            /// 搖擺頻率
                            /// </summary>

                            swingFreq = item.swingFreq;
                            /// <summary>
                            /// 電流
                            /// </summary>

                            current = item.current;
                            /// <summary>
                            /// 空跳速度
                            /// </summary>

                            jumpingSpeed = item.jumpingSpeed;
                            /// <summary>
                            /// 空跳延時
                            /// </summary>

                            jumpingDelay = item.jumpingDelay;
                            /// <summary>
                            /// 攝像頭焊接間隙
                            /// </summary>

                            trimWasherGap = item.trimWasherGap;
                            /// <summary>
                            /// 掃描延時
                            /// </summary>

                            scanningDelay = item.scanningDelay;
                            /// <summary>
                            /// Scan掃描層波形
                            /// </summary>

                            scanParametersWaveform = item.scanParametersWaveform;
                            /// <summary>
                            /// Scan掃描層焊點大小
                            /// </summary>

                            scanSpotSize = item.scanSpotSize;
                            /// <summary>
                            /// Scan掃描層設定功率
                            /// </summary>

                            scanPower = item.scanPower;
                            /// <summary>
                            /// Scan掃描層脉衝能量
                            /// </summary>

                            scanPulseEnergy = item.scanPulseEnergy;
                            /// <summary>
                            /// Scan掃描層頻率
                            /// </summary>

                            scanFrequency = item.scanFrequency;
                            /// <summary>
                            /// Scan掃描層焊接速度
                            /// </summary>

                            scanLinearSpeed = item.scanLinearSpeed;
                            /// <summary>
                            /// Scan掃描層填充類型
                            /// </summary>

                            scanFillingPattern = item.scanFillingPattern;
                            /// <summary>
                            /// Scan掃描層電流
                            /// </summary>

                            scanCurrnet = item.scanCurrnet;
                            /// <summary>
                            ///Scan掃描層空跳速度
                            /// </summary>

                            scanJumpingSpeed = item.scanJumpingSpeed;
                            /// <summary>
                            /// Scan掃描層空跳延時
                            /// </summary>

                            scanJumpingDelay = item.scanJumpingDelay;
                            /// <summary>
                            /// Scan掃描層掃描延時
                            /// </summary>

                            scanScanningDelay = item.scanScanningDelay;
                            /// <summary>
                            /// 焊接類型 gai4
                            /// </summary>

                            patternType = item.patternType;
                            /// <summary>
                            /// Laser焊接層參數波形
                            /// </summary>

                            parametersWaveform = item.parametersWaveform;
                            /// <summary>
                            /// Laser焊接層焊點尺寸
                            /// </summary>

                            spotSize = item.spotSize;

                            /// <summary>
                            ///Laser焊接層功率 GAI2
                            /// </summary>

                            power = item.power;
                            /// <summary>
                            /// Laser焊接層脈衝能量
                            /// </summary>

                            pulseEnergy = item.pulseEnergy;
                            /// <summary>
                            /// Laser焊接層頻率
                            /// </summary>

                            frequency = item.frequency;
                            /// <summary>
                            /// Laser焊接層線性速度 
                            /// </summary>

                            linearSpeed = item.linearSpeed;
                            /// <summary>
                            /// Laser焊接層填充方式
                            /// </summary>

                            fillingPattern = item.fillingPattern;
                            /// <summary>
                            /// CCD版本
                            /// </summary>

                            ccd =item.ccd;
                            /// <summary>
                            /// 普雷斯特檢測結果
                            /// </summary>

                            precitecResult = item.precitecResult;
                            /// <summary>
                            /// 普雷斯特檢測等級
                            /// </summary>

                            precitecGrading = item.precitecGrading;
                            /// <summary>
                            /// 普雷斯特數值
                            /// </summary>

                            precitecValue = item.precitecValue;
                            /// <summary>
                            /// 普雷斯特檢測版本
                            /// </summary>

                            precitecProcessRev = item.precitecProcessRev;
                            /// <summary>
                            /// 預扭
                            /// </summary>

                            preTorque = item.preTorque;
                            /// <summary>
                            /// 預推
                            /// </summary>

                            prePush = item.prePush;
                            /// <summary>
                            /// 起點半徑
                            /// </summary>

                            startingRadius = item.startingRadius;
                            /// <summary>
                            /// 終點半徑
                            /// </summary>

                            endingRadius = item.endingRadius;
                            /// <summary>
                            /// 內圈圈數
                            /// </summary>

                            innerRingQty = item.innerRingQty;
                            /// <summary>
                            /// 外圈圈數
                            /// </summary>

                            outerRingQty = "";
                            /// <summary>
                            /// 螺旋
                            /// </summary>

                            sprialPitch = "";
                            /// <summary>
                            /// 內環第一段波時間
                            /// </summary>

                            a1stFiberCoreWaveTimePeriod = "";
                            /// <summary>
                            /// 內環第一段波功率
                            /// </summary>

                            a1stFiberCoreWavePower = "";
                            /// <summary>
                            /// 外環第一段波時間
                            /// </summary>

                            a1stOuterRingWaveTimePeriod = "";
                            /// <summary>
                            /// 外環第一段波功率
                            /// </summary>

                            a1stOuterRingWavePower = "";

                            /// <summary>
                            /// 內環第二段波時間
                            /// </summary>

                            a2ndFiberCoreWaveTimePeriod = "";
                            /// <summary>
                            /// 內環第二段波功率
                            /// </summary>

                            a2ndFiberCoreWavePower = "";
                            /// <summary>
                            /// 外環第二段波時間
                            /// </summary>

                            a2ndOuterRingWaveTimePeriod = "";
                            /// <summary>
                            /// 外環第二段波功率
                            /// </summary>

                            a2ndOuterRingWavePower = "";

                            /// <summary>
                            /// 內環第三段波時間
                            /// </summary>

                            a3rdFiberCoreWaveTimePeriod = "";
                            /// <summary>
                            /// 內環第三段波功率
                            /// </summary>

                            a3rdFiberCoreWavePower = "";
                            /// <summary>
                            /// 外環第三段波時間
                            /// </summary>

                            a3rdOuterRingWaveTimePeriod = "";
                            /// <summary>
                            /// 外環第三段波功率
                            /// </summary>

                            a3rdOuterRingWavePower = "";

                            /// <summary>
                            /// 內環第四段波時間
                            /// </summary>

                            a4thFiberCoreWaveTimePeriod = "";
                            /// <summary>
                            /// 內環第四段波功率
                            /// </summary>

                            a4thFiberCoreWavePower = "";
                            /// <summary>
                            /// 外環第四段波時間
                            /// </summary>

                            a4thOuterRingWaveTimePeriod = "";
                            /// <summary>
                            /// 外環第四段波功率
                            /// </summary>

                            a4thOuterRingWavePower = "";

                            /// <summary>
                            /// 內環第五段波時間
                            /// </summary>

                            a5thFiberCoreWaveTimePeriod = "";
                            /// <summary>
                            /// 內環第五段波功率
                            /// </summary>

                            a5thFiberCoreWavePower = "";
                            /// <summary>
                            /// 外環第五段波時間
                            /// </summary>

                            a5thOuterRingWaveTimePeriod = "";
                            /// <summary>
                            /// 外環第五段波功率
                            /// </summary>

                            a5thOuterRingWavePower = "";

                            /// <summary>
                            /// 內環第六段波時間
                            /// </summary>

                            a6thFiberCoreWaveTimePeriod = "";
                            /// <summary>
                            /// 內環第六段波功率
                            /// </summary>

                            a6thFiberCoreWavePower = "";
                            /// <summary>
                            /// 外環第六段波時間
                            /// </summary>

                            a6thOuterRingWaveTimePeriod = "";
                            /// <summary>
                            /// 外環第六段波功率
                            /// </summary>

                            a6thOuterRingWavePower = "";
                            /// <summary>
                            /// 標準速度(mm/s)
                            /// </summary>

                            Speed = item.Speed;
                            /// <summary>
                            /// (mm)
                            /// </summary>

                            Position = "";
                            /// <summary>
                            /// 擺動幅度(mm)
                            /// </summary>


                            WobbleAmplitude = item.WobbleAmplitude;
                            /// <summary>
                            /// 擺動頻率(HZ)
                            /// </summary>

                            WobbleFrequency = item.WobbleFrequency;
                            /// <summary>
                            /// 波形模式
                            /// </summary>

                            waveMode = "";
                            /// <summary>
                            /// 起點半徑
                            /// </summary>

                            scanStartingRadius = "";

                            /// <summary>
                            /// 終點半徑
                            /// </summary>

                            scanEndingRadius = "";
                            /// <summary>
                            /// 內圈圈數
                            /// </summary>

                            scanInnerRingQty = "0";
                            /// <summary>
                            /// 外圈圈數
                            /// </summary>

                            scanOuterRingQty = "0";
                            /// <summary>
                            /// 螺旋間距
                            /// </summary>

                            scanSprialPitch = "";
                            /// <summary>
                            /// 脉衝輪廓
                            /// </summary>

                            pulseProfile = item.pulseProfile;

       }
      public ParaInfo()
       {

       }
        /// <summary>
        /// 軟件名稱，必填參數
        /// </summary>
        public string softwareName ="HG数据追溯系统";
        /// <summary>
        /// 軟件版本，必填參數
        /// </summary>
        public string softwareVersion = "Ver2.5.0.5";
        /// <summary>
        /// 圖檔名稱
        /// </summary>
        public string patternName = "HG准直激光控制系统";
        /// <summary>
        /// 圖檔版本
        /// </summary>
        public string patternVersion = "V1.2.10-20221111";
        /// <summary>
        /// CCD軟件版本
        /// </summary>
        public string ccdVersion = "9.5SR2";

        public string TestHeightZ { set; get; }
        public string Difference { set; get; }
        public string ReferenceV { set; get; }
       
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
        /// 內環第一段波功率
        /// </summary>

        public string a1stFiberCoreWavePower { get; set; }
        /// <summary>
        /// 外環第一段波時間
        /// </summary>

        public string a1stOuterRingWaveTimePeriod { get; set; }
        /// <summary>
        /// 外環第一段波功率
        /// </summary>

        public string a1stOuterRingWavePower { get; set; }

        /// <summary>
        /// 內環第二段波時間
        /// </summary>

        public string a2ndFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第二段波功率
        /// </summary>

        public string a2ndFiberCoreWavePower { get; set; }
        /// <summary>
        /// 外環第二段波時間
        /// </summary>

        public string a2ndOuterRingWaveTimePeriod { get; set; }
        /// <summary>
        /// 外環第二段波功率
        /// </summary>

        public string a2ndOuterRingWavePower { get; set; }

        /// <summary>
        /// 內環第三段波時間
        /// </summary>

        public string a3rdFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第三段波功率
        /// </summary>

        public string a3rdFiberCoreWavePower { get; set; }
        /// <summary>
        /// 外環第三段波時間
        /// </summary>

        public string a3rdOuterRingWaveTimePeriod { get; set; }
        /// <summary>
        /// 外環第三段波功率
        /// </summary>

        public string a3rdOuterRingWavePower { get; set; }

        /// <summary>
        /// 內環第四段波時間
        /// </summary>

        public string a4thFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第四段波功率
        /// </summary>

        public string a4thFiberCoreWavePower { get; set; }
        /// <summary>
        /// 外環第四段波時間
        /// </summary>

        public string a4thOuterRingWaveTimePeriod { get; set; }
        /// <summary>
        /// 外環第四段波功率
        /// </summary>

        public string a4thOuterRingWavePower { get; set; }

        /// <summary>
        /// 內環第五段波時間
        /// </summary>

        public string a5thFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第五段波功率
        /// </summary>

        public string a5thFiberCoreWavePower { get; set; }
        /// <summary>
        /// 外環第五段波時間
        /// </summary>

        public string a5thOuterRingWaveTimePeriod { get; set; }
        /// <summary>
        /// 外環第五段波功率
        /// </summary>

        public string a5thOuterRingWavePower { get; set; }

        /// <summary>
        /// 內環第六段波時間
        /// </summary>

        public string a6thFiberCoreWaveTimePeriod { get; set; }
        /// <summary>
        /// 內環第六段波功率
        /// </summary>

        public string a6thFiberCoreWavePower { get; set; }
        /// <summary>
        /// 外環第六段波時間
        /// </summary>

        public string a6thOuterRingWaveTimePeriod { get; set; }
        /// <summary>
        /// 外環第六段波功率
        /// </summary>

        public string a6thOuterRingWavePower { get; set; }
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
    class AppleReturnInfo
    {
        public string status { get; set; }
        public string id { get; set; }
        public string contact { get; set; }
        public string error { get; set; }
    }
}
