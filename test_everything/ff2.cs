using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_everything
{
    public partial class ff2 : Form
    {
        PCS PCS1 = new PCS(); //先創造一個要顯示用的物件 
        public ff2(PCS PCS2)
        {
            InitializeComponent(); 
            PCS1 = PCS2;
            //////避免閃爍
            tableLayoutPanel3.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel3, true, null);
            tableLayoutPanel2.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel2, true, null);
        }

        private void ff2_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Enabled = true;
            F_off_grid.Text = PCS1.F_offgrid.ToString() + "Hz"; F_grid.Text = PCS1.F_grid.ToString("#0.00") + "Hz";
            V1_grid.Text = PCS1.V_grid1.ToString() + "V"; V2_grid.Text = PCS1.V_grid2.ToString() + "V";
            V3_gird.Text = PCS1.V_grid3.ToString() + "V"; V_dc.Text = PCS1.V_dc.ToString() + "V";
            I_dc.Text = PCS1.I_dc.ToString() + "A"; P_dc.Text = PCS1.P_dc.ToString() + "A";
            I_n.Text = PCS1.I_n.ToString() + "A"; Chg_kWh.Text = PCS1.Kwh_chg.ToString() + "kWh";
            Dischg_kWh.Text = PCS1.Kwh_dischg.ToString() + "kWh"; V1_out.Text = PCS1.V_out1.ToString() + "V";
            V2_out.Text = PCS1.V_out2.ToString() + "V"; V3_Out.Text = PCS1.V_out3.ToString() + "V";
            I1_out.Text = PCS1.I_out1.ToString() + "A"; I2_out.Text = PCS1.I_out2.ToString() + "A";
            I3_out.Text = PCS1.I_out3.ToString() + "A"; P_sum.Text = PCS1.P_sum.ToString() + "kW";
            Q_sum.Text = PCS1.Q_sum.ToString() + "kVar"; S_sum.Text = PCS1.S_sum.ToString() + "kVA";
            S1_out.Text = PCS1.S_out1.ToString() + "kVA"; S2_out.Text = PCS1.S_out2.ToString() + "kVA";
            S3_out.Text = PCS1.S_out3.ToString() + "kVA"; P1_out.Text = PCS1.P_out1.ToString() + "kW";
            P2_out.Text = PCS1.P_out2.ToString() + "kW"; P3_out.Text = PCS1.P_out3.ToString() + "kW";
            PF1.Text = PCS1.Pf_out1.ToString(); PF2.Text = PCS1.Pf_out2.ToString();
            PF3.Text = PCS1.Pf_out3.ToString(); status.Text = PCS1.Status_operation.ToString();
            status_grid.Text = PCS1.Status_grid.ToString(); temp_sink.Text = PCS1.Temp_sink.ToString() + "°C";
            temp_internal.Text = PCS1.Temp_inner.ToString() + "°C"; Error1.Text = PCS1.Error1.ToString();
            Error2.Text = PCS1.Error2.ToString(); Error3.Text = PCS1.Error3.ToString();
            Error4.Text = PCS1.Error4.ToString();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            F_off_grid.Text = PCS1.F_offgrid.ToString() + "Hz"; F_grid.Text = PCS1.F_grid.ToString() + "Hz";
            V1_grid.Text = PCS1.V_grid1.ToString() + "V"; V2_grid.Text = PCS1.V_grid2.ToString() + "V";
            V3_gird.Text = PCS1.V_grid3.ToString() + "V"; V_dc.Text = PCS1.V_dc.ToString() + "V";
            I_dc.Text = PCS1.I_dc.ToString() + "A"; P_dc.Text = PCS1.P_dc.ToString() + "A";
            I_n.Text = PCS1.I_n.ToString() + "A"; Chg_kWh.Text = PCS1.Kwh_chg.ToString() + "kWh";
            Dischg_kWh.Text = PCS1.Kwh_dischg.ToString() + "kWh"; V1_out.Text = PCS1.V_out1.ToString() + "V";
            V2_out.Text = PCS1.V_out2.ToString() + "V"; V3_Out.Text = PCS1.V_out3.ToString() + "V";
            I1_out.Text = PCS1.I_out1.ToString() + "A"; I2_out.Text = PCS1.I_out2.ToString() + "A";
            I3_out.Text = PCS1.I_out3.ToString() + "A"; P_sum.Text = PCS1.P_sum.ToString() + "kW";
            Q_sum.Text = PCS1.Q_sum.ToString() + "kVar"; S_sum.Text = PCS1.S_sum.ToString() + "kVA";
            S1_out.Text = PCS1.S_out1.ToString() + "kVA"; S2_out.Text = PCS1.S_out2.ToString() + "kVA";
            S3_out.Text = PCS1.S_out3.ToString() + "kVA"; P1_out.Text = PCS1.P_out1.ToString() + "kW";
            P2_out.Text = PCS1.P_out2.ToString() + "kW"; P3_out.Text = PCS1.P_out3.ToString() + "kW";
            PF1.Text = PCS1.Pf_out1.ToString(); PF2.Text = PCS1.Pf_out2.ToString();
            PF3.Text = PCS1.Pf_out3.ToString(); status.Text = PCS1.Status_operation.ToString();
            status_grid.Text = PCS1.Status_grid.ToString(); temp_sink.Text = PCS1.Temp_sink.ToString() + "°C";
            temp_internal.Text = PCS1.Temp_inner.ToString() + "°C"; Error1.Text = PCS1.Error1.ToString();
            Error2.Text = PCS1.Error2.ToString(); Error3.Text = PCS1.Error3.ToString();
            Error4.Text = PCS1.Error4.ToString();
        }
        private void ff2_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }


    }
}
