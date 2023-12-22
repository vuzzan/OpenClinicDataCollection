using EasyTcp4;
using log4net.Config;
using log4net;
using System.IO.Ports;
using System.Reflection;
using EasyTcp4.ClientUtils;
using System.Collections;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using EasyTcp4.ServerUtils;
using AutoUpdaterDotNET;
using EasyTcp4.Protocols.Tcp;

namespace OpenClinicDataCollection
{
    public partial class FormMain : Form
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(
                "m0"
                );
        static bool appStop = false;
        static ObservableCollection<MachineData> listMachineData = new ObservableCollection<MachineData>();
        public string HOSTNAME = "";
        public string PostURL = "";
        public string AutoUpdateURL = "";
        public FormMain()
        {
            InitializeComponent();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        private async Task<bool> StartLoadConfig()
        {
            foreach (MachineData client in listMachineData)
            {
                if (client.MachineType == "TCP")
                {
                    if (client.MachineObject != null)
                    {
                        EasyTcpClient tcpClient = (EasyTcpClient)client.MachineObject;
                        tcpClient.OnDisconnect -= _EasyTcpClient_OnDisconnect;
                        tcpClient.Dispose();
                        tcpClient = null;
                    }
                }
                else if (client.MachineType == "RS232")
                {
                    try
                    {
                        if (client.MachineObject != null)
                        {
                            SerialPort _SerialPort = (SerialPort)client.MachineObject;
                            _SerialPort.Close();
                            _SerialPort = null;
                            log.Info("Closed RS232: " + client.MachineName + " | " + client.MachineCOM + " " + client.MachineBaudrate);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Info("ERROR: Close RS232: " + client.MachineName + " | " + client.MachineCOM + " " + client.MachineBaudrate + " " + ex.Message);
                    }

                }
            }
            addLog("Start App-Load - Clear");
            listMachineData.Clear();

            addLog("Start App-Load Config");
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                var response = await client.GetAsync(PostURL + "?f=list");
                if (response.IsSuccessStatusCode)
                {
                    var resultText = await response.Content.ReadAsStringAsync();
                    addLog("Get data done to from server. Data Length: " + resultText.Length);
                    addLog(resultText);
                    try
                    {
                        addLog("Parse list machine");
                        JObject o1 = JObject.Parse(resultText);
                        JArray arr1 = (JArray)o1.GetValue("machine");
                        foreach (JObject obj in arr1)
                        {
                            string MAC_CODE = (string)obj.GetValue("MAC_CODE");
                            string MAC_NAME = (string)obj.GetValue("MAC_NAME");
                            string MAC_CONNECT = (string)obj.GetValue("MAC_CONNECT");
                            string[] tmp = MAC_CONNECT.Split("|");
                            if (tmp.Length > 1)
                            {
                                if (MAC_CONNECT.ToUpper().IndexOf("COM") > -1)
                                {
                                    listMachineData.Add(new MachineData()
                                    {
                                        MachineCode = MAC_CODE,
                                        MachineName = MAC_NAME,
                                        MachineType = "RS232",
                                        MachineCOM = tmp[0].ToUpper(),
                                        MachineBaudrate = tmp[1].ToUpper(),
                                        MachineObject = null
                                    });
                                    addLog(">>>> Data machine added: [" + MAC_NAME + "] [" + MAC_CODE + "] " + MAC_CONNECT);
                                }
                                else
                                {
                                    listMachineData.Add(new MachineData()
                                    {
                                        MachineCode = MAC_CODE,
                                        MachineName = MAC_NAME,
                                        MachineType = "TCP",
                                        MachineIP = tmp[0].ToUpper(),
                                        MachinePort = tmp[1].ToUpper(),
                                        MachineBaudrate = tmp[2].ToUpper(),
                                        MachineObject = null
                                    });
                                    addLog(">>>> Data machine added: [" + MAC_NAME + "] [" + MAC_CODE + "] " + MAC_CONNECT);
                                }
                            }
                            else
                            {
                                addLog("ERROR: Data machine not correct: [" + MAC_NAME + "] [" + MAC_CODE + "] " + MAC_CONNECT);
                            }
                        }

                        RefreshTable();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        addLog("ERROR GET DATA FROM REMOTE: " + ex.Message);
                        return false;
                    }
                }
                else
                {
                    addLog($"Failed with status code {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                addLog("Report Data: " + ex.Message);
            }
            return false;
        }

        private void RefreshTable(string msg = "")
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(RefreshTable), new object[] { msg });
                //_ = this.Invoke(new Action<bool>(RefreshTable), new object[] { msg });
            }
            else
            {
                try
                {
                    dataGridView1.DataSource = listMachineData;
                    dataGridView1.Refresh();
                }
                catch (Exception ex)
                {
                    addLog("ERROR: " + ex.Message);
                }
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
                    listBox1.Items.Add(DateTime.Now.ToString("HH:mm:ss ") + msg);
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

        }

        private void StartApp()
        {
            addLog("Connect machine ...");
            foreach (MachineData client in listMachineData)
            {
                if (client.MachineType == "TCP")
                {
                    new Thread(() =>
                    {
                        RetryConnectMachine(client);
                    }).Start();
                }
                else if (client.MachineType == "RS232")
                {
                    try
                    {
                        SerialPort _serialPort = new SerialPort(
                                client.MachineCOM,
                                Convert.ToInt32(client.MachineBaudrate),
                                Parity.None, 8, StopBits.One);
                        _serialPort.Open();
                        _serialPort.DataReceived += _serialPort_DataReceived;
                        _serialPort.ErrorReceived += _serialPort_ErrorReceived; ;
                        _serialPort.PinChanged += _serialPort_PinChanged;
                        _serialPort.Disposed += _serialPort_Disposed;
                        client.MachineObject = _serialPort;
                        client.MachineStatus = "Connecting";
                        RefreshTable();
                        addLog("Connect RS232: " + client.ToString());
                    }
                    catch (Exception ex)
                    {
                        addLog("ERROR: Connect RS232: " + client.ToString() + " " + ex.Message);
                    }

                }
            }
        }

        private void _serialPort_Disposed(object? sender, EventArgs e)
        {
            addLog("_serialPort_Disposed");
        }

        private void _serialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            foreach (MachineData client in listMachineData)
            {
                if (client.MachineType == "TCP" && client == sender)
                {
                    //EasyTcpClient tcpClient = (EasyTcpClient)sender;
                }
                else if (client.MachineType == "RS232" && client == sender)
                {
                    try
                    {
                        SerialPort _serialPort = (SerialPort)sender;
                        client.MachineStatus = "Connnected";
                        addLog("Connected RS232: " + client.ToString());
                    }
                    catch (Exception ex)
                    {
                        addLog("ERROR: Connect RS232: " + client.ToString() + " " + ex.Message);
                    }

                }
            }
            RefreshTable();
        }

