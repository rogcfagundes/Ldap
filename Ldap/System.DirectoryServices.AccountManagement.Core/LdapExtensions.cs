using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.DirectoryServices.AccountManagement.Core.Internal;

namespace System.DirectoryServices.AccountManagement.Core
{
    public static class LdapExtensions
    {
        public static IServiceCollection AddLdapConnection(this IServiceCollection services, Func<IServiceProvider, IPrincipalContext> implementationFactory)
        {
            services.AddSingleton<IPrincipalContext>(implementationFactory);
            return services;
        }

        public static IServiceCollection AddLdapConnection(this IServiceCollection services, string configSection = "Ldap:Client")
        {
            services.AddSingleton<IPrincipalContext>((provider) =>
            {
                if (String.IsNullOrEmpty(configSection))
                    throw new ArgumentNullException($"The argument configSection can not be null or empty when AddLdapConnection.");
                var configuration = provider.GetService<IConfiguration>();
                var section = new LdapSection();
                configuration.GetSection(configSection).Bind(section);                
                var context = CreatePrincipalContext(provider, section.container, section.principal, section.credential, section.port, section.UseSSL);
                return context;
            });
            return services;
        }

        public static IServiceCollection AddLdapConnection(this IServiceCollection services, string container, string principal, string credential, int port = LdapConstants.PORT, bool useSSL = false)
        {
            services.AddSingleton<IPrincipalContext>((provider) =>
            {                
                var context = CreatePrincipalContext(provider, container, principal, credential, port, useSSL);
                return context;
            });
            return services;
        }

        private static IPrincipalContext CreatePrincipalContext(IServiceProvider provider, string container, string principal, string credential, int port, bool useSSL)
        {
            if (String.IsNullOrEmpty(container))
                throw new ArgumentNullException($"The argument container can not be null or empty when AddLdapConnection.");
            if (String.IsNullOrEmpty(principal))
                throw new ArgumentNullException($"The argument principal can not be null or empty when AddLdapConnection.");
            if (String.IsNullOrEmpty(credential))
                throw new ArgumentNullException($"The argument credential can not be null or empty when AddLdapConnection.");
            if (port <= 0)
                port = LdapConstants.PORT;
            var logger = provider.GetService<ILoggerFactory>().CreateLogger<PrincipalContext>();
            var context = new PrincipalContext(container, principal, credential, port, useSSL);
            context.TryConnect();
            return context;
        }
        
    } 
}
