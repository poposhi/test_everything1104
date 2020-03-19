using System;


namespace test_everything
{
    class Smooth
    {
        public static double p_variance = 0;    //平滑化功率變動限制
        //public static double pv_rate = 250;
        public static DateTime BaseTime;
        public static double pv_p_avg; //	每3秒平均輸出功率 
        public static double pv_rated = 1000;
        public static double count; //計數器 
        public static double p_variance_cal = 0;
        //////AC耦合

        public static double p_last; //上一次輸出功率
        public static double p_pv_max;
        public static double p_pv_min;

        ///////考慮逆送
        public static double p_limit;
        public static double p_limit_new;
        public static double ramp_down;
        public static double anti_pv_rated = 1000;
        public static double Flow_Back_p_variance_cal = 0; //沒有用到 
    }
    /*
    class Grid_Control
    {
        public static double cc_i = 0;
        public static double cc_v = 0;

        public static bool bms_fault = false;
        public static bool pcs_fault = false;
        public static bool control_set_flag; //設定參數
        public static string[] mode_define;
        public static string mode_name = "無";
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
        public static bool DC_Couple = false;
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
        public static int remote = 0;         ///////是否運作於ems控制模式 0:SEMS 1:EMS  2:Local
        public static int last_mode = 0;      ///////上次運作模式

        public static int schedule_mode = 0;      ///////目前排程模式
        public static int schedule_mode_last = 0; ///////上次排程模式
        public static double p_ref = 0;         //////目標輸出實功
        public static double q_ref = 0;         //////目標輸出虛功
        public static int limit_condition = 0;  //////輸出限制條件
        public static double p_tr = 0;
        //////平滑化防止逆送共用
        public static double Grid_v_test = 21600;


        //龍井
        public static bool PV_mode_no;
        public static bool PV_mode;
        public static bool Wind_mode_no;
        public static bool Wind_mode;
        //限制
        public static double BMS_p_max = 1000;
        public static double BMS_p_min = -1000;
        public static double BMS_q_max = 1000;
        public static double BMS_q_min = -1000;
        //Error
        public static bool[,] error = new bool[4, 7];
        public static bool[,] Communicate_error = new bool[4, 16];
        //SEMS
        public static int SEMS_Communicate_error_count = 0;
    }*/
}
