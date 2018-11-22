namespace System.DirectoryServices.AccountManagement.Core.Internal.Filters
{
    public class AndFilter : BinaryLogicalFilter
    {
        public AndFilter(IFilterEncoder encoder) 
            : base(encoder)
        {
        }
        private const String AMPERSAND = "&";
        protected override string GetLogicalOperator()
        {
            return AMPERSAND;
        }
        public AndFilter And(IFilter query)
        {
            Append(query);
            return this;
        }
    }
}
