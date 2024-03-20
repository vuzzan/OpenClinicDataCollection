namespace HL7.Dotnetcore
{
    public class SubComponent : MessageElement
    {
        public SubComponent(string val, HL7Encoding encoding)
        {
            Encoding = encoding;
            Value = val;
        }

        protected override void ProcessValue()
        {
        }
    }
}
