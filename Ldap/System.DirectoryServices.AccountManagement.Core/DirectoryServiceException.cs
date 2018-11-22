namespace System.DirectoryServices.AccountManagement.Core
{
    public class DirectoryServiceException : Exception
    {
        public DirectoryServiceException(IPrincipalContext context, string message) : this(context, message, null)
        {
        }
        public DirectoryServiceException(IPrincipalContext context, string message, Exception innerException) : base(message, innerException)
        {
            Context = context;            
        }
        public virtual IPrincipalContext Context { get; protected set; }
        public virtual string Principal { get; protected set; }
        public virtual int Port { get; set; }
        public virtual bool UseSSL { get; set; } = false;
    }
}
