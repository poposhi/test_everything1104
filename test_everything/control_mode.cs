using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;//debug msg在『輸出』視窗觀看
namespace test_everything
{
    class Grid_Control
    {
        #region 龍井新增 電網控制器變數 
        public static int flag = 0;     ///穩定輸出 1、平滑化2 
        public static double p_out = 0;         //////目標輸出實功
        public static double q_out = 0;         //////目標輸出虛功
        #endregion
        public static string[] mode_define;
        public static string mode_name;
        public static string[] schedule_define;
        public static string schedule_name;
        public static int combine_mode_n = 0;
        public static int mode = 0;  ///目前模式:0:停止、1:穩定輸出、2:平滑化、3:需量、4:排程 
        public static int mode_last = 0;      ///////目前排程模式
        #region 平滑化
        public static double p_diff = 0; //需要補償的功率
        public static double p_tr = 0;
        public static double p_last = 0;
        public static double meter_p = 0;       ///////pv輸出功率
        public static double bat_p = 0;  //儲能系統現在的功率輸出
        public static double p_variance = 2; //平滑化的功率變動率 
        public static double pv_p_rate = 5000;

        #endregion

        public static double q_diff = 0;
        public static ushort p_rtu = 0;
        public static ushort q_rtu = 0;
        public static double soc_max = 0;         //SOC最大值
        public static double soc_min = 0;         //SOC最小值
        public static double back_limit = 0;      //防逆流限制值
        public static double C_Rate_Limit = 1;//////BMS輸出1C限制
        public static bool DC_Couple = true;
        ///////////
        //public static double pv_p_rate = 2000;
        public static double[] meter_p_last = new double[2] { 0, 0 };
        
        public static double PCS_p = 0;       ///////PCS輸出功率
        public static double Grid_f = 0;       ///////PCS輸出功率
        public static double Grid_v = 0;
        public static bool mode_change = true; ///////模式是否更改
        public static double p_bus_rate = 500;  ///////系統額定功率(pcs)
        public static double p_bat_rate = 375;
        public static bool remote = true;         ///////是否運作於ems控制模式
        public static int last_mode = 0;      ///////上次運作模式

        public static int schedule_mode = 0;      ///////目前排程模式
        public static int schedule_mode_last = 0; ///////上次排程模式
        public static double p_ref = 0;         //////目標輸出實功
        public static double q_ref = 0;         //////目標輸出虛功
        public static int limit_condition = 0;  //////輸出限制條件
        
        //////平滑化防止逆送共用
        public static double Grid_v_test = 21600;
    }
    class FR_Hys_Control
    {
        //
        // public static double freq_limit = 0;      //頻率下限值
        // public static double freq_ramp = 0;       //頻率變動斜率
        public static double f1_set = 0;          //頻率點F1
        public static double f2_set = 0;          //頻率點F2
        public static double f3_set = 0;          //頻率點F3
        public static double f4_set = 0;          //頻率點F4
        public static double f5_set = 0;          //頻率點F5
        public static double f6_set = 0;          //頻率點F6
        public static double p1_set = 0;          //功率點P1    百分比功率 
        public static double p2_set = 0;          //功率點P2
        public static double p3_set = 0;          //功率點P3
        public static double p4_set = 0;         //功率點P4
        public static double p5_set = 0;         //功率點P5
        public static double p6_set = 0;         //功率點P6
        public static double p_base = 0;         //頻率實功基底值
        public static bool FR_Enable = false;    ///頻率實功模式參數勾選
        public static double p_tr = 0;
        /////////////////////////////////////////
        public static int Hys_line = 1;
        public static double grid_f_last=60.00;
        public static double p_val_last;
        //////測試
        public static int test_flag = 0;
        public static double f_test = 584;
    }
    class Vq_Control
    {
        public static double v1_set = 0;          //頻率點F1
        public static double v2_set = 0;          //頻率點F2
        public static double v3_set = 0;          //頻率點F3
        public static double v4_set = 0;          //頻率點F4
        public static double v5_set = 0;          //頻率點F5
        public static double v6_set = 0;          //頻率點F6
        public static double q1_set = 0;          //功率點P1
        public static double q2_set = 0;          //功率點P2
        public static double q3_set = 0;          //功率點P3
        public static double q4_set = 0;         //功率點P4
        public static double q5_set = 0;         //功率點P5
        public static double q6_set = 0;         //功率點P6
        public static double q_base = 0;         //頻率實功基底值
        public static double v_base = 0;
        //public static bool FR_Enable = false;    ///頻率實功模式參數勾選
        public static double q_tr = 0;

        /////////////////////////////////////////
        public static int Hys_line = 1;
        public static double grid_v_last;
        public static double q_val_last;
        ///////測試用
        public static double test_flag = 0;

    }
    class control_mode
    {

