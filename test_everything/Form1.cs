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
using System.IO; // 讀取寫入文字檔 
using System.Data.OleDb; //讀EXCEL
using Excel = Microsoft.Office.Interop.Excel;

// MongoDB1 
using MongoDB.Bson;
using MongoDB.Driver;
// watchdog
using System.IO.Pipes; // for inter process communication
using System.Runtime.InteropServices;
using System.Security.Principal; // for TokenImpersonationLevel EnumerationPrincipal

namespace test_everything
{
    public partial class Form1 : Form
    {

        #region MongoDB2 宣告變數 
        private MongoClient dbconn;
        private IMongoDatabase db;
        private MongoClient ems_dbconn;
        private IMongoDatabase ems_db;
        //private string mlabconn = "mongodb://localhost:27017/?wtimeoutMS=200";  //mlab提供的連線字串 
        //private string mlabconn = "mongodb://tsai_user:0000@localhost:27017";
        #endregion
        #region watchdog1       
        NamedPipeClientStream pipeClient;                                                 /////watchdog宣告變數
        StreamString ss;
        string showstr = "";
        ThreadingTimer _ThreadTimer0 = null;
        int count = 0;
        public delegate void PrintHandler(TextBox tb, string text);

        #endregion
        #region 排程輸出測試 1
        DateTime time1 = DateTime.Now;
        //做 start end物件 
        public static List<Start_End> sche_obj = new List<Start_End>();

        #endregion


        #region 宣告變數  串列連結  Master List 
        SerialPort serialPort = new SerialPort();
        ModbusSerialMaster master_test_everthing;
        private Class1 aaaaaaa = new Class1("amy");
        private Class1 bbbbbbb = new Class1("babe");
        List<Class1> school = new List<Class1>();
        private PCS PCS_Kehua = new PCS();

        //test
        player player_name = new player();



        //電網變數
        static double grid_f = 0;
        static double grid_v = 0;
        //不是我的
        List<TextBox> listAI = new List<TextBox>();
        List<TextBox> listAO = new List<TextBox>();
        List<PictureBox> listDI = new List<PictureBox>();
        List<PictureBox> listDO = new List<PictureBox>();
        #endregion
        public Form1()
        {
            #region Watchdog2
            pipeClient =
               new NamedPipeClientStream(".", "namepipe",
                   PipeDirection.InOut, PipeOptions.None,
                   TokenImpersonationLevel.Impersonation);
            #endregion
            #region watchdog3
            Thread.Sleep(500);
            Start();

            

            #endregion
            #region Initial
            InitializeComponent();
            bt_test_read_pcs.Enabled = false;
            InitialListView();
            textBox_q.Enabled = false;
            textBox_p.Enabled = false;
            #endregion
            #region MongoDB3 連線建立 連線設定

            ////Local端，MongoDB連線Timeout設定
            MongoClientSettings settings = new MongoClientSettings();
            settings.WaitQueueSize = int.MaxValue;
            settings.ConnectTimeout = new TimeSpan(0, 0, 0, 0, 1000);
            settings.ServerSelectionTimeout = new TimeSpan(0, 0, 0, 0, 1000);
            settings.SocketTimeout = new TimeSpan(0, 0, 0, 0, 1000);
            settings.WaitQueueTimeout = new TimeSpan(0, 0, 0, 0, 0100);
            settings.Server = new MongoServerAddress("localhost");
            this.dbconn = new MongoClient(settings);
            //this.db = dbconn.GetDatabase("Tsai_Test");  //資料庫名稱 
            this.db = dbconn.GetDatabase("test1");  //資料庫名稱 
                                                    //this.dbconn = new MongoClient(mlabconn);   //設立連線  
                                                    //this.db = dbconn.GetDatabase("solar");  //資料庫名稱   


            ////Server端，MongoDB連線Timeout設定
            //MongoIdentity identity = new MongoInternalIdentity("admin", "root");
            //MongoIdentityEvidence evidence = new PasswordEvidence("pc152");
            //MongoClientSettings esettings = new MongoClientSettings();
            //esettings.WaitQueueSize = int.MaxValue;
            //esettings.ConnectTimeout = new TimeSpan(0, 0, 0, 0, 200);
            //esettings.ServerSelectionTimeout = new TimeSpan(0, 0, 0, 0, 200);
            //esettings.SocketTimeout = new TimeSpan(0, 0, 0, 0, 200);
            //esettings.WaitQueueTimeout = new TimeSpan(0, 0, 0, 0, 200);
            ////esettings.Server = new MongoServerAddress("140.118.172.75");
            //esettings.Server = new MongoServerAddress("192.168.2.10");
            //esettings.Credential = new MongoCredential(null, identity, evidence);
            //this.ems_dbconn = new MongoClient(esettings);
            //this.ems_db = ems_dbconn.GetDatabase("chang");  //資料庫名稱 

            #endregion

            #region 排程輸出測試 2
            //timer_sche.Enabled = true;
            //讀取文字檔案 
            //讀取每一行 放到string array  
            string[] lines = System.IO.File.ReadAllLines(@"D:\test_everything\test_everything\bin\Debug\shcedule.txt");
            int[] output = new int[24];
            int count = 0;
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Debug.Print("\t" + line);
                output[count] = Int32.Parse(line);
                count++;
            }

            //把讀到的每一個功率排程 存放到list，每一個相差10秒 

