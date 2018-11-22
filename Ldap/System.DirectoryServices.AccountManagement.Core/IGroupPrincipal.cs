using System.Collections.Generic;

namespace System.DirectoryServices.AccountManagement.Core
{
    public interface IGroupPrincipal : IPrincipal
    {
        IEnumerable<IPrincipal> Members { get; }
        IEnumerable<IPrincipal> GetMembers();
        IEnumerable<IPrincipal> GetMembers(bool recursive);
    }
}