        public static void fp_Hys_control(double grid_f) // return  Grid_Control.p_diff = FR_Hys_Control.p_base * p_val * 0.01;
        {

            ///////1為橘線(下降)    0為藍線(上升)  2為區域
            ////假如模式改變了就把上次的頻率儲存下來> 假如頻率很低 直接輸出最大功率  並且儲存現在頻率 >>假如頻率在6跟1之間 單純走斜率 >>假如頻率在3跟4之間 單純走斜率 >>假如頻率很大直接吸收最大功率 
            ////假如頻率介於6跟3之間代表介於遲滯>>假如頻率在上升 介在二跟6之間 輸出功率 =p2 >>
            double p_val = 0;
            if (Grid_Control.mode_change == true)  //假如模式改變了就把上次的頻率儲存下來 
            {
                FR_Hys_Control.grid_f_last = grid_f;
            }
            if (grid_f <= FR_Hys_Control.f1_set)       //假如頻率很低 直接輸出最大功率  並且儲存現在頻率 
            {
                p_val = FR_Hys_Control.p1_set;
                FR_Hys_Control.grid_f_last = grid_f;
            }
            else if (grid_f <= FR_Hys_Control.f6_set && grid_f > FR_Hys_Control.f1_set) //假如頻率在6跟1之間 單純走斜率 
            {
                p_val = (grid_f - FR_Hys_Control.f1_set) * (FR_Hys_Control.p6_set - FR_Hys_Control.p1_set) / (FR_Hys_Control.f6_set - FR_Hys_Control.f1_set) + FR_Hys_Control.p1_set;
                FR_Hys_Control.grid_f_last = grid_f;
            }
            else if (grid_f >= FR_Hys_Control.f3_set && grid_f < FR_Hys_Control.f4_set) //假如頻率在3跟4之間 單純走斜率 
            {
                p_val = (grid_f - FR_Hys_Control.f3_set) * (FR_Hys_Control.p4_set - FR_Hys_Control.p3_set) / (FR_Hys_Control.f4_set - FR_Hys_Control.f3_set) + FR_Hys_Control.p3_set;
                FR_Hys_Control.grid_f_last = grid_f;
            }
            else if (grid_f >= FR_Hys_Control.f4_set) //假如頻率很大直接吸收最大功率 
            {
                p_val = FR_Hys_Control.p4_set;
                FR_Hys_Control.grid_f_last = grid_f;
            }
            /////////////////////////遲滯部分            
            else if (FR_Hys_Control.f6_set < grid_f && grid_f < FR_Hys_Control.f3_set)
            {/////假如頻率介於6跟3之間 代表介於遲滯 ，若頻率增加 
                if (grid_f >= FR_Hys_Control.grid_f_last)    ////頻率增加 可能在線上或是藍箭頭
                {
                    if (FR_Hys_Control.Hys_line == 1)       /////藍色曲線/可能在中間區域
                    {
                        if (grid_f < FR_Hys_Control.f2_set)                   ///////上邊界
                        {
                            Debug.Print(grid_f + "hz 頻率增加" + "Hys_line == 1");
                            
                            p_val = FR_Hys_Control.p2_set; //假如頻率在緩衝區頻率又增加又在藍色線上又小於p2 >就代表在上邊界 因此輸出上邊界功率
                        }
                        else    ////上升線 斜率 ，如果頻率在緩衝區 又在上升線上 頻率大於p2 >輸出上升線 斜率功率 
                        {
                            Debug.Print(grid_f + "hz頻率增加" + "功率輸出斜線 ");
                            p_val = (grid_f - FR_Hys_Control.f2_set) * (FR_Hys_Control.p3_set - FR_Hys_Control.p2_set) / (FR_Hys_Control.f3_set - FR_Hys_Control.f2_set) + FR_Hys_Control.p2_set;
                            FR_Hys_Control.grid_f_last = grid_f;
                        }
                    }
                    else if (FR_Hys_Control.Hys_line == 0)  /////假如不在上升曲線    ///假如頻率是在下降曲線  (左)
                    {
                        Debug.Print(grid_f+"hz 頻率增加" + "Hys_line == 0");
                        if (grid_f <= (FR_Hys_Control.p_val_last - FR_Hys_Control.p2_set) * (FR_Hys_Control.f3_set - FR_Hys_Control.f2_set) / (FR_Hys_Control.p3_set - FR_Hys_Control.p2_set) + FR_Hys_Control.f2_set)  /////遲滯保持不變
                        {
                            //計算出來的結果還在緩衝區內部 
                            Debug.Print(grid_f + "hz 頻率增加" + "頻率在線上");
                            p_val = FR_Hys_Control.p_val_last;
                        }
                        else       /////假如不在上升線的左邊就要更新功率輸出 並且設定目前在上升曲線上 
                        {
                            //我加的
                            //p_val = (grid_f - FR_Hys_Control.f2_set) * (FR_Hys_Control.p3_set - FR_Hys_Control.p2_set) / (FR_Hys_Control.f3_set - FR_Hys_Control.f2_set) + FR_Hys_Control.p2_set;
                            FR_Hys_Control.Hys_line = 1;
                        }
                    }
                }

                else if (grid_f < FR_Hys_Control.grid_f_last)    ////頻率減少
                {
                    if (FR_Hys_Control.Hys_line == 0)  /////橘曲線
                    {
                        if (grid_f >= FR_Hys_Control.f5_set)                   ///////上邊界
                        {
                            p_val = FR_Hys_Control.p5_set;
                        }
                        else                 ////右邊界
                        {
                            p_val = (grid_f - FR_Hys_Control.f6_set) * (FR_Hys_Control.p5_set - FR_Hys_Control.p6_set) / (FR_Hys_Control.f5_set - FR_Hys_Control.f6_set) + FR_Hys_Control.p6_set;
                            FR_Hys_Control.grid_f_last = grid_f;
                        }
                    }
                    else if (FR_Hys_Control.Hys_line == 1)  /////藍曲線
                    {
                        if (grid_f >= (FR_Hys_Control.p_val_last - FR_Hys_Control.p5_set) * (FR_Hys_Control.f6_set - FR_Hys_Control.f5_set) / (FR_Hys_Control.p6_set - FR_Hys_Control.p5_set) + FR_Hys_Control.f5_set)  /////遲滯保持不變                           
                        {
                            p_val = FR_Hys_Control.p_val_last;
                            //我加的
                            //p_val = (grid_f - FR_Hys_Control.f6_set) * (FR_Hys_Control.p5_set - FR_Hys_Control.p6_set) / (FR_Hys_Control.f5_set - FR_Hys_Control.f6_set) + FR_Hys_Control.p6_set;
                        }
                        else                                                                           /////到達藍曲線
                        {
                            p_val = (grid_f - FR_Hys_Control.f6_set) * (FR_Hys_Control.p5_set - FR_Hys_Control.p6_set) / (FR_Hys_Control.f5_set - FR_Hys_Control.f6_set) + FR_Hys_Control.p6_set;
                            FR_Hys_Control.Hys_line = 0;
                        }
                    }
                }
            }
            FR_Hys_Control.p_val_last = p_val;
            Grid_Control.p_diff = FR_Hys_Control.p_base * p_val * 0.01;  // 百分比換算 
            //if (Vq_Control.q_tr > 50)
            //{
            //    Vq_Control.q_tr = Vq_Control.q_tr - 50;
            //}
            //else if (Vq_Control.q_tr < -50)
            //{
            //    Vq_Control.q_tr = Vq_Control.q_tr + 50;
            //}
            //else
            //{
            //    Vq_Control.q_tr = 0;
            //}
        }
        