            count = 0;
            DateTime starttime = time1;
            DateTime endtime = starttime.AddSeconds(10);
            foreach (var out1 in output)
            {

                sche_obj.Add(new Start_End(starttime, endtime, out1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                starttime = starttime.AddSeconds(10);
                endtime = endtime.AddSeconds(10);
                count++;
            }
            //確認是否每個功率都有存入 list
            lv_Print(listView1, "確認是否每個功率都有存入 list");
            foreach (var item1 in sche_obj)
            {
                lv_Print(listView1, item1.ToString(), item1.ToMode());
            }
            //比較時間 知道到地幾個


            //輸出
            lv_Print(listView1, time1.ToString());
            #endregion
        }
        #region watchdog4
        private void Start()
        {
            string currentName = new StackTrace(true).GetFrame(0).GetMethod().Name;
            this._ThreadTimer0 = new ThreadingTimer(new System.Threading.TimerCallback(WatchDog), currentName, 0, 1000);
        }
        private void WatchDog(object state)
        {
            count = count + 1;
            if (count >= 10)
            {
                count = 0;
            }
            try
            {
                using (var pipe = new NamedPipeClientStream(".", "p", PipeDirection.InOut))
                {
                    using (var stream = new StreamWriter(pipe))
                    {
                        pipe.Connect(200);
                        stream.Write(count.ToString());
                        Print(textBox_wdt, DateTime.Now.ToString());

                    }

                }
            }
            catch { }
        }
        public class StreamString
        {
            private Stream ioStream;
            private UnicodeEncoding streamEncoding;

            public StreamString(Stream ioStream)
            {
                this.ioStream = ioStream;
                streamEncoding = new UnicodeEncoding();
            }

            public string ReadString()
            {
                try
                {
                    int len;
                    len = ioStream.ReadByte() * 256;
                    len += ioStream.ReadByte();
                    byte[] inBuffer = new byte[len];
                    ioStream.Read(inBuffer, 0, len);

                    return streamEncoding.GetString(inBuffer);
                }
                catch { return "error"; }
            }

            public int WriteString(string outString)
            {
                byte[] outBuffer = streamEncoding.GetBytes(outString);
                int len = outBuffer.Length;
                if (len > UInt16.MaxValue)
                {
                    len = (int)UInt16.MaxValue;
                }
                try
                {
                    ioStream.WriteByte((byte)(len / 256));
                    ioStream.WriteByte((byte)(len & 255));
                    ioStream.Write(outBuffer, 0, len);
                    ioStream.Flush();
                }
                catch { }
                return outBuffer.Length + 2;
            }
        }
        #endregion
        //////////Textbox用於執行續或委派時需用之方法
        public static void Print(TextBox tb, string text)
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (tb.InvokeRequired)
            {
                PrintHandler ph = new PrintHandler(Print);
                tb.Invoke(ph, tb, text);
            }
            else
            {
                tb.Text = text;
            }
        }
        #region MongoDB4 儲存
        #region Event_Log
        public class Event_Log : IComparable<Event_Log>
        {
            public DateTime time;
            public string id;
            public string type;
            public string _event;
            public string checktime;
            public string returntime;
            public string level;

            public DateTime Time
            {
                get { return time; }
                set { time = value; }
            }
            public string ID
            {
                get { return id; }
                set { id = value; }
            }
            public string Type
            {
                get { return type; }
                set { type = value; }
            }
            public string _Event
            {
                get { return _event; }
                set { _event = value; }
            }
            public string Checktime
            {
                get { return checktime; }
                set { checktime = value; }
            }
            public string Returntime
            {
                get { return returntime; }
                set { returntime = value; }
            }
            public string Level
            {
                get { return level; }
                set { level = value; }
            }
            public string ToString1()
            {
                return time.ToString();
            }
            public Event_Log(DateTime time, string id, string _event, string returntime)
            {
                this.time = time;
                this.id = id;
                //this.type = type;
                this._event = _event;
                //this.checktime = checktime;
                this.returntime = returntime;
                //this.level = level;
            }
            int IComparable<Event_Log>.CompareTo(Event_Log other)
            {
                throw new NotImplementedException();
            }
        }           /////事件紀錄
        public class SchComparerby : IComparer<Event_Log>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(Event_Log x, Event_Log y)
            {
                if (x.Time < y.Time)
                    return -1;
                if (x.Time > y.Time)
                    return 1;
                //該段為Speed相等時才會由Power比較
                //依power由小排到大
                return 0;
            }
        }
        public List<Event_Log> event_log = new List<Event_Log>();
        class RESET_Error
        {
            public static double error_count = 0;
            public static double count = 0;
            public static int EMS_Error = 0;
            public static int EMS_Error1 = 0;
            public static bool EMS_Flag = false;
        }
        #endregion

        private void Mongo_Reset(string Slave, string error_msg, DateTime time)
        {
            try
            {//嘗試在列表增加一個事件 
                event_log.Add(new Event_Log(time, Slave, error_msg, "null"));
                if (event_log.Count > 50) //讓程式LIST裡面暫存的 事件保持最多50個
                {
                    event_log.RemoveAt(0);//移除指定的資料 
                }
            }
            catch
            {
                Console.WriteLine("Reset Error");
            }
            try
            { //
                DateTime last_event = DateTime.Now;
                string event_str;
                var sort = Builders<BsonDocument>.Sort.Descending("time"); //根據時間來排列資料，最新的放在前面 降序排列
                var coll = db.GetCollection<BsonDocument>("alarm");  //指定寫入給"categories"此collection  
                var filter = Builders<BsonDocument>.Filter.Eq("ID", Slave) & Builders<BsonDocument>.Filter.Eq("event", error_msg);

                var cursor = coll.Find(filter).Sort(sort).Limit(1).ToList(); // 轉換成list 等等可以疊代
                foreach (var event_log in cursor)
                { //解析接收到的資料 ，接收到的資料是一個的字典 
                    last_event = (DateTime)event_log.GetValue("time");
                    event_str = event_log.GetValue("event").ToString();
                }
                var filter2 = Builders<BsonDocument>.Filter.Eq("ID", Slave) & Builders<BsonDocument>.Filter.Eq("event", error_msg) & Builders<BsonDocument>.Filter.Eq("time", last_event);
                var update = Builders<BsonDocument>.Update.Set("returntime", DateTime.Now)
                                                          .Set("EMS_RESET_Error", RESET_Error.EMS_Error);
                coll.UpdateOne(filter2, update); //更新現有資料 
                //coll.Find(filter).Sort(sort)
            }
            catch
            {
                Console.WriteLine("Reset Error");
            }

        }

