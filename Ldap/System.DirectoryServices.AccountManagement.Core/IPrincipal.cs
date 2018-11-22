using System.Collections.Generic;
using System.DirectoryServices.AccountManagement.Core.Internal.Filters;

namespace System.DirectoryServices.AccountManagement.Core
{
    public interface IPrincipal : IDisposable
    {
        IFilterEncoder FilterEncoder { get; set; }
        Guid? Guid { get; }
        string UserPrincipalName { get; set; }
        string SamAccountName { get; set; }
        string DisplayName { get; set; }
        string Description { get; set; }
        IPrincipalContext Context { get; }
        string DistinguishedName { get; }
        string StructuralObjectClass { get; }
        string Name { get; set; }
        IEnumerable<IPrincipal> GetGroups(IPrincipalContext contextToQuery);
        IEnumerable<IPrincipal> GetGroups();
        bool IsMemberOf(string identityValue);
        bool IsMemberOf(IGroupPrincipal group);
        string Mail { get; set; }
        string Manager { get; set; }
        string Title { get; set; }
        string Comment { get; set; }
        string[] MemberOf { get; set; }
    }
}
