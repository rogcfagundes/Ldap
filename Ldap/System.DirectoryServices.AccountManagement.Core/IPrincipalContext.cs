using System.DirectoryServices.AccountManagement.Core.Internal.Filters;

namespace System.DirectoryServices.AccountManagement.Core
{
    public interface IPrincipalContext : IDisposable
    {
        IGroupPrincipal FindGroupByIdentity(string identityValue);
        IUserPrincipal FindUserByIdentity(string identityValue);
        IPrincipal FindByIdentity(string identityValue);
        IFilterEncoder FilterEncoder { get; set; }
        int ConnectionRetryCount { get; }
        string Container { get; }
        string UserDistinghuishedName { get; }
        string UserDistinghuishedNameCredential { get; }
        int Port { get; }
        bool ValidateCredentials(string samAccountName, string credential);
        bool TryConnect();
        bool Connected { get; }
    }
}
