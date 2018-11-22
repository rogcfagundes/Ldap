using System.Text;

namespace System.DirectoryServices.AccountManagement.Core.Internal.Filters
{
    public abstract class AbstractFilter : IFilter
    {
        public AbstractFilter(IFilterEncoder encoder)
        {
            FilterEncoder = encoder;
        }
        public virtual IFilterEncoder FilterEncoder { get; set; }
        public virtual string Encode()
        {
            StringBuilder buf = new StringBuilder(LdapConstants.DEFAULT_BUFFER_SIZE);
            buf = Encode(buf);
            return buf.ToString();
        }
        public abstract StringBuilder Encode(StringBuilder buf);
        public override string ToString()
        {
            return Encode();
        }
    }
}
