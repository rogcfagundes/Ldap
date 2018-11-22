using Moq;
using Novell.Directory.Ldap;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement.Core.Internal.Filters;
using Xunit;

namespace System.DirectoryServices.AccountManagement.Core.Tests
{
    public class IntegrationUnitTest
    {        
        private const String DOMAIN_CONTROLLER = "yourserver_or_domain.com";
        private const String PRINCIPAL = "CN=your user name,OU=ouvalue,DC=dcvalue,DC=dcvalue";
        private const String CREDENTIAL = "your user name password";
        private const String VALID_GROUPNAME = "group name";
        private const String VALID_GROUPNAME2 = "group name";
        private const String VALID_USERNAME = "user samaccountname";
        private const String VALID_USERNAME_PASSWORD = "your user name password";
        private const String INVALID_USERNAME = "ScoobyDoo";
        private const String INVALID_USERNAME_PASSWORD = "";
        private const String USER_MEMEBEROF = "user samaccountname";
        private const String VALID_AMERICAS_USERNAME = "user samaccountname";
        private const String VALID_AMERICAS_USERNAME_PASSWORD = "your user name password";
        private const String VALID_EUROPE_USERNAME = "user samaccountname";
        private const String VALID_EUROPE_USERNAME_PASSWORD = "your user name password";
        private const String VALID_ASIA_USERNAME = "user samaccountname";
        private const String VALID_ASIA_USERNAME_PASSWORD = "your user name password";
        private static Mock<LdapAttributeSet> attributes;
        private static Mock<LdapEntry> entry;
        private static IPrincipalContext context;
        private static Mock<LdapConnection> ldapConnection;
        private static Mock<LdapSearchResults> searchResults;

        private static IPrincipalContext BuildPrincipalContext()
        {
            IPrincipalContext principalContext = new PrincipalContext(DOMAIN_CONTROLLER, PRINCIPAL, CREDENTIAL);
            return principalContext;
        }
        
        private void SetupMock()
        {
            attributes = new Mock<LdapAttributeSet>();
            entry = new Mock<LdapEntry>();
            searchResults = new Mock<LdapSearchResults>();
            ldapConnection = new Mock<LdapConnection>();
            ldapConnection.Setup(c => c.Connected).Returns(true);
            ldapConnection.Setup(c => c.Bound).Returns(true);
            PrincipalContext principalContext = new PrincipalContext("container", "principal", "credential");
            principalContext.Connection = ldapConnection.Object;
            context = principalContext;
        }

