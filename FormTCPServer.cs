using EasyTcp4;
using EasyTcp4.ClientUtils;
using EasyTcp4.Protocols;
using EasyTcp4.Protocols.Tcp;
using EasyTcp4.ServerUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7.Dotnetcore
{
    public partial class FormTCPServer : Form
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(
                "m0"
                );

        public FormTCPServer()
        {
            InitializeComponent();
        }
        EasyTcpServer server;
        private void button1_Click(object sender, EventArgs e)
        {
            ushort port = (ushort)Convert.ToUInt64(textBox1.Text);
            server = new EasyTcpServer(new PlainTcpProtocol());
            server.OnDataReceive += Server_OnDataReceive;
            server.OnDisconnect += Server_OnDisconnect;
            server.OnError += Server_OnError;
            server.OnConnect += Server_OnConnect;
            server.OnDataReceiveAsync += Server_OnDataReceiveAsync;
            server.Start("127.0.0.1", port);
            //server.OnDataReceive += (sender, message) => log.Info(message.Data);
            addLog("Server start on listening port: " + port);
        }

        private Task Server_OnDataReceiveAsync(object sender, EasyTcp4.Message message)
        {
            //string result = System.Text.Encoding.UTF8.GetString(message.Data);

            //addLog("Server_OnDataReceiveAsync: " + result);
            return Task.CompletedTask;
        }

        private void Server_OnConnect(object? sender, EasyTcpClient e)
        {
            EasyTcpServer server = (EasyTcpServer)sender;
            //e.Send();
            addLog("Server_OnConnect");
        }

        private void Server_OnError(object? sender, Exception e)
        {
            addLog("Server_OnError");
        }

        private void Server_OnDisconnect(object? sender, EasyTcpClient e)
        {
            addLog("Server_OnDisconnect");
        }

        private void Server_OnDataReceive(object? sender, EasyTcp4.Message e)
        {
            //(sender, message) => addLog(message.Data);
            string result = System.Text.Encoding.UTF8.GetString(e.Data);
            // Remove First and end chracter
            result = result.Substring(1, result.Length-3);
            addLog(result);
            //string ackMessage = "MSH|^~\\&|SAPP|SFCT|RAPP|RFCT|20080312181835||ADT^A01|0D23ACC3-17CD-4FF4-BE66-AD4A6572079E|P|2.4";
            string ackMessage = "MSH|^~\\&||ACK|";
            Message message = new HL7.Dotnetcore.Message(result);

            // Parse this message

            bool isParsed = false;

            try
            {
                isParsed = message.ParseMessage(true);
            }
            catch (Exception ex)
            {
                // Handle the exception
                addLog(ex.Message);
            }
            ackMessage = message.GetACK().SerializeMessage(false);
            log.Info("Send ACK: " + ackMessage);
            ackMessage = ((char)11).ToString() + ackMessage + ((char)28).ToString() + ((char)13).ToString();
            
            byte[] ackMessageBytes = Encoding.UTF8.GetBytes(ackMessage);
            EasyTcpServer client = (EasyTcpServer) sender;
            client.SendAll(ackMessageBytes);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            server.SendAll(textBox2.Text.ToString());
            addLog("Send " + textBox2.Text);
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

        private void FormTCPServer_Load(object sender, EventArgs e)
        {

        }

        private void FormTCPServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server != null)
            {
                server.Dispose();
            }
        }
    }
}
