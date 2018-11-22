using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Xunit;

namespace System.DirectoryServices.AccountManagement.Core.Tests
{
    public class LdapExtensionTests
    {
        private const string CONTAINER = "yourserver_or_domain.com";
        private const string PRINCIPAL = "CN=your user name,OU=ouvalue,DC=dcvalue,DC=dcvalue";
        private const string CREDENTIAL = "your user name password";
        private const string CONFIGSECTION = "Ldap:Client";

        [Fact]
        public void AddLdapConnectionVariables()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ILoggerFactory>(new LoggerFactory());
            services.AddLdapConnection(CONTAINER, PRINCIPAL, CREDENTIAL);
            IServiceProvider provider = services.BuildServiceProvider();
            TestConnection(provider);
        }

        [Fact]
        public void AddLdapConnectionConfig()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ILoggerFactory>(new LoggerFactory());
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, false);
            IConfiguration configuration = builder.Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddLdapConnection(CONFIGSECTION);
            IServiceProvider provider = services.BuildServiceProvider();
            TestConnection(provider);
        }

        [Fact]
        public void AddSSLLdapConnectionFactory()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLdapConnection(factory =>
            {
                IPrincipalContext context = new PrincipalContext(CONTAINER, PRINCIPAL, CREDENTIAL, 3269, true);
                context.TryConnect();
                return context;
            });
            IServiceProvider provider = services.BuildServiceProvider();
            TestConnection(provider);
        }

        [Fact]
        public void AddLdapConnectionFactory()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLdapConnection(factory =>
            {
                IPrincipalContext context = new PrincipalContext(CONTAINER, PRINCIPAL, CREDENTIAL);
                context.TryConnect();
                return context;
            });
            IServiceProvider provider = services.BuildServiceProvider();
            TestConnection(provider);
        }

        [Fact]
        public void ExpectedExceptions()
        {
            Exception expected = null;
            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddLdapConnection(null, PRINCIPAL, CREDENTIAL);
                IServiceProvider provider = services.BuildServiceProvider();
                TestConnection(provider);
            }
            catch (Exception e)
            {
                expected = e;
            }
            Assert.NotNull(expected);
            expected = null;
            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddLdapConnection(CONTAINER, null, CREDENTIAL);
                IServiceProvider provider = services.BuildServiceProvider();
                TestConnection(provider);
            }
            catch (Exception e)
            {
                expected = e;
            }
            Assert.NotNull(expected);
            expected = null;
            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddLdapConnection(CONTAINER, PRINCIPAL, null);
                IServiceProvider provider = services.BuildServiceProvider();
                TestConnection(provider);
            }
            catch (Exception e)
            {
                expected = e;
            }
            Assert.NotNull(expected);
            expected = null;
            try
            {
                IServiceCollection services = new ServiceCollection();
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, false);
                IConfiguration configuration = builder.Build();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddLdapConnection(string.Empty);
                IServiceProvider provider = services.BuildServiceProvider();
                TestConnection(provider);
            }
            catch (Exception e)
            {
                expected = e;
            }
            Assert.NotNull(expected);
            expected = null;
            try
            {
                IServiceCollection services = new ServiceCollection();
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, false);
                IConfiguration configuration = builder.Build();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddLdapConnection("Ldap:Client1");
                IServiceProvider provider = services.BuildServiceProvider();
                TestConnection(provider);
            }
            catch (Exception e)
            {
                expected = e;
            }
            Assert.NotNull(expected);
            expected = null;
            try
            {
                IServiceCollection services = new ServiceCollection();
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, false);
                IConfiguration configuration = builder.Build();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddLdapConnection("Ldap:Client2");
                IServiceProvider provider = services.BuildServiceProvider();
                TestConnection(provider);
            }
            catch (Exception e)
            {
                expected = e;
            }
            Assert.NotNull(expected);
            expected = null;
            try
            {
                IServiceCollection services = new ServiceCollection();
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, false);
                IConfiguration configuration = builder.Build();
                services.AddSingleton<IConfiguration>(configuration);
                services.AddLdapConnection("Ldap:Client3");
                IServiceProvider provider = services.BuildServiceProvider();
                TestConnection(provider);
            }
            catch (Exception e)
            {
                expected = e;
            }
            Assert.NotNull(expected);
        }

        private void TestConnection(IServiceProvider provider)
        {
            using (var context = provider.GetService<IPrincipalContext>())
            {
                Assert.True(context.Connected);
            }
        }
    }
}
