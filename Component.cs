using System;
using System.Collections.Generic;

namespace HL7.Dotnetcore
{
    public class Component : MessageElement
    {
        internal List<SubComponent> SubComponentList { get; set; }

        public bool IsSubComponentized { get; set; } = false;

        private bool isDelimiter = false;

        public Component(HL7Encoding encoding, bool isDelimiter = false)
        {
            this.isDelimiter = isDelimiter;
            SubComponentList = new List<SubComponent>();
            Encoding = encoding;
        }
        public Component(string pValue, HL7Encoding encoding)
        {
            SubComponentList = new List<SubComponent>();
            Encoding = encoding;
            Value = pValue;
        }

        protected override void ProcessValue()
        {
            List<string> allSubComponents;

            if (isDelimiter)
                allSubComponents = new List<string>(new[] { Value });
            else
                allSubComponents = MessageHelper.SplitString(_value, Encoding.SubComponentDelimiter);

            if (allSubComponents.Count > 1)
                IsSubComponentized = true;

            SubComponentList = new List<SubComponent>();

            foreach (string strSubComponent in allSubComponents)
            {
                SubComponent subComponent = new SubComponent(Encoding.Decode(strSubComponent), Encoding);
                SubComponentList.Add(subComponent);
            }
        }

        public SubComponent SubComponents(int position)
        {
            position = position - 1;

            try
            {
                return SubComponentList[position];
            }
            catch (Exception ex)
            {
                throw new HL7Exception("SubComponent not availalbe Error-" + ex.Message);
            }
        }

        public List<SubComponent> SubComponents()
        {
            return SubComponentList;
        }
    }
}
