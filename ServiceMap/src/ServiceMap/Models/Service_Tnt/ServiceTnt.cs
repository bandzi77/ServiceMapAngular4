using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Models.Service_Tnt
{
    public class ServiceTnt
    {
        public string DepotCode { get; set; }
        public string Town { get; set; }
        public string FromPostcode { get; set; }
        public string ToPostcode { get; set; }
        public bool Sobota { get; set; }
        public bool EX9 { get; set; }
        public bool EX10 { get; set; }
        public bool EX12 { get; set; }
        public string Priority { get; set; }
        public bool? WieczorneDostarczenie { get; set; }
        public string StandardDeliveryOd { get; set; }
        public string StandardDeliveryDo { get; set; }
        public string PickUpDomesticZgl { get; set; }
        public string DateTimePickUpEksportSmZgl { get; set; }
        public bool? SamochodZwindaDostepnyWstandardzie { get; set; }
        public string DiplomatNextDay { get; set; }
        public bool? SerwisMiejski { get; set; }
        public bool? SerwisPodmiejski { get; set; }
        public Single? PickUpDomesticCzas { get; set; }
        public Single? PickUpEksportSmCzas { get; set; }
        public long RowNumber { get; set; }
        public int TotalCount { get; set; }
    }


    public class Test
    {
        public string DepotCode { get; set; }
        public string Town { get; set; }
        public string FromPostcode { get; set; }
        public string ToPostcode { get; set; }
      

    }
}

