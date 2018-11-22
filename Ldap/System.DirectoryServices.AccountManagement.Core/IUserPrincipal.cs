using System.Collections.Generic;

namespace System.DirectoryServices.AccountManagement.Core
{
    public interface IUserPrincipal : IPrincipal
    {
        IEnumerable<IPrincipal> GetAuthorizationGroups();
        IEnumerable<String> Roles { get; }
    }
}
