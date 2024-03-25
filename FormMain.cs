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
using System.Windows.Markup;
using System.Windows.Interop;
using System.Reflection.PortableExecutable;
using System.Data.SQLite;
using System.Xml;
using System.Security.Cryptography;
using System.Data;
using System;

namespace HL7.Dotnetcore
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
        public bool ISLOCAL = false;
        public string AutoUpdateURL = "";




        public FormMain()
        {
            InitializeComponent();
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        private void LoadConfigLocal()
        {
            try
            {
                CreateTable();

                JObject o1 = JObject.Parse(File.ReadAllText(@"config.json"));
                HOSTNAME = (string)o1.GetValue("HOST");
                PostURL = (string)o1.GetValue("URL");
                PostURL = "http://" + HOSTNAME + PostURL;
                AutoUpdateURL = (string)o1.GetValue("AUTOUPDATE");
                AutoUpdateURL = "http://" + HOSTNAME + AutoUpdateURL;

                JToken isLocal;
                if (o1.TryGetValue("ISLOCAL", out isLocal))
                {
                    ISLOCAL = (bool)o1.GetValue("ISLOCAL");

                }

                addLog("HOSTNAME: " + HOSTNAME);
                addLog("PostURL: " + PostURL);
                addLog("AutoUpdateURL: " + AutoUpdateURL);

                JArray arr1 = (JArray)o1.GetValue("LIST");
                foreach (JObject obj in arr1)
                {
                    string MAC_CODE = (string)obj.GetValue("MAC_CODE");
                    string MAC_NAME = (string)obj.GetValue("MAC_NAME");
                    string MAC_CONNECT = (string)obj.GetValue("MAC_CONNECT");
                    string MAC_ENDLINE = (string)obj.GetValue("MAC_ENDLINE");

                    string[] tmp = MAC_CONNECT.Split("|");
                    if (tmp.Length >= 1)
                    {
                        if (MAC_CONNECT.ToUpper().IndexOf("COM") > -1)
                        {
                            MachineData dta = new MachineData()
                            {
                                MachineCode = MAC_CODE,
                                MachineName = MAC_NAME,
                                MachineType = "RS232",
                                MachineCOM = tmp[0].ToUpper(),
                                MachineBaudrate = (tmp.Length > 1) ? tmp[1].ToUpper() : "9600",
                                MachineObject = null,
                                DataEndLine = MAC_ENDLINE
                            };
                            listMachineData.Add(dta);
                            addLog(">>>> Data machine added: [" + MAC_NAME + "] [" + MAC_CODE + "] " + MAC_CONNECT);
                        }
                        else
                        {
                            MachineData tcp = new MachineData()
                            {
                                MachineCode = MAC_CODE,
                                MachineName = MAC_NAME,
                                MachineType = "TCP",
                                MachineIP = tmp[0].ToUpper(),
                                MachinePort = tmp[1].ToUpper(),
                                MachineBaudrate = tmp[2].ToUpper(),
                                MachineObject = null,
                                DataEndLine = MAC_ENDLINE
                            };
                            if (tcp.MachineBaudrate == "PASSIVE")
                            {
                                tcp.MachineIP = "0.0.0.0";
                            }
                            if (Convert.ToInt32(tcp.MachinePort) <= 10)
                            {
                                tcp.MachinePort = "5000";
                            }
                            listMachineData.Add(tcp);
                            addLog(">>>> Data machine added: [" + MAC_NAME + "] [" + MAC_CODE + "] " + MAC_CONNECT);
                        }
                    }
                    else
                    {
                        addLog("ERROR: Data machine not correct: [" + MAC_NAME + "] [" + MAC_CODE + "] " + MAC_CONNECT);
                    }
                }

            }
            catch (Exception ex)
            {
                addLog("ERROR:" + ex.Message);
            }
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

            if (ISLOCAL == false)
            {
                addLog("Start App-Load - Clear");
                listMachineData.Clear();

                addLog("Start load config from remote..." + PostURL);
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
                                string MAC_ENDLINE = (string)obj.GetValue("MAC_ENDLINE");
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
                                            MachineObject = null,
                                            DataEndLine = MAC_ENDLINE
                                        });
                                        addLog(">>>> Data machine added: [" + MAC_NAME + "] [" + MAC_CODE + "] " + MAC_CONNECT);
                                    }
                                    else
                                    {
                                        // TCP
                                        MachineData tcp = new MachineData()
                                        {
                                            MachineCode = MAC_CODE,
                                            MachineName = MAC_NAME,
                                            MachineType = "TCP",
                                            MachineIP = tmp[0].ToUpper(),
                                            MachinePort = tmp[1].ToUpper(),
                                            MachineBaudrate = tmp[2].ToUpper(),
                                            MachineObject = null,
                                            DataEndLine = MAC_ENDLINE
                                        };
                                        if (tcp.MachineBaudrate == "PASSIVE")
                                        {
                                            tcp.MachineIP = "0.0.0.0";
                                        }
                                        if (Convert.ToInt32(tcp.MachinePort) <= 10)
                                        {
                                            tcp.MachinePort = "5000";
                                        }
                                        listMachineData.Add(tcp);
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
            }
            else
            {
                RefreshTable();
                return true;
                addLog("Config from local.");
            }
            return false;
        }

        private void RefreshTable(string msg = "")
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(RefreshTable), new object[] { msg });
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
            tabControl1.SelectedIndex = 1;
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
                        client.StringData = "";
                        _serialPort.DataReceived += _serialPort_DataReceived;
                        //_serialPort.ErrorReceived += _serialPort_ErrorReceived; ;
                        _serialPort.PinChanged += _serialPort_PinChanged;
                        _serialPort.Disposed += _serialPort_Disposed;
                        _serialPort.Open();
                        client.MachineObject = _serialPort;
                        client.MachineStatus = "Connecting";
                        RefreshTable();
                        addLog("Connect RS232: " + client.ToString());
                    }
                    catch (Exception ex)
                    {
                        client.MachineStatus = "ERROR: " + ex.Message;
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
                            Thread.Sleep(10000);
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
                addLog("ERROR: " + ex.Message);

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
                    EasyTcpServer _EasyTcpServer = new EasyTcpServer(new PlainTcpProtocol(10240));
                    _EasyTcpServer.OnDataReceive += _EasyTcpServer_OnDataReceive; ;
                    _EasyTcpServer.OnConnect += _EasyTcpClient_OnConnect;
                    _EasyTcpServer.OnDisconnect += _EasyTcpServer_OnDisconnect; ;
                    _EasyTcpServer.Start(client.MachineIP, (ushort)Convert.ToInt32(client.MachinePort));
                    client.MachineObject = _EasyTcpServer;
                    client.MachineStatus = "Listen " + client.MachinePort + "...";
                    RefreshTable();
                    break;
                }
                else
                {
                    EasyTcpClient _EasyTcpClient = new EasyTcpClient(new PlainTcpProtocol(10240));
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
                    ReportData(client.MachineCode, client.MachineName, result);
                }
            }
        }

        private void _EasyTcpServer_OnDisconnect(object? sender, EasyTcpClient e)
        {
            try
            {
                if (listMachineData.Count > 0)
                {
                    foreach (MachineData client in listMachineData)
                    {
                        if (client.MachineType == "TCP" && client.MachineObject == sender)
                        {
                            addLog(">>>> Client disconnected: " + client.ToString());
                            client.MachineStatus = "Listen...";
                            RefreshTable();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                addLog("ERROR: " + ex.Message);
            }
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
                        SerialPort _serialPort = (SerialPort)sender;
                        string line = _serialPort.ReadLine();
                        addLog("" + line);
                        client.StringData += line;
                        if (client.DataEndLine == null)
                        {
                            // Default
                            client.DataEndLine = "GLU";
                        }
                        if (line.Contains(client.DataEndLine) == true)
                        {
                            addLog(" " + client.StringData);
                            ReportData(client.MachineCode, client.MachineName, client.StringData);

                            client.StringData = "";
                        }
                        //byte[] data = new byte[_serialPort.BytesToRead];
                        //_serialPort.Read(data, 0, data.Length);
                        //addLog("RS232 read: " + ByteArrayToString(data));
                        //ReportData(client.MachineCode, client.MachineName, result);
                    }
                    catch (Exception ex)
                    {
                        addLog("ERROR RS232 read: " + ex.Message);
                    }
                }
            }
        }

        private async Task<bool> ReportData(string machineCode, string machineName, string result, bool saveTable = true)
        {
            try
            {
                if (saveTable == true)
                {
                    SaveTable(machineCode, machineName, result);
                }

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                var parameters = new Dictionary<string, string>();
                parameters.Add("machineCode", machineCode);
                parameters.Add("machineName", machineName);
                parameters.Add("result", result);
                parameters.Add("repost", saveTable.ToString());
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



        private void button4_Click(object sender, EventArgs e)
        {
            //ReportData("Code", "NEO TEST", "DATA TEST POST");
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
                // Close connection
                CloseConnection();
                //
                appStop = true;
            }
        }

        private void CloseConnection()
        {
            try
            {
                foreach (MachineData client in listMachineData)
                {
                    string result = "";
                    if (client.MachineType == "RS232")
                    {
                        try
                        {
                            SerialPort sp = (SerialPort)client.MachineObject;
                            if (sp != null)
                            {
                                sp.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            addLog("ERROR RS232 Close: " + ex.Message);
                        }
                    }
                    else if (client.MachineType == "TCP")
                    {
                        try
                        {
                            if (client.MachineBaudrate == "PASSIVE")
                            {
                                EasyTcpServer sp = (EasyTcpServer)client.MachineObject;
                                if (sp != null)
                                {
                                    sp.Dispose();
                                }
                                log.Info("Close TCP passive. Close port " + client.ToString());
                            }
                            else
                            {
                                EasyTcpClient sp = (EasyTcpClient)client.MachineObject;
                                if (sp != null)
                                {
                                    sp.Dispose();
                                }
                                log.Info("Close TCP. " + client.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            addLog("ERROR TCP Dispose: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                addLog("ERROR RS232 read: " + ex.Message);
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

        private void FormMain_MinimumSizeChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        static void SaveTable(MachineData mac, String data)
        {
            SaveTable(mac.MachineCode, mac.MachineName, data);
        }
        static void SaveTable(string code, string name, String data)
        {
            using (var connection = new SQLiteConnection("Data Source=hello.db"))
            {
                connection.Open();
                try
                {
                    //data = data.Replace("\n", "<br>").Replace("\'", "''").Replace("\"", "\"\"");
                    data = data.Replace("\'", "''").Replace("\"", "\"\"");
                    Console.WriteLine(connection.AutoCommit);
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"insert into machine_data (MAC_NAME,MAC_CODE,MAC_DATA,MAC_TIME) values(" +
                    "'" + name + "','" + code + "'," +
                    "'" + data + "',datetime('now', 'localtime'))";
                    command.ExecuteNonQuery();
                    log.Info("Write db done");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                connection.Close();
            }


        }
        static void CreateTable()
        {
            using (var connection = new SQLiteConnection("Data Source=hello.db"))
            {
                connection.Open();
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS machine_data (
	""id""	INTEGER,
	""MAC_CODE""	TEXT,
	""MAC_NAME""	TEXT,
	""MAC_DATA""	TEXT,
	""MAC_TIME""	TEXT,
	PRIMARY KEY(""id"" AUTOINCREMENT)
);";
                    command.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }


        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {


        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var rowsCount = dataGridView1.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;

            var row = dataGridView1.SelectedRows[0];
            if (row == null) return;
            // Select data from grid 2
            string machinecode = row.Cells[0].Value.ToString();
            tabControl1.SelectedIndex = 0;
            GridDataSellect(machinecode);
        }

        private void GridDataSellect(string? machinecode)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("Time", typeof(string));
                table.Columns.Add("Code", typeof(string));
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("Value", typeof(string));

                using (var connection = new SQLiteConnection("Data Source=hello.db"))
                {
                    connection.Open();
                    using (SQLiteCommand fmd = connection.CreateCommand())
                    {
                        fmd.CommandText = @"SELECT * FROM machine_data order by id desc";
                        SQLiteDataReader r = fmd.ExecuteReader();
                        while (r.Read())
                        {


                            string MAC_CODE = (string)r["MAC_CODE"];
                            string MAC_NAME = (string)r["MAC_NAME"];
                            string MAC_DATA = (string)r["MAC_DATA"];
                            string MAC_TIME = (string)r["MAC_TIME"];

                            table.Rows.Add(MAC_TIME, MAC_CODE, MAC_NAME, MAC_DATA);

                        }

                        connection.Close();
                    }

                }
                // Update view
                dataGridView2.DataSource = table;

            }
            catch (Exception e)
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var rowsCount = dataGridView2.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;
            var row = dataGridView2.SelectedRows[0];
            if (row == null)
            {
                return;
            }
            // Select data from grid 2
            string machinetime = row.Cells[0].Value.ToString();
            string machinecode = row.Cells[1].Value.ToString();
            string machinename = row.Cells[2].Value.ToString();
            string machinedata = row.Cells[3].Value.ToString();
            ReportData(machinecode, machinename, machinedata, false);
            //
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var rowsCount = dataGridView2.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1) return;
            var row = dataGridView2.SelectedRows[0];
            if (row == null)
            {
                return;
            }
            string machinedata = row.Cells[3].Value.ToString();
            MessageBox.Show(machinedata);
        }
    }
}