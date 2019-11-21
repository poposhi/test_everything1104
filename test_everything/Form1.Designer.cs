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
            this.bt_test_fp = new System.Windows.Forms.Button();
            this.bt_test_read_pcs = new System.Windows.Forms.Button();
            this.bt_test_timer = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.bt_test_vq = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.textBox_p = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_q = new System.Windows.Forms.TextBox();
            this.bt_pq = new System.Windows.Forms.Button();
            this.tmfp = new System.Windows.Forms.Timer(this.components);
            this.tm_vq = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 373);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "測試彈跳視窗 ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt_start
            // 
            this.bt_start.Location = new System.Drawing.Point(12, 12);
            this.bt_start.Name = "bt_start";
            this.bt_start.Size = new System.Drawing.Size(127, 44);
            this.bt_start.TabIndex = 1;
            this.bt_start.Text = "start";
            this.bt_start.UseVisualStyleBackColor = true;
            this.bt_start.Click += new System.EventHandler(this.bt_start_Click);
            // 
            // bt_read
            // 
            this.bt_read.Location = new System.Drawing.Point(12, 72);
            this.bt_read.Name = "bt_read";
            this.bt_read.Size = new System.Drawing.Size(127, 44);
            this.bt_read.TabIndex = 2;
            this.bt_read.Text = "read1_delegate";
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
            this.bt_test_thead.Location = new System.Drawing.Point(12, 137);
            this.bt_test_thead.Name = "bt_test_thead";
            this.bt_test_thead.Size = new System.Drawing.Size(127, 44);
            this.bt_test_thead.TabIndex = 3;
            this.bt_test_thead.Text = "test_thead";
            this.bt_test_thead.UseVisualStyleBackColor = true;
            this.bt_test_thead.Click += new System.EventHandler(this.bt_test_thead_Click);
            // 
            // bt_test_fp
            // 
            this.bt_test_fp.Location = new System.Drawing.Point(12, 195);
            this.bt_test_fp.Name = "bt_test_fp";
            this.bt_test_fp.Size = new System.Drawing.Size(63, 44);
            this.bt_test_fp.TabIndex = 129;
            this.bt_test_fp.Text = "test_fp";
            this.bt_test_fp.UseVisualStyleBackColor = true;
            this.bt_test_fp.Click += new System.EventHandler(this.bt_test_fp_Click);
            // 
            // bt_test_read_pcs
            // 
            this.bt_test_read_pcs.Location = new System.Drawing.Point(12, 245);
            this.bt_test_read_pcs.Name = "bt_test_read_pcs";
            this.bt_test_read_pcs.Size = new System.Drawing.Size(93, 44);
            this.bt_test_read_pcs.TabIndex = 130;
            this.bt_test_read_pcs.Text = "test_read_pcs";
            this.bt_test_read_pcs.UseVisualStyleBackColor = true;
            this.bt_test_read_pcs.Click += new System.EventHandler(this.bt_test_read_pcs_Click);
            // 
            // bt_test_timer
            // 
            this.bt_test_timer.Location = new System.Drawing.Point(12, 295);
            this.bt_test_timer.Name = "bt_test_timer";
            this.bt_test_timer.Size = new System.Drawing.Size(74, 44);
            this.bt_test_timer.TabIndex = 131;
            this.bt_test_timer.Text = "test_timer";
            this.bt_test_timer.UseVisualStyleBackColor = true;
            this.bt_test_timer.Click += new System.EventHandler(this.bt_test_timer_Click);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(145, 24);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(460, 343);
            this.listView1.TabIndex = 132;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // bt_test_vq
            // 
            this.bt_test_vq.Location = new System.Drawing.Point(76, 195);
            this.bt_test_vq.Name = "bt_test_vq";
            this.bt_test_vq.Size = new System.Drawing.Size(63, 44);
            this.bt_test_vq.TabIndex = 133;
            this.bt_test_vq.Text = "test_vq";
            this.bt_test_vq.UseVisualStyleBackColor = true;
            this.bt_test_vq.Click += new System.EventHandler(this.bt_test_vq_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("新細明體", 12F);
            this.radioButton1.Location = new System.Drawing.Point(624, 18);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(90, 24);
            this.radioButton1.TabIndex = 134;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "實虛功";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("新細明體", 12F);
            this.radioButton2.Location = new System.Drawing.Point(624, 44);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(110, 24);
            this.radioButton2.TabIndex = 135;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "頻率控制";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(624, 74);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(88, 19);
            this.radioButton3.TabIndex = 136;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "電壓控制";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // textBox_p
            // 
            this.textBox_p.Location = new System.Drawing.Point(678, 114);
            this.textBox_p.Name = "textBox_p";
            this.textBox_p.Size = new System.Drawing.Size(110, 25);
            this.textBox_p.TabIndex = 137;
            this.textBox_p.TextChanged += new System.EventHandler(this.textBox_p_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 14F);
            this.label1.Location = new System.Drawing.Point(611, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 24);
            this.label1.TabIndex = 138;
            this.label1.Text = "實功";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 14F);
            this.label2.Location = new System.Drawing.Point(611, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 139;
            this.label2.Text = "虛功";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // textBox_q
            // 
            this.textBox_q.Location = new System.Drawing.Point(678, 149);
            this.textBox_q.Name = "textBox_q";
            this.textBox_q.Size = new System.Drawing.Size(110, 25);
            this.textBox_q.TabIndex = 140;
            this.textBox_q.TextChanged += new System.EventHandler(this.textBox_q_TextChanged);
            // 
            // bt_pq
            // 
            this.bt_pq.Location = new System.Drawing.Point(611, 169);
            this.bt_pq.Name = "bt_pq";
            this.bt_pq.Size = new System.Drawing.Size(62, 44);
            this.bt_pq.TabIndex = 141;
            this.bt_pq.Text = "下指令";
            this.bt_pq.UseVisualStyleBackColor = true;
            this.bt_pq.Click += new System.EventHandler(this.button2_Click);
            // 
            // tmfp
            // 
            this.tmfp.Interval = 1000;
            this.tmfp.Tick += new System.EventHandler(this.tmfp_Tick);
            // 
            // tm_vq
            // 
            this.tm_vq.Interval = 1000;
            this.tm_vq.Tick += new System.EventHandler(this.tm_vq_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(161, 373);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 44);
            this.button2.TabIndex = 142;
            this.button2.Text = "測龍井Fp曲線";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(245, 373);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 44);
            this.button3.TabIndex = 143;
            this.button3.Text = "測龍井Vq曲線";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(329, 373);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(78, 44);
            this.button4.TabIndex = 144;
            this.button4.Text = "clear";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.bt_pq);
            this.Controls.Add(this.textBox_q);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_p);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.bt_test_vq);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.bt_test_timer);
            this.Controls.Add(this.bt_test_read_pcs);
            this.Controls.Add(this.bt_test_fp);
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
        private System.Windows.Forms.Button bt_test_fp;
        private System.Windows.Forms.Button bt_test_read_pcs;
        private System.Windows.Forms.Button bt_test_timer;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button bt_test_vq;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.TextBox textBox_p;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_q;
        private System.Windows.Forms.Button bt_pq;
        private System.Windows.Forms.Timer tmfp;
        private System.Windows.Forms.Timer tm_vq;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