        private void mongo_test(DateTime time_now)
        {
            int time_offset = 8;
            var coll = db.GetCollection<BsonDocument>("coll_1");  //指定寫入給"categories"此collection  
            coll.InsertOne(new BsonDocument { { "time", time_now.AddHours(time_offset) } });
        }
        #endregion
        private void InitialListView()//初始化ListView的格式大小 
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.LabelEdit = false;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("time", 150);
            listView1.Columns.Add("message", 200);
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
        public static void lv_Print(ListView list, string message)// 輸入listview ,兩個str
        {
            String time = DateTime.Now.ToString();
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (list.InvokeRequired)
            {
                Listview_Print ph = new Listview_Print(lv_Print);
                list.Invoke(ph, list, time, message);  //我覺得這裡不需要 time
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
        #region 練習delegate
        //正妹跟你借30萬
        //死黨跟你借100
        //魯蛇跟你借10塊錢

        delegate int CustomAction(int amount);
        CustomAction peopleAction;

        private int loadtogirl(int loadmoney)
        {
            return loadmoney * 10;
        }
        private int loadmen(int loadmoney)
        {
            return loadmoney;
        }

        private string load1(CustomAction people, int amount)
        {
            return people(amount).ToString();
        }

        private void load(string person, int amount)
        {
            if (person == "girl")
            {
                Debug.Print((amount * 10).ToString());
            }
            if (person == "men")
            {
                Debug.Print((amount).ToString());
            }




        }
        private void bt_read_Click(object sender, EventArgs e)
        {
            lv_Print(listView1, System.Configuration.ConfigurationManager.AppSettings["test_key"]);
            peopleAction = loadtogirl;
            Debug.Print(load1(peopleAction, 10));
            peopleAction = loadmen;
            Debug.Print(load1(peopleAction, 10));
            var player1 = new player();
            viewer viewer1 = new viewer();
            player1.playerdie += viewer1.See_People_Die;
            player1.Hurt();
            player1.Hurt();
            player1.Hurt();
            player1.Hurt();

        }
        #endregion
        private void bt_test_fp_Click(object sender, EventArgs e)
        {
            //設定工作點  
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
            FR_Hys_Control.p_base = 100;
            double grid_f = 60;
            control_mode.fp_Hys_control(grid_f);
            //lv_Print(listView1, DateTime.Now.ToString(), "輸出功率 "+ Grid_Control.p_diff.ToString());    //輸出功率 

            for (int i = 0; i < 50; i++)
            {
                grid_f += 0.01;
                control_mode.fp_Hys_control(grid_f);
                lv_Print(listView1, DateTime.Now.ToString(), grid_f + "hz  " + Grid_Control.p_diff.ToString("#0.00") + "w");    //輸出功率 
            }
            for (int i = 0; i < 100; i++)
            {
                grid_f -= 0.01;
                control_mode.fp_Hys_control(grid_f);
                lv_Print(listView1, DateTime.Now.ToString(), grid_f + "hz  " + Grid_Control.p_diff.ToString("#0.00") + "w");    //輸出功率 
            }

            for (int i = 0; i < 50; i++)
            {
                grid_f += 0.01;
                control_mode.fp_Hys_control(grid_f);
                lv_Print(listView1, DateTime.Now.ToString(), grid_f + "hz  " + Grid_Control.p_diff.ToString("#0.00") + "w");    //輸出功率 
            }

            for (int i = 0; i < 10; i++)
            {
                grid_f += 0.1;
                control_mode.fp_Hys_control(grid_f);
                lv_Print(listView1, DateTime.Now.ToString(), grid_f + "hz  " + Grid_Control.p_diff.ToString("#0.00") + "w");    //輸出功率 
            }
            for (int i = 0; i < 20; i++)
            {
                grid_f -= 0.1;
                control_mode.fp_Hys_control(grid_f);
                lv_Print(listView1, DateTime.Now.ToString(), grid_f + "hz  " + Grid_Control.p_diff.ToString("#0.00") + "w");    //輸出功率 
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
                serialPort.PortName = "COM6";
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
                //master_test_everthing.WriteSingleRegister(1, 6008, 555); //結果會寫入46009 
                master_test_everthing.ReadInputRegisters(1, 1, 1);//04
                master_test_everthing.ReadHoldingRegisters(1, 1, 1);//03
            }
            catch (Exception ex)
            {
                Debug.Print("modbus Exception" + ex.Message);
                lv_Print(listView1, DateTime.Now.ToString(), "modbus Exception" + ex.Message);
            }

            bt_test_read_pcs.Enabled = true;
            #endregion
            //timer1.Enabled = true;
            //timer2.Enabled = true;


            Debug.Print("dsadas");
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
                    using (var frm = new ff2(PCS_Kehua))
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

                    string ExceptionCode = "0";

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

        int c = 0;
        private void bt_test_thead_Click(object sender, EventArgs e)
        {
            ///
            /// ThreadingTimer  使用方法  先創造物件 指定函式 一定要輸入一個名稱 ?? 指定延遲  指定執行週期
            string name = "abc_name";
            this._ThreadTimer = new ThreadingTimer(new TimerCallback(loop_print), name, 0, 1000);  /////每50ms判斷一次是否換秒
            ////
            int x = 188;
            Debug.Print("into bt_test_thead_Click{0}", x); //印出變數
            Debug.Print($"into bt_test_thead_Click{x} type{x.GetType()}");
            Thread wait = new Thread(new ThreadStart(a_print));
            Thread wait2 = new Thread(new ThreadStart(a_print));
            Thread.Sleep(1000);
            Debug.Print("等1秒後 ");
            wait.Start();
            wait.Join(); ////等待所有平行緒執行完畢後才會繼續往下執行 ， 要其他平行緒等待著一個平行緒執行完才會繼續執行 

            Debug.Print("Wait之後 ");
            Thread.Sleep(1000);
            Debug.Print(" Wait之後  等1秒後 ");

        }
        private void a_print()
        {
            Debug.Print("  ppppppppp");
            Thread.Sleep(3000);
            Debug.Print(" 等3秒後  ppppppppp");
        }
        private void loop_print(object state)
        {
            Debug.Print("loop");
            c++;
            if (c > 5)
            {
                _ThreadTimer.Dispose();
            }
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
                    PCS_Error_log(Device.Device_ID, time_now, PCS_Kehua);
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

        private void PCS_Error_log(string Device_ID, DateTime error_time, PCS PCS1)
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
            lv_Print(listView1, time.ToString(), error_msg + "recovery");
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
            Read_PCS_Kehua("1", "1", PCS_Kehua, ref time_now); // 包含把資料送到pcs的變數 
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
            lv_Print(listView1, dd.ToString(), currentName);


        }
        private void pprint()
        {
            DateTime dt = DateTime.Now;
            Debug.Print(dt.ToString());
        }

        private void bt_test_vq_Click(object sender, EventArgs e)
        {
            //設定工作點  
            Vq_Control.q_base = 100;
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
            control_mode.Vq_control(grid_v, true);  ////輸入電壓會修改q輸出 
            //lv_Print(listView1, DateTime.Now.ToString(), Vq_Control.q_tr.ToString());    //輸出
            for (int i = 0; i < 10; i++)
            {
                grid_v += 1.8;
                control_mode.Vq_control(grid_v, true);
                lv_Print(listView1, DateTime.Now.ToString(), grid_v + "v  " + Vq_Control.q_tr.ToString("#0.00") + "var");    //輸出
            }
            for (int i = 0; i < 20; i++)
            {
                grid_v -= 1.8;
                control_mode.Vq_control(grid_v, true);
                lv_Print(listView1, DateTime.Now.ToString(), grid_v + "v  " + Vq_Control.q_tr.ToString("#0.00") + "var");    //輸出
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
            //按下按鈕後寫入 pq
            byte id = 1;
            try
            {
                ushort pp, qq;
                pp = Convert.ToUInt16(textBox_p.Text);
                qq = Convert.ToUInt16(textBox_q.Text);
                master_test_everthing.WriteSingleRegister(id, 6005, pp); //p 寫入 46006 
                Thread.Sleep(500);
                master_test_everthing.WriteSingleRegister(id, 6002, qq); //q
            }
            catch { lv_Print(listView1, "寫入 pq 錯誤"); }

        }

        private void tmfp_Tick(object sender, EventArgs e)
        {

            try
            {
                Thread oThreadA = new Thread(new ThreadStart(Read_PCS));
                oThreadA.Start();

                control_mode.fp_Hys_control(PCS_Kehua.F_grid);
                master_test_everthing.WriteSingleRegister(1, 6005, (ushort)Grid_Control.p_diff); //p

            }
            catch
            {
                lv_Print(listView1, "tmfp_Tick error");
            }
        }
        private void tm_vq_Tick(object sender, EventArgs e)
        {
            try
            {
                Thread oThreadA = new Thread(new ThreadStart(Read_PCS));
                oThreadA.Start();

                grid_v = (PCS_Kehua.V_grid1 + PCS_Kehua.V_grid2 + PCS_Kehua.V_grid3) / 3;
                control_mode.Vq_control(grid_v, true);
                master_test_everthing.WriteSingleRegister(1, 6002, (ushort)Vq_Control.q_tr); //q

            }
            catch
            {
                lv_Print(listView1, "tm_vq_Tick error");
            }


        }
        private void stroe_code()
        {
            //PCS實功和虛功皆設定為0  stop 
            byte id = 1;
            master_test_everthing.WriteSingleRegister(id, 6006, 0); //p
            Thread.Sleep(500);
            master_test_everthing.WriteSingleRegister(id, 6003, 0); //q
            Thread.Sleep(2500);
            //有特定的等待時間限制嗎 


            //4179 master_PCS.WriteSingleRegister(id, 6001, (ushort)PV_Command.SelectedIndex);
            master_test_everthing.WriteSingleRegister(id, 6001, 1); //開機
            master_test_everthing.WriteSingleRegister(id, 6005, 1); //????
            master_test_everthing.WriteSingleRegister(id, 6006, 1); //孤島模式  實際的地址 是6007
            master_test_everthing.WriteSingleRegister(id, 6009, 1); //遠端模式 


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private int to_2complement(int value)
        {//把功率轉換成2的補數 
            if (value > 32768)
            {

                return value - 65536;
            }
            else
            {
                return value;
            }
            //Now value is -100
        }
        private int negative2complement(int value)
        {//把功率轉換成2的補數 
            if (value < 0)
            {
                //return value + 256;
                return value + 65536;
            }
            else
            {
                return value;
            }
            //Now value is -100
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            #region 設定參數 
            double Hys_line = 0;
            double grid_f_last = 60;
            double grid_p_last = 100;
            double grid_p_base = 100;
            double grid_f = 60;
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
            FR_Hys_Control.p_base = 100;
            #endregion
            control2.Pf_control(ref Hys_line, ref Grid_Control.p_out, ref grid_f_last, ref grid_p_last, grid_p_base, grid_f, FR_Hys_Control.f1_set, FR_Hys_Control.f2_set, FR_Hys_Control.f3_set, FR_Hys_Control.f4_set, FR_Hys_Control.f5_set, FR_Hys_Control.f6_set, FR_Hys_Control.p1_set, FR_Hys_Control.p2_set, FR_Hys_Control.p3_set, FR_Hys_Control.p4_set, FR_Hys_Control.p5_set, FR_Hys_Control.p6_set);
            for (int i = 0; i < 50; i++)
            {
                grid_f += 0.01;
                control2.Pf_control(ref Hys_line, ref Grid_Control.p_out, ref grid_f_last, ref grid_p_last, grid_p_base, grid_f, FR_Hys_Control.f1_set, FR_Hys_Control.f2_set, FR_Hys_Control.f3_set, FR_Hys_Control.f4_set, FR_Hys_Control.f5_set, FR_Hys_Control.f6_set, FR_Hys_Control.p1_set, FR_Hys_Control.p2_set, FR_Hys_Control.p3_set, FR_Hys_Control.p4_set, FR_Hys_Control.p5_set, FR_Hys_Control.p6_set);
                lv_Print(listView1, DateTime.Now.ToString(), grid_f + "hz  " + Grid_Control.p_out.ToString("#0.00") + "w");    //輸出功率 
            }
            for (int i = 0; i < 100; i++)
            {
                grid_f -= 0.01;
                control2.Pf_control(ref Hys_line, ref Grid_Control.p_out, ref grid_f_last, ref grid_p_last, grid_p_base, grid_f, FR_Hys_Control.f1_set, FR_Hys_Control.f2_set, FR_Hys_Control.f3_set, FR_Hys_Control.f4_set, FR_Hys_Control.f5_set, FR_Hys_Control.f6_set, FR_Hys_Control.p1_set, FR_Hys_Control.p2_set, FR_Hys_Control.p3_set, FR_Hys_Control.p4_set, FR_Hys_Control.p5_set, FR_Hys_Control.p6_set);
                lv_Print(listView1, DateTime.Now.ToString(), grid_f + "hz  " + Grid_Control.p_out.ToString("#0.00") + "w");    //輸出功率 
            }

            for (int i = 0; i < 50; i++)
            {
                grid_f += 0.01;
                control2.Pf_control(ref Hys_line, ref Grid_Control.p_out, ref grid_f_last, ref grid_p_last, grid_p_base, grid_f, FR_Hys_Control.f1_set, FR_Hys_Control.f2_set, FR_Hys_Control.f3_set, FR_Hys_Control.f4_set, FR_Hys_Control.f5_set, FR_Hys_Control.f6_set, FR_Hys_Control.p1_set, FR_Hys_Control.p2_set, FR_Hys_Control.p3_set, FR_Hys_Control.p4_set, FR_Hys_Control.p5_set, FR_Hys_Control.p6_set);
                lv_Print(listView1, DateTime.Now.ToString(), grid_f + "hz  " + Grid_Control.p_out.ToString("#0.00") + "w");    //輸出功率 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            #region 設定參數 vq 
            double Hys_line = 0;
            double grid_v_last = 380;
            double grid_q_last = 100;
            double grid_q_base = 100;
            double grid_v = 380;
            double base_v = 380;
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
            #endregion
            control2.Vq_control(ref Hys_line, ref Grid_Control.q_out, ref grid_v_last, ref grid_q_last, grid_q_base, base_v, grid_v, Vq_Control.v1_set, Vq_Control.v2_set, Vq_Control.v3_set, Vq_Control.v4_set, Vq_Control.v5_set, Vq_Control.v6_set, Vq_Control.q1_set, Vq_Control.q2_set, Vq_Control.q3_set, Vq_Control.q4_set, Vq_Control.q5_set, Vq_Control.q6_set);

            for (int i = 0; i < 50; i++)
            {
                grid_v += 0.2;
                control2.Vq_control(ref Hys_line, ref Grid_Control.q_out, ref grid_v_last, ref grid_q_last, grid_q_base, base_v, grid_v, Vq_Control.v1_set, Vq_Control.v2_set, Vq_Control.v3_set, Vq_Control.v4_set, Vq_Control.v5_set, Vq_Control.v6_set, Vq_Control.q1_set, Vq_Control.q2_set, Vq_Control.q3_set, Vq_Control.q4_set, Vq_Control.q5_set, Vq_Control.q6_set);
                lv_Print(listView1, DateTime.Now.ToString(), grid_v + "v  " + Grid_Control.q_out.ToString("#0.00") + "var");    //輸出功率 
            }
            for (int i = 0; i < 100; i++)
            {
                grid_v -= 0.2;
                //control2.Vq_control(ref Hys_line, ref Grid_Control.q_out, ref grid_v_last, ref grid_q_last, grid_q_base, base_v, grid_v, Vq_Control.v1_set, Vq_Control.v2_set, Vq_Control.v3_set, Vq_Control.v4_set, Vq_Control.v5_set, Vq_Control.v6_set, Vq_Control.q1_set, Vq_Control.q2_set, Vq_Control.q3_set, Vq_Control.q4_set, Vq_Control.q5_set, Vq_Control.q6_set);
                lv_Print(listView1, DateTime.Now.ToString(), grid_v + "v  " + Grid_Control.q_out.ToString("#0.00") + "var");    //輸出功率 
            }

            for (int i = 0; i < 50; i++)
            {
                grid_v += 0.2;
                control2.Vq_control(ref Hys_line, ref Grid_Control.q_out, ref grid_v_last, ref grid_q_last, grid_q_base, base_v, grid_v, Vq_Control.v1_set, Vq_Control.v2_set, Vq_Control.v3_set, Vq_Control.v4_set, Vq_Control.v5_set, Vq_Control.v6_set, Vq_Control.q1_set, Vq_Control.q2_set, Vq_Control.q3_set, Vq_Control.q4_set, Vq_Control.q5_set, Vq_Control.q6_set);
                lv_Print(listView1, DateTime.Now.ToString(), grid_v + "v  " + Grid_Control.q_out.ToString("#0.00") + "var");    //輸出功率 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            InitialListView();
        }

        private void bt_TEST_FUnc_Click(object sender, EventArgs e)
        {
            Debug.Print("{0}", negative2complement(-127));
        }
        #region c# call py
        public string run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\johnny\AppData\Local\Programs\Python\Python36\python.exe";
            start.Arguments = string.Format("\"{0}\" \"{1}\"", cmd, args);
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                    return result;
                }
            }
        }

        //呼叫python核心程式碼
        public static void RunPythonScript(string sArgName, string args = "", params string[] teps)
        {
            Process p = new Process();//創造一個處理程序並且給他PY PATH cmd 
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + sArgName;// 獲得python檔案的絕對路徑（將檔案放在c#的debug資料夾中可以這樣操作）
            p.StartInfo.FileName = @"C:\Users\johnny\AppData\Local\Programs\Python\Python36\python.exe";//沒有配環境變數的話，可以像我這樣寫python.exe的絕對路徑。如果配了，直接寫"python.exe"即可
            string sArguments = path;
            foreach (string sigstr in teps) //對於每個輸入的參數 
            {
                sArguments += " " + sigstr;//傳遞引數
            }

            sArguments += " " + args; //當每一個參數連結起來組合成一個指令 

            p.StartInfo.Arguments = sArguments;

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.RedirectStandardInput = true;

            p.StartInfo.RedirectStandardError = true;

            p.StartInfo.CreateNoWindow = true;

            p.Start();
            p.BeginOutputReadLine(); //讀取輸出 STR
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            Console.ReadLine();
            p.WaitForExit();
        }
        //輸出列印的資訊
        static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                AppendText(e.Data + Environment.NewLine);
            }
            else
            {
                Debug.Print("IsNullOrEmpty");

            }
        }
        public delegate void AppendTextCallback(string text);
        public static void AppendText(string text)
        {
            Console.WriteLine(text);     //此處在控制檯輸出.py檔案print的結果
            Debug.Print(text);

        }
        //c# call py 主要功能副程式 
        static void Option1_ExecProcess()
        {
            // 1) Create Process Info
            var psi = new ProcessStartInfo(); //創造一個處理程序 
            psi.FileName = @"C:\Users\johnny\AppData\Local\Programs\Python\Python36\python.exe";

            // 2) 提供路境以及變數 
            var script = @"E:\DaysBetweenDates.py";//(因為我沒放debug下，所以直接寫的絕對路徑,替換掉上面的路徑了)

            var start = "2019-1-1";
            var end = "2019-1-22";

            psi.Arguments = $"\"{script}\" \"{start}\" \"{end}\"";  //輸入的變數總共包含了 程式碼的路徑 程式碼需要的變數兩個 

            // 3) Process configuration
            psi.UseShellExecute = false;  //不需要使用shell
            psi.CreateNoWindow = true; //不需要顯示互動視窗 
            psi.RedirectStandardOutput = true; //重新導向輸出 這樣子才可以拿到輸出 
            psi.RedirectStandardError = true;  //錯誤同理 需要重新導向 

            // 4) Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi)) //啟動處理程序但是不知道在幹嘛 
                                                     //using 陳述式 執行完後會釋放資源 
            {
                errors = process.StandardError.ReadToEnd(); //
                results = process.StandardOutput.ReadToEnd();  //接收輸出 
            }

            // 5) Display output



            Debug.Print("ERRORS:");
            Debug.Print(errors);
            Debug.Print("");
            Debug.Print("Results:");
            Debug.Print(results);
            //Debug.Print(results);

        }
        #endregion

        private void TEST_py_Click(object sender, EventArgs e)
        {
            /*
            string[] strArr =new string[2];//引數列表 輸入進去的參數 
            string sArguments = @"main.py";//腳本的名字 
            strArr[0] = "2";
            strArr[1] = "3";
            string sss = "";
            RunPythonScript(sArguments, "-u", sss);//輸入的腳本的名字  還有輸入的參數 
            */
            Option1_ExecProcess();


        }

        private void bt_write_Click(object sender, EventArgs e)
        {
            //將矩陣int 寫入文字檔 
            // 將字串寫入TXT檔  list to txt 
            StreamWriter str = new StreamWriter(@"D:\test_everything\test_everything\bin\Debug\ttteeesssttt.txt");

            // array 
            int[] load_Array = { 1, 2, 3, 4, 5 };
            //write
            foreach (var i in load_Array)
            {
                str.WriteLine(i.ToString());
            }
            str.WriteLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss fff  "));
            str.Close();
            //string WriteWord = "cccc rewrite";
            //str.WriteLine(WriteWord);
            //str.WriteLine("bbb");


            // 以下List 裡為int 型態
            List<int> load = new List<int>();
            load.Add(32);
        }

        private void bt_read_txt_Click(object sender, EventArgs e)
        {
            //讀取每一行 放到string array 
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\johnny\source\repos\test_everything\python_write.txt");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Debug.Print("\t" + line);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable aa = new DataTable();
            try
            {
                aa = GetExcelData(@"D:\test_everything\test_everything\PV_OUTPUT.xlsx");
            }
            catch (Exception ex)
            {

                Debug.Print(ex.Message);
            }


            Debug.Print(aa.Rows[0][0].ToString());
            Debug.Print(aa.Rows[1][0].ToString());
            Debug.Print(aa.Rows[2][0].ToString());
            Debug.Print(aa.Rows.Count.ToString());
            float[] pv_out = new float[aa.Rows.Count];
            for (int i = 0; i < aa.Rows.Count; i++)
            {
                pv_out[i] = Convert.ToSingle(aa.Rows[i][0]);
            }
            Debug.Print(pv_out[0].ToString());
            Debug.Print(pv_out[1].ToString());

            // 將字串寫入TXT檔  arrat to txt 
            StreamWriter str = new StreamWriter(@"D:\test_everything\test_everything\pv_out.txt");
            //write
            for (int i = 0; i < aa.Rows.Count; i++)
            {
                str.WriteLine(pv_out[i]);
            }
            str.Close();
        }

        #region  排程的物件 可以比較 
        /////定義class 

        /* 增加排程的方法 
         * Start_List.Add(new Start_End(starttime, endtime, mode, fixed_p, fixed_q, steady_value, smooth_limit, back_p, soc_max, soc_min, ramp_up, ramp_down, p_base, q_base, fp_base, combine_mode, c_parameters));
                    _Hour.SelectedIndex = endtime.Hour;
                    _Minute.SelectedIndex = endtime.Minute;
            //自動排序
                Start_List.Sort((x, y) => { return x.Start_time.CompareTo(y.Start_time); });
            /////比較新排程是否有重疊 如果沒有重疊就加入排程 
                IEnumerable<Start_End> filteringQuery =
                from c in Start_List
                where (c.End_Time > starttime && starttime >= c.Start_time) || (c.Start_time < endtime && c.End_Time >= endtime)
                select c;
                ////若無重疊排程，加入新排程，排程時間自動延續
                int repeat_time = filteringQuery.Count();
                if (repeat_time == 0)
                {
                    Start_List.Add(new Start_End(starttime, endtime, mode, fixed_p, fixed_q, steady_value, smooth_limit, back_p, soc_max, soc_min, ramp_up, ramp_down, p_base, q_base, fp_base, combine_mode, c_parameters));
                    _Hour.SelectedIndex = endtime.Hour;
                    _Minute.SelectedIndex = endtime.Minute;
                }
                else
                {
                    MessageBox.Show("Error");
                }
         */
        public class Start_End : IComparable<Start_End>
        {

            private DateTime start_time;
            private DateTime end_time;
            private int mode;
            private double smooth_limit;
            private double steady_value;
            private double fixed_p;
            private double back_p;
            private double soc_max;
            private double soc_min;
            private double ramp_up;
            private double ramp_down;
            private double p_base;
            private double fp_base;
            private double q_base;
            private double combine_mode;
            private double c_parameters;
            private double fixed_q;

            #region 變數 get set 
            public DateTime Start_time
            {
                get { return start_time; }
                set { start_time = value; }
            }
            public DateTime End_Time
            {
                get { return end_time; }
                set { end_time = value; }
            }
            public int Mode
            {
                get { return mode; }
                set { mode = value; }
            }
            public double Fixed_P
            {
                get { return fixed_p; }
                set { fixed_p = value; }
            }
            public double Fixed_Q
            {
                get { return fixed_q; }
                set { fixed_q = value; }
            }
            public double P_Base
            {
                get { return p_base; }
                set { p_base = value; }
            }
            public double FP_Base
            {
                get { return fp_base; }
                set { fp_base = value; }
            }
            public double Q_Base
            {
                get { return q_base; }
                set { q_base = value; }
            }
            public double Steady_Value
            {
                get { return steady_value; }
                set { steady_value = value; }
            }
            public double Smooth_Limit
            {
                get { return smooth_limit; }
                set { smooth_limit = value; }
            }
            public double Back_P
            {
                get { return back_p; }
                set { back_p = value; }
            }
            public double SOC_max
            {
                get { return soc_max; }
                set { soc_min = value; }
            }
            public double SOC_min
            {
                get { return soc_min; }
                set { soc_min = value; }
            }
            public double Ramp_Up
            {
                get { return ramp_up; }
                set { ramp_up = value; }
            }
            public double Ramp_Down
            {
                get { return ramp_down; }
                set { ramp_down = value; }
            }
            public double Combine_mode
            {
                get { return combine_mode; }
                set { combine_mode = value; }
            }

            public double C_parameters
            {
                get { return c_parameters; }
                set { c_parameters = value; }
            }
            #endregion
            //建購子
            public Start_End(DateTime start_time, DateTime end_time, int mode, double fixed_p, double fixed_q, double steady_value, double smooth_limit, double back_p, double soc_max, double soc_min, double ramp_up, double ramp_down, double p_base, double q_base, double fp_base, double combine_mode, double c_parameters)
            {
                this.start_time = start_time;
                this.end_time = end_time;
                this.mode = mode;
                this.fixed_p = fixed_p;
                this.fixed_q = fixed_q;
                this.steady_value = steady_value;
                this.smooth_limit = smooth_limit;
                this.back_p = back_p;
                this.ramp_up = ramp_up;
                this.ramp_down = ramp_down;
                this.soc_max = soc_max;
                this.soc_min = soc_min;
                this.p_base = p_base;
                this.q_base = q_base;
                this.c_parameters = c_parameters;
                this.combine_mode = combine_mode;
                this.fp_base = fp_base;
            }
            //這個物件的功能  可以查看開始結束時間跟功能 
            public override string ToString()
            {
                return start_time.ToString("HH:mm") + "~" + end_time.ToString("HH:mm");
            }
            public string ToMode()
            {
                return Mode.ToString();
            }

            int IComparable<Start_End>.CompareTo(Start_End other)
            {
                throw new NotImplementedException();
            }
        }

        public class SchComparerby1 : IComparer<Start_End>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(Start_End x, Start_End y)
            {
                if (x.Start_time < y.Start_time)
                    return -1;
                if (x.Start_time > y.Start_time)
                    return 1;
                //該段為Speed相等時才會由Power比較
                //依power由小排到大
                return 0;//兩個開始的時間一樣，平等 
            }


        }
        #endregion
        #region 讀取Excel 
        // example
        //DataTable aa = new DataTable();
        //    try
        //    {aa = GetExcelData(@"D:\test_everything\test_everything\PV_OUTPUT.xlsx");}
        //    catch (Exception ex)
        //    {
        //        Debug.Print(ex.Message);
        //    }
        private Stopwatch wath = new Stopwatch();
        /// <summary>
        /// 使用COM讀取Excel
        /// </summary>
        /// <param name="excelFilePath">路徑</param>
        /// <returns>DataTabel</returns>
        public DataTable GetExcelData(string excelFilePath)
        {
            Excel.Application app = new Excel.Application();
            Excel.Sheets sheets;
            Excel.Workbook workbook = null;
            object oMissiong = System.Reflection.Missing.Value;
            System.Data.DataTable dt = new System.Data.DataTable();
            wath.Start();
            try
            {
                if (app == null)
                {
                    return null;
                }
                workbook = app.Workbooks.Open(excelFilePath, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong,
                    oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
                //將資料讀入到DataTable中——Start   
                sheets = workbook.Worksheets;
                Excel.Worksheet worksheet = (Excel.Worksheet)sheets.get_Item(1);//讀取第一張表
                if (worksheet == null)
                    return null;
                string cellContent;
                int iRowCount = worksheet.UsedRange.Rows.Count;
                int iColCount = worksheet.UsedRange.Columns.Count;
                Excel.Range range;
                //負責列頭Start
                DataColumn dc;
                int ColumnID = 1;
                range = (Excel.Range)worksheet.Cells[1, 1];
                while (range.Text.ToString().Trim() != "")
                {
                    dc = new DataColumn();
                    dc.DataType = System.Type.GetType("System.String");
                    dc.ColumnName = range.Text.ToString().Trim();
                    dt.Columns.Add(dc);

                    range = (Excel.Range)worksheet.Cells[1, ++ColumnID];
                }
                //End
                for (int iRow = 2; iRow <= iRowCount; iRow++)
                {
                    DataRow dr = dt.NewRow();
                    for (int iCol = 1; iCol <= iColCount; iCol++)
                    {
                        range = (Excel.Range)worksheet.Cells[iRow, iCol];
                        cellContent = (range.Value2 == null) ? "" : range.Text.ToString();
                        dr[iCol - 1] = cellContent;
                    }
                    dt.Rows.Add(dr);
                }
                wath.Stop();
                TimeSpan ts = wath.Elapsed;
                //將資料讀入到DataTable中——End
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                workbook.Close(false, oMissiong, oMissiong);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                workbook = null;
                app.Workbooks.Close();
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        #endregion
        #region excel  寫入excel
        /// <summary>
        /// If the supplied excel File does not exist then Create it
        /// </summary>
        /// <param name="FileName"></param>
        //輸入地址會創造一個Excel檔案 
        private void CreateExcelFile(string FileName)
        {
            //create
            object Nothing = System.Reflection.Missing.Value;
            var app = new Excel.Application();
            app.Visible = false;
            Excel.Workbook workBook = app.Workbooks.Add(Nothing);
            Excel.Worksheet worksheet = (Excel.Worksheet)workBook.Sheets[1];
            worksheet.Name = "Work";
            //headline
            worksheet.Cells[1, 1] = "FileName"; //第一行 
            worksheet.Cells[1, 2] = "FindString";
            worksheet.Cells[1, 3] = "ReplaceString";

            worksheet.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing);
            workBook.Close(false, Type.Missing, Type.Missing);
            app.Quit();
        }

        /// <summary>
        /// open an excel file,then write the content to file
        /// </summary>
        /// <param name="FileName">file name</param>
        /// <param name="findString">first cloumn</param>
        /// <param name="replaceString">second cloumn</param>
        //寫入一個已經存在的excel檔案，輸入要插入的數值
        private void WriteToExcel(string excelName, string filename, string findString, string replaceString)
        {
            //open
            object Nothing = System.Reflection.Missing.Value;
            var app = new Excel.Application();
            app.Visible = false;
            Excel.Workbook mybook = app.Workbooks.Open(excelName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing);
            Excel.Worksheet mysheet = (Excel.Worksheet)mybook.Worksheets[1];
            mysheet.Activate();
            //get activate sheet max row count
            int maxrow = mysheet.UsedRange.Rows.Count + 1;
            mysheet.Cells[maxrow, 1] = filename;// 在cloumn 1 寫入值
            mysheet.Cells[maxrow, 2] = findString;
            //mysheet.Cells[maxrow, 3] = replaceString;
            mysheet.Cells[maxrow, 4] = replaceString;
            mybook.Save();
            mybook.Close(false, Type.Missing, Type.Missing);
            mybook = null;
            //quit excel app
            app.Quit();
        }
        #endregion
        // 沒用到的副程式 
        public DataTable GetExcelTableByOleDB(string strExcelPath, string tableName)
        {
            try
            {
                DataTable dtExcel = new DataTable();
                //資料表
                DataSet ds = new DataSet();
                //獲取副檔名
                string strExtension = System.IO.Path.GetExtension(strExcelPath);
                string strFileName = System.IO.Path.GetFileName(strExcelPath);
                //Excel的連線
                OleDbConnection objConn = null;
                switch (strExtension)
                {
                    case ".xls":
                        objConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strExcelPath + ";" + "Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;\"");
                        break;
                    case ".xlsx":
                        objConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strExcelPath + ";" + "Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;\"");
                        break;
                    default:
                        objConn = null;
                        break;
                }
                if (objConn == null)
                {
                    return null;
                }
                objConn.Open();
                //獲取Excel中所有Sheet表的資訊
                //System.Data.DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                //獲取Excel的第一個Sheet表名
                //string tableName = schemaTable.Rows[0][2].ToString().Trim();
                string strSql = "select * from [" + tableName + "]";
                //獲取Excel指定Sheet表中的資訊
                OleDbCommand objCmd = new OleDbCommand(strSql, objConn);
                OleDbDataAdapter myData = new OleDbDataAdapter(strSql, objConn);
                myData.Fill(ds, tableName);//填充資料
                objConn.Close();
                //dtExcel即為excel檔案中指定表中儲存的資訊
                dtExcel = ds.Tables[tableName];
                return dtExcel;
            }
            catch (Exception ex)
            {

                Debug.Print(ex.Message);
                return null;
            }
        }

        private void bt_test_db_Click(object sender, EventArgs e)
        {
            DateTime time_command_now = DateTime.Now;
            mongo_test(time_command_now);
        }

        private void timer_sche_Tick(object sender, EventArgs e)
        {
            #region 排程輸出測試 3
            foreach (var item1 in sche_obj)
            {
                //if 時間對 就輸出 
                if (item1.End_Time > DateTime.Now && DateTime.Now >= item1.Start_time)
                {
                    lv_Print(listView1, DateTime.Now.ToString(), item1.ToMode());
                }


            }
            #endregion
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            WriteToExcel(@"D:\test_everything\test_everything\ssss.xlsx","1","2","444");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //build_Control_Setting();
            //更新資料庫 
            var coll = db.GetCollection<BsonDocument>("Control_Setting");  
            var filter = Builders<BsonDocument>.Filter.Eq("Control_Setting", "Control_Setting");// & Builders<BsonDocument>.Filter.Eq("event", error_msg) & Builders<BsonDocument>.Filter.Eq("time", last_event);//////搜尋故障事件和發生時間以填入復歸時間
            var update = Builders<BsonDocument>.Update.Set("time", DateTime.Now);
            coll.UpdateOne(filter, update);
        }
        private void build_Control_Setting()
        {
            int time_offset = 8;
            DateTime time_now = DateTime.Now;
            var coll = db.GetCollection<BsonDocument>("Control_Setting");  //指定寫入給"categories"此collection  
            coll.InsertOne(new BsonDocument
            {
                {"Control_Setting","Control_Setting" },
                { "time", time_now.AddHours(time_offset) },
                { "control_mode", "remote" },
                { "schedule_enable","turn_on"},
                { "steady_setpoint", 100 },
                {"pcs_output",100 },
                { "soc_max", 95},{ "soc_min", 5},
                { "PV_rate",1000}
            });
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //using System.IO.Ports;
            SerialPort _serialPort = new SerialPort();
            try
            {
                _serialPort.PortName = "COM6";
                _serialPort.BaudRate = 9600;
                _serialPort.Parity = Parity.None;
                _serialPort.StopBits = StopBits.One;
                _serialPort.DataBits = 8;
                _serialPort.Open();
            }
            catch (Exception)
            {

                MessageBox.Show("打开串口失败!");
            };

            try
            {
                byte[] testbt = { 0x01, 0x64, 0xcf, 0x16, 0x00, 0x04, 0x08, 0x00, 0x64 , 0x00, 0x00};
                //byte[] testbt = { 0x01, 0x03, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00 };
                modbus_crc(ref testbt);
                _serialPort.Write(testbt, 0, testbt.Length);
                // ref : https://www.blueshop.com.tw/board/FUM20050124192253INM/BRD20181217162351ARN.html
            }
            catch (Exception ex)
            {
                Debug.Print("modbus Exception" + ex.Message);
                MessageBox.Show("modbus Exception" + ex.Message);
            }
            _serialPort.Close();
        }
        private void modbus_crc(ref byte[] data)
        {
            try
            {
                ushort CRCFull = 0xFFFF;
                byte CRCHigh = 0xFF, CRCLow= 0xFF;
                char CRCLSB;

                for (int i = 0; i < (data.Length-2); i++)
                {
                    CRCFull = (ushort)(CRCFull ^ data[i]);
                    for (int j = 0; j < 8; j++)

                    {
                        CRCLSB = (char)(CRCFull & 0x0001);

                        CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);
                        if (CRCLSB == 1)
                        { CRCFull = (ushort)(CRCFull ^ 0xA001); }
                    }
                }

                CRCLow = (byte)((CRCFull >> 8) & 0xFF);
                CRCHigh = (byte)(CRCFull & 0xFF);
                data[data.Length-2] = CRCHigh;
                data[data.Length-1] = CRCLow;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
