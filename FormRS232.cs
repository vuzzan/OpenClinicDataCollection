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
                _serialPort = new SerialPort(comboBox1.Text, 19200, Parity.None, 8, StopBits.One);
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
            addLog("SIM COM RECEIVE");
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
    }
}
