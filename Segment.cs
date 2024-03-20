using System;
using System.Collections.Generic;

namespace HL7.Dotnetcore
{
    public class Segment : MessageElement
    {
        internal FieldCollection FieldList { get; set; }
        internal int SequenceNo { get; set; }

        public string Name { get; set; }

        public Segment(HL7Encoding encoding)
        {
            FieldList = new FieldCollection();
            Encoding = encoding;
        }

        public Segment(string name, HL7Encoding encoding)
        {
            FieldList = new FieldCollection();
            Name = name;
            Encoding = encoding;
        }

        protected override void ProcessValue()
        {
            List<string> allFields = MessageHelper.SplitString(_value, Encoding.FieldDelimiter);

            allFields.RemoveAt(0);

            for (int i = 0; i < allFields.Count; i++)
            {
                string strField = allFields[i];
                Field field = new Field(Encoding);

                if (Name == "MSH" && i == 0)
                    field.IsDelimitersField = true; // special case

                field.Value = strField;
                FieldList.Add(field);
            }

            if (Name == "MSH")
            {
                var field1 = new Field(Encoding);
                field1.IsDelimitersField = true;
                field1.Value = Encoding.FieldDelimiter.ToString();

                FieldList.Insert(0, field1);
            }
        }

        public Segment DeepCopy()
        {
            var newSegment = new Segment(Name, Encoding);
            newSegment.Value = Value;

            return newSegment;
        }

        public void AddEmptyField()
        {
            AddNewField(string.Empty);
        }

        public void AddNewField(string content, int position = -1)
        {
            AddNewField(new Field(content, Encoding), position);
        }

        public void AddNewField(string content, bool isDelimiters)
        {
            var newField = new Field(Encoding);

            if (isDelimiters)
                newField.IsDelimitersField = true; // Prevent decoding

            newField.Value = content;
            AddNewField(newField, -1);
        }

        public bool AddNewField(Field field, int position = -1)
        {
            try
            {
                if (position < 0)
                {
                    FieldList.Add(field);
                }
                else
                {
                    position = position - 1;
                    FieldList.Add(field, position);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Unable to add new field in segment " + Name + " Error - " + ex.Message);
            }
        }

        public Field Fields(int position)
        {
            position = position - 1;

            try
            {
                return FieldList[position];
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Field not available Error - " + ex.Message);
            }
        }

        public List<Field> GetAllFields()
        {
            return FieldList;
        }

        public int GetSequenceNo()
        {
            return SequenceNo;
        }
    }
}
