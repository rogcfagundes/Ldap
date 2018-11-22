using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement.Core.Internal;

namespace System.DirectoryServices.AccountManagement.Core
{
    public class GroupPrincipal : Principal, IGroupPrincipal
    {
        public GroupPrincipal(IPrincipalContext context, string samAccountName)
            : base(context)
        {            
            SamAccountName = samAccountName;            
        }
        public virtual IEnumerable<IPrincipal> Members
        {
            get
            {
                _logger?.LogDebug($"");
                return GetMembers();
            }
        }        
        public virtual IEnumerable<IPrincipal> GetMembers()
        {
            _logger?.LogDebug($"");
            return GetMembers(false);
        }
        public virtual IEnumerable<IPrincipal> GetMembers(bool recursive)
        {
            _logger?.LogDebug($""); 
            return NovellDirectoryLdapUtils.GetMembers(FilterEncoder, this, _logger);
        }        
    }
}
