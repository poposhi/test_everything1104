using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace test_everything
{

    class control2
    {
        //    回傳P_tr 
        public static void Pf_control(ref double Hys_line, ref double p_tr, ref double grid_f_last, ref double p_val_last, double bat_rate_p, double grid_f, double fset1, double fset2, double fset3, double fset4, double fset5, double fset6, double pset1, double pset2, double pset3, double pset4, double pset5, double pset6)
        {

            ///////1為橘線    0為藍線  2為區域
            double p_val = 0;
            //if (Grid_Control.flag != 5)
            /*if (Grid_Control.mode_change == true)
            {
                grid_f_last = grid_f;
                Grid_Control.flag = 5;
            }*/
           // Debug.Print($" {0}hz 最外層, grid_f");
            if (grid_f <= fset1)            ///////////////磁滯以外
            {
                p_val = pset1;
                grid_f_last = grid_f;
            }
            else if (grid_f <= fset6 && grid_f > fset1)
            {
                p_val = (grid_f - fset1) * (pset6 - pset1) / (fset6 - fset1) + pset1;
                grid_f_last = grid_f;
            }
            else if (grid_f >= fset3 && grid_f < fset4)
            {
                p_val = (grid_f - fset3) * (pset4 - pset3) / (fset4 - fset3) + pset3;
                grid_f_last = grid_f;
            }
            else if (grid_f >= fset4)
            {
                p_val = pset4;
                grid_f_last = grid_f;
            }
            /////////////////////////遲滯部分            
            else if (fset6 < grid_f && grid_f < fset3) /////電壓介於遲滯
            {
                if (grid_f >= grid_f_last)    ////電壓增加
                {
                    if (Hys_line == 1)       /////藍曲線
                    {
                        if (grid_f < fset2)                   ///////上邊界
                        {
                            p_val = pset2;
                        }
                        else                 ////右邊界
                        {
                            p_val = (grid_f - fset2) * (pset3 - pset2) / (fset3 - fset2) + pset2;
                            grid_f_last = grid_f;
                        }
                    }
                    else if (Hys_line == 0)  /////橘曲線
                    {
                        if (grid_f <= (p_val_last - pset2) * (fset3 - fset2) / (pset3 - pset2) + fset2)  /////遲滯保持不變
                        {
                            grid_f_last = grid_f;
                            p_val = p_val_last;
                        }
                        else                                                                           /////到達藍曲線
                        {
                            grid_f_last = grid_f;
                            p_val = (grid_f - fset2) * (pset3 - pset2) / (fset3 - fset2) + pset2;
                            Hys_line = 1;
                        }
                    }
                }

                else if (grid_f < grid_f_last)    ////f減少
                {
                    if (Hys_line == 0)  /////橘曲線
                    {
                        if (grid_f >= fset5)                   ///////上邊界
                        {
                            Debug.Print("f減少 {0}hz Hys_line == 0",grid_f);
                            p_val = pset5;
                        }
                        else                 ////右邊界
                        {
                            Debug.Print("f減少 {0}hz  Hys_line == 0 else", grid_f);
                            p_val = (grid_f - fset6) * (pset5 - pset6) / (fset5 - fset6) + pset6;
                            grid_f_last = grid_f;
                        }
                    }
                    else if (Hys_line == 1)  /////藍曲線
                    {
                        if (grid_f >= (p_val_last - pset5) * (fset6 - fset5) / (pset6 - pset5) + fset5)  /////遲滯保持不變                           
                        {
                            Debug.Print("f減少 {0}hz Hys_line == 1", grid_f);
                            p_val = p_val_last;
                        }
                        else                                                                           /////到達藍曲線
                        {
                            Debug.Print("f減少 {0}hz Hys_line == 1 else", grid_f);
                            p_val = (grid_f - fset6) * (pset5 - pset6) / (fset5 - fset6) + pset6;
                            Hys_line = 0;
                        }
                    }
                }
            }
            p_val_last = p_val;
            p_tr = bat_rate_p * p_val * 0.01;
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
            
        }
        //OK    回傳q_tr 輸出q 
        public static void Vq_control(ref double Hys_line, ref double q_tr, ref double grid_v_last, ref double q_val_last, double bat_rate_q,double base_v, double grid_v, double vset1, double vset2, double vset3, double vset4, double vset5, double vset6, double qset1, double qset2, double qset3, double qset4, double qset5, double qset6)
        { /////需定義一開始為藍線和儲存Grid_v_last值
            //int Hys_line = 1;              ///////1為藍線    0為橘線  2為區域
            grid_v = grid_v * 100 / base_v;
            double q_val = 0;
            /////////似乎非必要??
            //////////////// 我把這裡除掉 也把這個輸入除掉 
            ////if (Grid_Control.flag != 6)
            /*if (Grid_Control.mode_change == true)
            {
                grid_v_last = grid_v;
                Grid_Control.flag = 6;

            }
            
            if (pq_flag == false)
            {
                if (p_diff > 50)
                {
                    p_diff = p_diff - 50;
                }
                else if (p_diff < -50)
                {
                    p_diff = p_diff + 50;
                }
                else
                {
                    p_diff = 0;
                }

            }*/
            Debug.Print("grid_v{0}v vset1{1}v", grid_v, vset1);
            if (grid_v <= vset1)            ///////////////磁滯以外
            {
                q_val = qset1;
                grid_v_last = grid_v;
            }
            else if (grid_v <= vset6 && grid_v > vset1)
            {
                q_val = (grid_v - vset1) * (qset6 - qset1) / (vset6 - vset1) + qset1;
                grid_v_last = grid_v;
            }
            else if (grid_v >= vset3 && grid_v < vset4)
            {
                q_val = (grid_v - vset3) * (qset4 - qset3) / (vset4 - vset3) + qset3;
                grid_v_last = grid_v;
            }
            else if (grid_v >= vset4)
            {
                q_val = qset4;
                grid_v_last = grid_v;
            }
            /////////////////////////遲滯部分            
            else if (vset6 < grid_v && grid_v < vset3) /////電壓介於遲滯
            {
                Debug.Print("進入遲滯");
                if (grid_v >= grid_v_last)    ////電壓增加
                {
                    if (Hys_line == 1)       /////藍曲線
                    {
                        if (grid_v < vset2)                   ///////上邊界
                        {
                            q_val = qset2;
                        }
                        else                 ////右邊界
                        {
                            q_val = (grid_v - vset2) * (qset3 - qset2) / (vset3 - vset2) + qset2;
                            grid_v_last = grid_v;
                        }
                    }
                    else if (Hys_line == 0)  /////橘曲線
                    {
                        if (grid_v <= (q_val_last - qset2) * (vset3 - vset2) / (qset3 - qset2) + vset2)  /////遲滯保持不變
                        {
                            q_val = q_val_last;
                        }
                        else                                                                           /////到達藍曲線
                        {
                            q_val = (grid_v - vset2) * (qset3 - qset2) / (vset3 - vset2) + qset2;
                            Hys_line = 1;
                        }
                    }
                }

                else if (grid_v < grid_v_last)    ////電壓減少
                {
                    if (Hys_line == 0)  /////橘曲線
                    {
                        if (grid_v >= vset5)                   ///////上邊界
                        {
                            q_val = qset5;
                        }
                        else                 ////右邊界
                        {
                            q_val = (grid_v - vset6) * (qset5 - qset6) / (vset5 - vset6) + qset6;
                            grid_v_last = grid_v;
                        }
                    }
                    else if (Hys_line == 1)  /////藍曲線
                    {
                        if (grid_v >= (q_val_last - qset5) * (vset6 - vset5) / (qset6 - qset5) + vset5)  /////遲滯保持不變                           
                        {
                            q_val = q_val_last;
                        }
                        else                                                                           /////到達藍曲線
                        {
                            q_val = (grid_v - vset6) * (qset5 - qset6) / (vset5 - vset6) + qset6;
                            Hys_line = 0;
                        }
                    }
                }
            }
            q_val_last = q_val;
            q_tr = bat_rate_q * q_val * 0.01;
            //grid_v_last = grid_v;

        }
    }
}
