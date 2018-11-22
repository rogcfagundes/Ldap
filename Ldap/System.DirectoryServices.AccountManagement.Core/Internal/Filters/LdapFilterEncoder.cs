using System.Text;

namespace System.DirectoryServices.AccountManagement.Core.Internal.Filters
{
    public class LdapFilterEncoder: IFilterEncoder
    {
        private const int HEX = 16;
        private static String[] NAME_ESCAPE_TABLE = new String[96];
        private static String[] FILTER_ESCAPE_TABLE = new String['\\' + 1];
        private const int RFC2849_MAX_BASE64_CHARS_PER_LINE = 76;

        static LdapFilterEncoder()
        {
            for (char c = Convert.ToChar(0); c < ((int)' '); c++)
            {
                NAME_ESCAPE_TABLE[c] = "\\" + ToTwoCharHex(c);
            }
            NAME_ESCAPE_TABLE['#'] = "\\#";
            NAME_ESCAPE_TABLE[','] = "\\,";
            NAME_ESCAPE_TABLE[';'] = "\\;";
            NAME_ESCAPE_TABLE['='] = "\\=";
            NAME_ESCAPE_TABLE['+'] = "\\+";
            NAME_ESCAPE_TABLE['<'] = "\\<";
            NAME_ESCAPE_TABLE['>'] = "\\>";
            NAME_ESCAPE_TABLE['\"'] = "\\\"";
            NAME_ESCAPE_TABLE['\\'] = "\\\\";
            // fill with char itself
            for (char c = Convert.ToChar(0); c < FILTER_ESCAPE_TABLE.Length; c++)
            {
                FILTER_ESCAPE_TABLE[c] = Convert.ToString(c);
            }
            // escapes (RFC2254)
            FILTER_ESCAPE_TABLE['*'] = "\\2a";
            FILTER_ESCAPE_TABLE['('] = "\\28";
            FILTER_ESCAPE_TABLE[')'] = "\\29";
            FILTER_ESCAPE_TABLE['\\'] = "\\5c";
            FILTER_ESCAPE_TABLE[0] = "\\00";
        }
        private static String ToTwoCharHex(char c)
        {
            String raw = ((int)c).ToString("X").ToUpper();
            if (raw.Length > 1)
            {
                return raw;
            }
            else
            {
                return "0" + raw;
            }
        }
        public virtual String FilterEncode(String value)
        {
            if (value == null)
                return null;
            // make buffer roomy
            StringBuilder encodedValue = new StringBuilder(value.Length * 2);
            int length = value.Length;
            char[] charArray = value.ToCharArray();
            for (int i = 0; i < length; i++)
            {
                char c = charArray[i];

                if (c < FILTER_ESCAPE_TABLE.Length)
                {
                    encodedValue.Append(FILTER_ESCAPE_TABLE[c]);
                }
                else
                {
                    // default: add the char
                    encodedValue.Append(c);
                }
            }
            return encodedValue.ToString();
        }
    }
}
