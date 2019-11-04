using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_everything
{
    class Grid_Control
    {
        public static string[] mode_define;
        public static string mode_name;
        public static string[] schedule_define;
        public static string schedule_name;
        public static int combine_mode_n = 0;
        public static int mode = 0;  ///目前模式:0:停止、1:穩定輸出、2:平滑化、3:需量、4:排程 
        public static int mode_last = 0;      ///////目前排程模式
        public static double p_diff = 0;
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
        public static double meter_p = 0;       ///////pv輸出功率
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
        public static double p_tr = 0;
        //////平滑化防止逆送共用
        public static double Grid_v_test = 21600;
    }
    class FR_Hys_Control
    {
        // public static double freq_limit = 0;      //頻率下限值
        // public static double freq_ramp = 0;       //頻率變動斜率
        public static double f1_set = 0;          //頻率點F1
        public static double f2_set = 0;          //頻率點F2
        public static double f3_set = 0;          //頻率點F3
        public static double f4_set = 0;          //頻率點F4
        public static double f5_set = 0;          //頻率點F5
        public static double f6_set = 0;          //頻率點F6
        public static double p1_set = 0;          //功率點P1
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
        public static double grid_f_last;
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
        public static double v_base = 22800;
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
        public static void fp_Hys_control(double grid_f)
        {

            ///////1為橘線    0為藍線  2為區域
            double p_val = 0;
            if (Grid_Control.mode_change == true)
            {
                FR_Hys_Control.grid_f_last = grid_f;
            }
            if (grid_f <= FR_Hys_Control.f1_set)            ///////////////磁滯以外
            {
                p_val = FR_Hys_Control.p1_set;
                FR_Hys_Control.grid_f_last = grid_f;
            }
            else if (grid_f <= FR_Hys_Control.f6_set && grid_f > FR_Hys_Control.f1_set)
            {
                p_val = (grid_f - FR_Hys_Control.f1_set) * (FR_Hys_Control.p6_set - FR_Hys_Control.p1_set) / (FR_Hys_Control.f6_set - FR_Hys_Control.f1_set) + FR_Hys_Control.p1_set;
                FR_Hys_Control.grid_f_last = grid_f;
            }
            else if (grid_f >= FR_Hys_Control.f3_set && grid_f < FR_Hys_Control.f4_set)
            {
                p_val = (grid_f - FR_Hys_Control.f3_set) * (FR_Hys_Control.p4_set - FR_Hys_Control.p3_set) / (FR_Hys_Control.f4_set - FR_Hys_Control.f3_set) + FR_Hys_Control.p3_set;
                FR_Hys_Control.grid_f_last = grid_f;
            }
            else if (grid_f >= FR_Hys_Control.f4_set)
            {
                p_val = FR_Hys_Control.p4_set;
                FR_Hys_Control.grid_f_last = grid_f;
            }
            /////////////////////////遲滯部分            
            else if (FR_Hys_Control.f6_set < grid_f && grid_f < FR_Hys_Control.f3_set) /////電壓介於遲滯
            {
                if (grid_f >= FR_Hys_Control.grid_f_last)    ////電壓增加
                {
                    if (FR_Hys_Control.Hys_line == 1)       /////藍曲線
                    {
                        if (grid_f < FR_Hys_Control.f2_set)                   ///////上邊界
                        {
                            p_val = FR_Hys_Control.p2_set;
                        }
                        else                 ////右邊界
                        {
                            p_val = (grid_f - FR_Hys_Control.f2_set) * (FR_Hys_Control.p3_set - FR_Hys_Control.p2_set) / (FR_Hys_Control.f3_set - FR_Hys_Control.f2_set) + FR_Hys_Control.p2_set;
                            FR_Hys_Control.grid_f_last = grid_f;
                        }
                    }
                    else if (FR_Hys_Control.Hys_line == 0)  /////橘曲線
                    {
                        if (grid_f <= (FR_Hys_Control.p_val_last - FR_Hys_Control.p2_set) * (FR_Hys_Control.f3_set - FR_Hys_Control.f2_set) / (FR_Hys_Control.p3_set - FR_Hys_Control.p2_set) + FR_Hys_Control.f2_set)  /////遲滯保持不變
                        {
                            p_val = FR_Hys_Control.p_val_last;
                        }
                        else                                                                           /////到達藍曲線
                        {
                            p_val = (grid_f - FR_Hys_Control.f2_set) * (FR_Hys_Control.p3_set - FR_Hys_Control.p2_set) / (FR_Hys_Control.f3_set - FR_Hys_Control.f2_set) + FR_Hys_Control.p2_set;
                            FR_Hys_Control.Hys_line = 1;
                        }
                    }
                }

                else if (grid_f < FR_Hys_Control.grid_f_last)    ////電壓減少
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
            Grid_Control.p_diff = FR_Hys_Control.p_base * p_val * 0.01;
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
        public static void Vq_control(double grid_v, bool pq_mode)
        { /////需定義一開始為藍線和儲存Grid_v_last值
            //int Hys_line = 1;              ///////1為藍線    0為橘線  2為區域
            grid_v = grid_v * 100 / Vq_Control.v_base;
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
                            q_val = Vq_Control.q_val_last;
                        }
                        else                                                                           /////到達藍曲線
                        {
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
    }

}
