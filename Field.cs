using System;
using System.Collections.Generic;
using System.Linq;
using HL7.Dotnetcore;

namespace HL7.Dotnetcore
{
    public class Field : MessageElement
    {
        private List<Field> _RepetitionList;

        internal ComponentCollection ComponentList { get; set; }

        public bool IsComponentized { get; set; } = false;
        public bool HasRepetitions { get; set; } = false;
        public bool IsDelimitersField { get; set; } = false;

        internal List<Field> RepetitionList
        {
            get
            {
                if (_RepetitionList == null)
                    _RepetitionList = new List<Field>();

                return _RepetitionList;
            }
            set
            {
                _RepetitionList = value;
            }
        }

        protected override void ProcessValue()
        {
            if (IsDelimitersField)  // Special case for the delimiters fields (MSH)
            {
                var subcomponent = new SubComponent(_value, Encoding);

                ComponentList = new ComponentCollection();
                Component component = new Component(Encoding, true);

                component.SubComponentList.Add(subcomponent);

                ComponentList.Add(component);
                return;
            }

            HasRepetitions = _value.Contains(Encoding.RepeatDelimiter);

            if (HasRepetitions)
            {
                _RepetitionList = new List<Field>();
                List<string> individualFields = MessageHelper.SplitString(_value, Encoding.RepeatDelimiter);

                for (int index = 0; index < individualFields.Count; index++)
                {
                    Field field = new Field(individualFields[index], Encoding);
                    _RepetitionList.Add(field);
                }
            }
            else
            {
                List<string> allComponents = MessageHelper.SplitString(_value, Encoding.ComponentDelimiter);

                ComponentList = new ComponentCollection();

                foreach (string strComponent in allComponents)
                {
                    Component component = new Component(Encoding);
                    component.Value = strComponent;
                    ComponentList.Add(component);
                }

                IsComponentized = ComponentList.Count > 1;
            }
        }

        public Field(HL7Encoding encoding)
        {
            ComponentList = new ComponentCollection();
            Encoding = encoding;
        }

        public Field(string value, HL7Encoding encoding)
        {
            ComponentList = new ComponentCollection();
            Encoding = encoding;
            Value = value;
        }

        public bool AddNewComponent(Component com)
        {
            try
            {
                ComponentList.Add(com);
                return true;
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Unable to add new component Error - " + ex.Message);
            }
        }

        public bool AddNewComponent(Component component, int position)
        {
            try
            {
                ComponentList.Add(component, position);
                return true;
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Unable to add new component Error - " + ex.Message);
            }
        }

        public Component Components(int position)
        {
            position = position - 1;

            try
            {
                return ComponentList[position];
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Component not available Error - " + ex.Message);
            }
        }

        public List<Component> Components()
        {
            return ComponentList;
        }

        public List<Field> Repetitions()
        {
            if (HasRepetitions)
            {
                return RepetitionList;
            }
            return null;
        }

        public Field Repetitions(int repetitionNumber)
        {
            if (HasRepetitions)
            {
                return RepetitionList[repetitionNumber - 1];
            }
            return null;
        }

        public bool RemoveEmptyTrailingComponents()
        {
            try
            {
                for (var eachComponent = ComponentList.Count - 1; eachComponent >= 0; eachComponent--)
                {
                    if (ComponentList[eachComponent].Value == "")
                        ComponentList.Remove(ComponentList[eachComponent]);
                    else
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HL7Exception("Error removing trailing comonents - " + ex.Message);
            }
        }
        public void AddRepeatingField(Field field)
        {
            if (!HasRepetitions)
            {
                throw new HL7Exception("Repeating field must have repetions (HasRepetitions = true)");
            }
            if (_RepetitionList == null)
            {
                _RepetitionList = new List<Field>();
            }
            _RepetitionList.Add(field);
        }
    }
}
