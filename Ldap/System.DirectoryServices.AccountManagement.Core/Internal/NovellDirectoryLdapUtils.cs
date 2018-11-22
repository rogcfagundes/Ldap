using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement.Core.Internal.Filters;

namespace System.DirectoryServices.AccountManagement.Core.Internal
{
    public static class NovellDirectoryLdapUtils
    {

        public static readonly String[] ATTRIBUTES =
        {
            LdapConstants.OBJECTGUID,
            LdapConstants.USERPRINCIPALNAME,
            LdapConstants.SAMACCOUNTNAME,
            LdapConstants.DISPLAYNAME,
            LdapConstants.DESCRIPTION,
            LdapConstants.DISTINGUISHEDNAME,
            LdapConstants.OBJECTCLASS,
            LdapConstants.NAME,
            LdapConstants.MAIL,
            LdapConstants.MANAGER,
            LdapConstants.TITLE,
            LdapConstants.MEMBEROF,
            LdapConstants.COMMENT
        };

        public static IEnumerable<IPrincipal> GetMembers(IFilterEncoder encoder, GroupPrincipal principal, ILogger logger = null)
        {
            logger?.LogDebug($"");
            principal.Context.TryConnect();
            LdapConnection connection = ((PrincipalContext)principal.Context).Connection;
            AndFilter andFilter = new AndFilter(encoder);
            andFilter.And(new EqualsFilter(encoder, LdapConstants.OBJECTCLASS, LdapConstants.GROUP));
            andFilter.And(new EqualsFilter(encoder, LdapConstants.SAMACCOUNTNAME, principal.SamAccountName));
            String searchFilter = andFilter.ToString();
            logger?.LogDebug($"");
            LdapSearchResults result = connection.Search(LdapConstants.BASEDN, LdapConnection.SCOPE_SUB, searchFilter, new string[] { LdapConstants.MEMBER }, false);
            if (result.HasMore())
            {
                logger?.LogDebug($"");
                while (result.HasMore())
                {
                    LdapEntry entry = result.Next();
                    LdapAttributeSet attributes = entry.getAttributeSet();
                    String[] groupDNArray = GetAttributeArray(attributes, LdapConstants.MEMBER, logger);
                    foreach (String groupDN in groupDNArray)
                    {
                        //search by each group by their distinguished name
                        Principal foundPrincipal = FindByIdentity(encoder, ((PrincipalContext)principal.Context), groupDN, PrincipalType.User, logger, LdapConstants.DISTINGUISHEDNAME);
                        if (foundPrincipal != null)
                            yield return foundPrincipal;
                    }
                    break;
                }
            }
        }
        public static IEnumerable<IPrincipal> GetAuthorizationGroups(IFilterEncoder encoder, UserPrincipal principal, ILogger logger = null)
        {
            logger?.LogDebug($"");
            principal.Context.TryConnect();
            LdapConnection connection = ((PrincipalContext)principal.Context).Connection;
            AndFilter andFilter = new AndFilter(encoder);
            andFilter.And(new EqualsFilter(encoder, LdapConstants.OBJECTCLASS, LdapConstants.PERSON));
            andFilter.And(new EqualsFilter(encoder, LdapConstants.SAMACCOUNTNAME, principal.SamAccountName));
            String searchFilter = andFilter.Encode();
            logger?.LogDebug($"");
            LdapSearchResults result = connection.Search(LdapConstants.BASEDN, LdapConnection.SCOPE_SUB, searchFilter, new string[] { LdapConstants.MEMBEROF }, false);
            if (result.HasMore())
            {
                logger?.LogDebug($"");
                while (result.HasMore())
                {
                    LdapEntry entry = result.Next();
                    LdapAttributeSet attributes = entry.getAttributeSet();
                    String[] groupDNArray = GetAttributeArray(attributes, LdapConstants.MEMBEROF, logger);
                    foreach (String groupDN in groupDNArray)
                    {
                        //search by each group by their distinguished name
                        Principal foundPrincipal = FindByIdentity(encoder, ((PrincipalContext)principal.Context), groupDN, PrincipalType.Group, logger, LdapConstants.DISTINGUISHEDNAME);
                        if (foundPrincipal != null)
                            yield return foundPrincipal;
                    }
                    break;
                }
            }
        }
        public static IEnumerable<String> GetGroups(IFilterEncoder encoder, IUserPrincipal principal, ILogger logger = null)
        {
            logger?.LogDebug($"");
            return GetGroups(encoder, principal.Context, principal.SamAccountName, logger);
        }
        public static IEnumerable<String> GetGroups(IFilterEncoder encoder, IPrincipalContext context, string samAccountName, ILogger logger = null)
        {
            PrincipalContext principalContext = context as PrincipalContext;
            logger?.LogDebug($"");
            principalContext.TryConnect();
            LdapConnection connection = principalContext.Connection;
            AndFilter andFilter = new AndFilter(encoder);
            andFilter.And(new EqualsFilter(encoder, LdapConstants.OBJECTCLASS, LdapConstants.PERSON));
            andFilter.And(new EqualsFilter(encoder, LdapConstants.SAMACCOUNTNAME, samAccountName));
            String searchFilter = andFilter.Encode();
            logger?.LogDebug($"");
            LdapSearchResults result = connection.Search(LdapConstants.BASEDN, LdapConnection.SCOPE_SUB, searchFilter, new string[] { LdapConstants.MEMBEROF }, false);
            logger?.LogDebug($"");
            while (result.HasMore())
            {
                LdapEntry entry = result.Next();
                LdapAttributeSet attributes = entry.getAttributeSet();
                String[] groupDNArray = GetAttributeArray(attributes, LdapConstants.MEMBEROF, logger);
                foreach (String groupDN in groupDNArray)
                {
                    yield return groupDN;
                }
            }
        }
        public static bool IsMemberOf(IFilterEncoder encoder, GroupPrincipal group, string identityValue, ILogger logger = null)
        {
            try
            {
                logger?.LogDebug($"");
                group.Context.TryConnect();
                LdapConnection connection = ((PrincipalContext)group.Context).Connection;
                logger?.LogDebug($"");
                AndFilter andFilter = new AndFilter(encoder);
                andFilter.And(new EqualsFilter(encoder, LdapConstants.OBJECTCLASS, LdapConstants.PERSON));
                andFilter.And(new EqualsFilter(encoder, LdapConstants.SAMACCOUNTNAME, identityValue));
                andFilter.And(new EqualsFilter(encoder, LdapConstants.MEMBEROF, group.DistinguishedName));
                String searchFilter = andFilter.Encode();
                logger?.LogDebug($"");
                LdapSearchResults result = connection.Search(LdapConstants.BASEDN, LdapConnection.SCOPE_SUB, searchFilter, new string[] { LdapConstants.SAMACCOUNTNAME }, false);
                logger?.LogDebug($"");
                return result.HasMore();
            }
            catch (Exception e)
            {
                logger?.LogDebug($"", e);
                throw new DirectoryServiceException(group.Context, e.Message, e);
            }
        }
        public static Principal FindByIdentity(IFilterEncoder encoder, PrincipalContext context, string identityValue, PrincipalType type, ILogger logger = null, String searchBy = LdapConstants.SAMACCOUNTNAME)
        {
            Principal findResult = null;
            try
            {
                logger?.LogDebug($"");
                context.TryConnect();
                LdapConnection connection = context.Connection;
                AndFilter andFilter = new AndFilter(encoder);
                switch (type)
                {
                    case PrincipalType.Group:
                        logger?.LogDebug($"");
                        andFilter.And(new EqualsFilter(encoder, LdapConstants.OBJECTCLASS, LdapConstants.GROUP));
                        break;
                    case PrincipalType.User:
                        logger?.LogDebug($"");
                        andFilter.And(new EqualsFilter(encoder, LdapConstants.OBJECTCLASS, LdapConstants.USER));
                        break;
                }
                andFilter.And(new EqualsFilter(encoder, searchBy, identityValue));
                String searchFilter = andFilter.Encode();
                logger?.LogDebug($"");
                LdapSearchResults result = connection.Search(LdapConstants.BASEDN, LdapConnection.SCOPE_SUB, searchFilter, ATTRIBUTES, false);
                while (result.HasMore())
                {
                    logger?.LogDebug($"");
                    LdapEntry entry = result.Next();
                    LdapAttributeSet attributes = entry.getAttributeSet();
                    String[] objectClassArray = GetAttributeArray(attributes, LdapConstants.OBJECTCLASS, logger);
                    foreach (String objectClass in objectClassArray)
                    {
                        logger?.LogDebug($"");
                        if (LdapConstants.GROUP.Equals(objectClass, StringComparison.InvariantCultureIgnoreCase))
                        {
                            logger?.LogDebug($"");
                            findResult = new GroupPrincipal(context, identityValue);
                            findResult.StructuralObjectClass = LdapConstants.GROUP;
                            break;
                        }
                        else if (LdapConstants.USER.Equals(objectClass, StringComparison.InvariantCultureIgnoreCase))
                        {
                            logger?.LogDebug($"");
                            findResult = new UserPrincipal(context, identityValue);
                            findResult.StructuralObjectClass = LdapConstants.USER;
                            break;
                        }
                    }
                    logger?.LogDebug($"");
                    findResult.LdapEntry = entry;
                    findResult.Guid = GetGuidAttribute(attributes, LdapConstants.OBJECTGUID, logger);
                    findResult.UserPrincipalName = GetAttribute(attributes, LdapConstants.USERPRINCIPALNAME, logger);
                    findResult.SamAccountName = GetAttribute(attributes, LdapConstants.SAMACCOUNTNAME, logger);
                    findResult.DisplayName = GetAttribute(attributes, LdapConstants.DISPLAYNAME, logger);
                    findResult.Description = GetAttribute(attributes, LdapConstants.DESCRIPTION, logger);
                    findResult.DistinguishedName = GetAttribute(attributes, LdapConstants.DISTINGUISHEDNAME, logger);
                    findResult.Name = GetAttribute(attributes, LdapConstants.NAME, logger);
                    findResult.Mail = GetAttribute(attributes, LdapConstants.MAIL, logger);
                    findResult.Manager = GetAttribute(attributes, LdapConstants.MANAGER, logger);
                    findResult.Title = GetAttribute(attributes, LdapConstants.TITLE, logger);
                    findResult.MemberOf = GetAttributeArray(attributes, LdapConstants.MEMBEROF, logger);
                    findResult.Comment = GetAttribute(attributes, LdapConstants.COMMENT, logger);
                    findResult.ContextRaw = context;
                    findResult.Context = context;
                    if (String.IsNullOrEmpty(findResult.UserPrincipalName))
                    {
                        logger?.LogDebug($"");
                        findResult.UserPrincipalName = findResult.SamAccountName;
                    }
                    break;
                }
            }
            catch (Exception e)
            {
                logger?.LogDebug($"", e);
                throw new DirectoryServiceException(context, e.Message, e);
            }
            return findResult;
        }
        public static String[] GetAttributeArray(LdapAttributeSet attributes, String attributeName, ILogger logger = null)
        {
            String[] value = null;
            try
            {
                if (attributes != null && !attributes.IsEmpty())
                {
                    LdapAttribute attribute = attributes.getAttribute(attributeName);
                    if (attribute != null)
                    {
                        logger?.LogDebug($"");
                        value = attribute.StringValueArray;
                    }
                }
            }
            catch (Exception e)
            {
                logger?.LogDebug($"", e);
                value = null;
            }
            return value;
        }
        public static String GetAttribute(LdapAttributeSet attributes, String attributeName, ILogger logger = null)
        {
            String value = null;
            try
            {
                if (attributes != null && !attributes.IsEmpty())
                {
                    LdapAttribute attribute = attributes.getAttribute(attributeName);
                    if (attribute != null)
                    {
                        logger?.LogDebug($"");
                        value = attribute.StringValue;
                    }
                }
            }
            catch (Exception e)
            {
                logger?.LogDebug($"", e);
                value = null;
            }
            return value;
        }
        public static Guid GetGuidAttribute(LdapAttributeSet attributes, String attributeName, ILogger logger = null)
        {
            Guid value = Guid.Empty;
            try
            {
                if (attributes != null && !attributes.IsEmpty())
                {
                    logger?.LogDebug($"");
                    LdapAttribute attribute = attributes.getAttribute(attributeName);
                    var bytes = (byte[])(attribute.ByteValue as object);
                    value = new Guid(bytes);
                }
            }
            catch (Exception e)
            {
                logger?.LogDebug($"", e);
                value = Guid.Empty;
            }
            return value;
        }
    }
}
