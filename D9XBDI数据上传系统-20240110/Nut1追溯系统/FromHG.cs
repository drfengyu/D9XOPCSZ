using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 卓汇数据追溯系统
{
       public class FromHG
        {
            public string Barcode { get; set; }
            //
            public string swing_amplitude; //摆幅      
            public string swing_freq; //摆动频率    
            public string pattern_type; //摆动图像类型  
            public string PrecitecValue; //值
            public string PrecitecProcessRev; //版本号 
            public string PrecitecGrading; //检测等级  
            public string PrecitecResult; //总结果

            public results[] results;

            public heights[] heights;
        }

       public class results
        {
            public string result; //焊接结果
            public string test; //焊点
            public string sub_test; //参数
            public string units; //单位
            public string value; //值
        }
       public class heights {
           public string test; //焊点
           public string sub_test; //参数
           public string units; //单位
           public string value; //值
       }
        ////焊点1 RM2
        //public string location1_powerDiode = "20";
        //public string location1_powerFiber = "20";
        //public string location1_frequency = "900";
        //public string location1_waveform = "3";
        //public string location1_laser_speed = "100";
        //public string location1_jump_speed = "2000";
        //public string location1_jump_delay = "10000";
        //public string location1_position_delay = "0";
        //public string location1_pulse_profile = "0";
        //public string location1_laser_height1 = "0.1";

        ////焊点2 LM1   
        //public string location2_powerDiode = "20";
        //public string location2_powerFiber = "20";
        //public string location2_frequency = "900";
        //public string location2_waveform = "3";
        //public string location2_laser_speed = "100";
        //public string location2_jump_speed = "2000";
        //public string location2_jump_delay = "10000";
        //public string location2_position_delay = "0";
        //public string location2_pulse_profile = "0";
        //public string location2_laser_height1 = "0.1";

        ////焊点3 LT
        //public string location3_powerDiode = "20";
        //public string location3_powerFiber = "20";
        //public string location3_frequency = "900";
        //public string location3_waveform = "3";
        //public string location3_laser_speed = "100";
        //public string location3_jump_speed = "2000";
        //public string location3_jump_delay = "10000";
        //public string location3_position_delay = "0";
        //public string location3_pulse_profile = "0";
        //public string location3_laser_height1 = "0.1";

        ////焊点4 LT-R
        //public string location4_powerDiode = "20";
        //public string location4_powerFiber = "20";
        //public string location4_frequency = "900";
        //public string location4_waveform = "3";
        //public string location4_laser_speed = "100";
        //public string location4_jump_speed = "2000";
        //public string location4_jump_delay = "10000";
        //public string location4_position_delay = "0";
        //public string location4_pulse_profile = "0";
        //public string location4_laser_height1 = "0.1";

        ////焊点5 LM2
        //public string location5_powerDiode = "20";
        //public string location5_powerFiber = "20";
        //public string location5_frequency = "900";
        //public string location5_waveform = "3";
        //public string location5_laser_speed = "100";
        //public string location5_jump_speed = "2000";
        //public string location5_jump_delay = "10000";
        //public string location5_position_delay = "0";
        //public string location5_pulse_profile = "0";
        //public string location5_laser_height1 = "0.1";

        ////焊点6 LB
        //public string location6_powerDiode = "20";
        //public string location6_powerFiber = "20";
        //public string location6_frequency = "900";
        //public string location6_waveform = "3";
        //public string location6_laser_speed = "100";
        //public string location6_jump_speed = "2000";
        //public string location6_jump_delay = "10000";
        //public string location6_position_delay = "0";
        //public string location6_pulse_profile = "0";
        //public string location6_laser_height1 = "0.1";

        ////焊点7 LB-R
        //public string location7_powerDiode = "20";
        //public string location7_powerFiber = "20";
        //public string location7_frequency = "900";
        //public string location7_waveform = "3";
        //public string location7_laser_speed = "100";
        //public string location7_jump_speed = "2000";
        //public string location7_jump_delay = "10000";
        //public string location7_position_delay = "0";
        //public string location7_pulse_profile = "0";
        //public string location7_laser_height1 = "0.1";

        ////焊点8 RM1
        //public string location8_powerDiode = "20";
        //public string location8_powerFiber = "20";
        //public string location8_frequency = "900";
        //public string location8_waveform = "3";
        //public string location8_laser_speed = "100";
        //public string location8_jump_speed = "2000";
        //public string location8_jump_delay = "10000";
        //public string location8_position_delay = "0";
        //public string location8_pulse_profile = "0";
        //public string location8_laser_height1 = "0.1";

        ////焊点9 RT
        //public string location9_powerDiode = "20";
        //public string location9_powerFiber = "20";
        //public string location9_frequency = "900";
        //public string location9_waveform = "3";
        //public string location9_laser_speed = "100";
        //public string location9_jump_speed = "2000";
        //public string location9_jump_delay = "10000";
        //public string location9_position_delay = "0";
        //public string location9_pulse_profile = "0";
        //public string location9_laser_height1 = "0.1";

        ////焊点10 RT-R
        //public string location10_powerDiode = "20";
        //public string location10_powerFiber = "20";
        //public string location10_frequency = "900";
        //public string location10_waveform = "3";
        //public string location10_laser_speed = "100";
        //public string location10_jump_speed = "2000";
        //public string location10_jump_delay = "10000";
        //public string location10_position_delay = "0";
        //public string location10_pulse_profile = "0";
        //public string location10_laser_height1 = "0.1";

        ////焊点11 RM3
        //public string location11_powerDiode = "20";
        //public string location11_powerFiber = "20";
        //public string location11_frequency = "900";
        //public string location11_waveform = "3";
        //public string location11_laser_speed = "100";
        //public string location11_jump_speed = "2000";
        //public string location11_jump_delay = "10000";
        //public string location11_position_delay = "0";
        //public string location11_pulse_profile = "0";
        //public string location11_laser_height1 = "0.1";

        ////焊点12 RB
        //public string location12_powerDiode = "20";
        //public string location12_powerFiber = "20";
        //public string location12_frequency = "900";
        //public string location12_waveform = "3";
        //public string location12_laser_speed = "100";
        //public string location12_jump_speed = "2000";
        //public string location12_jump_delay = "10000";
        //public string location12_position_delay = "0";
        //public string location12_pulse_profile = "0";
        //public string location12_laser_height1 = "0.1";

        ////焊点13 RB-R
        //public string location13_powerDiode = "20";
        //public string location13_powerFiber = "20";
        //public string location13_frequency = "900";
        //public string location13_waveform = "3";
        //public string location13_laser_speed = "100";
        //public string location13_jump_speed = "2000";
        //public string location13_jump_delay = "10000";
        //public string location13_position_delay = "0";
        //public string location13_pulse_profile = "0";
        //public string location13_laser_height1 = "0.1";

    }
