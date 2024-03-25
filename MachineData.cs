using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7.Dotnetcore
{
    public class MachineData
    {
        public string MachineCode { get; set; }
        public string MachineName { get; set; }
        public string MachineType { get; set; }
        public string MachineConnect { 
            get {
                if (MachineType == "TCP")
                {
                    return MachineIP + ":" + MachinePort;
                }
                else
                {
                    return MachineCOM + ":" + MachineBaudrate;
                }
            } 
        }
        public string MachineStatus { get; set; }
        public MachineData() { }
        [Browsable(false)]
        public string DataEndLine { get; set; }
        [Browsable(false)]
        public string MachineIP { get; set; }
        [Browsable(false)]
        public string MachinePort { get; set; }
        [Browsable(false)]
        public string MachineCOM { get; set; }
        [Browsable(false)]
        public string MachineBaudrate { get; set; }
        [Browsable(false)]
        public object MachineObject { get; set; }
        [Browsable(false)]
        public string StringData { get; set; }
        public string ToString()
        {
            if (MachineType == "TCP")
            {
                return "[" + MachineCode + "] [" + MachineName + "] [" + MachineIP + ":" + MachinePort + "] ";
            }
            else
            {
                return "[" + MachineCode + "] [" + MachineName + "] [" + MachineCOM + ":" + MachineBaudrate + "] End="  + DataEndLine;
            }
        }
    }
}
