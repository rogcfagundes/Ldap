using System.Text;

namespace System.DirectoryServices.AccountManagement.Core.Internal.Filters
{
    public abstract class CompareFilter : AbstractFilter
    {
        private readonly String attribute;
        private readonly String value;
        private readonly String encodedValue;
        public CompareFilter(IFilterEncoder encoder, String attribute, String value)
            : base(encoder)
        {
            this.attribute = attribute;
            this.value = value;
            this.encodedValue = EncodeValue(value);
        }
        public CompareFilter(IFilterEncoder encoder, String attribute, int value) 
            : base(encoder)
        {
            this.attribute = attribute;
            this.value = Convert.ToString(value);
            this.encodedValue = FilterEncoder.FilterEncode(this.value);
        }
        public virtual String getEncodedValue()
        {
            return encodedValue;
        }
        protected virtual String EncodeValue(String value)
        {
            return FilterEncoder.FilterEncode(value);
        }        
        public override StringBuilder Encode(StringBuilder buff)
        {
            buff.Append('(');
            buff.Append(attribute).Append(GetCompareString()).Append(encodedValue);
            buff.Append(')');
            return buff;
        }
        protected abstract String GetCompareString();
    }
}
