using System.Text;

namespace System.DirectoryServices.AccountManagement.Core.Internal.Filters
{
    public interface IFilter
    {
        IFilterEncoder FilterEncoder { get; }
        String Encode();
        StringBuilder Encode(StringBuilder buf);
    }
}
