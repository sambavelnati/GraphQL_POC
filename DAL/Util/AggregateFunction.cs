using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore3Sample
{
    public enum AggregateFunction
    {
        MAX,
        MIN,
        AVG,
        SUM
    }

    public class AggregateFunctionGetter
    {
        public static string GetLINQFunction(AggregateFunction aggregateFunction)
        {
            switch (aggregateFunction)
            {
                case AggregateFunction.SUM:
                    return "Sum";
                case AggregateFunction.MAX:
                    return "Max";
                case AggregateFunction.MIN:
                    return "Min";
                case AggregateFunction.AVG:
                    return "Average";
                default:
                    throw new Exception("Unhandled Parameter Exception for AggregateFunction" + aggregateFunction.ToString());
            }
        }
    }
}
