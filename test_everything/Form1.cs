using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// 下面四個4  nmodbus
using Modbus;
using Modbus.Device;
using Modbus.Data;
using Modbus.Message;

using System.Diagnostics;//debug msg在『輸出』視窗觀看
using System.IO.Ports;  //for serial port
using System.Threading;  // 可以讓整個執行緒停止  Thread.Sleep(2000);
using ThreadingTimer = System.Threading.Timer;  //可以開一個平行緒計算時間 


namespace test_everything
{
    public partial class Form1 : Form
    {
        #region 宣告變數  串列連結  Master List 
        SerialPort serialPort = new SerialPort();
        ModbusSerialMaster master_test_everthing;
        private Class1 aaaaaaa = new Class1("amy");
        private Class1 bbbbbbb = new Class1("babe");
        List<Class1> school = new List<Class1>();
        private PCS PCS_TEST_everthing = new PCS();

        //電網變數
        static double grid_f = 0;
        static double grid_v=0;
        //不是我的
        List<TextBox> listAI = new List<TextBox>();
        List<TextBox> listAO = new List<TextBox>();
        List<PictureBox> listDI = new List<PictureBox>();
        List<PictureBox> listDO = new List<PictureBox>();
        #endregion
        public Form1()
        {
            InitializeComponent();
            bt_test_read_pcs.Enabled = false;
            InitialListView();
            textBox_q.Enabled = false;
            textBox_p.Enabled = false;
        }
        private void InitialListView()//初始化ListView的格式大小 
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.LabelEdit = false;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("time", 150);
            listView1.Columns.Add("message",200);
            //雙緩衝
            listView1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
   | System.Reflection.BindingFlags.NonPublic).SetValue(listView1, true, null);
        }

        #region 控制項委派用
        public delegate void Listview_Print(ListView list, string time, string message);//time type 沒改
        public delegate void lPrintHandler(Label label, string text);
        public static void l_Print(Label tb, string text)
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (tb.InvokeRequired)
            {
                lPrintHandler ph = new lPrintHandler(l_Print);
                tb.Invoke(ph, tb, text);
            }
            else
            {
                tb.Text = text;
            }
        }
        public static void lv_Print(ListView list, string time, string message)// 輸入listview ,兩個str
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (list.InvokeRequired)
            {
                Listview_Print ph = new Listview_Print(lv_Print);
                list.Invoke(ph, list, time, message);
            }
            else
            {
                String[] row = { time, message };
                ListViewItem item = new ListViewItem(row);
                //ADD ITEMS
                list.Items.Add(item);
                if (list.Items.Count > 1000)
                {
                    list.Items.RemoveAt(1);
                }
            }
        }
        public static void lv_Print(ListView list,string message)// 輸入listview ,兩個str
        {
            String time = DateTime.Now.ToString();
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (list.InvokeRequired)
            {
                Listview_Print ph = new Listview_Print(lv_Print);
                list.Invoke(ph, list, time, message);
            }
            else
            {
                String[] row = { time, message };
                ListViewItem item = new ListViewItem(row);
                //ADD ITEMS
                list.Items.Add(item);
                if (list.Items.Count > 1000)
                {
                    list.Items.RemoveAt(1);
                }
            }
        }
        #endregion
        private void bt_test_fp_Click(object sender, EventArgs e)
        {
            //設定工作點  
            FR_Hys_Control.f1_set = 59;
            FR_Hys_Control.f2_set =59.3 ;
            FR_Hys_Control.f3_set = 60.9;
            FR_Hys_Control.f4_set = 61;
            FR_Hys_Control.f5_set = 60.7;
            FR_Hys_Control.f6_set = 59.1;
            FR_Hys_Control.p1_set = 100; //百分比功率好像是直接那樣寫
            FR_Hys_Control.p2_set = 90;
            FR_Hys_Control.p3_set = -90;
            FR_Hys_Control.p4_set = -100;
            FR_Hys_Control.p5_set = -90;
            FR_Hys_Control.p6_set = 90;
            FR_Hys_Control.p_base = 250;
            double grid_f = 60;
            control_mode.fp_Hys_control(grid_f);
            lv_Print(listView1, DateTime.Now.ToString(), Grid_Control.p_diff.ToString());    //輸出功率 
            for (int i= 0; i <10;i++)
            {
                grid_f +=  0.1;
                control_mode.fp_Hys_control(grid_f);
                lv_Print(listView1, DateTime.Now.ToString(), grid_f+"hz  "+ Grid_Control.p_diff.ToString()+"w");    //輸出功率 
            }
            for (int i = 0; i < 20; i++)
            {
                grid_f -= 0.1;
                control_mode.fp_Hys_control(grid_f);
                lv_Print(listView1, DateTime.Now.ToString(), grid_f + "hz  " + Grid_Control.p_diff.ToString() + "w");    //輸出功率 
            }
            #region 不會用到    測試可以比較的物件 
            /*aaaaaaa.num_people = 10;
            bbbbbbb.num_people = 20;
            school.Add(bbbbbbb);
            school.Add(aaaaaaa);
            foreach (var item in school)
            {
                Debug.Print(item.teacher_name);
            }
            school.Sort();
            foreach (var item in school)
            {
                Debug.Print(item.teacher_name);
            }
            //Debug.Print( school.ToString());
            */
            #endregion
        }
        //可以在副程式之外  創立物件 


        private void bt_start_Click(object sender, EventArgs e)
        { //副程式目的  :: 建立一個master rtu 並且寫入資料 
          //設定serialPort參數 柯華pcs參數 传输模式：RTU 波特率：默认为 9600bps，并可设置为 2400，4800，19200bps 校验位：无校验 数据位：8bit 停止位：1bit 
            #region rtu 連線
            try
            {
                serialPort.PortName = "COM3";
                serialPort.BaudRate = 9600;
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.Open();
                master_test_everthing = ModbusSerialMaster.CreateRtu(serialPort);
                Debug.Print(DateTime.Now.ToString() + " =>Open " + serialPort.PortName + " sucessfully!");
                lv_Print(listView1, DateTime.Now.ToString(), " =>Open " + serialPort.PortName + " sucessfully!");
            }
            catch
            {
                serialPort.Close();
                Thread.Sleep(2000);
                serialPort.Open();
                Console.WriteLine(DateTime.Now.ToString() + " =>Disconnect " + serialPort.PortName);
                lv_Print(listView1, DateTime.Now.ToString() + " =>Disconnect " + serialPort.PortName);
            }
            #endregion
            //master_test_everthing.WriteSingleRegister(1, 5001, 555);
            #region modbus 通訊測試
            try
            { 
                master_test_everthing.Transport.Retries = 0;   //don't have to do retries
                master_test_everthing.Transport.ReadTimeout = 300; //milliseconds
                                                    //master.ReadHoldingRegisters(1, startAddress, numofPoints);
                master_test_everthing.WriteSingleRegister(1, 6008, 555); //結果會寫入46009 
                byte slaveID = 1;
                ushort startAddress = 1, vvalue = 1;
            }
            catch(Exception ex)
            {
                Debug.Print("modbus Exception"+ ex.Message);
                lv_Print(listView1, DateTime.Now.ToString(), "modbus Exception" + ex.Message);
            }

            bt_test_read_pcs.Enabled = true;
            #endregion
            //timer1.Enabled = true;
            //timer2.Enabled = true;

        }
        private void bt_read_Click(object sender, EventArgs e)
        {
            ushort[] a;
            a=master_test_everthing.ReadHoldingRegisters(1,2,1);
            Debug.Print(a[0].ToString());
            
        }
        //


        private void button1_Click(object sender, EventArgs e)
        {//檢查這個視窗是否有已經被打開，假如已經打開了就把它放到最前面  假如還沒有打開這個視窗，就把他打開並且畫面置中
            try
            {
                bool Isopen = false; //檢查這個視窗是否有已經被打開
                foreach (Form f in Application.OpenForms) //假如已經打開了就把它放到最前面 ， ::掃描了每一個視窗 ，尋找對應的名稱 把他打開並且放在最前面 
                {
                    if (f.Text == "ff2")
                    {
                        Isopen = true;
                        f.BringToFront();
                    }
                }
                if (Isopen == false) //假如還沒有打開這個視窗，就把他打開並且畫面置中
                {
                    using (var frm = new ff2(PCS_TEST_everthing))
                    {
                        frm.StartPosition = FormStartPosition.CenterParent;   /////顯示畫面置中
                        frm.ShowDialog();
                    }
                }
                button1.BackColor = Color.SkyBlue; //正常情況按鈕的顏色是藍色 
            }
            catch //假如發生故障就把按鈕改成紅色 
            {
                button1.BackColor = Color.Red;
            }
            //ff2 ff2_name = new ff2(PCS_TEST_everthing);
            //ff2_name.Show();

        }
        private void Serial_master_Connect(ref SerialPort serialport, ref ModbusSerialMaster master, Serial_port port, Button open_btn, Button close_btn2)
        { //串列連線 並且改變按鈕啟動狀態 ，輸入 :ref 串列連線物件  ref Master物件 ，串列設定檔 ，開啟 關閉按鈕，

            #region RTU
            //取得串列連線的參數  port baud bit
            serialport.PortName = port.Com.ToString();
            serialport.BaudRate = port.baud;
            serialport.DataBits = port.bit;
            //設定 奇偶校驗位
            if (port.parity == 0)
                serialport.Parity = Parity.None;
            else if (port.parity == 2)
                serialport.Parity = Parity.Even;
            else
                serialport.Parity = Parity.Odd;
            //設定停止位元
            if (port.stop_bit == 0)
                serialport.StopBits = StopBits.One;
            else
                serialport.StopBits = StopBits.Two;

            try
            {// 串列 開啟連線 送到master裡面 設定重新連線次數  等待時間 更改按鈕 
                serialport.Open(); 
                master = ModbusSerialMaster.CreateRtu(serialport);
                master.Transport.Retries = 0;   //don't have to do retries
                master.Transport.ReadTimeout = 1000; //milliseconds
                Console.WriteLine(DateTime.Now.ToString() + " =>Open " + serialport.PortName + " sucessfully!");
                open_btn.Enabled = false;
                close_btn2.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            #endregion
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                byte slaveID = 1;
                ushort startAddress = 0;
                ushort numofPoints = 4;
                //read DI(1xxxx)
                //bool[] status = master.ReadInputs(slaveID, startAddress, numofPoints);
                //for (int i = 0; i < numofPoints; i++)
                //{
                //    if (status[i] == true)
                //        listDI[i].BackColor = Color.DodgerBlue;
                //    else
                //        listDI[i].BackColor = Color.Navy;
                //}
                //read DO(0xxxx)
                //bool[] coilstatus = master.ReadCoils(slaveID, startAddress, numofPoints);
                //for (int i = 0; i < numofPoints; i++)/
                //{
                //    if (coilstatus[i] == true)
                //        listDO[i].BackColor = Color.Red;
                //    else
                //        listDO[i].BackColor = Color.DarkRed;
                //}
                //read AI(3xxxx)                        
                ushort[] register = master_test_everthing.ReadInputRegisters(slaveID, startAddress, numofPoints);
                for (int i = 0; i < numofPoints; i++)
                {
                    //listAI[i].Text = register[i].ToString();
                    
                    //If you need to show the value with other unit, you have to caculate the gain and offset
                    //eq. 0 to 0kg, 32767 to 1000kg
                    //0 (kg) = gain * 0 + offset
                    //1000 (kg) = gain *32767 + offset
                    //=> gain=1000/32767, offset=0
                    //double value = (double)register[i] * 10.0 / 32767;
                    //listAI[i].Text = value.ToString("0.00");
                }
                int a =0;
                //read AO(4xxxx)
                //ushort[] holdingregister = master.ReadHoldingRegisters(slaveID, startAddress, numofPoints);
                //for (int i = 0; i < numofPoints; i++)
                //{
                //    listAO[i].Text = holdingregister[i].ToString();

                //    //If you need to show the value with other unit, you have to caculate the gain and offset
                //    //eq. 0 to 0 mA, 32767 to 20 mA
                //    //0 (mA) = gain * 0 + offset
                //    //20 (mA) = gain *32767 + offset
                //    //=> gain=20/32767, offset=0
                //    //double holdvalue = (double)holdingregister[i] * 20.0 / 32767;
                //    //listAO[i].Text = holdvalue.ToString("0.00");
                //}
            }
            catch (Exception exception)
            {
                Debug.Print(exception.Message);
                //Connection exception
                //No response from server.
                //The server maybe close the com port, or response timeout.
                if (exception.Source.Equals("System"))
                {
                    Console.WriteLine(DateTime.Now.ToString() + " " + exception.Message);
                }
                //The server return error code.
                //You can get the function code and exception code.
                if (exception.Source.Equals("nModbusPC"))
                {
                    string str = exception.Message;
                    int FunctionCode=0;
                    string ExceptionCode="0";

                    //str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    //FunctionCode = Convert.ToInt16(str.Remove(str.IndexOf("\r\n")));
                    //Console.WriteLine("Function Code: " + FunctionCode.ToString("X"));

                    //str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    //ExceptionCode = str.Remove(str.IndexOf("-"));
                    switch (ExceptionCode.Trim())
                    {
                        case "1":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal function!");
                            break;
                        case "2":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data address!");
                            break;
                        case "3":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data value!");
                            break;
                        case "4":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Slave device failure!");
                            break;
                    }
                    /*
                       //Modbus exception codes definition                            
                       * Code   * Name                                      * Meaning
                         01       ILLEGAL FUNCTION                            The function code received in the query is not an allowable action for the server.
                         
                         02       ILLEGAL DATA ADDRESS                        The data addrdss received in the query is not an allowable address for the server.
                         
                         03       ILLEGAL DATA VALUE                          A value contained in the query data field is not an allowable value for the server.
                           
                         04       SLAVE DEVICE FAILURE                        An unrecoverable error occurred while the server attempting to perform the requested action.
                             
                         05       ACKNOWLEDGE                                 This response is returned to prevent a timeout error from occurring in the client (or master)
                                                                              when the server (or slave) needs a long duration of time to process accepted request.
                          
                         06       SLAVE DEVICE BUSY                           The server (or slave) is engaged in processing a long–duration program command , and the
                                                                              client (or master) should retransmit the message later when the server (or slave) is free.
                             
                         08       MEMORY PARITY ERROR                         The server (or slave) attempted to read record file, but detected a parity error in the memory.
                             
                         0A       GATEWAY PATH UNAVAILABLE                    The gateway is misconfigured or overloaded.
                             
                         0B       GATEWAY TARGET DEVICE FAILED TO RESPOND     No response was obtained from the target device. Usually means that the device is not present on the network.
                     */
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Debug.Print(listAO.ToString());
            string currentName = new StackTrace(true).GetFrame(0).GetMethod().Name;
            //this._ThreadTimer = new ThreadingTimer(new System.Threading.TimerCallback(a_print), currentName, 0, 100);
            //this._ThreadTimer2 = new ThreadingTimer(new System.Threading.TimerCallback(bt_test_thead_Click), currentName, 0, 1000);
        }


        //Thread oThreadA = new Thread(new ThreadStart(Read_PCS));  
        // ???  不能夠開一個平行緒  Thread wait = new Thread(new ThreadStart(bt_test_thead_Click)); 
        // 要傳入的變數 ,延遲多久開始執行,  每隔多久執行一次  ，所以應該是開一個平行緒一直重複的在做這些事情 
   
        
        private void bt_test_thead_Click(object sender, EventArgs e)
        {
            Debug.Print("into bt_test_thead_Click") ;
            Thread wait = new Thread(new ThreadStart(a_print));
            
            Thread.Sleep(1000);
            Debug.Print("等1秒後 ");
            wait.Start();
            
            Debug.Print("Wait之後 ");
            Thread.Sleep(1000);
            Debug.Print(" Wait之後  等1秒後 ");

        }
        private void a_print ()
        {
            Debug.Print("  ppppppppp") ;
            Thread.Sleep(3000);
            Debug.Print(" 等3秒後  ppppppppp");
        }


 

        private void Read_PCS_Kehua(string Port_ID, string Device_ID, PCS Device, ref DateTime time_now)
        {
            if (Port_ID != "None") //
            {
                byte idd = 1;

                try
                {//
                    byte id = byte.Parse(Port_ID);
                    Device.Holding_register = master_test_everthing.ReadInputRegisters(id, 5000, 54); // 讀取大部分的資料  從5001開始
                    time_now = DateTime.Now; //更新現在時間 
                    Device.Put_Data1(); //根據地址轉換資料並且放入相對應的變數 (地址轉文字)
                    PCS_Error_log(Device.Device_ID, time_now, PCS_TEST_everthing);
                    if (Device.Error_count > 5)                             //不了解這個的意思 
                    {
                        // Mongo_EReset(Device_ID, Device.Communication_error, time_now);
                        Debug.Print("into if (Device.Error_count > 5)    ");
                    }
                    time_now = DateTime.Now;
                    Device.Error_count = 0;
                    Device.Continuous_Communication_ErrorSeconds = 0;
                    Thread.Sleep(20);                                       //為什麼這裡會需要等待20毫秒 
                }
                catch (Exception exception)
                {
                    time_now = PCS_Communication_Ex(time_now, exception, Device.Device_ID, ref Device);
                    Console.WriteLine("PCS_Read_Error");
                    Debug.Print("PCS_Read_Error");
                }
            }
            else { Device.Read_or_not = false; } //假如沒有串列連結設定檔 要顯示沒有讀取 
        }

        private DateTime PCS_Communication_Ex(DateTime time_now, Exception exception, string Device_ID, ref PCS Device)
        {
            time_now = DateTime.Now;
            Device.Error_count = Device.Error_count + 1;//		故障次數加一 
            string str = exception.Message;
            int FunctionCode;
            if (exception.Source.Equals("System")) //假如例外是這樣子  設備的通訊錯誤就是這樣 
            {
                Device.Communication_error = "No response from Device";
            }
            else if (exception.Source.Equals("NModbus4")) //假如是nmodbus故障 ，就把故障名稱放到通訊故障 
            {
                try
                {
                    str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    FunctionCode = Convert.ToInt16(str.Remove(str.IndexOf("\r\n")));
                    str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    FunctionCode = Convert.ToInt16(str.Remove(str.IndexOf("-")));
                    switch (FunctionCode)
                    {
                        case 1:
                            Device.Communication_error = "ILLEGAL FUNCTION";
                            break;
                        case 2:
                            Device.Communication_error = "ILLEGAL DATA ADDRESS";
                            break;
                        case 3:
                            Device.Communication_error = "ILLEGAL DATA VALUE";
                            break;
                        case 4:
                            Device.Communication_error = "SLAVE DEVICE FAILURE";
                            break;
                        case 5:
                            Device.Communication_error = "ACKNOWLEDGE";
                            break;
                        case 6:
                            Device.Communication_error = "SLAVE DEVICE BUSY";
                            break;
                        case 8:
                            Device.Communication_error = "MEMORY PARITY ERROR";
                            break;
                        case 10:
                            Device.Communication_error = "GATEWAY PATH UNAVAILABLE";
                            break;
                        case 11:
                            Device.Communication_error = "GATEWAY TARGET DEVICE FAILED TO RESPOND";
                            break;
                        default:
                            Device.Communication_error = "Other Problem";
                            break;
                    }
                }
                catch
                {
                    if (Device.Communication_error != "No response from Device")
                    {
                        Device.Communication_error = "NModbus4 Problem";
                    }
                    Console.WriteLine("PCS_Comunnication_Ex_Error");
                }
            }
            if (Device.Error_count == 5) //假如故障到達5次就上傳到資料庫 
            {
                Device.Read_or_not = false;
                Debug.Print("into  PCS_Communication_Ex  if (Device.Error_count == 5)");
            }
            return time_now;
        }

        private void PCS_Error_log(string Device_ID, DateTime error_time,PCS PCS1)
        {
            #region Error1   Error1 (ushort )
            int a = 0;
            string err_msg = "";
            if (PCS1.Error1 != PCS1.Errorbit2ushort[0])                //////判斷PCS故障碼是否變更
            {
                a = PCS1.Error1 & 1;                            //////判斷故障原因(Bit1)
                if (a == 1)
                {
                    if (PCS1.Error_bit[0, 0] == false)           //////發生故障，判斷上一次是否正常，若正常，即為新故障
                    {
                        err_msg = "Insulation Fault";
                        PCS1.Error_bit[0, 0] = true;
                        pcs_error(Device_ID, err_msg, error_time); //故障代碼要映射到對應的暫存器 
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 0] == true)            ///////系統正常，判斷上一次是否故障，若故障，即為故障復歸
                    {
                        err_msg = "Insulation Fault";
                        PCS1.Error_bit[0, 0] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////
                
                a = PCS1.Error1 & 2;
                if (a == 2)
                {
                    if (PCS1.Error_bit[0, 1] == false)
                    {
                        err_msg = "Leakage Current";
                        PCS1.Error_bit[0, 1] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 1] == true)
                    {
                        err_msg = "Leakage Current";
                        PCS1.Error_bit[0, 1] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////
                a = PCS1.Error1 & 4;
                if (a == 4)
                {
                    if (PCS1.Error_bit[0, 2] == false)
                    {
                        err_msg = "DC Over Voltage";
                        PCS1.Error_bit[0, 2] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 2] == true)
                    {
                        err_msg = "DC Over Voltage";
                        PCS1.Error_bit[0, 2] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////
                a = PCS1.Error1 & 8;
                if (a == 8)
                {
                    if (PCS1.Error_bit[0, 3] == false)
                    {
                        err_msg = "Grid Voltage Abnornal";
                        PCS1.Error_bit[0, 3] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 3] == true)
                    {
                        err_msg = "Grid Voltage Abnornal";
                        PCS1.Error_bit[0, 3] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////
                a = PCS1.Error1 & 16;
                if (a == 16)
                {
                    if (PCS1.Error_bit[0, 4] == false)
                    {
                        err_msg = "Grid Line Connection Abnornal";
                        PCS1.Error_bit[0, 4] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 4] == true)
                    {
                        err_msg = "Grid Line Connection Abnornal";
                        PCS1.Error_bit[0, 4] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////
                //a = PCS1.Error1 & 32;
                //if (a == 32)
                //{
                //    if (PCS1.Error_bit[0, 5] == false)
                //    {
                //        err_msg = "備用";
                //        PCS1.Error_bit[0, 5] = true;
                //    }
                //}
                //else if (a == 0)
                //{
                //    if (PCS1.Error_bit[0, 5] == true)
                //    {
                //        err_msg = "備用";
                //        PCS1.Error_bit[0, 5] = false;
                //    }
                //}
                /////////////////////////////////////////////
                a = PCS1.Error1 & 64;
                if (a == 64)
                {
                    if (PCS1.Error_bit[0, 6] == false)
                    {
                        err_msg = "Grid Frequency Abnormal";
                        PCS1.Error_bit[0, 6] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 6] == true)
                    {
                        err_msg = "Grid Frequency Abnormal";
                        PCS1.Error_bit[0, 6] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                //////////////////////////////////////
                a = PCS1.Error1 & 128;
                if (a == 128)
                {
                    if (PCS1.Error_bit[0, 7] == false)
                    {
                        err_msg = "IGBT Over Temperature";
                        PCS1.Error_bit[0, 7] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 7] == true)
                    {
                        err_msg = "IGBT Over Temperature";
                        PCS1.Error_bit[0, 7] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////////
                //a = PCS1.Error1 & 256;
                //if (a == 256)
                //{
                //    if (PCS1.Error_bit[0, 8] == false)
                //    {
                //        err_msg = "保留";
                //        PCS1.Error_bit[0, 8] = true;
                //    }
                //}
                //else if (a == 0)
                //{
                //    if (PCS1.Error_bit[0, 8] == true)
                //    {
                //        err_msg = "保留";
                //        PCS1.Error_bit[0, 8] = false;
                //    }
                //}
                ///////////////////////////////////////////////
                a = PCS1.Error1 & 512;
                if (a == 512)
                {
                    if (PCS1.Error_bit[0, 9] == false)
                    {
                        err_msg = "Over Current";
                        PCS1.Error_bit[0, 9] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 9] == true)
                    {
                        err_msg = "Over Current";
                        PCS1.Error_bit[0, 9] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                //////////////////////////////////////////////////
                a = PCS1.Error1 & 1024;
                if (a == 1024)
                {
                    if (PCS1.Error_bit[0, 10] == false)
                    {
                        err_msg = "DC Soft Boot Fault";
                        PCS1.Error_bit[0, 10] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 10] == true)
                    {
                        err_msg = "DC Soft Boot Fault";
                        PCS1.Error_bit[0, 10] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////////
                a = PCS1.Error1 & 2048;
                if (a == 2048)
                {
                    if (PCS1.Error_bit[0, 11] == false)
                    {
                        err_msg = "DC Contactor Fault";
                        PCS1.Error_bit[0, 11] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 11] == true)
                    {
                        err_msg = "DC Contactor Fault";
                        PCS1.Error_bit[0, 11] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                //////////////////////////////////////////////
                a = PCS1.Error1 & 4096;
                if (a == 4096)
                {
                    if (PCS1.Error_bit[0, 12] == false)
                    {
                        err_msg = "Wind Turbine Fault";
                        PCS1.Error_bit[0, 12] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 12] == true)
                    {
                        err_msg = "Wind Turbine Fault";
                        PCS1.Error_bit[0, 12] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////////////////
                a = PCS1.Error1 & 8192;
                if (a == 8192)
                {
                    if (PCS1.Error_bit[0, 13] == false)
                    {
                        err_msg = "Contactor Fault";
                        PCS1.Error_bit[0, 13] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 13] == true)
                    {
                        err_msg = "Contactor Fault";
                        PCS1.Error_bit[0, 13] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ////////////////////////////////////////////////////////
                a = PCS1.Error1 & 16384;
                if (a == 16384)
                {
                    if (PCS1.Error_bit[0, 14] == false)
                    {
                        err_msg = "Switch Disconnect in Operation";
                        PCS1.Error_bit[0, 14] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 14] == true)
                    {
                        err_msg = "Switch Disconnect in Operation";
                        PCS1.Error_bit[0, 14] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////////////
                a = PCS1.Error1 & 32768;
                if (a == 32768)
                {
                    if (PCS1.Error_bit[0, 15] == false)
                    {
                        err_msg = "Hardware Fault";
                        PCS1.Error_bit[0, 15] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[0, 15] == true)
                    {
                        err_msg = "Hardware Fault";
                        PCS1.Error_bit[0, 15] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }

            }
            #endregion
            PCS1.Errorbit2ushort[0] = PCS1.Error1;
            #region PCS Error2
            if (PCS1.Error2 != PCS1.Errorbit2ushort[1])
            {
                a = PCS1.Error2 & 1;
                if (a == 1)
                {
                    if (PCS1.Error_bit[1, 0] == false)
                    {
                        err_msg = "Internal Over temperature";
                        PCS1.Error_bit[1, 0] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 0] == true)
                    {
                        err_msg = "Internal Over temperature";
                        PCS1.Error_bit[1, 0] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////

                a = PCS1.Error2 & 2;
                if (a == 2)
                {
                    if (PCS1.Error_bit[1, 1] == false)
                    {
                        err_msg = "Soft Boot Fault";
                        PCS1.Error_bit[1, 1] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 1] == true)
                    {
                        err_msg = "Soft Boot Fault";
                        PCS1.Error_bit[1, 1] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////
                a = PCS1.Error2 & 4;
                if (a == 4)
                {
                    if (PCS1.Error_bit[1, 2] == false)
                    {
                        err_msg = "Communication Fault";
                        PCS1.Error_bit[1, 2] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 2] == true)
                    {
                        err_msg = "Communication Fault";
                        PCS1.Error_bit[1, 2] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////
                a = PCS1.Error2 & 8;
                if (a == 8)
                {
                    if (PCS1.Error_bit[1, 3] == false)
                    {
                        err_msg = "Lightning Arrester Fault";
                        PCS1.Error_bit[1, 3] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 3] == true)
                    {
                        err_msg = "Lightning Arrester Fault";
                        PCS1.Error_bit[1, 3] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////
                a = PCS1.Error2 & 16;
                if (a == 16)
                {
                    if (PCS1.Error_bit[1, 4] == false)
                    {
                        err_msg = "Emergency Stop Fault";
                        PCS1.Error_bit[1, 4] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 4] == true)
                    {
                        err_msg = "Emergency Stop Fault";
                        PCS1.Error_bit[1, 4] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////
                a = PCS1.Error2 & 32;
                if (a == 32)
                {
                    if (PCS1.Error_bit[1, 5] == false)
                    {
                        err_msg = "BMS System Fault";
                        PCS1.Error_bit[1, 5] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 5] == true)
                    {
                        err_msg = "BMS System Fault";
                        PCS1.Error_bit[1, 5] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////////
                a = PCS1.Error2 & 64;
                if (a == 64)
                {
                    if (PCS1.Error_bit[1, 6] == false)
                    {
                        err_msg = "BMS Communication Fault";
                        PCS1.Error_bit[1, 6] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 6] == true)
                    {
                        err_msg = "BMS Communication Fault";
                        PCS1.Error_bit[1, 6] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                //////////////////////////////////////
                a = PCS1.Error2 & 128;
                if (a == 128)
                {
                    if (PCS1.Error_bit[1, 7] == false)
                    {
                        err_msg = "Backflow Prevention Communication Fault";
                        PCS1.Error_bit[1, 7] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 7] == true)
                    {
                        err_msg = "Backflow Prevention Communication Fault";
                        PCS1.Error_bit[1, 7] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////////
                a = PCS1.Error2 & 256;
                if (a == 2048)
                {
                    if (PCS1.Error_bit[1, 8] == false)
                    {
                        err_msg = "CANA Wiring Off";
                        PCS1.Error_bit[1, 8] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 8] == true)
                    {
                        err_msg = "CANA Wiring Off";
                        PCS1.Error_bit[1, 8] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////////////
                a = PCS1.Error2 & 512;
                if (a == 512)
                {
                    if (PCS1.Error_bit[1, 9] == false)
                    {
                        err_msg = "CANB Wiring Off";
                        PCS1.Error_bit[1, 9] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 9] == true)
                    {
                        err_msg = "CANB Wiring Off";
                        PCS1.Error_bit[1, 9] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                //////////////////////////////////////////////////
                a = PCS1.Error2 & 1024;
                if (a == 1024)
                {
                    if (PCS1.Error_bit[1, 10] == false)
                    {
                        err_msg = "Phase-Locked Abnormal";
                        PCS1.Error_bit[1, 10] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 10] == true)
                    {
                        err_msg = "Phase-Locked Abnormal";
                        PCS1.Error_bit[1, 10] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////////
                //a = PCS1.Error2 & 2048;
                //if (a == 2048)
                //{
                //    if (PCS1.Error_bit[1, 11] == false)
                //    {
                //        err_msg = "備用";
                //        PCS1.Error_bit[1, 11] = true;
                //        pcs_error(Device_ID, err_msg, error_time);
                //    }
                //}
                //else if (a == 0)
                //{
                //    if (PCS1.Error_bit[1, 11] == true)
                //    {
                //        err_msg = "備用";
                //        PCS1.Error_bit[1, 11] = false;
                //        pcs_error(Device_ID, err_msg, error_time);
                //    }
                //}
                //////////////////////////////////////////////
                a = PCS1.Error2 & 4096;
                if (a == 4096)
                {
                    if (PCS1.Error_bit[1, 12] == false)
                    {
                        err_msg = "Heat Sink Over Temperature";
                        PCS1.Error_bit[1, 12] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 12] == true)
                    {
                        err_msg = "Heat Sink Over Temperature";
                        PCS1.Error_bit[1, 12] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////////////////
                a = PCS1.Error2 & 8192;
                if (a == 8192)
                {
                    if (PCS1.Error_bit[1, 13] == false)
                    {
                        err_msg = "Converter Hardware Over Current";
                        PCS1.Error_bit[1, 13] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 13] == true)
                    {
                        err_msg = "Converter Hardware Over Current";
                        PCS1.Error_bit[1, 13] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ////////////////////////////////////////////////////////
                a = PCS1.Error2 & 16384;
                if (a == 16384)
                {
                    if (PCS1.Error_bit[1, 14] == false)
                    {
                        err_msg = "Drive Fault";
                        PCS1.Error_bit[1, 14] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 14] == true)
                    {
                        err_msg = "Drive Fault";
                        PCS1.Error_bit[1, 14] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////////////
                a = PCS1.Error2 & 32768;
                if (a == 32768)
                {
                    if (PCS1.Error_bit[1, 15] == false)
                    {
                        err_msg = "PV Over Current";
                        PCS1.Error_bit[1, 15] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[1, 15] == true)
                    {
                        err_msg = "PV Over Current";
                        PCS1.Error_bit[1, 15] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }

            }
            #endregion
            PCS1.Errorbit2ushort[1] = PCS1.Error2;
            #region PCS Error3
            if (PCS1.Error3 != PCS1.Errorbit2ushort[2])
            {
                a = PCS1.Error3 & 1;
                if (a == 1)
                {
                    if (PCS1.Error_bit[2, 0] == false)
                    {
                        err_msg = "Battery Over Voltage";
                        PCS1.Error_bit[2, 0] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 0] == true)
                    {
                        err_msg = "Battery Over Voltage";
                        PCS1.Error_bit[2, 0] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////
                a = PCS1.Error3 & 2;
                if (a == 2)
                {
                    if (PCS1.Error_bit[2, 1] == false)
                    {
                        err_msg = "Battery Light Load under Voltage ";
                        PCS1.Error_bit[2, 1] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 1] == true)
                    {
                        err_msg = "Battery Light Load under Voltage";
                        PCS1.Error_bit[2, 1] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////
                a = PCS1.Error3 & 4;
                if (a == 4)
                {
                    if (PCS1.Error_bit[2, 2] == false)
                    {
                        err_msg = "DC Over Current";
                        PCS1.Error_bit[2, 2] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 2] == true)
                    {
                        err_msg = "DC Over Current";
                        PCS1.Error_bit[2, 2] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////
                a = PCS1.Error3 & 8;
                if (a == 8)
                {
                    if (PCS1.Error_bit[2, 3] == false)
                    {
                        err_msg = "Ouput Voltage Abnornal";
                        PCS1.Error_bit[2, 3] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 3] == true)
                    {
                        err_msg = "Ouput Voltage Abnornal";
                        PCS1.Error_bit[2, 3] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////
                a = PCS1.Error3 & 16;
                if (a == 16)
                {
                    if (PCS1.Error_bit[2, 4] == false)
                    {
                        err_msg = "Output Voltage unsatisfied Off Grid Condition";
                        PCS1.Error_bit[2, 4] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 4] == true)
                    {
                        err_msg = "Output Voltage unsatisfied Off Grid Condition";
                        PCS1.Error_bit[2, 4] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////
                a = PCS1.Error3 & 32;
                if (a == 32)
                {
                    if (PCS1.Error_bit[2, 5] == false)
                    {
                        err_msg = "Over Current Protection";
                        PCS1.Error_bit[2, 5] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 5] == true)
                    {
                        err_msg = "Over Current Protection";
                        PCS1.Error_bit[2, 5] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////////
                a = PCS1.Error3 & 64;
                if (a == 64)
                {
                    if (PCS1.Error_bit[2, 6] == false)
                    {
                        err_msg = "Short Protection";
                        PCS1.Error_bit[2, 6] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 6] == true)
                    {
                        err_msg = "Short Protection";
                        PCS1.Error_bit[2, 6] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                //////////////////////////////////////
                a = PCS1.Error3 & 128;
                if (a == 128)
                {
                    if (PCS1.Error_bit[2, 7] == false)
                    {
                        err_msg = "Communication Line Abnormal Protection";
                        PCS1.Error_bit[2, 7] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 7] == true)
                    {
                        err_msg = "Communication Line Abnormal Protection";
                        PCS1.Error_bit[2, 7] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////////
                a = PCS1.Error3 & 256;
                if (a == 2048)
                {
                    if (PCS1.Error_bit[2, 8] == false)
                    {
                        err_msg = "DC Fuse Disconnenct";
                        PCS1.Error_bit[2, 8] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 8] == true)
                    {
                        err_msg = "DC Fuse Disconnenct";
                        PCS1.Error_bit[2, 8] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////////////
                a = PCS1.Error3 & 512;
                if (a == 512)
                {
                    if (PCS1.Error_bit[2, 9] == false)
                    {
                        err_msg = "Battery Heavy load Under Voltage";
                        PCS1.Error_bit[2, 9] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 9] == true)
                    {
                        err_msg = "Battery Heavy load Under Voltage";
                        PCS1.Error_bit[2, 9] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                //////////////////////////////////////////////////
                a = PCS1.Error3 & 1024;
                if (a == 1024)
                {
                    if (PCS1.Error_bit[2, 10] == false)
                    {
                        err_msg = "Battey Under Voltage Warning";
                        PCS1.Error_bit[2, 10] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 10] == true)
                    {
                        err_msg = "Battey Under Voltage Warning";
                        PCS1.Error_bit[2, 10] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////////
                a = PCS1.Error3 & 2048;
                if (a == 2048)
                {
                    if (PCS1.Error_bit[2, 11] == false)
                    {
                        err_msg = "Main Detector Power Abnormal";
                        PCS1.Error_bit[2, 11] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 11] == true)
                    {
                        err_msg = "Main Detector Power Abnormal";
                        PCS1.Error_bit[2, 11] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                //////////////////////////////////////////////
                a = PCS1.Error3 & 4096;
                if (a == 4096)
                {
                    if (PCS1.Error_bit[2, 12] == false)
                    {
                        err_msg = "Battery Discharge Under Voltage Protection";
                        PCS1.Error_bit[2, 12] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 12] == true)
                    {
                        err_msg = "Battery Discharge Under Voltage Protection";
                        PCS1.Error_bit[2, 12] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                /////////////////////////////////////////////////////
                a = PCS1.Error3 & 8192;
                if (a == 8192)
                {
                    if (PCS1.Error_bit[2, 13] == false)
                    {
                        err_msg = "Battery Voltage unsatisfy Charge Condition";
                        PCS1.Error_bit[2, 13] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 13] == true)
                    {
                        err_msg = "Battery Voltage unsatisfy Charge Condition";
                        PCS1.Error_bit[2, 13] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ////////////////////////////////////////////////////////
                a = PCS1.Error3 & 16384;
                if (a == 16384)
                {
                    if (PCS1.Error_bit[2, 14] == false)
                    {
                        err_msg = "Over Load Warning";
                        PCS1.Error_bit[2, 14] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 14] == true)
                    {
                        err_msg = "Over Load Warning";
                        PCS1.Error_bit[2, 14] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }
                ///////////////////////////////////////////////
                a = PCS1.Error3 & 32768;
                if (a == 32768)
                {
                    err_msg = "PV Over Current";
                    if (PCS1.Error_bit[2, 15] == false)
                    {
                        err_msg = "External Detector Fault";
                        PCS1.Error_bit[2, 15] = true;
                        pcs_error(Device_ID, err_msg, error_time);
                    }
                }
                else if (a == 0)
                {
                    if (PCS1.Error_bit[2, 15] == true)
                    {
                        err_msg = "External Detector Fault";
                        PCS1.Error_bit[2, 15] = false;
                        pcs_recovery(Device_ID, err_msg, error_time);
                    }
                }

            }
            #endregion                       
            PCS1.Errorbit2ushort[2] = PCS1.Error3;
        }
        //pcs_error pcs_recovery這兩個副程式是因為 PCS_Error_log裡面有但是不需要用到 所以隨便應付 
        private void pcs_error(string Device_ID, string err_msg, DateTime error_time)
        {
            lv_Print(listView1, error_time.ToString(), err_msg);
            Debug.Print(" pcs_error");
        }
        private void pcs_recovery(string Slave, string error_msg, DateTime time)
        {
            lv_Print(listView1, time.ToString(), error_msg+ "recovery");
            Debug.Print(" pcs_recovery");
        }

        private void bt_test_read_pcs_Click(object sender, EventArgs e)
        {
            
            // 應該要開平行緒 
            Thread oThreadA = new Thread(new ThreadStart(Read_PCS));          //讀取COM1            
            oThreadA.Start();
        }
        private void Read_PCS()
        {
            DateTime time_now = DateTime.Now;
            Read_PCS_Kehua("1", "1", PCS_TEST_everthing, ref time_now); // 包含把資料送到pcs的變數 
        }
        ThreadingTimer _ThreadTimer = null;
        ThreadingTimer _ThreadTimer2 = null;
        private void bt_test_timer_Click(object sender, EventArgs e)
        {
            //建立代理物件TimerCallback，該代理將被定時呼叫
            //TimerCallback timerDelegate = new TimerCallback(pprint);
            string currentName = new StackTrace(true).GetFrame(0).GetMethod().Name; //取得現在副程式的名稱 
            //this._ThreadTimer2 = new ThreadingTimer(new System.Threading.TimerCallback(bt_test_thead_Click), currentName, 0, 1000);
            pprint();
            Debug.Print(currentName);
            DateTime dd = DateTime.Now;
            lv_Print(listView1,dd.ToString(), currentName);
            
    
        }
        private void pprint()
        {
            DateTime dt = DateTime.Now;
            Debug.Print(dt.ToString());
        }

        private void bt_test_vq_Click(object sender, EventArgs e)
        {
            //設定工作點  
            Vq_Control.q_base =200;
            Vq_Control.v_base =380;
            Vq_Control.v1_set =95;
            Vq_Control.v2_set =97;
            Vq_Control.v3_set =104;
            Vq_Control.v4_set =105;
            Vq_Control.v5_set =103;
            Vq_Control.v6_set =96;
            Vq_Control.q1_set =100;
            Vq_Control.q2_set =80;
            Vq_Control.q3_set =-80;
            Vq_Control.q4_set =-100;
            Vq_Control.q5_set =-80;
            Vq_Control.q6_set =80;
            double grid_v = 380;   //361-399
            control_mode.Vq_control(grid_v,true);
            lv_Print(listView1, DateTime.Now.ToString(), Vq_Control.q_tr.ToString());    //輸出
            for (int i = 0; i < 10; i++)
            {
                grid_v += 1.8;
                control_mode.Vq_control(grid_v, true);
                lv_Print(listView1, DateTime.Now.ToString(), grid_v + "v  " + Vq_Control.q_tr.ToString() + "var");    //輸出
            }
            for (int i = 0; i < 20; i++)
            {
                grid_v -= 1.8;
                control_mode.Vq_control(grid_v, true);
                lv_Print(listView1, DateTime.Now.ToString(), grid_v + "v  " + Vq_Control.q_tr.ToString() + "var");    //輸出
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox_p.Enabled = true;
            textBox_q.Enabled = true;
            tmfp.Enabled = false;
            tm_vq.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            tmfp.Enabled = true;
            tm_vq.Enabled = false;
            #region  f-p設定點
            FR_Hys_Control.f1_set = 59;
            FR_Hys_Control.f2_set = 59.3;
            FR_Hys_Control.f3_set = 60.9;
            FR_Hys_Control.f4_set = 61;
            FR_Hys_Control.f5_set = 60.7;
            FR_Hys_Control.f6_set = 59.1;
            FR_Hys_Control.p1_set = 100; //百分比功率好像是直接那樣寫
            FR_Hys_Control.p2_set = 90;
            FR_Hys_Control.p3_set = -90;
            FR_Hys_Control.p4_set = -100;
            FR_Hys_Control.p5_set = -90;
            FR_Hys_Control.p6_set = 90;
            FR_Hys_Control.p_base = 250;
            double grid_f = 60;
            #endregion
            
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            tmfp.Enabled = false;
            tm_vq.Enabled = true;
            #region v-q設定點
            Vq_Control.q_base = 200;
            Vq_Control.v_base = 380;
            Vq_Control.v1_set = 95;
            Vq_Control.v2_set = 97;
            Vq_Control.v3_set = 104;
            Vq_Control.v4_set = 105;
            Vq_Control.v5_set = 103;
            Vq_Control.v6_set = 96;
            Vq_Control.q1_set = 100;
            Vq_Control.q2_set = 80;
            Vq_Control.q3_set = -80;
            Vq_Control.q4_set = -100;
            Vq_Control.q5_set = -80;
            Vq_Control.q6_set = 80;
            double grid_v = 380;   //361-399
            #endregion

        }


        private void textBox_p_TextChanged(object sender, EventArgs e)
        {
            try
            {
                master_test_everthing.WriteSingleRegister(1, 6006, Convert.ToUInt16(textBox_p.Text)); //p   //寫入暫存器
            }
            catch
            {
                lv_Print(listView1, "Cant write p ");
            }
        
        }

        private void textBox_q_TextChanged(object sender, EventArgs e)
        {
            
            try
            {
                master_test_everthing.WriteSingleRegister(1, 6003, Convert.ToUInt16(textBox_q.Text)); //q   //寫入暫存器
            }
            catch
            {
                lv_Print(listView1, "Cant write q");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        
        private void tmfp_Tick(object sender, EventArgs e)
        {
            
            grid_f = PCS_TEST_everthing.F_grid;
            control_mode.fp_Hys_control(grid_f);
            master_test_everthing.WriteSingleRegister(1, 6003, (ushort)Grid_Control.p_diff); //q
        }
        private void tm_vq_Tick(object sender, EventArgs e)
        {
            grid_v = (PCS_TEST_everthing.V_grid1 + PCS_TEST_everthing.V_grid2 + PCS_TEST_everthing.V_grid3) / 3;
            control_mode.Vq_control(grid_v, true);
            master_test_everthing.WriteSingleRegister(1, 6003, (ushort)Vq_Control.q_tr); //q
        }
        private void stroe_code()
        {
            //PCS實功和虛功皆設定為0
            byte id = 1;
            master_test_everthing.WriteSingleRegister(id, 6006, 0); //p
            Thread.Sleep(500);
            master_test_everthing.WriteSingleRegister(id, 6003, 0); //q
            Thread.Sleep(2500);
            //有特定的等待時間限制嗎 


            //4179 master_PCS.WriteSingleRegister(id, 6001, (ushort)PV_Command.SelectedIndex);
            master_test_everthing.WriteSingleRegister(id, 6001, 1); //開機
            master_test_everthing.WriteSingleRegister(id, 6005, 1); //????
            master_test_everthing.WriteSingleRegister(id, 6007, 1); //孤島模式 
            master_test_everthing.WriteSingleRegister(id, 6009, 1); //遠端模式 
            

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