        private void _serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            addLog("_serialPort_ErrorReceived ");
        }

        private void _EasyTcpClient_OnDisconnect(object? sender, EasyTcpClient e)
        {
            try
            {
                if (listMachineData.Count > 0)
                {
                    foreach (MachineData client in listMachineData)
                    {
                        if (client.MachineType == "TCP" && client.MachineObject == sender)
                        {
                            client.MachineStatus = "Disconnected";
                            addLog("Disconnect " + client.ToString());
                            new Thread(() =>
                            {
                                RetryConnectMachine(client);
                            }).Start();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void RetryConnectMachine(MachineData client)
        {
            while (appStop == false)
            {
                addLog("Try connect to: " + client.ToString());
                if (client.MachineIP.ToUpper().Contains("LOCALHOST") ||
                    client.MachineIP.ToUpper().Contains("127.0.0.1"))
                {
                    break;
                }
                if (client.MachineBaudrate == "PASSIVE")
                {
                    // Open listening
                    EasyTcpServer _EasyTcpServer = new EasyTcpServer(new PlainTcpProtocol());
                    _EasyTcpServer.OnDataReceive += _EasyTcpServer_OnDataReceive; ;
                    _EasyTcpServer.OnConnect += _EasyTcpClient_OnConnect;
                    _EasyTcpServer.OnDisconnect += _EasyTcpServer_OnDisconnect; ;
                    _EasyTcpServer.Start(client.MachineIP, (ushort)Convert.ToInt32(client.MachinePort));
                    client.MachineObject = _EasyTcpServer;
                    client.MachineStatus = "Connecting";
                    RefreshTable();
                    break;
                }
                else
                {
                    //EasyTcpClient _EasyTcpClient = new EasyTcpClient(new PlainTcpProtocol(10240));
                    EasyTcpClient _EasyTcpClient = new EasyTcpClient(new PrefixLengthProtocol());
                    _EasyTcpClient.OnDataReceive += Client_OnDataReceive;
                    _EasyTcpClient.OnConnect += _EasyTcpClient_OnConnect;
                    _EasyTcpClient.OnDisconnect += _EasyTcpClient_OnDisconnect;
                    client.MachineObject = _EasyTcpClient;
                    client.MachineStatus = "Connecting";

                    if (_EasyTcpClient.Connect(client.MachineIP, (ushort)Convert.ToInt32(client.MachinePort)) == true)
                    {
                        RefreshTable();
                        break;
                    }
                    else
                    {
                        addLog(">>>> Retry connect after 10s: " + client.ToString());
                        Thread.Sleep(10000);
                    }
                }

            }
            //addLog("Try connect done: " + client.ToString());
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        private void _EasyTcpServer_OnDataReceive(object? sender, EasyTcp4.Message e)
        {
            addLog("_EasyTcpServer_OnDataReceive " + ByteArrayToString(e.Data));
        }

        private void _EasyTcpServer_OnDisconnect(object? sender, EasyTcpClient e)
        {
            _EasyTcpClient_OnDisconnect(sender, e);
        }

        private void _EasyTcpClient_OnConnect(object? sender, EasyTcpClient e)
        {
            foreach (MachineData client in listMachineData)
            {
                if (client.MachineType == "TCP" && client.MachineObject == sender)
                {
                    client.MachineStatus = "Connected";
                    addLog("Connected " + client.MachineName);
                    RefreshTable();

                }
            }
            RefreshTable();
        }

        private void Client_OnDataReceive(object? sender, EasyTcp4.Message e)
        {
            foreach (MachineData client in listMachineData)
            {
                string result = "";
                if (client.MachineType == "TCP" && client.MachineObject == sender)
                {
                    result = System.Text.Encoding.UTF8.GetString(e.Data);
                    if (result.Length <= 2)
                    {
                        log.Info("Ping: " + ByteArrayToString(e.Data) + " " + client.ToString());
                        continue;
                    }
                    //////////////////////////////////////////////////
                    ///// Remove First and end chracter
                    result = result.Substring(1, result.Length - 3);
                    addLog(result);
                    //string ackMessage = "MSH|^~\\&|SAPP|SFCT|RAPP|RFCT|20080312181835||ADT^A01|0D23ACC3-17CD-4FF4-BE66-AD4A6572079E|P|2.4";
                    //string ackMessage = "MSH|^~\\&||ACK|";
                    //HL7.Dotnetcore.Message message = new HL7.Dotnetcore.Message(result);
                    //// Parse this message
                    //bool isParsed = false;
                    //try
                    //{
                    //    isParsed = message.ParseMessage(true);
                    //}
                    //catch (Exception ex)
                    //{
                    //    addLog(ex.Message);
                    //}
                    //ackMessage = message.GetACK().SerializeMessage(false);
                    //log.Info("Send ACK: " + ackMessage);
                    //ackMessage = ((char)11).ToString() + ackMessage + ((char)28).ToString() + ((char)13).ToString();
                    //byte[] ackMessageBytes = Encoding.UTF8.GetBytes(ackMessage);
                    //EasyTcpClient easyTcpClient = (EasyTcpClient)client.MachineObject;
                    //easyTcpClient.Send(ackMessageBytes);
                    //////////////////////////////////////////////////
                    ReportData(client.MachineCode, client.MachineName, result);
                }
            }
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            foreach (MachineData client in listMachineData)
            {
                string result = "";
                if (client.MachineType == "RS232" && client.MachineObject == sender)
                {
                    try
                    {
                        SerialPort sp = (SerialPort)sender;
                        result = sp.ReadExisting();
                        ReportData(client.MachineCode, client.MachineName, result);
                    }
                    catch (Exception ex)
                    {
                        addLog("ERROR RS232 read: " + ex.Message);
                    }
                }
            }
        }

        private async Task<bool> ReportData(string machineCode, string machineName, string result)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                var parameters = new Dictionary<string, string>();
                parameters.Add("machineCode", machineCode);
                parameters.Add("machineName", machineName);
                parameters.Add("result", result);
                parameters.Add("Content-Type", "application/x-www-form-urlencoded");
                HttpContent content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync(PostURL, content);
                if (response.IsSuccessStatusCode)
                {
                    var resultText = await response.Content.ReadAsStringAsync();
                    addLog("    >>>> " + resultText);
                    addLog(">>>> Post data done to : " + machineCode + " " + machineName + " Length: " + resultText.Length);
                    return true;
                }
                else
                {
                    addLog($"Failed with status code {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                addLog("Report Data: " + ex.Message);
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new FormTCPServer();
            frm.Show();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            StartApplication();
        }

        private async void StartApplication()
        {
            await StartLoadConfig();
            StartApp();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var frm = new FormRS232();
            frm.Show();
        }



        private async void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                //
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                this.Text = "OpenClinic Data Collection - assemblyVersion=" + assemblyVersion;
                LoadConfigLocal();
                AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;
                AutoUpdater.Start(AutoUpdateURL);
                //
                StartApplication();
                //
            }
            catch (Exception ex)
            {
                addLog("ERROR:" + ex.Message);
            }
        }
        bool autoUpdater = false;
        private void AutoUpdater_ApplicationExitEvent()
        {
            autoUpdater = true;
            Application.Exit();
        }

        private void LoadConfigLocal()
        {
            try
            {
                JObject o1 = JObject.Parse(File.ReadAllText(@"config.json"));
                HOSTNAME = (string)o1.GetValue("HOST");
                PostURL = (string)o1.GetValue("URL");
                PostURL = "http://"+HOSTNAME + PostURL;
                AutoUpdateURL = (string)o1.GetValue("AUTOUPDATE");
                AutoUpdateURL = "http://" + HOSTNAME + AutoUpdateURL;

                addLog("HOSTNAME: " + HOSTNAME);
                addLog("PostURL: " + PostURL);
                addLog("AutoUpdateURL: " + AutoUpdateURL);

                JArray arr1 = (JArray)o1.GetValue("LIST");
                foreach (JObject obj in arr1)
                {
                    string MachineName = (string)obj.GetValue("Name");
                    string Type = (string)obj.GetValue("Type");
                    if (Type == "TCP")
                    {
                        //string Port = (string)obj.GetValue("Port");
                        // TCP PORT
                        listMachineData.Add(new MachineData()
                        {
                            MachineName = MachineName,
                            MachineType = Type,
                            MachineIP = (string)obj.GetValue("Ip"),
                            MachinePort = (string)obj.GetValue("Port"),
                            MachineObject = new EasyTcpClient()
                        });
                    }
                    else
                    {
                        string MachineCOM = (string)obj.GetValue("COM");
                        string MachineBaudrate = (string)obj.GetValue("Baudrate");
                        // COM PORT
                        listMachineData.Add(new MachineData()
                        {
                            MachineName = MachineName,
                            MachineType = Type,
                            MachineCOM = MachineCOM,
                            MachineBaudrate = MachineBaudrate,
                            MachineObject = null
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                addLog("ERROR:" + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReportData("Code", "NEO TEST", "DATA TEST POST");
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (autoUpdater == true)
            {
                return;
            }
            if (MessageBox.Show("Tắt chương trình sẽ không nhận được dữ liệu từ các máy ?? ", "Confirm close", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                appStop = true;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            foreach (MachineData client in listMachineData)
            {
                string result = "";
                if (client.MachineType == "RS232")
                {
                    try
                    {
                        SerialPort sp = (SerialPort)client.MachineObject;
                        sp.Write("TEST DATA");
                    }
                    catch (Exception ex)
                    {
                        addLog("ERROR RS232 read: " + ex.Message);
                    }
                }
                else if (client.MachineType == "TCP")
                {
                    try
                    {
                        if (client.MachineBaudrate == "PASSIVE")
                        {
                            EasyTcpServer sp = (EasyTcpServer)sender;
                            sp.SendAll("TEST DATA");
                        }
                        else
                        {
                            EasyTcpClient sp = (EasyTcpClient)sender;
                            sp.Send("TEST DATA");
                        }
                    }
                    catch (Exception ex)
                    {
                        addLog("ERROR TCP read: " + ex.Message);
                    }
                }
            }
        }
    }
}