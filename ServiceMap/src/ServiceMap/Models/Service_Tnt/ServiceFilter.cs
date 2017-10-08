using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Models.Service_Tnt
{
    public class ServiceFilter
    {
        public string PostCode { get; set; }
        public string CityName { get; set; }
 }
    }

    public class PageInfo
    {
        public string OrderBy { get; set; }
        public Int16? CurrentPage { get; set; }
        public Int16? PageSize { get; set;
    }

}
