using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_everything
{
    class Class1 : IComparable<Class1>
    {
        public int num_people { get; set; } //不需要分號 
        public string teacher_name { get; set; }
      
        int IComparable<Class1>.CompareTo(Class1 other)
        {
            return num_people.CompareTo(other.num_people);
        }
        //建構子 
        public Class1(string name)
        {
            teacher_name = name;
        }

 
    }
    public class Serial_port   //串列連線設定檔 
                               //應該是在儲存串列通訊的參數
    {
        public bool[] id_enable = new bool[8]; //設備啟動狀態 最多可以連線8個設備 
        public string com = "";
        public int baud = 0;
        public int bit = 0;
        public int parity = 0;
        public int stop_bit = 0;
        public string id1 = "";
        public string id2 = "";
        public string id3 = "";
        public string id4 = "";
        public string id5 = "";
        public string id6 = "";
        public string id7 = "";
        public string id8 = "";


        public string Com { get { return com; } set { com = value; } }
        public int Baud { get { return baud; } set { baud = value; } }
        public int Bit { get { return bit; } set { bit = value; } }
        public int Parity { get { return parity; } set { parity = value; } }
        public int Stop_bit { get { return stop_bit; } set { stop_bit = value; } }
        public string Id1 { get { return id1; } set { id1 = value; } }            //////PCS           ///////  string1     ///////  MBMS     //////// Frequency
        public string Id2 { get { return id2; } set { id2 = value; } }             //////PV_Inverter   ///////  string2     ///////  Meter1
        public string Id3 { get { return id3; } set { id3 = value; } }             //////              ///////  string3     ///////  Meter2
        public string Id4 { get { return id4; } set { id4 = value; } }             //////              ///////  string4
        public string Id5 { get { return id5; } set { id5 = value; } }             //////              ///////  string5
        public string Id6 { get { return id6; } set { id6 = value; } }             //////              ///////  radiance
        public string Id7 { get { return id7; } set { id7 = value; } }             //////              ///////  wind_speed
        public string Id8 { get { return id8; } set { id8 = value; } }             //////              ///////  temp
    }
    public class PCS
    {
        private string device_ID = "";
        private double error_count = 0;
        private bool read_or_not;
        private string communication_error = "";
        private double count = 0;
        private bool ems_flag = false;      //判斷是否缺值
        private int ems_error = 0;
        private ushort[] holding_register = new ushort[53];
        private ushort error1 = 0;
        private ushort error2 = 0;
        private ushort error3 = 0;
        private ushort error4 = 0;
        private double v_grid1 = 0;
        private double v_grid2 = 0;
        private double v_grid3 = 0;
        private double v_out1 = 0;
        private double v_out2 = 0;
        private double v_out3 = 0;
        private double i_out1 = 0;
        private double i_out2 = 0;
        private double i_out3 = 0;
        private double f_offgrid = 0;
        private double f_grid = 0;
        private double i_n = 0;
        private double temp_inner = 0;
        private double temp_sink = 0;
        private double v_dc = 0;
        private double i_dc = 0;
        private double p_dc = 0;
        private double s_sum = 0;
        private double p_sum = 0;
        private double q_sum = 0;
        private double s_out1 = 0;
        private double p_out1 = 0;
        private double pf_out1 = 0;
        private double s_out2 = 0;
        private double p_out2 = 0;
        private double pf_out2 = 0;
        private double s_out3 = 0;
        private double p_out3 = 0;
        private double pf_out3 = 0;
        private double kwh_chg = 0;
        private double kwh_dischg = 0;
        private double status_operation = 0;
        private double status_grid = 0;
        private ushort[] errorbit2ushort = new ushort[3];
        private bool[,] error_bit = new bool[3, 16];
        public double Continuous_Communication_ErrorSeconds { get; set; }
        public string communication_w_error { get; set; }
        public double error_w_count { get; set; }
        public double S_rated { get; set; }
        public bool control_pcs { get; set; }
        public double Error_count { get { return error_count; } set { error_count = value; } }
        public string Communication_error { get { return communication_error; } set { communication_error = value; } }
        public double Count { get { return count; } set { count = value; } }
        public bool Ems_flag { get { return ems_flag; } set { ems_flag = value; } }  //判斷是否缺值
        public int Ems_error { get { return ems_error; } set { ems_error = value; } }
        public ushort[] Holding_register { get { return holding_register; } set { holding_register = value; } }
        public ushort Error1 { get { return error1; } set { error1 = value; } }
        public ushort Error2 { get { return error2; } set { error2 = value; } }
        public ushort Error3 { get { return error3; } set { error3 = value; } }
        public ushort Error4 { get { return error4; } set { error4 = value; } }
        public double V_grid1 { get { return v_grid1; } set { v_grid1 = value; } }
        public double V_grid2 { get { return v_grid2; } set { v_grid2 = value; } }
        public double V_grid3 { get { return v_grid3; } set { v_grid3 = value; } }
        public double V_out1 { get { return v_out1; } set { v_out1 = value; } }
        public double V_out2 { get { return v_out2; } set { v_out2 = value; } }
        public double V_out3 { get { return v_out3; } set { v_out3 = value; } }
        public double I_out1 { get { return i_out1; } set { i_out1 = value; } }
        public double I_out2 { get { return i_out2; } set { i_out2 = value; } }
        public double I_out3 { get { return i_out3; } set { i_out3 = value; } }
        public double F_offgrid { get { return f_offgrid; } set { f_offgrid = value; } }
        public double F_grid { get { return f_grid; } set { f_grid = value; } }
        public double I_n { get { return i_n; } set { i_n = value; } }
        public double Temp_inner { get { return temp_inner; } set { temp_inner = value; } }
        public double Temp_sink { get { return temp_sink; } set { temp_sink = value; } }
        public double V_dc { get { return v_dc; } set { v_dc = value; } }
        public double I_dc { get { return i_dc; } set { i_dc = value; } }
        public double P_dc { get { return p_dc; } set { p_dc = value; } }
        public double S_sum { get { return s_sum; } set { s_sum = value; } }
        public double P_sum { get { return p_sum; } set { p_sum = value; } }
        public double Q_sum { get { return q_sum; } set { q_sum = value; } }
        public double S_out1 { get { return s_out1; } set { s_out1 = value; } }
        public double P_out1 { get { return p_out1; } set { p_out1 = value; } }
        public double Pf_out1 { get { return pf_out1; } set { pf_out1 = value; } }
        public double S_out2 { get { return s_out2; } set { s_out2 = value; } }
        public double P_out2 { get { return p_out2; } set { p_out2 = value; } }
        public double Pf_out2 { get { return pf_out2; } set { pf_out2 = value; } }
        public double S_out3 { get { return s_out3; } set { s_out3 = value; } }
        public double P_out3 { get { return p_out3; } set { p_out3 = value; } }
        public double Pf_out3 { get { return pf_out3; } set { pf_out3 = value; } }
        public double Kwh_chg { get { return kwh_chg; } set { kwh_chg = value; } }
        public double Kwh_dischg { get { return kwh_dischg; } set { kwh_dischg = value; } }
        public double Status_operation { get { return status_operation; } set { status_operation = value; } }
        public double Status_grid { get { return status_grid; } set { status_grid = value; } }
        public ushort[] Errorbit2ushort { get => errorbit2ushort; set => errorbit2ushort = value; }
        public bool[,] Error_bit { get => error_bit; set => error_bit = value; }
        public string Device_ID { get => device_ID; set => device_ID = value; }
        public bool Read_or_not { get => read_or_not; set => read_or_not = value; }

        private static double Negative_two_num(ushort num1, ushort num2)
        {
            double result = 0;
            if (num1 >= 32768)
            {
                result = (double)(num1 - 65535) * 65536 + (num2 - 65536);
            }
            else
            {
                result = num1 * 65536 + num2;
            }
            return result;
        }
        private static double Negative_num(ushort num) //2' 補數轉換
        {
            double result = 0;
            if (num >= 32768)
            {
                result = num - 65535;
            }
            else
            {
                result = num;
            }
            return result;
        }
        public void Put_Data1() //把讀取到的資料 放到相應的變數 
        {
            status_operation = holding_register[0]; //5001
            Error1 = holding_register[1];
            Error2 = holding_register[2];
            Error3 = holding_register[3];
            Error4 = holding_register[4];
            v_grid1 = Negative_num(holding_register[5]) * 0.1;//電壓電流應該不會有負數吧 
            v_grid2 = Negative_num(holding_register[6]) * 0.1;
            v_grid3 = Negative_num(holding_register[7]) * 0.1;
            v_out1 = Negative_num(holding_register[8]) * 0.1;
            v_out2 = Negative_num(holding_register[9]) * 0.1;
            v_out3 = Negative_num(holding_register[10]) * 0.1;
            i_out1 = Negative_num(holding_register[11]) * 0.1;
            i_out2 = Negative_num(holding_register[12]) * 0.1;
            i_out3 = Negative_num(holding_register[13]) * 0.1;
            f_offgrid = holding_register[14] * 0.01;
            f_grid = holding_register[15] * 0.01;
            i_n = Negative_num(holding_register[16]) * 0.1;
            temp_inner = Negative_num(holding_register[17]) * 0.1;
            temp_sink = Negative_num(holding_register[18]) * 0.1;
            v_dc = holding_register[19] * 0.1;
            i_dc = Negative_num(holding_register[20]) * 0.1;
            p_dc = Negative_num(holding_register[21]) * 0.1;
            s_sum = holding_register[22] * 0.1;
            p_sum = Negative_num(holding_register[23]) * 0.1;
            q_sum = Negative_num(holding_register[24]) * 0.1;
            s_out1 = holding_register[25] * 0.1;
            p_out1 = Negative_num(holding_register[26]) * 0.1;
            pf_out1 = Negative_num(holding_register[27]) * 0.01;
            s_out2 = holding_register[29] * 0.1;
            p_out2 = Negative_num(holding_register[30]) * 0.1;
            pf_out2 = Negative_num(holding_register[31]) * 0.01;
            s_out3 = holding_register[33] * 0.1;
            p_out3 = Negative_num(holding_register[34]) * 0.1;
            pf_out3 = Negative_num(holding_register[35]) * 0.01;
            kwh_chg = (holding_register[44] * 65536 + holding_register[45]) * 0.1;
            kwh_dischg = (holding_register[46] * 65536 + holding_register[47]) * 0.1;
            status_grid = holding_register[52];
        }
    }
}
