using System.DirectoryServices.AccountManagement.Core.Internal;

namespace System.DirectoryServices.AccountManagement.Core
{
    public class LdapSection
    {
        public virtual string container { get; set; }
        public virtual string principal { get; set; }
        public virtual string credential { get; set; }
        public virtual int port { get; set; } = LdapConstants.PORT;
        public virtual bool UseSSL { get; set; } = false;
    }
}