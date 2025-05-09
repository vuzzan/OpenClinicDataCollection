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
using System.Text.Json;
using OpenClinicDataCollection;
using System.Data.Entity;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static OpenClinicDataCollection.frmPreview;
using System.Xml.Linq;

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
        public string APP_ID = "";
        public string CLINIC_ID = "";
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

                if (listMachineData.Count > 0)
                {

                }


                JObject o1 = JObject.Parse(File.ReadAllText(@"config.json"));
                HOSTNAME = (string)o1.GetValue("HOST");
                APP_ID = (string)o1.GetValue("APP_ID");
                CLINIC_ID = (string)o1.GetValue("CLINIC_ID");
                HOSTNAME = (string)o1.GetValue("HOST");
                PostURL = (string)o1.GetValue("URL");
                AutoUpdateURL = (string)o1.GetValue("AUTOUPDATE");
                // 
                txtHost.Text = HOSTNAME;
                txtClinicID.Text = CLINIC_ID;
                txtAppID.Text = APP_ID;
                txtAPIURL.Text = PostURL;
                //
                if (HOSTNAME.ToLower().Contains("https://"))
                {
                    PostURL = HOSTNAME + PostURL;
                    AutoUpdateURL = HOSTNAME + AutoUpdateURL;
                }
                else
                {
                    if (HOSTNAME.ToLower().Contains("http://"))
                    {
                        PostURL = HOSTNAME + PostURL;
                        AutoUpdateURL = HOSTNAME + AutoUpdateURL;
                    }
                    else
                    {
                        PostURL = "http://" + HOSTNAME + PostURL;
                        AutoUpdateURL = "http://" + HOSTNAME + AutoUpdateURL;
                    }
                }
                JToken isLocal;
                if (o1.TryGetValue("ISLOCAL", out isLocal))
                {
                    ISLOCAL = (bool)o1.GetValue("ISLOCAL");
                }

                addLog("HOSTNAME: " + HOSTNAME);
                addLog("PostURL: " + PostURL);
                addLog("AutoUpdateURL: " + AutoUpdateURL);
                if (ISLOCAL == true)
                {
                    JArray arr1 = (JArray)o1.GetValue("LIST");
                    foreach (JObject obj in arr1)
                    {
                        string APP_ID = (string)obj.GetValue("APP_ID");
                        string MAC_MODEL = (string)obj.GetValue("MAC_MODEL");
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
                                    App_ID = APP_ID,
                                    MachineModel = MAC_MODEL,
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
                                    App_ID = APP_ID,
                                    MachineModel = MAC_MODEL,
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
                    int AppID = Convert.ToInt16(APP_ID);
                    int cid = Convert.ToInt16(CLINIC_ID);
                    addLog("Get data: " + PostURL + "?f=list&id=" + AppID + "&cid=" + cid);
                    var response = await client.GetAsync(PostURL + "?f=list&id=" + AppID + "&cid=" + cid);
                    if (response.IsSuccessStatusCode)
                    {
                        var resultText = await response.Content.ReadAsStringAsync();
                        addLog("Get data done to from server. Data Length: " + resultText.Length);
                        addLog(resultText);
                        try
                        {
                            addLog("Parse list machine");
                            JObject o1 = JObject.Parse(resultText);

                            JToken tokennull = o1["machine"];

                            //if (o1.GetValue("machine") == JValue.)
                            if (tokennull.Type == JTokenType.Null)
                            {
                                MessageBox.Show("Không có máy nào ở AppId=" + AppID + ". Check phía server DB config.");
                                addLog("NO MACHINE - GET DATA FROM REMOTE: ");
                                return true;
                            }
                            JArray arr1 = (JArray)o1.GetValue("machine");
                            foreach (JObject obj in arr1)
                            {
                                string APP_ID = (string)obj.GetValue("APP_ID");
                                string CLINIC_ID = (string)obj.GetValue("CLINIC_ID");
                                string MAC_MODEL = (string)obj.GetValue("MAC_MODEL");
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
                                            App_ID = APP_ID,
                                            ClinicID = CLINIC_ID,
                                            MachineModel = MAC_MODEL,
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
                                            App_ID = APP_ID,
                                            ClinicID = CLINIC_ID,
                                            MachineModel = MAC_MODEL,
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
            tabControl1.SelectedIndex = 0;
            addLog("Connect machine ...");
            foreach (MachineData client in listMachineData)
            {
                if (client.MachineType == "TCP")
                {
                    Thread thread = new Thread(() =>
                    {
                        RetryConnectMachine(client);
                    });
                    client.thread = thread;
                    thread.Start();

                }
                else if (client.MachineType == "RS232")
                {
                    try
                    {
                        string baudrate = client.MachineBaudrate;
                        string valueParity = "0";
                        //MachineBaudrate = 9600,0,8,1
                        //public enum Parity => None = 0,Odd = 1,Even = 2,Mark = 3,Space = 4
                        //public enum StopBits => None = 0,One = 1,Two = 2,OnePointFive = 3
                        //public enum databits => 5->8
                        string databits = "8";
                        string stopBit = "1";
                        //None = 0,One = 1,Two = 2,OnePointFive = 3,
                        string[] tmp2 = client.MachineBaudrate.Split(",");
                        if (tmp2.Length >= 4)
                        {
                            baudrate = tmp2[0];
                            valueParity = tmp2[1];
                            databits = tmp2[2];
                            stopBit = tmp2[3];
                        }
                        if (Convert.ToInt16(stopBit) < 0 || Convert.ToInt16(stopBit) > 3)
                        {
                            stopBit = "1"; // One
                        }
                        if (Convert.ToInt16(valueParity) < 0 || Convert.ToInt16(valueParity) > 4)
                        {
                            valueParity = "0"; // None
                        }
                        if (Convert.ToInt16(databits) < 5 || Convert.ToInt16(databits) > 8)
                        {
                            databits = "8"; // 8
                        }
                        addLog("Connect param: baudrate=" + baudrate + " databits=" + databits);
                        addLog("Parity(None = 0,Odd = 1,Even = 2,Mark = 3,Space = 4)=" + valueParity);
                        addLog("StopBit(None = 0,One = 1,Two = 2,OnePointFive = 3)=" + stopBit);
                        //SerialPort _serialPort = new SerialPort(
                        //        client.MachineCOM,
                        //        Convert.ToInt32(client.MachineBaudrate),
                        //        Parity.None, 8, StopBits.One);
                        SerialPort _serialPort = new SerialPort(
                                client.MachineCOM,
                                Convert.ToInt32(baudrate),
                                (Parity)Convert.ToInt16(valueParity),
                                Convert.ToInt16(databits),
                                (StopBits)Convert.ToInt16(stopBit));

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
                if (client.threadStop == true)
                {
                    break;
                }
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
                        addLog(">>>> Retry connect after 5s: " + client.ToString());
                        for (int i = 0; i < 50; i++)
                        {
                            if (client.threadStop == true)
                            {
                                break;
                            }
                            Thread.Sleep(100);
                        }
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
                string rawResult = "";
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
                    rawResult = result;
                    result = result.Substring(1, result.Length - 3);
                    addLog(result);
                    ReportData(client.MachineCode, client.MachineName, result, rawResult);
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
                string rawResult = "";
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
                    rawResult = result;
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
                    ReportData(client.MachineCode, client.MachineName, result, rawResult);
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
                        byte[] data = new byte[_serialPort.BytesToRead];
                        _serialPort.Read(data, 0, data.Length);
                        if (data[data.Length - 1] == 0xFF)
                        {
                            log.Debug("Recv FF");
                            return;
                        }
                        //data.ToList().ForEach(b =>
                        //{
                        //    addLog("Recv " + b.ToString());
                        //});
                        string hex = BitConverter.ToString(data);
                        addLog("Recv " + data.Length + " bytes. [" + hex.Replace("-", " ") + "]");
                        bool isStop = false;
                        if (data[data.Length - 1] == 0x00)
                        {
                            addLog("STOP RECV 0x00");
                            isStop = true;
                        }

                        if (data[data.Length - 1] == 0x04)
                        {
                            addLog("STOP RECV 0x04");
                            isStop = true;
                        }
                        if (data[data.Length - 1] == 0x05)
                        {
                            addLog("SERIAL CONTROL MODE");
                            client.DataEndLine = "CONTROL";
                        }
                        string line = System.Text.Encoding.ASCII.GetString(data);
                        addLog("Recv ASCII: " + line);
                        client.StringData += line;
                        if (client.DataEndLine == null)
                        {
                            // Default
                            client.DataEndLine = "GLU";
                        }
                        if (
                            (client.DataEndLine.Trim().Length >= 3 && line.Contains(client.DataEndLine) == true)
                            || isStop == true)
                        {
                            addLog(" " + client.StringData);
                            ReportData(client.MachineCode, client.MachineName, client.StringData, "");

                            client.StringData = "";
                        }
                        // SEND ACK
                        if (client.DataEndLine == "CONTROL")
                        {
                            // SEND ACK
                            byte[] byteACk = { 0x06 };
                            _serialPort.Write(byteACk, 0, 1);
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

        private async Task<bool> ReportData(string machineCode, string machineName, string result, string rawResult, bool saveTable = true)
        {
            try
            {
                //Machines machines = new Machines(appID, machineModel, machineCode, machineName, result,rawResult);
                //var jsonDataResult= JsonConvert.SerializeObject(machines.JsonData);
                if (saveTable == true)
                {
                    SaveTable(machineCode, machineName, result);
                }


                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                var parameters = new Dictionary<string, string>();
                parameters.Add("APP_ID", APP_ID);
                parameters.Add("clinic_id", CLINIC_ID);
                parameters.Add("machineCode", machineCode);
                parameters.Add("machineName", machineName);
                parameters.Add("result", result);
                //parameters.Add("jsondata", jsonDataResult);
                parameters.Add("repost", saveTable.ToString());
                parameters.Add("Content-Type", "application/x-www-form-urlencoded");
                HttpContent content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync(PostURL, content);
                if (response.IsSuccessStatusCode)
                {
                    var resultText = await response.Content.ReadAsStringAsync();
                    addLog("    >>>> " + resultText);
                    try
                    {
                        JObject o1 = JObject.Parse(resultText);
                        addLog("    >>>> equipment_data: " + o1["equipment_data"]);
                        JObject parse_data = (JObject)o1["parse_data"];
                        JObject result_data = (JObject)parse_data["result"];
                        JArray xetnghiem = (JArray)result_data["xetnghiem"];
                        if (xetnghiem.Count == 0)
                        {
                            addLog("    >>>> List data OK: ");
                            JArray arr1 = (JArray)result_data.GetValue("data");
                            foreach (JObject obj in arr1)
                            {
                                string MA_CHI_SO = (string)obj.GetValue("MA_CHI_SO");
                                string TEN_CHI_SO = (string)obj.GetValue("TEN_CHI_SO");
                                string VALUE = (string)obj.GetValue("VALUE");
                                addLog("         >>>> " + MA_CHI_SO + " [" + TEN_CHI_SO + "] " + VALUE);
                            }
                        }
                        else
                        {
                            addLog("    >>>> Có lỗi khi thêm table xetnghiem: ");
                            foreach (JValue obj in xetnghiem)
                            {
                                addLog("         >>>> " + obj.ToString());
                            }
                        }

                        addLog("    >>>> equipment_data: " + o1["equipment_data"]);
                        addLog(">>>> Post data done to : " + machineCode + " " + machineName + " Length: " + resultText.Length);
                        return true;
                    }
                    catch (Exception eee)
                    {
                        addLog(">>>> Co loi:  " + eee.Message);
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
            string machinemodel = row.Cells[0].Value.ToString();
            string machinecode = row.Cells[1].Value.ToString();
            string machinename = row.Cells[2].Value.ToString();
            string machinedata = row.Cells[3].Value.ToString();
            ReportData(machinecode, machinename, machinedata, "", false);
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

        private void button5_Click(object sender, EventArgs e)
        {
            using (var connection = new SQLiteConnection("Data Source=hello.db"))
            {
                connection.Open();
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"delete * from machine_data";
                    command.ExecuteNonQuery();
                    log.Info("Clear db done");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                connection.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Update and reload start....");
                var obj = new
                {
                    CLINIC_ID = txtClinicID.Text.Trim(),
                    APP_ID = txtAppID.Text.Trim(),
                    HOST = txtHost.Text.Trim(),
                    URL = txtAPIURL.Text.Trim(),
                    AUTOUPDATE = "/openclinic/app/download/update.xml",
                    ISLOCAL = false,
                    LIST = new List<string>()
                };
                var jsonString = JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                log.Info("Update : " + jsonString);
                File.WriteAllText(@"config.json", jsonString);

                log.Info("Reload start ");
                StopConnection();
                LoadConfigLocal();

                StartApplication();
                log.Info("Reload Done");
            }
            catch (Exception ex)
            {
                log.Info("Error write json: " + ex.Message);
            }
        }

        private void StopConnection()
        {
            try
            {
                log.Info("Stop connection ");
                foreach (MachineData client in listMachineData)
                {
                    if (client.MachineType == "TCP")
                    {
                        if (client.MachineObject != null)
                        {
                            client.threadStop = true;
                            if (client.thread != null)
                            {

                            }
                            if (client.MachineBaudrate == "PASSIVE")
                            {
                                EasyTcpServer sp = (EasyTcpServer)client.MachineObject;
                                log.Info("Closed " + sp.ToString());

                                sp.OnDisconnect -= _EasyTcpClient_OnDisconnect;
                                sp.Dispose();
                                sp = null;
                            }
                            else
                            {
                                EasyTcpClient tcpClient = (EasyTcpClient)client.MachineObject;
                                log.Info("Closed " + tcpClient.ToString());
                                tcpClient.OnDisconnect -= _EasyTcpClient_OnDisconnect;
                                tcpClient.Dispose();
                                tcpClient = null;
                            }

                        }
                    }
                    else if (client.MachineType == "RS232")
                    {
                        try
                        {
                            if (client.MachineObject != null)
                            {
                                SerialPort _SerialPort = (SerialPort)client.MachineObject;
                                log.Info("Closed RS232: " + _SerialPort.ToString());
                                _SerialPort.Close();
                                _SerialPort = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Info("ERROR: Close RS232: " + client.MachineName + " | " + client.MachineCOM + " " + client.MachineBaudrate + " " + ex.Message);
                        }

                    }
                }

                listMachineData.Clear();
                log.Info("Clear all listMachineData list");
                dataGridView1.DataSource = null;
            }
            catch (Exception ex)
            {
                log.Info("Error StopConnection: " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "logs/log.log");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenUrl(HOSTNAME + "/openclinic/app/logs/eqpdata.txt");
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("//MachineBaudrate = 9600,0,8,1\r\n//public enum Parity => None = 0,Odd = 1,Even = 2,Mark = 3,Space = 4\r\n//public enum StopBits => None = 0,One = 1,Two = 2,OnePointFive = 3\r\n//public enum databits => 5->8\r\n");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            using (var connection = new SQLiteConnection("Data Source=hello.db"))
            {
                connection.Open();
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = @"delete from machine_data";
                    command.ExecuteNonQuery();
                    log.Info("Clear db done");
                }
                catch (Exception ex)
                {
                    log.Info(ex.Message);
                }

                connection.Close();
            }
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "logs/log.log";
                string uploadUrl = PostURL + "?f=uploadlog";
                await Upload(uploadUrl, filePath);
                MessageBox.Show("Upload done");
            }
            catch (Exception ee)
            {
                log.Error(ee);
            }
        }

        //private async Task<System.IO.Stream> Upload(string actionUrl, string paramString, Stream paramFileStream, byte[] paramFileBytes)
        private async Task<System.IO.Stream> Upload(string actionUrl, string filePath)
        {
            try
            {
                //HttpContent stringContent = new StringContent(paramString);
                //HttpContent fileStreamContent = new StreamContent(paramFileStream);
                HttpContent fileStreamContent = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                //HttpContent bytesContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                using (var client = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    //formData.Add(stringContent, "param1", "param1");
                    formData.Add(fileStreamContent, "fileToUpload", "log");
                    //formData.Add(bytesContent, "fileToUpload", "log");
                    var response = await client.PostAsync(actionUrl, formData);
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    return await response.Content.ReadAsStreamAsync();
                }
            }
            catch (Exception eee)
            {
                log.Error(eee);
            }
            return null;
        }
    }
}