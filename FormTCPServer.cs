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
        object server;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string[] tmp = textBox1.Text.Split(":");
            if (tmp.Length == 2)
            {
                if (tmp[1].Length > 0 && Convert.ToInt32(tmp[1]) > 100)
                {
                    // Active
                    button1.Text = "Connect";

                }
                else
                {
                    // Active
                    button1.Text = "Connect";

                }
            }
            else
            {
                button1.Text = "Listen";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string[] tmp = textBox1.Text.Split(":");
            if (tmp.Length == 2 && Convert.ToInt32(tmp[1]) > 100)
            {
                EasyTcpClient _EasyTcpClient = new EasyTcpClient(new PlainTcpProtocol(10240));
                _EasyTcpClient.OnDataReceive += Server_OnDataReceive;
                _EasyTcpClient.OnConnect += Server_OnConnect;
                _EasyTcpClient.OnDisconnect += Server_OnDisconnect;

                if (_EasyTcpClient.Connect(tmp[0].Trim(), (ushort)Convert.ToInt32(tmp[1])) == true)
                {
                }

                server = (EasyTcpClient)_EasyTcpClient;
                addLog("Connect to " + tmp[0].Trim() + ": " + (ushort)Convert.ToInt32(tmp[1]));
            }
            else
            {
                ushort port = (ushort)Convert.ToUInt64(textBox1.Text);
                EasyTcpServer obj = new EasyTcpServer(new PlainTcpProtocol(10240));
                obj.OnDataReceive += Server_OnDataReceive;
                obj.OnDisconnect += Server_OnDisconnect;
                obj.OnError += Server_OnError;
                obj.OnConnect += Server_OnConnect;
                obj.OnDataReceiveAsync += Server_OnDataReceiveAsync;
                obj.Start("127.0.0.1", port);
                addLog("Server start on listening port: " + port);

                server = (EasyTcpServer)obj;
            }

        }

        private Task Server_OnDataReceiveAsync(object sender, EasyTcp4.Message message)
        {
            return Task.CompletedTask;
        }

        private void Server_OnConnect(object? sender, EasyTcpClient e)
        {
            addLog("OnConnect");
        }

        private void Server_OnError(object? sender, Exception e)
        {
            addLog("OnError");
        }

        private void Server_OnDisconnect(object? sender, EasyTcpClient e)
        {
            addLog("OnDisconnect");
        }

        private void Server_OnDataReceive(object? sender, EasyTcp4.Message e)
        {
            //(sender, message) => addLog(message.Data);
            string result = System.Text.Encoding.UTF8.GetString(e.Data);
            // Remove First and end chracter
            result = result.Substring(1, result.Length - 3);
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
            //
            if (server != null && server.GetType() == typeof(EasyTcpServer))
            {
                EasyTcpServer client = (EasyTcpServer)server;
                client.SendAll(ackMessageBytes);
            }
            else if (server != null && server.GetType() == typeof(EasyTcpClient))
            {
                EasyTcpClient client = (EasyTcpClient)server;
                client.Send(ackMessageBytes);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (server != null && server.GetType() == typeof(EasyTcpServer))
            {
                EasyTcpServer client = (EasyTcpServer)server;
                client.SendAll(textBox2.Text.ToString());
                addLog("Send " + textBox2.Text);
            }
            else if (server != null && server.GetType() == typeof(EasyTcpClient))
            {
                EasyTcpClient client = (EasyTcpClient)server;
                client.Send(textBox2.Text.ToString());
                addLog("Send " + textBox2.Text);
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

        private void FormTCPServer_Load(object sender, EventArgs e)
        {

        }

        private void FormTCPServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server != null)
            {
                if (server != null && server.GetType() == typeof(EasyTcpServer))
                {
                    EasyTcpServer client = (EasyTcpServer)server;
                    client.Dispose();
                }
                else if (server != null && server.GetType() == typeof(EasyTcpClient))
                {
                    EasyTcpClient client = (EasyTcpClient)server;
                    client.Dispose();
                }
            }
        }


    }
}
