using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Sql
{
    internal static  class SqlQuery
    {
        public static string sGetServicesTnt = @"
            [dbo].[GetServicesTnt] 
            @postCode,
            @town, 
            @order_by, 
            @start, 
            @limit";
    }
}
