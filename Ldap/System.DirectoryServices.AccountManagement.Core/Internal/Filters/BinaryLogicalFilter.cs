using System.Collections.Generic;
using System.Text;

namespace System.DirectoryServices.AccountManagement.Core.Internal.Filters
{
    public abstract class BinaryLogicalFilter : AbstractFilter
    {
        public BinaryLogicalFilter(IFilterEncoder encoder) 
            : base(encoder)
        {
        }
        private List<IFilter> queryList = new List<IFilter>();
        public override StringBuilder Encode(StringBuilder buff)
        {
            if (queryList.Count <= 0)
            {
                // only output query if contains anything
                return buff;
            }
            else if (queryList.Count == 1)
            {
                // don't add the &
                IFilter query = queryList[0];
                return query.Encode(buff);
            }
            else
            {
                buff.Append("(").Append(GetLogicalOperator());
                foreach (IFilter query in queryList)
                {
                    query.Encode(buff);
                }
                buff.Append(")");
                return buff;
            }
        }
        protected abstract String GetLogicalOperator();
        public BinaryLogicalFilter Append(IFilter query)
        {
            queryList.Add(query);
            return this;
        }
        public BinaryLogicalFilter AppenddAll(IList<IFilter> subQueries)
        {
            queryList.AddRange(subQueries);
            return this;
        }
    }
}
