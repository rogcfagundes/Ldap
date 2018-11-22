using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement.Core.Internal;

namespace System.DirectoryServices.AccountManagement.Core
{
    public class UserPrincipal : Principal, IUserPrincipal
    {
        public UserPrincipal(IPrincipalContext context, string samAccountName)
            :base(context)
        {
            Context = context;
            SamAccountName = samAccountName;           
        }
        public IEnumerable<string> Roles
        {
            get
            {
                _logger?.LogDebug($"");
                return NovellDirectoryLdapUtils.GetGroups(FilterEncoder, this, _logger);
            }
        }        
        public IEnumerable<IPrincipal> GetAuthorizationGroups()
        {
            _logger?.LogDebug($"");
            return NovellDirectoryLdapUtils.GetAuthorizationGroups(FilterEncoder, this, _logger);
        }
    }
}
