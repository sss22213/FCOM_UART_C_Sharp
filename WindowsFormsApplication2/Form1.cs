using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace WindowsFormsApplication1
    { 
    
    public partial class Form1 : Form
    {


        SerialPort port=new SerialPort();
        int rx, tx=0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form.CheckForIllegalCrossThreadCalls =false;

            foreach (String port1 in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(port1);

            }

        }

        private void textBox5_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("ss");
                button3.PerformClick();


            }
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((comboBox1.Text).Length < 1 || (textBox1.Text).Length < 1 || (textBox2.Text).Length < 1 || (textBox3.Text).Length < 1)
                {
                    MessageBox.Show("請確認資料是否完整");
                    return;


                }
                else
                {
                    port.PortName = comboBox1.Text;
                    port.BaudRate = Convert.ToInt32(textBox1.Text);
                    port.DataBits = Convert.ToInt32(textBox3.Text);
                    port.Parity = Parity.None;
                    if (Convert.ToInt32(textBox2.Text) == 1)
                    {
                        port.StopBits = StopBits.One;
                    }
                    else if (Convert.ToInt32(textBox2.Text) == 2)
                    {
                        port.StopBits = StopBits.Two;
                    }
                    else
                    {
                        MessageBox.Show("只可以為1或是2");
                        return;


                    }
                    port.Open();
                    button1.Visible = false;
                    button5.Visible = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
               

            }
             catch(Exception ex)
                {
                    MessageBox.Show("發生錯誤，請聯絡你的系統管理員");
                    return;

                
                }







        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
           
                string sk = "";
               
                byte[] s1 = new byte[port.BytesToRead];
                port.Read(s1, 0, s1.Length);
                for (int i = 0; i < s1.Length;i++ )
                {
                    sk += s1[i];

                }

                if (sk.Length > 0)
                {
                  textBox4.Text = textBox4.Text + "\r\n" + DateTime.Now +": "+String.Format("{0:X}",Convert.ToInt16(sk)) + "\r\n";
                 textBox4.SelectionStart = textBox4.Text.Length;
                 textBox4.ScrollToCaret();
                 rx += 1;
                 label6.Text = "已接收" + rx + "筆資料";

                }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Interval = 10;



            button2.Visible = false;
            button4.Visible = true;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy == false)
            {
                backgroundWorker1.RunWorkerAsync();
            
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //  sixty = (TextBox3.Text).Length
            //If Not sixty = 2 Then
            //    MsgBox("數字輸入錯誤")
            //    Return
            //Else
            //    For i As Integer = 1 To sixty

            //        k1 = Val(GetChar(TextBox3.Text, i))
            //        k1 = sixtygetten(k1, i)
            //        SUM = (k1 * 16 ^ (sixty - i)) + SUM

            //    Next
            //End If
            //

            int sixty = 0;
            string g1 = textBox5.Text;
            int k1 = 0;
            int SUM =0;
            sixty = (textBox5.Text).Length;
            if (sixty != 2)
            {
                MessageBox.Show("輸入錯誤");
                return;

            }
            else
            {
                for (int i = 0; i < sixty; i++)
                {
                    string g2 = g1.Substring(i, 1);
                    if (g2 == "A" || g2 == "B" || g2 == "C" || g2 == "D" || g2 == "E" || g2 == "F")
                    {
                        k1 = sixthget(i);
                    }
                    else
                    {
                        k1 = Convert.ToInt16(g1.Substring(i, 1));
                    }


                    SUM = Convert.ToInt16((k1 * Math.Pow(16,1-i)) )+ SUM;
                   
                }

            }
            
       
            byte[] buffer = new byte[1];
            buffer[0] = Convert.ToByte(SUM);
            port.Write(buffer, 0, buffer.Length);
            tx += 1;
            label5.Text = "已傳送" + tx + "筆資料";
            textBox5.Text = "";


        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (backgroundWorker1.IsBusy == true)
            {
                backgroundWorker1.CancelAsync();

            
            }
            button3.Enabled = false;
            button4.Visible = false;
            button2.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4.PerformClick();
            button1.Visible = true;
            button2.Enabled = false;
            button5.Visible = false;
            port.Close();

        }

      public int sixthget(int i)
        {
            string g1 = textBox5.Text;
            int k1=0;
            if (g1.Substring(i, 1) == "A")
            {
                k1 = 10;
            }
            if (g1.Substring(i, 1) == "B")
            {
                k1 = 11;
            }
            if (g1.Substring(i, 1) == "C")
            {
                k1 = 12;
            }
            if (g1.Substring(i, 1) == "D")
            {
                k1 = 13;
            }
            if (g1.Substring(i, 1) == "E")
            {
                k1 = 14;
            }
            if (g1.Substring(i, 1) == "F")
            {
                k1 = 15;
            }

            return k1;

        
        }

      private void textBox5_TextChanged(object sender, EventArgs e)
      {

      }

    }
  

}
