using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using Polly;
using Polly.Retry;
using System.DirectoryServices.AccountManagement.Core.Internal;
using System.DirectoryServices.AccountManagement.Core.Internal.Filters;

namespace System.DirectoryServices.AccountManagement.Core
{
    public class PrincipalContext : IPrincipalContext
    {
        internal readonly ILogger<PrincipalContext> _logger;
        private bool _disposed;
        private object sync_root = new object();        
        private LdapConnection _connection = null;

        public PrincipalContext(string container, string principalDn, string credential, ILogger<PrincipalContext> logger = null)
            : this(container, principalDn, credential, LdapConstants.PORT, logger)
        {
        }
        public PrincipalContext(string container, string principalDn, string credential, int port, ILogger<PrincipalContext> logger = null)
            : this(container, principalDn, credential, port, false, logger)
        {
        }
        public PrincipalContext(string container, string principalDn, string credential, int port, bool useSSL, ILogger<PrincipalContext> logger = null)
        {            
            Container = container;
            UserDistinghuishedName = principalDn;
            UserDistinghuishedNameCredential = credential;
            Port = port;
            UseSSL = useSSL;
            FilterEncoder = new LdapFilterEncoder();
            _logger = logger;
        }
        public virtual IFilterEncoder FilterEncoder { get; set; }        
        public virtual Boolean Connected { get { return Authenticated; } }
        public virtual string Container { get; protected set; }
        public virtual string UserDistinghuishedName { get; protected set; }
        public virtual String UserDistinghuishedNameCredential { get; set; }
        public virtual int Port { get; set; }
        public virtual int ConnectionRetryCount { get; set; } = 0;
        public virtual bool UseSSL { get; set; } = false;
        public virtual Boolean Authenticated
        {
            get
            {
                _logger?.LogDebug($"");
                return _connection != null && _connection.Connected && _connection.Bound && !_disposed;
            }
        }
        public virtual LdapConnection Connection
        {
            get
            {
                _logger?.LogDebug($"");
                TryConnect();
                return _connection;
            }
            set { _connection = value; }
        }
        public virtual bool ValidateCredentials(string principal, string credential)
        {
            Boolean authenticated = false;
            IPrincipal userPrincipal = null;
            try
            {
                _logger?.LogDebug($"");
                TryConnect();
                _logger?.LogDebug($"");
                userPrincipal = NovellDirectoryLdapUtils.FindByIdentity(FilterEncoder, this, principal, PrincipalType.User, _logger);
                _logger?.LogDebug($"");
                if (userPrincipal != null && !String.IsNullOrEmpty(userPrincipal.DistinguishedName))
                {
                    _logger?.LogDebug($"");
                    using (LdapConnection localConnection = new LdapConnection())
                    {
                        _logger?.LogDebug($"");
                        if (UseSSL)
                        {
                            _logger?.LogDebug($"");
                            localConnection.SecureSocketLayer = true;
                        }
                        _logger?.LogDebug($"");
                        localConnection.Connect(Container, Port);
                        _logger?.LogDebug($"");
                        localConnection.Bind(userPrincipal.DistinguishedName, credential);
                        _logger?.LogDebug($"");
                        authenticated = localConnection.Bound;
                    }
                }
            }
            catch (Exception e)
            {
                _logger?.LogError($"", e);
                Console.Error.WriteLine($"Error while trying to find identity. Error: {e.ToString()}");
                authenticated = false;
            }
            return authenticated;
        }
        public virtual void Connect()
        {            
            if (_connection == null)
            {
                _logger?.LogDebug($"");
                _connection = new LdapConnection();
            }                
            if (UseSSL)
            {
                _logger?.LogDebug($"");
                _connection.SecureSocketLayer = true;
            }                
            if (!_connection.Connected)
            {
                _logger?.LogDebug($"");
                _connection.Connect(Container, Port);
            }
                
            if (!_connection.Bound)
            {
                _logger?.LogDebug($"");
                _connection.Bind(UserDistinghuishedName, UserDistinghuishedNameCredential);
            }                
        }
        public virtual void Disconnect()
        {
            try
            {
                if (_connection != null)
                {
                    
                    if (_connection.Connected || _connection.Bound)
                    {
                        _logger?.LogDebug($"");
                        _connection.Disconnect();
                    }                        
                }
            }
            catch (Exception e)
            {
                _logger?.LogError($"", e);
                Console.Error.WriteLine($"Error while disconnecting. Error: {e.ToString()}");
            }
        }
        public virtual bool TryConnect()
        {
            ConnectionRetryCount = 0;
            bool connected = false;
            lock (sync_root)
            {
                var policy = RetryPolicy
                    .Handle<Exception>()
                    .WaitAndRetryForever((retryAttempt, ex, time) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    , (ex, attempt, time, context) =>
                    {
                        _logger?.LogDebug($"");
                        ConnectionRetryCount += 1;
                        _logger?.LogDebug($"");
                        Disconnect();
                        _logger?.LogError($"", ex);
                        Console.Error.WriteLine($"An Failure had ocurred estabilishing a connection '{ex.Message}', retrying again in '{time.ToString()}'.", ex.ToString());
                    }
                );
                policy.Execute(() =>
                {
                    _logger?.LogDebug($"");
                    Connect();
                    _logger?.LogDebug($"");
                    connected = true;
                });
            }
            return connected;
        }  
        public virtual IPrincipal FindByIdentity(string identityValue)
        {
            _logger?.LogDebug($"");
            return FindByIdentity(identityValue, PrincipalType.None);
        }
        public virtual IPrincipal FindByIdentity(string identityValue, PrincipalType principalType)
        {
            _logger?.LogDebug($"");
            return NovellDirectoryLdapUtils.FindByIdentity(FilterEncoder, this, identityValue, principalType, _logger);
        }
        public virtual IGroupPrincipal FindGroupByIdentity(string identityValue)
        {
            _logger?.LogDebug($"");
            return FindByIdentity(identityValue, PrincipalType.Group) as GroupPrincipal;
        }
        public virtual IUserPrincipal FindUserByIdentity(string identityValue)
        {
            _logger?.LogDebug($"");
            return FindByIdentity(identityValue, PrincipalType.User) as UserPrincipal;
        }
        public virtual void Dispose()
        {
            _logger?.LogDebug($"");
            if (_disposed) return;
            _disposed = true;
            Disconnect();
            _connection?.Dispose();
            _connection = null;
            GC.SuppressFinalize(this);
        }
    }
}
