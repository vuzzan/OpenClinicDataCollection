using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7.Dotnetcore
{
    public partial class FormRS232 : Form
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(
                "m0"
                );
        public FormRS232()
        {
            InitializeComponent();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
            comboBox1.SelectedIndex = 0;
        }
        SerialPort _serialPort;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _serialPort = new SerialPort(comboBox1.Text, 9600, Parity.None, 8, StopBits.One);
                _serialPort.WriteTimeout = 1000;
                _serialPort.ReadTimeout = 1000;
                _serialPort.DataReceived += _serialPort_DataReceived;
                _serialPort.ErrorReceived += _serialPort_ErrorReceived;
                _serialPort.Disposed += _serialPort_Disposed;
                _serialPort.Open();
                addLog("OPEN " + comboBox1.Text + " SIMULATOR");
            }
            catch (Exception ex)
            {
                addLog("Send Error: " + ex.Message);
            }
        }

        private void _serialPort_Disposed(object? sender, EventArgs e)
        {
            addLog("Port " + comboBox1.Text + " Disposed");
            _serialPort = null;

        }

        private void _serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            addLog("ErrorReceived " + comboBox1.Text + " ");
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort _serialPort = (SerialPort)sender;
                byte[] data = new byte[_serialPort.BytesToRead];
                _serialPort.Read(data, 0, data.Length);
                //data.ToList().ForEach(b => {
                //    addLog("Recv " );
                //});
                string hex = BitConverter.ToString(data);
                addLog("Recv " + hex.Replace("-", " 0x"));
            }
            catch (Exception ex)
            {
                addLog("ERROR RS232 read: " + ex.Message);
            }
        }
        //test
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (_serialPort == null)
                {
                    addLog("Not connected");
                }
                else
                {
                    _serialPort.Write(textBox2.Text);
                    addLog("Send " + textBox2.Text);
                }
            }
            catch (Exception ex)
            {
                addLog("Send Error: " + ex.Message);
            }

        }

        private void addLog(string msg)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<string>(addLog), new object[] { msg });
                }
                else
                {
                    log.Info(msg);
                    listBox1.Items.Add(msg);
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

        }

        private void FormRS232_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort = null;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Send ENQ
            sendByte(0x05);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Send ACK
            sendByte(0x04);
        }
        //special characters used for msg framing
        //#define ENQ                0x05            //ASCII 0x05 == ENQUIRY
        //#define ACK                0x06
        //#define STX                0x02            //START OF TEXT
        //#define ETX                0x03            //END OF TEXT
        //#define EOT                0x04            //END OF TRANSMISSION
        //#define LF                 0x0A            //LINEFEED
        //#define NUL                0x00            //NULL termination character
        private void sendByte(int b)
        {
            if (_serialPort == null)
            {
                addLog("Not connected");
            }
            else
            {
                byte[] data = new byte[] { (byte)b };
                _serialPort.Write(data, 0, 1);
                addLog("Send byte: " + BitConverter.ToString(data));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[] { 0x0d, 0x0a, 0x04 };
            _serialPort.Write(data, 0, 3);
        }
    }
}
