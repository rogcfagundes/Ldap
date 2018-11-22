namespace System.DirectoryServices.AccountManagement.Core.Internal.Filters
{
    public class EqualsFilter : CompareFilter
    {
        private const String EQUALS_SIGN = "=";
        public EqualsFilter(IFilterEncoder encoder, String attribute, String value) 
            : base(encoder, attribute, value)
        {
        }
        public EqualsFilter(IFilterEncoder encoder, String attribute, int value) 
            : base(encoder, attribute, value)
        {
        }
        protected override String GetCompareString()
        {
            return EQUALS_SIGN;
        }
    }
}
