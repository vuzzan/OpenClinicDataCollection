using HL7.Dotnetcore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenClinicDataCollection;
using Message = HL7.Dotnetcore.Message;
using Newtonsoft.Json;
using System.Data.Entity.Core.Metadata.Edm;

namespace OpenClinicDataCollection
{
    public class Machines
    {
        public string APP_ID { get; set; }
        public string MachineCode { get; set; }
        public string MachineName { get; set; }
        public string Result { get; set; }
        public object JsonData { get; set; }

        dynamic MyDynamic = new System.Dynamic.ExpandoObject();

        List<Dictionary<string, string>> resultData = new List<Dictionary<string, string>>();

        Dictionary<string, string> XML1 = new Dictionary<string, string>();



        public Machines(string appID, string Code, string Name, string result)
        {

            this.APP_ID = Convert.ToString(appID);
            this.MachineCode = Code;
            this.MachineName = Name;
            this.Result = result;
            SetJsonData(Code, Name, result);
            this.JsonData = GetDataJson();
        }

        private object GetDataJson()
        {
            MyDynamic.BN = XML1;
            MyDynamic.DATA = resultData;
            return MyDynamic;
        }

        public void SetJsonData(string name, string code, string result)
        {
            string codeMachine = name.Substring(0, 2);
            switch (codeMachine)
            {
                case "HH":
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
                    break;
                case "NT":
                    break;
            }
        }
    }
}
