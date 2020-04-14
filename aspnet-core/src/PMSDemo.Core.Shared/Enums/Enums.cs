using System;
using System.Collections.Generic;
using System.Text;

namespace PMSDemo.Enums
{
    public enum UnitsEnum
    {
        None,
        Count,
        Percentage,
        Money
    }

    public enum ComparisonMethodEnum
    {
        GreaterThanOrEqual,
        LessThanOrEqual
    }

    public enum DataTypeEnum
    {
        Number,
        DateTime,
        Boolean
    }

    public enum CompletionStatusEnum
    {
        NotStarted,
        InProgress,
        Completed
    }
}