        [Fact]
        public void AmericasValidateValidCredentials()
        {
            using (IPrincipalContext _principalContext = BuildPrincipalContext())
            {
                bool isValid = _principalContext.ValidateCredentials(VALID_AMERICAS_USERNAME, VALID_AMERICAS_USERNAME_PASSWORD);
                Assert.True(isValid);
            }
        }
        [Fact]
        public void EuropeValidateValidCredentials()
        {
            using (IPrincipalContext _principalContext = BuildPrincipalContext())
            {
                bool isValid = _principalContext.ValidateCredentials(VALID_EUROPE_USERNAME, VALID_EUROPE_USERNAME_PASSWORD);
                Assert.True(isValid);
            }
        }
        [Fact]
        public void AsiaValidateValidCredentials()
        {
            using (IPrincipalContext _principalContext = BuildPrincipalContext())
            {
                bool isValid = _principalContext.ValidateCredentials(VALID_ASIA_USERNAME, VALID_ASIA_USERNAME_PASSWORD);
                Assert.True(isValid);
            }
        }
        [Fact]
        public void AmericasUserPrincipalFindByIdentity()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IUserPrincipal userPrincipal = principalContext.FindUserByIdentity(VALID_AMERICAS_USERNAME))
                {
                    Assert.NotNull(userPrincipal);
                    Assert.NotNull(userPrincipal.Context);                    
                    Assert.NotNull(userPrincipal.Description);
                    Assert.NotNull(userPrincipal.DisplayName);
                    Assert.NotNull(userPrincipal.DistinguishedName);
                    Assert.NotNull(userPrincipal.Guid);
                    Assert.NotNull(userPrincipal.Name);
                    Assert.NotNull(userPrincipal.SamAccountName);
                    Assert.NotNull(userPrincipal.StructuralObjectClass);
                    Assert.NotNull(userPrincipal.UserPrincipalName);
                }
            }
        }
        [Fact]
        public void EuropeUserPrincipalFindByIdentity()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IUserPrincipal userPrincipal = principalContext.FindUserByIdentity(VALID_EUROPE_USERNAME))
                {
                    Assert.NotNull(userPrincipal);
                    Assert.NotNull(userPrincipal.Context);                    
                    Assert.NotNull(userPrincipal.Description);
                    Assert.NotNull(userPrincipal.DisplayName);
                    Assert.NotNull(userPrincipal.DistinguishedName);
                    Assert.NotNull(userPrincipal.Guid);
                    Assert.NotNull(userPrincipal.Name);
                    Assert.NotNull(userPrincipal.SamAccountName);
                    Assert.NotNull(userPrincipal.StructuralObjectClass);
                    Assert.NotNull(userPrincipal.UserPrincipalName);
                }
            }
        }
        [Fact]
        public void AsiaUserPrincipalFindByIdentity()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IUserPrincipal userPrincipal = principalContext.FindUserByIdentity(VALID_ASIA_USERNAME))
                {
                    Assert.NotNull(userPrincipal);
                    Assert.NotNull(userPrincipal.Context);                    
                    Assert.NotNull(userPrincipal.Description);
                    Assert.NotNull(userPrincipal.DisplayName);
                    Assert.NotNull(userPrincipal.DistinguishedName);
                    Assert.NotNull(userPrincipal.Guid);
                    Assert.NotNull(userPrincipal.Name);
                    Assert.NotNull(userPrincipal.SamAccountName);
                    Assert.NotNull(userPrincipal.StructuralObjectClass);
                    Assert.NotNull(userPrincipal.UserPrincipalName);
                }
            }
        }
        [Fact]
        public void PrincipalContextConstructor()
        {
            using (IPrincipalContext _principalContext = BuildPrincipalContext())
            {
                Assert.NotNull(_principalContext.Container);                
                Assert.NotNull(_principalContext.UserDistinghuishedName);
                Assert.True(PRINCIPAL.Equals(_principalContext.UserDistinghuishedName, StringComparison.InvariantCultureIgnoreCase));                
                Assert.True(DOMAIN_CONTROLLER.Equals(_principalContext.Container, StringComparison.InvariantCultureIgnoreCase));
            }
        }
        [Fact]
        public void ValidateValidCredentials()
        {
            using (IPrincipalContext _principalContext = BuildPrincipalContext())
            {
                bool isValid = _principalContext.ValidateCredentials(VALID_USERNAME, VALID_USERNAME_PASSWORD);
                Assert.True(isValid);
            }
        }
        [Fact]
        public void ValidateNotExistsCredentials()
        {
            String nonExistingUserPassword = String.Empty;
            using (IPrincipalContext _principalContext = BuildPrincipalContext())
            {
                bool isValid = _principalContext.ValidateCredentials(INVALID_USERNAME, INVALID_USERNAME_PASSWORD);
                Assert.False(isValid);
            }
        }

        [Fact]
        public void ValidateInvalidCredentials()
        {
            using (IPrincipalContext _principalContext = BuildPrincipalContext())
            {
                bool isValid = _principalContext.ValidateCredentials(VALID_USERNAME, INVALID_USERNAME_PASSWORD);
                Assert.False(isValid);
            }
        }
        [Fact]
        public void GroupPrincipalFindByIdentity()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IGroupPrincipal groupPrincipal = principalContext.FindGroupByIdentity(VALID_GROUPNAME))
                {
                    Assert.NotNull(groupPrincipal);
                    Assert.NotNull(groupPrincipal.Context);                    
                    Assert.NotNull(groupPrincipal.Description);
                    Assert.NotNull(groupPrincipal.DisplayName);
                    Assert.NotNull(groupPrincipal.DistinguishedName);
                    Assert.NotNull(groupPrincipal.Guid);
                    Assert.NotNull(groupPrincipal.Name);
                    Assert.NotNull(groupPrincipal.SamAccountName);
                    Assert.NotNull(groupPrincipal.StructuralObjectClass);
                    Assert.NotNull(groupPrincipal.UserPrincipalName);
                }
            }
        }
        [Fact]
        public void GroupPrincipalGetAuthorizationGroups()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IGroupPrincipal groupPrincipal = principalContext.FindGroupByIdentity(VALID_GROUPNAME))
                {
                    Assert.NotNull(groupPrincipal);
                    IEnumerable<IPrincipal> groups = groupPrincipal.GetGroups();
                    Assert.Null(groups);
                }
            }
        }
        [Fact]
        public void UserPrincipalFindByIdentity()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IUserPrincipal userPrincipal = principalContext.FindUserByIdentity(USER_MEMEBEROF))
                {
                    Assert.NotNull(userPrincipal);
                    Assert.NotNull(userPrincipal.Context);                    
                    Assert.NotNull(userPrincipal.Description);
                    Assert.NotNull(userPrincipal.DisplayName);
                    Assert.NotNull(userPrincipal.DistinguishedName);
                    Assert.NotNull(userPrincipal.Guid);
                    Assert.NotNull(userPrincipal.Name);
                    Assert.NotNull(userPrincipal.SamAccountName);
                    Assert.NotNull(userPrincipal.StructuralObjectClass);
                    Assert.NotNull(userPrincipal.UserPrincipalName);
                }
            }
        }
        [Fact]
        public void GroupPrincipalIsMemberOf()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IGroupPrincipal groupPrincipal = principalContext.FindGroupByIdentity(VALID_GROUPNAME))
                {
                    Assert.NotNull(groupPrincipal);
                    Assert.NotNull(((GroupPrincipal)groupPrincipal).ContextRaw);
                    using (IGroupPrincipal groupPrincipal2 = principalContext.FindGroupByIdentity(VALID_GROUPNAME2))
                    {
                        Assert.NotNull(groupPrincipal2);
                        bool isMemeberOf = groupPrincipal2.IsMemberOf(groupPrincipal);
                        Assert.False(isMemeberOf);
                    }
                }
            }
        }
        [Fact]
        public void UserPrincipalIsMemberOf()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IGroupPrincipal groupPrincipal = principalContext.FindGroupByIdentity(VALID_GROUPNAME))
                {
                    Assert.NotNull(groupPrincipal);
                    using (IUserPrincipal userPrincipal = principalContext.FindUserByIdentity(USER_MEMEBEROF))
                    {
                        Assert.NotNull(userPrincipal);
                        bool isMemeberOf = userPrincipal.IsMemberOf(groupPrincipal);
                        Assert.True(isMemeberOf);
                    }
                }
            }
        }

        [Fact]
        public void UserPrincipalGetAuthorizationGroups()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IUserPrincipal userPrincipal = principalContext.FindUserByIdentity(USER_MEMEBEROF))
                {
                    Assert.NotNull(userPrincipal);
                    IEnumerable<IPrincipal> groups = userPrincipal.GetAuthorizationGroups();
                    Assert.NotNull(groups);
                }

            }
        }
        [Fact]
        public void UserPrincipalIsNotMemberOf()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IGroupPrincipal groupPrincipal = principalContext.FindGroupByIdentity(VALID_GROUPNAME))
                {
                    Assert.NotNull(groupPrincipal);
                    using (IUserPrincipal userPrincipal = principalContext.FindUserByIdentity(VALID_USERNAME))
                    {
                        Assert.NotNull(userPrincipal);
                        bool isMemeberOf = userPrincipal.IsMemberOf(groupPrincipal);
                        Assert.False(isMemeberOf);
                    }
                }
            }
        }
        [Fact]
        public void GroupPrincipalMembers()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IGroupPrincipal groupPrincipal = principalContext.FindGroupByIdentity(VALID_GROUPNAME))
                {
                    Assert.NotNull(groupPrincipal);
                    IEnumerable<IPrincipal> collection = groupPrincipal.Members;
                    Assert.NotNull(collection);
                    int counter = 0;
                    foreach (IPrincipal principal in collection)
                    {
                        Assert.NotNull(principal);
                        Assert.NotNull(principal.DistinguishedName);
                        counter += 1;
                    }
                    Assert.True(counter > 0);
                }
            }
        }
        [Fact]
        public void UserPricipalGetAuthorizationGroups()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IUserPrincipal userPrincipal = principalContext.FindUserByIdentity(USER_MEMEBEROF))
                {
                    Assert.NotNull(userPrincipal);
                    IEnumerable<IPrincipal> groups = userPrincipal.GetGroups();
                    Assert.NotNull(groups);
                    int counter = 0;
                    foreach (IPrincipal group in groups)
                    {
                        IGroupPrincipal _principal = group as IGroupPrincipal;
                        Assert.NotNull(_principal);
                        Assert.NotNull(_principal.DistinguishedName);
                        counter += 1;
                        break;
                    }
                    Assert.True(counter > 0);
                }
            }
        }
        [Fact]
        public void PrincipalFindGroupByIdentity()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IPrincipal principal = principalContext.FindByIdentity(VALID_GROUPNAME))
                {
                    //principal is GroupPrincipal
                    IGroupPrincipal _principal = principal as IGroupPrincipal;
                    Assert.NotNull(_principal);
                    Assert.NotNull(_principal.Context);
                    Assert.NotNull(_principal.Description);
                    Assert.NotNull(_principal.DisplayName);
                    Assert.NotNull(_principal.DistinguishedName);
                    Assert.NotNull(_principal.Guid);
                    Assert.NotNull(_principal.Name);
                    Assert.NotNull(_principal.SamAccountName);
                    Assert.NotNull(_principal.StructuralObjectClass);
                    Assert.NotNull(_principal.UserPrincipalName);
                }
            }
        }
        [Fact]
        public void PrincipalFindUserByIdentity()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IPrincipal principal = principalContext.FindByIdentity(VALID_USERNAME))
                {                    
                    IUserPrincipal _principal = principal as IUserPrincipal;
                    Assert.NotNull(_principal);
                    Assert.NotNull(_principal.Context);
                    Assert.NotNull(_principal.Description);
                    Assert.NotNull(_principal.DisplayName);
                    Assert.NotNull(_principal.DistinguishedName);
                    Assert.NotNull(_principal.Guid);
                    Assert.NotNull(_principal.Name);
                    Assert.NotNull(_principal.SamAccountName);
                    Assert.NotNull(_principal.StructuralObjectClass);
                    Assert.NotNull(_principal.UserPrincipalName);
                }
            }
        }
        [Fact]
        public void UserPrincipalRoles()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IUserPrincipal userPrincipal = principalContext.FindUserByIdentity(VALID_USERNAME))
                {
                    Assert.NotNull(userPrincipal);
                    var expected = userPrincipal.Roles;
                    Assert.NotNull(expected);                    
                }
            }
        }
        [Fact]
        public void ConnectionReset()
        {
            using (IPrincipalContext principalContext = BuildPrincipalContext())
            {
                using (IUserPrincipal userPrincipalAmericas = principalContext.FindUserByIdentity(VALID_AMERICAS_USERNAME))
                {
                    Assert.NotNull(userPrincipalAmericas);
                    Assert.NotNull(userPrincipalAmericas.Context);                    
                    Assert.NotNull(userPrincipalAmericas.Description);
                    Assert.NotNull(userPrincipalAmericas.DisplayName);
                    Assert.NotNull(userPrincipalAmericas.DistinguishedName);
                    Assert.NotNull(userPrincipalAmericas.Guid);
                    Assert.NotNull(userPrincipalAmericas.Name);
                    Assert.NotNull(userPrincipalAmericas.SamAccountName);
                    Assert.NotNull(userPrincipalAmericas.StructuralObjectClass);
                    Assert.NotNull(userPrincipalAmericas.UserPrincipalName);
                }

                using (IUserPrincipal userPrincipalAsia = principalContext.FindUserByIdentity(VALID_ASIA_USERNAME))
                {
                    Assert.NotNull(userPrincipalAsia);
                    Assert.NotNull(userPrincipalAsia.Context);                    
                    Assert.NotNull(userPrincipalAsia.Description);
                    Assert.NotNull(userPrincipalAsia.DisplayName);
                    Assert.NotNull(userPrincipalAsia.DistinguishedName);
                    Assert.NotNull(userPrincipalAsia.Guid);
                    Assert.NotNull(userPrincipalAsia.Name);
                    Assert.NotNull(userPrincipalAsia.SamAccountName);
                    Assert.NotNull(userPrincipalAsia.StructuralObjectClass);
                    Assert.NotNull(userPrincipalAsia.UserPrincipalName);
                }

                using (IUserPrincipal userPrincipalEurope = principalContext.FindUserByIdentity(VALID_EUROPE_USERNAME))
                {
                    Assert.NotNull(userPrincipalEurope);
                    Assert.NotNull(userPrincipalEurope.Context);                    
                    Assert.NotNull(userPrincipalEurope.Description);
                    Assert.NotNull(userPrincipalEurope.DisplayName);
                    Assert.NotNull(userPrincipalEurope.DistinguishedName);
                    Assert.NotNull(userPrincipalEurope.Guid);
                    Assert.NotNull(userPrincipalEurope.Name);
                    Assert.NotNull(userPrincipalEurope.SamAccountName);
                    Assert.NotNull(userPrincipalEurope.StructuralObjectClass);
                    Assert.NotNull(userPrincipalEurope.UserPrincipalName);
                }
            }
        }
        [Fact]
        public void ValidateSSLConnection()
        {
            using (IPrincipalContext principalContext = new PrincipalContext(DOMAIN_CONTROLLER, PRINCIPAL, CREDENTIAL, 3269, true))
            {
                bool isValid = principalContext.ValidateCredentials(VALID_EUROPE_USERNAME, VALID_EUROPE_USERNAME_PASSWORD);
                Assert.True(isValid);
            }
        }
        [Fact]
        public void LdapFilterTests()
        {
            IFilterEncoder encoder = new LdapFilterEncoder();
            EqualsFilter equalsfilter = new EqualsFilter(encoder, "attribute", 0);
            Assert.NotNull(equalsfilter.getEncodedValue());

            AndFilter andfilter = new AndFilter(encoder);
            andfilter.AppenddAll(new List<IFilter>() {
                new EqualsFilter(encoder,"attribute1", 0),
                new EqualsFilter(encoder,"attribute2", "value")
            });
            Assert.NotNull(andfilter.ToString());
            andfilter = null;
            andfilter = new AndFilter(encoder);
            Assert.Null(andfilter.Encode(null));
        }

        [Fact]
        public void ValidateCredentialsFindByIdentity()
        {
            SetupMock();
            GroupPrincipal principal = new GroupPrincipal(context, "mockedName");
            ldapConnection.Setup(c => c.Bound).Returns(true);
            ldapConnection.Setup(c => c.Bind(It.IsAny<String>(), It.IsAny<String>()));
            var expected = context.ValidateCredentials("principal", "credential");
            Assert.True(((PrincipalContext)context).Authenticated);
            Assert.False(expected);
        }

        [Fact]
        public void PrincipalIsMemberOf()
        {            
            SetupMock();
            GroupPrincipal principal = new GroupPrincipal(context, "mockedName");
            ldapConnection.Setup(c => c.Bound).Returns(true);
            ldapConnection.Setup(c => c.Bind(It.IsAny<String>(), It.IsAny<String>())).Throws(new Exception("Mocked Exception"));
            var expected = principal.IsMemberOf("whatever");
            Assert.False(expected);
        }

        [Fact]
        public void LdapEncoderTests()
        {
            IFilterEncoder encoder = new LdapFilterEncoder();
            var expected = encoder.FilterEncode(null);
            Assert.Null(expected);
        }

        [Fact]
        public void TryConnectTests()
        {
            SetupMock();
            ldapConnection.Setup(c => c.Bound).Returns(false);
            ldapConnection.Setup(c => c.Connected).Returns(false);
            ldapConnection.Setup(c => c.Bind(It.IsAny<String>(), It.IsAny<String>())).Verifiable();
            ldapConnection.Setup(c => c.Connect(It.IsAny<String>(), It.IsAny<int>())).Callback(() => {
                if (context.ConnectionRetryCount == 0)
                    throw new Exception("Mocked");
            });
            var expected = context.TryConnect();
            Assert.True(expected);
        }
    }
}
