using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement.Core.Internal;
using System.DirectoryServices.AccountManagement.Core.Internal.Filters;

namespace System.DirectoryServices.AccountManagement.Core
{
    public abstract class Principal : IPrincipal
    {
        internal readonly ILogger _logger;
        protected Principal(IPrincipalContext context)
        {
            ContextRaw = context;
            Context = context;
            FilterEncoder = context.FilterEncoder;
            _logger = ((PrincipalContext)context)._logger;
        }
        public virtual IFilterEncoder FilterEncoder { get; set; }
        public virtual Guid? Guid { get; internal set; }
        public virtual string UserPrincipalName { get; set; }
        public virtual string SamAccountName { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Description { get; set; }
        public virtual IPrincipalContext Context { get; internal set; }
        public virtual string DistinguishedName { get; internal set; }
        public virtual string StructuralObjectClass { get; internal set; }
        public virtual string Name { get; set; }
        public virtual IPrincipalContext ContextRaw { get; set; }
        public virtual LdapEntry LdapEntry { get; set; }
        public virtual string Mail { get; set; }
        public virtual string Manager { get; set; }
        public virtual string Title { get; set; }
        public virtual string[] MemberOf { get; set; }
        public virtual string Comment { get; set; }
        
        public virtual IEnumerable<IPrincipal> GetGroups()
        {
            _logger?.LogDebug($"");
            return GetGroups(null);
        }
        public virtual IEnumerable<IPrincipal> GetGroups(IPrincipalContext contextToQuery)
        {
            if (this is IUserPrincipal)
            {
                _logger?.LogDebug($"");
                return NovellDirectoryLdapUtils.GetAuthorizationGroups(FilterEncoder, (UserPrincipal)this, _logger);
            }
            _logger?.LogDebug($"");
            return null;
        }
        
        public virtual bool IsMemberOf(string identityValue)
        {
            bool isMemberOf = false;
            try
            {
                if (this is IGroupPrincipal)
                {
                    _logger?.LogDebug($"");
                    isMemberOf = NovellDirectoryLdapUtils.IsMemberOf(FilterEncoder, (GroupPrincipal)this, identityValue, _logger);
                }                    
            }
            catch (Exception e)
            {
                _logger?.LogError($"", e);
                isMemberOf = false;
            }
            _logger?.LogDebug($"");
            return isMemberOf;
        }
        public virtual bool IsMemberOf(IGroupPrincipal group)
        {
            _logger?.LogDebug($"");
            return NovellDirectoryLdapUtils.IsMemberOf(FilterEncoder, group as GroupPrincipal, SamAccountName, _logger);
        }
        public virtual void Dispose()
        {
            _logger?.LogDebug($"");
            LdapEntry = null;
        }
    }
}
