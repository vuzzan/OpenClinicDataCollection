using HL7.Dotnetcore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenClinicDataCollection;
using Newtonsoft.Json;
using System.Data.Entity.Core.Metadata.Edm;
using System.Runtime.ConstrainedExecution;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;

namespace OpenClinicDataCollection
{
    public class Machines
    {
        public string APP_ID { get; set; }
        public string MachineName { get; set; }
        public string Result { get; set; }
        public object JsonData { get; set; }

        List<Dictionary<string, string>> resultData = new List<Dictionary<string, string>>();

        Dictionary<string, string> XML1 = new Dictionary<string, string>();

        public Machines(string appID, string Code, string Name, string result)
        {
            this.APP_ID = Convert.ToString(appID);
            this.MachineName = Name;
            this.Result = result;
            //SetDataJson(Code, result);
            this.JsonData = GetDataJson();
        }

        private object GetDataJson()
        {
            dynamic MyDynamic = new System.Dynamic.ExpandoObject();
            MyDynamic.BN = XML1;
            MyDynamic.DATA = resultData;
            return MyDynamic;
        }

        public void SetDataJson(string name, string macModel, string result, string rawResult)
        {
            string codeMachine = name.Substring(0, 2);
            switch (codeMachine)
            {
                case "HH":
                    if (macModel.ToUpper() == "HH1") {
                        string[] tokens = result.Split('|');

                        for (int i = 0; i < tokens.Length; i++)
                        {

                            if (tokens[i].ToString() == "\r\nPID")
                            {
                                XML1["HOTEN"] = tokens[i + 5];
                            }
                            if (tokens[i].ToString() == "\r\nOBR")
                            {
                                Dictionary<string, string> data = new Dictionary<string, string>();
                                XML1["DATE"] = tokens[i + 14];
                            }
                            else if (tokens[i].ToString() == "\r\nOBX")
                            {
                                if (tokens[i + 2] == "NM")
                                {
                                    Dictionary<string, string> data = new Dictionary<string, string>();
                                    data["Idx"] = tokens[i + 1];
                                    data["XN"] = tokens[i + 4];
                                    data["VAL"] = tokens[i + 5];
                                    data["UNIT"] = tokens[i + 6];
                                    resultData.Add(data);
                                }
                            }
                        }
                    }
                   
                    break;
                case "NT":
                    string[] lines = rawResult.Split("\r\n");// get line

                    if (macModel.ToUpper() == "NT1")
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            //get value of line
                            string[] dataLine = lines[i].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            string[] dataResult = lines[i].Split("\t", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            if (dataLine.Length == 0)
                            {
                                continue;
                            }
                            if (dataLine.Length == 1)
                            {
                                if (dataResult.Length == 1)
                                {
                                    XML1["DATE"] = lines[i];
                                    XML1["HOTEN"] = lines[i + 2];
                                    i = i + 2;
                                    continue;
                                }
                                else
                                {
                                    Dictionary<string, string> data = new Dictionary<string, string>();
                                    data["XN"] = dataResult[0];
                                    data["VAL"] = dataResult[1];
                                    data["UNIT"] = " ";
                                    resultData.Add(data);
                                }
                            }
                        }
                    }
                    else if (macModel.ToUpper() == "NT2")
                    {
                        foreach (string line in lines)
                        {
                            string dataline = line.Trim();
                            string[] valueLine = dataline.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                            if (valueLine.Length > 2)
                            {
                                Dictionary<string, string> data = new Dictionary<string, string>();
                                data["XN"] = valueLine[0];
                                data["VAL"] = valueLine[1];
                                data["UNIT"] = valueLine[2];
                                resultData.Add(data);
                            }
                            if (line.Count(c => c == '=') == 1)
                            {
                                string[] value = dataline.Split('=');
                                string HOTEN = value[1].Remove(value[1].Length - 1);
                                XML1["HOTEN"] = HOTEN;
                            }
                            for (int i = 0; i < valueLine.Length; i++)
                            {
                                if (valueLine[i].Count(c => c == '/') > 1)
                                {
                                    XML1["DATE"] = valueLine[i];
                                    break;
                                }
                            }
                        }
                    }

                    break;
            }
        }
    }
}
