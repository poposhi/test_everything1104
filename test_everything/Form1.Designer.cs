namespace test_everything
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.bt_start = new System.Windows.Forms.Button();
            this.bt_read = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.bt_test_thead = new System.Windows.Forms.Button();
            this.lb41001b1 = new System.Windows.Forms.Label();
            this.label41001 = new System.Windows.Forms.Label();
            this.lb41001b2 = new System.Windows.Forms.Label();
            this.lb41001b3 = new System.Windows.Forms.Label();
            this.lb41001b4 = new System.Windows.Forms.Label();
            this.lb41001b5 = new System.Windows.Forms.Label();
            this.lb41001b6 = new System.Windows.Forms.Label();
            this.lb41001b7 = new System.Windows.Forms.Label();
            this.lb41001b8 = new System.Windows.Forms.Label();
            this.lb41001b9 = new System.Windows.Forms.Label();
            this.lb41001b10 = new System.Windows.Forms.Label();
            this.lb41001b11 = new System.Windows.Forms.Label();
            this.lb41001b12 = new System.Windows.Forms.Label();
            this.lb41001b13 = new System.Windows.Forms.Label();
            this.label41002 = new System.Windows.Forms.Label();
            this.lb41002b2 = new System.Windows.Forms.Label();
            this.lb41002b5 = new System.Windows.Forms.Label();
            this.lb41002b6 = new System.Windows.Forms.Label();
            this.bt_test_class = new System.Windows.Forms.Button();
            this.bt_test_read_pcs = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(646, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "測試彈跳視窗 ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt_start
            // 
            this.bt_start.Location = new System.Drawing.Point(67, 77);
            this.bt_start.Name = "bt_start";
            this.bt_start.Size = new System.Drawing.Size(127, 44);
            this.bt_start.TabIndex = 1;
            this.bt_start.Text = "start";
            this.bt_start.UseVisualStyleBackColor = true;
            this.bt_start.Click += new System.EventHandler(this.bt_start_Click);
            // 
            // bt_read
            // 
            this.bt_read.Location = new System.Drawing.Point(67, 155);
            this.bt_read.Name = "bt_read";
            this.bt_read.Size = new System.Drawing.Size(127, 44);
            this.bt_read.TabIndex = 2;
            this.bt_read.Text = "read1";
            this.bt_read.UseVisualStyleBackColor = true;
            this.bt_read.Click += new System.EventHandler(this.bt_read_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 2000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // bt_test_thead
            // 
            this.bt_test_thead.Location = new System.Drawing.Point(67, 235);
            this.bt_test_thead.Name = "bt_test_thead";
            this.bt_test_thead.Size = new System.Drawing.Size(127, 44);
            this.bt_test_thead.TabIndex = 3;
            this.bt_test_thead.Text = "test_thead";
            this.bt_test_thead.UseVisualStyleBackColor = true;
            this.bt_test_thead.Click += new System.EventHandler(this.bt_test_thead_Click);
            // 
            // lb41001b1
            // 
            this.lb41001b1.AutoSize = true;
            this.lb41001b1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b1.Location = new System.Drawing.Point(617, 122);
            this.lb41001b1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b1.Name = "lb41001b1";
            this.lb41001b1.Size = new System.Drawing.Size(110, 17);
            this.lb41001b1.TabIndex = 111;
            this.lb41001b1.Text = "b1:Local_control ";
            // 
            // label41001
            // 
            this.label41001.AutoSize = true;
            this.label41001.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label41001.Location = new System.Drawing.Point(610, 108);
            this.label41001.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41001.Name = "label41001";
            this.label41001.Size = new System.Drawing.Size(170, 17);
            this.label41001.TabIndex = 112;
            this.label41001.Text = "Project specific status 41001";
            // 
            // lb41001b2
            // 
            this.lb41001b2.AutoSize = true;
            this.lb41001b2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b2.Location = new System.Drawing.Point(617, 136);
            this.lb41001b2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b2.Name = "lb41001b2";
            this.lb41001b2.Size = new System.Drawing.Size(121, 17);
            this.lb41001b2.TabIndex = 113;
            this.lb41001b2.Text = "b2:Remote_control ";
            // 
            // lb41001b3
            // 
            this.lb41001b3.AutoSize = true;
            this.lb41001b3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b3.Location = new System.Drawing.Point(617, 152);
            this.lb41001b3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b3.Name = "lb41001b3";
            this.lb41001b3.Size = new System.Drawing.Size(124, 17);
            this.lb41001b3.TabIndex = 114;
            this.lb41001b3.Text = "b3:Off_site_control ";
            // 
            // lb41001b4
            // 
            this.lb41001b4.AutoSize = true;
            this.lb41001b4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b4.Location = new System.Drawing.Point(617, 166);
            this.lb41001b4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b4.Name = "lb41001b4";
            this.lb41001b4.Size = new System.Drawing.Size(166, 17);
            this.lb41001b4.TabIndex = 115;
            this.lb41001b4.Text = "b4:Commissioning_control ";
            // 
            // lb41001b5
            // 
            this.lb41001b5.AutoSize = true;
            this.lb41001b5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b5.Location = new System.Drawing.Point(617, 180);
            this.lb41001b5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b5.Name = "lb41001b5";
            this.lb41001b5.Size = new System.Drawing.Size(62, 17);
            this.lb41001b5.TabIndex = 116;
            this.lb41001b5.Text = "b5:Ready";
            // 
            // lb41001b6
            // 
            this.lb41001b6.AutoSize = true;
            this.lb41001b6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b6.Location = new System.Drawing.Point(617, 194);
            this.lb41001b6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b6.Name = "lb41001b6";
            this.lb41001b6.Size = new System.Drawing.Size(139, 17);
            this.lb41001b6.TabIndex = 117;
            this.lb41001b6.Text = "b6:AC_Breaker_closed";
            // 
            // lb41001b7
            // 
            this.lb41001b7.AutoSize = true;
            this.lb41001b7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b7.Location = new System.Drawing.Point(617, 208);
            this.lb41001b7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b7.Name = "lb41001b7";
            this.lb41001b7.Size = new System.Drawing.Size(65, 17);
            this.lb41001b7.TabIndex = 118;
            this.lb41001b7.Text = "b7:Online";
            // 
            // lb41001b8
            // 
            this.lb41001b8.AutoSize = true;
            this.lb41001b8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b8.Location = new System.Drawing.Point(617, 222);
            this.lb41001b8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b8.Name = "lb41001b8";
            this.lb41001b8.Size = new System.Drawing.Size(73, 17);
            this.lb41001b8.TabIndex = 119;
            this.lb41001b8.Text = "b8:Standby";
            // 
            // lb41001b9
            // 
            this.lb41001b9.AutoSize = true;
            this.lb41001b9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b9.Location = new System.Drawing.Point(617, 236);
            this.lb41001b9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b9.Name = "lb41001b9";
            this.lb41001b9.Size = new System.Drawing.Size(63, 17);
            this.lb41001b9.TabIndex = 120;
            this.lb41001b9.Text = "b9:Alarm";
            // 
            // lb41001b10
            // 
            this.lb41001b10.AutoSize = true;
            this.lb41001b10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b10.Location = new System.Drawing.Point(617, 250);
            this.lb41001b10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b10.Name = "lb41001b10";
            this.lb41001b10.Size = new System.Drawing.Size(144, 17);
            this.lb41001b10.TabIndex = 121;
            this.lb41001b10.Text = "b10:Partial_battery_trip";
            // 
            // lb41001b11
            // 
            this.lb41001b11.AutoSize = true;
            this.lb41001b11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b11.Location = new System.Drawing.Point(617, 264);
            this.lb41001b11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b11.Name = "lb41001b11";
            this.lb41001b11.Size = new System.Drawing.Size(86, 17);
            this.lb41001b11.TabIndex = 122;
            this.lb41001b11.Text = "b11:PCS_trip";
            // 
            // lb41001b12
            // 
            this.lb41001b12.AutoSize = true;
            this.lb41001b12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b12.Location = new System.Drawing.Point(617, 278);
            this.lb41001b12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b12.Name = "lb41001b12";
            this.lb41001b12.Size = new System.Drawing.Size(59, 17);
            this.lb41001b12.TabIndex = 123;
            this.lb41001b12.Text = "b12:Trip";
            // 
            // lb41001b13
            // 
            this.lb41001b13.AutoSize = true;
            this.lb41001b13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41001b13.Location = new System.Drawing.Point(617, 292);
            this.lb41001b13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41001b13.Name = "lb41001b13";
            this.lb41001b13.Size = new System.Drawing.Size(93, 17);
            this.lb41001b13.TabIndex = 124;
            this.lb41001b13.Text = "b13:Grid_fault";
            // 
            // label41002
            // 
            this.label41002.AutoSize = true;
            this.label41002.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label41002.Location = new System.Drawing.Point(617, 325);
            this.label41002.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41002.Name = "label41002";
            this.label41002.Size = new System.Drawing.Size(170, 17);
            this.label41002.TabIndex = 125;
            this.label41002.Text = "Project specific status 41002\r\n";
            // 
            // lb41002b2
            // 
            this.lb41002b2.AutoSize = true;
            this.lb41002b2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41002b2.Location = new System.Drawing.Point(616, 344);
            this.lb41002b2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41002b2.Name = "lb41002b2";
            this.lb41002b2.Size = new System.Drawing.Size(53, 17);
            this.lb41002b2.TabIndex = 126;
            this.lb41002b2.Text = "b2:CSI ";
            // 
            // lb41002b5
            // 
            this.lb41002b5.AutoSize = true;
            this.lb41002b5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41002b5.Location = new System.Drawing.Point(616, 358);
            this.lb41002b5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41002b5.Name = "lb41002b5";
            this.lb41002b5.Size = new System.Drawing.Size(201, 17);
            this.lb41002b5.TabIndex = 127;
            this.lb41002b5.Text = "b5:Operation_islanding_condition";
            // 
            // lb41002b6
            // 
            this.lb41002b6.AutoSize = true;
            this.lb41002b6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb41002b6.Location = new System.Drawing.Point(616, 373);
            this.lb41002b6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb41002b6.Name = "lb41002b6";
            this.lb41002b6.Size = new System.Drawing.Size(140, 17);
            this.lb41002b6.TabIndex = 128;
            this.lb41002b6.Text = "b6:Black_start_enabled";
            // 
            // bt_test_class
            // 
            this.bt_test_class.Location = new System.Drawing.Point(67, 298);
            this.bt_test_class.Name = "bt_test_class";
            this.bt_test_class.Size = new System.Drawing.Size(127, 44);
            this.bt_test_class.TabIndex = 129;
            this.bt_test_class.Text = "test_class";
            this.bt_test_class.UseVisualStyleBackColor = true;
            this.bt_test_class.Click += new System.EventHandler(this.bt_test_class_Click);
            // 
            // bt_test_read_pcs
            // 
            this.bt_test_read_pcs.Location = new System.Drawing.Point(222, 29);
            this.bt_test_read_pcs.Name = "bt_test_read_pcs";
            this.bt_test_read_pcs.Size = new System.Drawing.Size(127, 44);
            this.bt_test_read_pcs.TabIndex = 130;
            this.bt_test_read_pcs.Text = "test_read_pcs";
            this.bt_test_read_pcs.UseVisualStyleBackColor = true;
            this.bt_test_read_pcs.Click += new System.EventHandler(this.bt_test_read_pcs_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bt_test_read_pcs);
            this.Controls.Add(this.bt_test_class);
            this.Controls.Add(this.label41002);
            this.Controls.Add(this.lb41001b13);
            this.Controls.Add(this.lb41001b12);
            this.Controls.Add(this.lb41001b11);
            this.Controls.Add(this.lb41001b10);
            this.Controls.Add(this.lb41001b9);
            this.Controls.Add(this.lb41001b8);
            this.Controls.Add(this.lb41001b7);
            this.Controls.Add(this.lb41001b6);
            this.Controls.Add(this.lb41001b5);
            this.Controls.Add(this.lb41001b4);
            this.Controls.Add(this.lb41001b3);
            this.Controls.Add(this.lb41001b2);
            this.Controls.Add(this.label41001);
            this.Controls.Add(this.lb41001b1);
            this.Controls.Add(this.lb41002b6);
            this.Controls.Add(this.lb41002b5);
            this.Controls.Add(this.lb41002b2);
            this.Controls.Add(this.bt_test_thead);
            this.Controls.Add(this.bt_read);
            this.Controls.Add(this.bt_start);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bt_start;
        private System.Windows.Forms.Button bt_read;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button bt_test_thead;
        private System.Windows.Forms.Label lb41001b1;
        private System.Windows.Forms.Label label41001;
        private System.Windows.Forms.Label lb41001b2;
        private System.Windows.Forms.Label lb41001b3;
        private System.Windows.Forms.Label lb41001b4;
        private System.Windows.Forms.Label lb41001b5;
        private System.Windows.Forms.Label lb41001b6;
        private System.Windows.Forms.Label lb41001b7;
        private System.Windows.Forms.Label lb41001b8;
        private System.Windows.Forms.Label lb41001b9;
        private System.Windows.Forms.Label lb41001b10;
        private System.Windows.Forms.Label lb41001b11;
        private System.Windows.Forms.Label lb41001b12;
        private System.Windows.Forms.Label lb41001b13;
        private System.Windows.Forms.Label label41002;
        private System.Windows.Forms.Label lb41002b2;
        private System.Windows.Forms.Label lb41002b5;
        private System.Windows.Forms.Label lb41002b6;
        private System.Windows.Forms.Button bt_test_class;
        private System.Windows.Forms.Button bt_test_read_pcs;
    }
}

