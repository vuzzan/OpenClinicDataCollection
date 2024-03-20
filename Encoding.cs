using System;
using System.Linq;
using System.Text;

namespace HL7.Dotnetcore
{
    public class HL7Encoding
    {
        public char FieldDelimiter { get; set; } = '|'; // \F\
        public char ComponentDelimiter { get; set; } = '^'; // \S\
        public char RepeatDelimiter { get; set; } = '~';  // \R\
        public char EscapeCharacter { get; set; } = '\\'; // \E\
        public char SubComponentDelimiter { get; set; } = '&'; // \T\
        public string SegmentDelimiter { get; set; } = "\r";
        public string PresentButNull { get; set; } = "\"\"";
        public string AllDelimiters => "" + FieldDelimiter + ComponentDelimiter + RepeatDelimiter + (EscapeCharacter == (char)0 ? "" : EscapeCharacter.ToString()) + SubComponentDelimiter;

        public HL7Encoding()
        {
        }

        public void EvaluateDelimiters(string delimiters)
        {
            FieldDelimiter = delimiters[0];
            ComponentDelimiter = delimiters[1];
            RepeatDelimiter = delimiters[2];

            if (delimiters[4] == FieldDelimiter)
            {
                EscapeCharacter = (char)0;
                SubComponentDelimiter = delimiters[3];
            }
            else
            {
                EscapeCharacter = delimiters[3];
                SubComponentDelimiter = delimiters[4];
            }
        }

        public void EvaluateSegmentDelimiter(string message)
        {
            string[] delimiters = new[] { "\r\n", "\n\r", "\r", "\n" };

            foreach (var delim in delimiters)
            {
                if (message.Contains(delim))
                {
                    SegmentDelimiter = delim;
                    return;
                }
            }

            throw new HL7Exception("Segment delimiter not found in message", HL7Exception.BAD_MESSAGE);
        }

        // Encoding methods based on https://github.com/elomagic/hl7inspector

        public string Encode(string val)
        {
            if (val == null)
                return PresentButNull;

            if (string.IsNullOrWhiteSpace(val))
                return val;

            var sb = new StringBuilder();

            for (int i = 0; i < val.Length; i++)
            {
                char c = val[i];

                bool continueEncoding = true;
                if (c == '<')
                {
                    continueEncoding = false;
                    // special case <B>
                    if (val.Length >= i + 3 && val[i + 1] == 'B' && val[i + 2] == '>')
                    {
                        sb.Append(EscapeCharacter);
                        sb.Append("H");
                        sb.Append(EscapeCharacter);
                        i += 2; // +1 in loop
                    }
                    // special case </B>
                    else if (val.Length >= i + 4 && val[i + 1] == '/' && val[i + 2] == 'B' && val[i + 3] == '>')
                    {
                        sb.Append(EscapeCharacter);
                        sb.Append("N");
                        sb.Append(EscapeCharacter);
                        i += 3; // +1 in loop
                    }
                    // special case <BR>
                    else if (val.Length >= i + 4 && val[i + 1] == 'B' && val[i + 2] == 'R' && val[i + 3] == '>')
                    {
                        sb.Append(EscapeCharacter);
                        sb.Append(".br");
                        sb.Append(EscapeCharacter);
                        i += 3; // +1 in loop
                    }
                    else
                        continueEncoding = true;
                }

                if (continueEncoding)
                {
                    if (c == ComponentDelimiter)
                    {
                        sb.Append(EscapeCharacter);
                        sb.Append("S");
                        sb.Append(EscapeCharacter);
                    }
                    else if (c == EscapeCharacter)
                    {
                        sb.Append(EscapeCharacter);
                        sb.Append("E");
                        sb.Append(EscapeCharacter);
                    }
                    else if (c == FieldDelimiter)
                    {
                        sb.Append(EscapeCharacter);
                        sb.Append("F");
                        sb.Append(EscapeCharacter);
                    }
                    else if (c == RepeatDelimiter)
                    {
                        sb.Append(EscapeCharacter);
                        sb.Append("R");
                        sb.Append(EscapeCharacter);
                    }
                    else if (c == SubComponentDelimiter)
                    {
                        sb.Append(EscapeCharacter);
                        sb.Append("T");
                        sb.Append(EscapeCharacter);
                    }
                    else if (c == 10 || c == 13) // All other non-visible characters will be preserved
                    {
                        string v = string.Format("{0:X2}", (int)c);

                        if (v.Length % 2 != 0) // make number of digits even, this test would only be needed for values > 0xFF
                            v = "0" + v;

                        sb.Append(EscapeCharacter);
                        sb.Append("X");
                        sb.Append(v);
                        sb.Append(EscapeCharacter);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }

            return sb.ToString();
        }

        public string Decode(string encodedValue)
        {
            if (string.IsNullOrWhiteSpace(encodedValue))
                return encodedValue;

            var result = new StringBuilder();

            for (int i = 0; i < encodedValue.Length; i++)
            {
                char c = encodedValue[i];

                if (c != EscapeCharacter)
                {
                    result.Append(c);
                    continue;
                }

                i++;
                int li = encodedValue.IndexOf(EscapeCharacter, i);

                if (li == -1)
                {
                    // throw new HL7Exception("Invalid escape sequence in HL7 string");
                    result.Append(EscapeCharacter);
                    result.Append(encodedValue[i]);
                    continue;
                }


                string seq = encodedValue.Substring(i, li - i);
                i = li;

                if (seq.Length == 0)
                    continue;

                switch (seq)
                {
                    case "H": // Start higlighting
                        result.Append("<B>");
                        break;
                    case "N": // normal text (end highlighting)
                        result.Append("</B>");
                        break;
                    case "F": // field separator
                        result.Append(FieldDelimiter);
                        break;
                    case "S": // component separator
                        result.Append(ComponentDelimiter);
                        break;
                    case "T": // subcomponent separator
                        result.Append(SubComponentDelimiter);
                        break;
                    case "R": // repetition separator
                        result.Append(RepeatDelimiter);
                        break;
                    case "E": // escape character
                        result.Append(EscapeCharacter);
                        break;
                    case ".br":
                        result.Append("<BR>");
                        break;
                    default:
                        if (seq.StartsWith("X"))
                        {
                            byte[] bytes = Enumerable.Range(0, seq.Length - 1)
                                .Where(x => x % 2 == 0)
                                .Select(x => Convert.ToByte(seq.Substring(x + 1, 2), 16))
                                .ToArray();
                            result.Append(Encoding.UTF8.GetString(bytes));
                        }
                        else
                        {
                            result.Append(seq);
                        }
                        break;
                }
            }

            return result.ToString();
        }
    }
}
