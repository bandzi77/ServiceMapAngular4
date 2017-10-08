using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceMap.Models.apiModels;
using ServiceMap.Models.Service_Tnt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;
using ServiceMap.Sql;
using ServiceMap.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceMap.Controllers.apiControllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ServicesTntController : Controller
    {
        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        private IDepotRepository depotContext;
        private IServiceTntRepository serviceTnContext;
        private IConfiguration configuration;
        private IUserService userService;
        private UserManager<AppUser> userManager;

        public ServicesTntController(UserManager<AppUser> usrMgr, IServiceTntRepository serviceTntRepository, IConfiguration config, IUserService usrService, IDepotRepository depotRepository)
        {
            depotContext = depotRepository;
            serviceTnContext = serviceTntRepository;
            configuration = config;
            userService = usrService;
            userManager = usrMgr;
        }

        [HttpGet("GetServices")]
        public async Task<IActionResult> GetServices([FromQuery] ServiceFilter filter, PageInfo page)
        {
            List<ServiceTnt> res = new List<ServiceTnt>();
            var currentUser = await userService.GetUser(User);

            // Jeśli nie jest administratorem to zwieksza ilość zapytań w ciągu dnia o jeden
            if (!User.IsInRole(configuration["Data:Roles:Superuser"]))
            {
                if ((currentUser.NumberOfRequestsPerDay??0) >= currentUser.LimitOfRequestsPerDay)
                {
                    return Ok(new
                    {
                        serviceTnt = res,
                        paging = new { totalCount = 0, pageSize = page.PageSize },
                        requestsPerDay= new { limitOfRequestsPerDay = currentUser.LimitOfRequestsPerDay, numberOfRequestsPerDay = currentUser.NumberOfRequestsPerDay },
                        result = new { success = false, message = ConstsData.ExceededNumberOfRequestsPerDay }
                    });
                }
                currentUser.NumberOfRequestsPerDay = currentUser.NumberOfRequestsPerDay == null ? 1 : currentUser.NumberOfRequestsPerDay + 1;
                await userManager.UpdateAsync(currentUser);
            }

            res = serviceTnContext.ServicesTnt.FromSql(SqlQuery.sGetServicesTnt, SqlBuilder.GetServicesTnt(filter, page)).ToList();

            var result = new
            {
                serviceTnt = res,
                paging = new { totalCount = res.Select(x => x.TotalCount).FirstOrDefault(), pageSize = page.PageSize },
                requestsPerDay = new { limitOfRequestsPerDay = currentUser.LimitOfRequestsPerDay, numberOfRequestsPerDay = currentUser.NumberOfRequestsPerDay },
                result = new { success = true, message = "" }
            };

            return Ok(result);
        }


        [HttpGet("GetDepotDetails")]
        public IActionResult GetDepotDetails(string depotCode)
        {
            var depotDetails = depotContext.DepotDetails.Where(x => x.DepotCode == depotCode).ToList();
            var result = new { depotDetails };
            return Ok(result);
        }

        //private List<DepotDetails> getMockDepotDetails()
        //{
        //    List<DepotDetails> result =
        //        new List<DepotDetails>()
        //        {
        //            new DepotDetails() {
        //                Id = 6,
        //                DepotCode = "GDN",
        //                AddressesTown = "Gdańsk",
        //                AddressesStreet = "ul. Budowlanych 64 C",
        //                ExitCustomsOfficeOfficeNumber = "PL 322050",
        //                AwkwInfoIsSystemOrDiplomat = 'S',
        //                AwkwInfoIsBHPCompliant = true,
        //                AwkwInfoSupportingLocation = "GDN",
        //                InternationalPackageHoursInfo = "Od 08:00 do 16:00 (tel na recepcje 22 5101 410)",
        //                DomesticPackageHoursInfo = "8:00-17:00 - tylko dokumenty i paczki do 30kg",
        //                SaturdayPackageHoursInfo = "09:30-10:00, tylko w przypadku opcji SA ( sobotnie doręczenie), brak możliwości odbioru przesyłek z COD",
        //                SaturdayOpsHoursInfo = "08:00-12:00 - tel magazzyn 693881401",
        //                WeekPackageHoursInfo = "08:00-16:00 na recepcji, między 16-18 na magazynie tylko i wyłącznie paszporty.",
        //                CustomsOfficeOfficeNumber = "PL 322050",
        //                ExitCustomsOfficeOfficeDesc = "dla przesyłek latających z lotniska w KTW: PL331040",
        //                CustomsOfficeOfficeDesc = "Urząd Celny II w Warszawie, oddział celny V w WAW",
        //                AddressesPostcode = "55-095",
        //                ContactInfo1Phone = "695772583/ PUD J.Mikszewski/tydz.nieparzysty 7-15,po 15 Monika Koscian i Mariusz Wojcieszak",
        //                ContactInfo1Extension = "7057 i 7095 grupa kurierów K01-K27   7056 Grupa kurierów k71-k84 (dawna wirazowa)  7922 Grupa kurierów d01-d17 oraz ah01-ah17",
        //                ContactInfo1Description = "agenci operacyjni OPS",
        //                ContactInfo2Phone = "695772591/  PUD.Monika Koscian Lub 785150592 Mariusz Wojcieszak /tydz.parzysty 7-15,po 15 J.Mikszewski ",
        //                ContactInfo2Extension = "693110366 / 695770540 Kierownicy Zmiany PUD",
        //                ContactInfo2Description = "691 912 686, 785150406, 695 770 236, 695770414 ",
        //                ContactInfo3Phone = "695770516 / OPS (8 - 20)  DYŹUR SOBOTA - Agent OPS nr tel 695770516",
        //                ContactInfo3Extension = "7934  Łukasz Gromadka Serwis Miejski 8-16",
        //                ContactInfo3Description = "695770564/ 695694844 OPS III zmiana",
        //                AfterHoursContactInfo1Phone = "691482867/kierownik zespolu kurierskiego",
        //                AfterHoursContactInfo1Extension = "Kierownik PUD 695776281",
        //                AfterHoursContactInfo1Description = null,
        //                AfterHoursContactInfo2Phone = "22 318 74 28 /Administracja PUD po 17",
        //                AfterHoursContactInfo2Extension = "wew 7428",
        //                AfterHoursContactInfo2Description = null,
        //                AfterHoursContactInfo3Phone = "22 322 09 34/ 22 318 74 27 Dyspozytorzy PUD po 17",
        //                AfterHoursContactInfo3Extension = "7934/ 7427",
        //                AfterHoursContactInfo3Description = "Katowice Lotnisko Pyrzowice",
        //                Name = "Gdańsk",
        //                SamedayUndelCutoffTimeInfo = "jeseli dyspozycja wplynie do 07:45 ( na terenie Kielc - moza wyslac taka dyspozycje do 10:00 ) jest szana ze przesylka wyjedzie tego samego dnia . Po tej godzinie kurierow juz nie ma w oddziale"
        //            },
        //            new DepotDetails() {
        //                Id = 6,
        //                DepotCode = "GDN",
        //                AddressesTown = "Gdańsk",
        //                AddressesStreet = "ul.Słowackiego 202a",
        //                ExitCustomsOfficeOfficeNumber = "PL 322050",
        //                AwkwInfoIsSystemOrDiplomat = 'S',
        //                AwkwInfoIsBHPCompliant = true,
        //                AwkwInfoSupportingLocation = "GDN",
        //                InternationalPackageHoursInfo = "Od 08:00 do 16:00 (tel na recepcje 22 5101 410)",
        //                DomesticPackageHoursInfo = "8:00-17:00 - tylko dokumenty i paczki do 30kg",
        //                SaturdayPackageHoursInfo = "09:30-10:00, tylko w przypadku opcji SA ( sobotnie doręczenie), brak możliwości odbioru przesyłek z COD",
        //                SaturdayOpsHoursInfo = "08:00-12:00 - tel magazzyn 693881401",
        //                WeekPackageHoursInfo = "08:00-16:00 na recepcji, między 16-18 na magazynie tylko i wyłącznie paszporty.",
        //                CustomsOfficeOfficeNumber = "PL 322050",
        //                ExitCustomsOfficeOfficeDesc = "dla przesyłek latających z lotniska w KTW: PL331040",
        //                CustomsOfficeOfficeDesc = "Urząd Celny II w Warszawie, oddział celny V w WAW",
        //                AddressesPostcode = "55-095",
        //                ContactInfo1Phone = "695772583/ PUD J.Mikszewski/tydz.nieparzysty 7-15,po 15 Monika Koscian i Mariusz Wojcieszak",
        //                ContactInfo1Extension = "7057 i 7095 grupa kurierów K01-K27   7056 Grupa kurierów k71-k84 (dawna wirazowa)  7922 Grupa kurierów d01-d17 oraz ah01-ah17",
        //                ContactInfo1Description = "agenci operacyjni OPS",
        //                ContactInfo2Phone = "695772591/  PUD.Monika Koscian Lub 785150592 Mariusz Wojcieszak /tydz.parzysty 7-15,po 15 J.Mikszewski ",
        //                ContactInfo2Extension = "693110366 / 695770540 Kierownicy Zmiany PUD",
        //                ContactInfo2Description = "691 912 686, 785150406, 695 770 236, 695770414 ",
        //                ContactInfo3Phone = "695770516 / OPS (8 - 20)  DYŹUR SOBOTA - Agent OPS nr tel 695770516",
        //                ContactInfo3Extension = "7934  Łukasz Gromadka Serwis Miejski 8-16",
        //                ContactInfo3Description = "695770564/ 695694844 OPS III zmiana",
        //                AfterHoursContactInfo1Phone = "691482867/kierownik zespolu kurierskiego",
        //                AfterHoursContactInfo1Extension = "Kierownik PUD 695776281",
        //                AfterHoursContactInfo1Description = null,
        //                AfterHoursContactInfo2Phone = "22 318 74 28 /Administracja PUD po 17",
        //                AfterHoursContactInfo2Extension = "wew 7428",
        //                AfterHoursContactInfo2Description = null,
        //                AfterHoursContactInfo3Phone = "22 322 09 34/ 22 318 74 27 Dyspozytorzy PUD po 17",
        //                AfterHoursContactInfo3Extension = "7934/ 7427",
        //                AfterHoursContactInfo3Description = "Katowice Lotnisko Pyrzowice",
        //                Name = "Gdańsk Lotnisko",
        //                SamedayUndelCutoffTimeInfo = "jeseli dyspozycja wplynie do 07:45 ( na terenie Kielc - moza wyslac taka dyspozycje do 10:00 ) jest szana ze przesylka wyjedzie tego samego dnia . Po tej godzinie kurierow juz nie ma w oddziale"
        //            }
        //        };
        //    return result;
        //}

        //private List<ServiceTnt> getMockData()
        //{
        //    var result = new List<ServiceTnt>()
        //    {
        //        new ServiceTnt()
        //        {
        //             DepotCode = "GDN",
        //             Town =  "ABISYNIA",
        //             FromPostcode = "83-440",
        //             ToPostcode = "83-440",
        //             Sobota = false,
        //             EX9 = false,
        //             EX10 = false,
        //             EX12 =false,
        //             Priority = new TimeSpan(13,0,0),
        //             WieczorneDostarczenie = false,
        //             StandardDeliveryOd = new TimeSpan(13,0,0),
        //             StandardDeliveryDo =  new TimeSpan(16,0,0),
        //             PickUpDomesticZgl =   new TimeSpan(13,0,0),
        //             DateTimePickUpEksportSmZgl =  new TimeSpan(12,0,0),
        //             SamochodZwindaDostepnyWstandardzie = false,
        //             DiplomatNextDay =   new TimeSpan(12,0,0),
        //             SerwisPodmiejski = false,
        //             PickUpDomesticCzas = 2.5f,
        //             PickUpEksportSmCzas = 2,
        //             SerwisMiejski = null
        //        },

        //        new ServiceTnt()
        //        {
        //             DepotCode = "GDN",
        //             Town =  "Gdańsk",
        //             FromPostcode = "83-440",
        //             ToPostcode = "83-440",
        //             Sobota = false,
        //             EX9 = false,
        //             EX10 = false,
        //             EX12 =false,
        //             Priority = new TimeSpan(13,0,0),
        //             WieczorneDostarczenie = false,
        //             StandardDeliveryOd = new TimeSpan(13,0,0),
        //             StandardDeliveryDo =  new TimeSpan(16,0,0),
        //             PickUpDomesticZgl =   new TimeSpan(13,0,0),
        //             DateTimePickUpEksportSmZgl =  new TimeSpan(12,0,0),
        //             SamochodZwindaDostepnyWstandardzie = false,
        //             DiplomatNextDay =   new TimeSpan(12,0,0),
        //             SerwisPodmiejski = false,
        //             PickUpDomesticCzas = 2.5f,
        //             PickUpEksportSmCzas = 2,
        //             SerwisMiejski = null
        //        },
        //        new ServiceTnt()
        //        {
        //             DepotCode = "KRA",
        //             Town =  "Kraków",
        //             FromPostcode = "83-440",
        //             ToPostcode = "83-440",
        //             Sobota = true,
        //             EX9 = false,
        //             EX10 = false,
        //             EX12 =true,
        //             Priority = new TimeSpan(13,0,0),
        //             WieczorneDostarczenie = false,
        //             StandardDeliveryOd = new TimeSpan(13,0,0),
        //             StandardDeliveryDo =  new TimeSpan(16,0,0),
        //             PickUpDomesticZgl =   new TimeSpan(13,0,0),
        //             DateTimePickUpEksportSmZgl =  new TimeSpan(12,0,0),
        //             SamochodZwindaDostepnyWstandardzie = false,
        //             DiplomatNextDay =   new TimeSpan(12,0,0),
        //             SerwisPodmiejski = false,
        //             PickUpDomesticCzas = 2.5f,
        //             PickUpEksportSmCzas = 2,
        //             SerwisMiejski = null
        //        },
        //         new ServiceTnt()
        //        {
        //             DepotCode = "WAW",
        //             Town =  "WARSZAWA",
        //             FromPostcode = "83-440",
        //             ToPostcode = "83-440",
        //             Sobota = false,
        //             EX9 = false,
        //             EX10 = false,
        //             EX12 =false,
        //             Priority = new TimeSpan(13,0,0),
        //             WieczorneDostarczenie = false,
        //             StandardDeliveryOd = new TimeSpan(13,0,0),
        //             StandardDeliveryDo =  new TimeSpan(16,0,0),
        //             PickUpDomesticZgl =   new TimeSpan(13,0,0),
        //             DateTimePickUpEksportSmZgl =  new TimeSpan(12,0,0),
        //             SamochodZwindaDostepnyWstandardzie = false,
        //             DiplomatNextDay =   new TimeSpan(12,0,0),
        //             SerwisPodmiejski = false,
        //             PickUpDomesticCzas = 2.5f,
        //             PickUpEksportSmCzas = 2,
        //             SerwisMiejski = null
        //        }
        //    };
        //    return result;
        //}
    }
}
