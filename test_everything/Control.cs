using System;
namespace test_everything
{
    class AC_Couple_Mode
    {
        public static void Smoothing_mode(double p_pv, double bat_p, double p_variance, double p_soc_compenstation)
        {
            //bat_p = Grid_Control.p_diff;  ///////測試用途
            p_variance = p_variance * 0.01;
            DateTime time_now = DateTime.Now;
            #region 註解  改變基準功率  
            //if (p_pv + bat_p < 100)
            //{
            //    Smooth.pv_p_rate = 100;
            //}
            //else if (p_pv + bat_p >= 400)
            //{
            //    Smooth.pv_p_rate = 500;
            //}
            //else if (p_pv + bat_p >= 300)
            //{
            //    Smooth.pv_p_rate = 400;
            //}
            //else if (p_pv + bat_p >= 200)
            //{
            //    Smooth.pv_p_rate = 300;
            //}
            //else if (p_pv + bat_p >= 100)
            //{
            //    Smooth.pv_p_rate = 200;
            //}
            #endregion
            #region 初始化  執行時機 ::只有變動模式才會執行
            if (Grid_Control.mode_change == true)
            {

                //Grid_Control.meter_p_last = new double[2] { p_pv, p_pv };//沒有用到 
                //Smooth.BaseTime = DateTime.Now; //沒有用到 
                Smooth.pv_p_avg = 0;            //平均功率 
                Smooth.p_last = p_pv + bat_p;   //初始條件，最一開始的目標值 
                //Smooth.pv_rated = Smooth.p_last;  //////若基底為目前平均功率
                Smooth.p_pv_max = Smooth.p_last + p_variance * Smooth.pv_rated * 0.1;//功率變動率10%當作上緩衝區
                Smooth.p_pv_min = Smooth.p_last - p_variance * Smooth.pv_rated * 0.1;//功率變動率10%當作下緩衝區
                Grid_Control.p_tr = Smooth.p_last; //設定目標值 
            }
            //Smooth.pv_p_rate = p_pv + bat_p;   //////若基底為目前平均功率 當此值為負數，將會異常
            #endregion

            #region 正常功能區  每3秒會執行一次  代表每次可以變動 3秒功率變動率的8成(剩下20%拿去做緩衝區 )
            if (Smooth.count >= 3)
            {
                Smooth.pv_p_avg = Smooth.pv_p_avg / 3 + p_soc_compenstation; // 計算前3秒PV輸出平均功率
                Smooth.count = 0;
                //Smooth.BaseTime = DateTime.Now;
                #region 過去3秒假如平均功率超過變動量的上下限制 ，就要把目標值設定在上/下限制 沒有超過 目標值就等於平均功率 
                if (Smooth.pv_p_avg - Smooth.p_last > p_variance * Smooth.pv_rated * 0.04) //////每3秒平均值變動範圍為  (80%功率變動率) /20 = (20%*80% )/20  =0.04
                {
                    Smooth.p_last = Smooth.p_last + p_variance * Smooth.pv_rated * 0.04;
                }
                else if (Smooth.pv_p_avg - Smooth.p_last < -p_variance * Smooth.pv_rated * 0.04)
                {
                    Smooth.p_last = Smooth.p_last - p_variance * Smooth.pv_rated * 0.04;
                }
                else //假如功率變動很小 
                {
                    Smooth.p_last = Smooth.pv_p_avg;
                }
                #endregion
                #region 功率變動率的10% 當作  功率變動緩衝區 減少pcs輸出改變次數 
                Smooth.p_pv_max = Smooth.p_last + p_variance * Smooth.pv_rated * 0.1;
                Smooth.p_pv_min = Smooth.p_last - p_variance * Smooth.pv_rated * 0.1;
                Smooth.pv_p_avg = 0;
                #endregion
            }

            #region soc補償量 計算最終的輸出功率  假如計算出來的結果 超出限制值 就不補償 
            double p_pv_soc = p_pv + p_soc_compenstation;
            if (p_pv_soc > Smooth.p_pv_max)//&& 計算出來結果 假如大於  最大功率 就限制在最大功率 
            {
                Grid_Control.p_tr = Smooth.p_pv_max;
            }
            else if (p_pv_soc < Smooth.p_pv_min)// && 計算出來結果 假如小於  最小功率 就限制在最小功率 
            {
                Grid_Control.p_tr = Smooth.p_pv_min;
            }
            else
            {
                Grid_Control.p_tr = p_pv_soc; //假如沒有超過上下限制值 就輸出
            }
            #endregion
            #endregion
            #region 每秒鐘會執行一次 累積計算PV輸出功率  計算計數器 
            //p_tr = SOC_limit(Grid_Control.soc_max, Grid_Control.soc_min, Grid_Control.soc_now, p_tr);
            Grid_Control.p_diff = Grid_Control.p_tr - p_pv; //儲能系統輸出功率 =目標功率- pv輸出功率 
            Smooth.pv_p_avg = Smooth.pv_p_avg + p_pv;
            Smooth.count = Smooth.count + 1;
            #region 假如有輸出虛功應該要慢慢的歸零 
            if (Vq_Control.q_tr > 50)
            {
                Vq_Control.q_tr = Vq_Control.q_tr - 50;
            }
            else if (Vq_Control.q_tr < -50)
            {
                Vq_Control.q_tr = Vq_Control.q_tr + 50;
            }
            else
            {
                Vq_Control.q_tr = 0;
            }
            #endregion

            #endregion
        }
    }
}