        public static void Vq_control(double grid_v, bool pq_mode) //return Vq_Control.q_tr = Vq_Control.q_base * q_val * 0.01;
        { /////需定義一開始為藍線和儲存Grid_v_last值
            //int Hys_line = 1;              ///////1為藍線    0為橘線  2為區域
            grid_v = grid_v * 100 / Vq_Control.v_base;  //換算百分比
            double q_val = 0;
            /////////似乎非必要??
            ////if (Grid_Control.flag != 6)
            if (Grid_Control.mode_change == true)
            {
                Vq_Control.grid_v_last = grid_v;
            }
            /////////////
            if (pq_mode == false)
            {
                if (Grid_Control.p_diff > 50)
                {
                    Grid_Control.p_diff = Grid_Control.p_diff - 50;
                }
                else if (Grid_Control.p_diff < -50)
                {
                    Grid_Control.p_diff = Grid_Control.p_diff + 50;
                }
                else
                {
                    Grid_Control.p_diff = 0;
                }

            }
            if (grid_v <= Vq_Control.v1_set)            ///////////////磁滯以外
            {
                q_val = Vq_Control.q1_set;
                Vq_Control.grid_v_last = grid_v;
            }
            else if (grid_v <= Vq_Control.v6_set && grid_v > Vq_Control.v1_set)
            {
                q_val = (grid_v - Vq_Control.v1_set) * (Vq_Control.q6_set - Vq_Control.q1_set) / (Vq_Control.v6_set - Vq_Control.v1_set) + Vq_Control.q1_set;
                Vq_Control.grid_v_last = grid_v;
            }
            else if (grid_v >= Vq_Control.v3_set && grid_v < Vq_Control.v4_set)
            {
                q_val = (grid_v - Vq_Control.v3_set) * (Vq_Control.q4_set - Vq_Control.q3_set) / (Vq_Control.v4_set - Vq_Control.v3_set) + Vq_Control.q3_set;
                Vq_Control.grid_v_last = grid_v;
            }
            else if (grid_v >= Vq_Control.v4_set)
            {
                q_val = Vq_Control.q4_set;
                Vq_Control.grid_v_last = grid_v;
            }
            /////////////////////////遲滯部分            
            else if (Vq_Control.v6_set < grid_v && grid_v < Vq_Control.v3_set) /////電壓介於遲滯
            {
                Debug.Print("{0}v進入遲滯", grid_v);
                if (grid_v >= Vq_Control.grid_v_last)    ////電壓增加
                {
                    if (Vq_Control.Hys_line == 1)       /////藍曲線
                    {
                        if (grid_v < Vq_Control.v2_set)                   ///////上邊界
                        {
                            q_val = Vq_Control.q2_set;
                        }
                        else                 ////右邊界
                        {
                            q_val = (grid_v - Vq_Control.v2_set) * (Vq_Control.q3_set - Vq_Control.q2_set) / (Vq_Control.v3_set - Vq_Control.v2_set) + Vq_Control.q2_set;
                            Vq_Control.grid_v_last = grid_v;
                        }
                    }
                    else if (Vq_Control.Hys_line == 0)  /////橘曲線
                    {
                        if (grid_v <= (Vq_Control.q_val_last - Vq_Control.q2_set) * (Vq_Control.v3_set - Vq_Control.v2_set) / (Vq_Control.q3_set - Vq_Control.q2_set) + Vq_Control.v2_set)  /////遲滯保持不變
                        {
                            q_val = Vq_Control.q_val_last;
                        }
                        else                                                                           /////到達藍曲線
                        {
                            q_val = (grid_v - Vq_Control.v2_set) * (Vq_Control.q3_set - Vq_Control.q2_set) / (Vq_Control.v3_set - Vq_Control.v2_set) + Vq_Control.q2_set;
                            Vq_Control.Hys_line = 1;
                        }
                    }
                }

                else if (grid_v < Vq_Control.grid_v_last)    ////電壓減少
                {
                    if (Vq_Control.Hys_line == 0)  /////橘曲線
                    {
                        if (grid_v >= Vq_Control.v5_set)                   ///////上邊界
                        {
                            q_val = Vq_Control.q5_set;
                        }
                        else                 ////右邊界
                        {
                            q_val = (grid_v - Vq_Control.v6_set) * (Vq_Control.q5_set - Vq_Control.q6_set) / (Vq_Control.v5_set - Vq_Control.v6_set) + Vq_Control.q6_set;
                            Vq_Control.grid_v_last = grid_v;
                        }
                    }
                    else if (Vq_Control.Hys_line == 1)  /////藍曲線
                    {
                        if (grid_v >= (Vq_Control.q_val_last - Vq_Control.q5_set) * (Vq_Control.v6_set - Vq_Control.v5_set) / (Vq_Control.q6_set - Vq_Control.q5_set) + Vq_Control.v5_set)  /////遲滯保持不變                           
                        {
                            Debug.Print(" {0}v電壓下降 Hline= 1 大於橘色線 ", grid_v);
                            q_val = Vq_Control.q_val_last;
                        }
                        else                                                                           /////到達藍曲線
                        {
                            Debug.Print(" {0}v電壓下降 Hline= 1 小於橘色線 ", grid_v);
                            q_val = (grid_v - Vq_Control.v6_set) * (Vq_Control.q5_set - Vq_Control.q6_set) / (Vq_Control.v5_set - Vq_Control.v6_set) + Vq_Control.q6_set;
                            Vq_Control.Hys_line = 0;
                        }
                    }
                }
            }
            Vq_Control.q_val_last = q_val;
            Vq_Control.q_tr = Vq_Control.q_base * q_val * 0.01;
            //grid_v_last = grid_v;

        }
        
        public static double map_to_line_x2y(double x_axis_diff, double x_axis_Lengh, double y_axis_Lengh, double y_axis_Benchmark)
        {//找到斜線上對應的數值 
            return x_axis_diff * y_axis_Lengh / x_axis_Lengh + y_axis_Benchmark;
        }
        public static double map_to_line_y2x(double y_axis_diff, double x_axis_Lengh, double y_axis_Lengh, double x_axis_Benchmark)
        {//找到斜線上對應的數值 
            return y_axis_diff * x_axis_Lengh / y_axis_Lengh + x_axis_Benchmark;
        }
    }

}
