using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;


namespace advtAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        [HttpPost]
        public IActionResult PutAd(string AdName,double AdPrice, string AdDescription, string AdLinks) //Put ad in database
        {
            string[] result = {"Code:-1",""};
            int ErrValid = 0;
            if (AdName.Length>200)
            {
                ErrValid = 1; //"Error! AD Name length more that 200 symbols"
            }
            if(AdDescription.Length>1000)
            {
                ErrValid = 2; //"Error! AD Description length more that 1000 symbols"
            }
            string[] AdLinksList = AdLinks.Split(',');
            if(AdLinksList.Length>3)
            {
                ErrValid = 3; //"Error! AD PhotoLinks cannot be more that 3 links"
            }
            if (ErrValid == 0)
            {
                using (var AdDatabase = new AdsContext())
                {
                    var ad = new Ads { AdName = AdName, AdPrice = AdPrice, AdDescription = AdDescription, AdMainLink = AdLinksList[0], AdLinks = AdLinks, CreationTime = DateTime.Now }; //Create ad 
                    AdDatabase.AdsTable.Add(ad);
                    AdDatabase.SaveChanges(); 
                    //Create response 
                    result[0] = "Code:0";
                    result[1] = $"AdId:{ad.Id}";

                    return new JsonResult(result, new JsonSerializerOptions { PropertyNamingPolicy = null });
                }
            }
            //Return if validation error
            result[0]=$"Code:{ErrValid}";
            result[1] = "Error";
            return new JsonResult(result, new JsonSerializerOptions { PropertyNamingPolicy = null });
        }

        [HttpGet]
        public IActionResult GetAdById(int AdId, bool fields) //Get ad by id 
        {
            using (var AdDatabase = new AdsContext())
            {
                List<Ads> AdsTableToResponse = new List<Ads>();
                if (fields) //If field return more field to user
                {
                    var AdsTable = AdDatabase.AdsTable.Where(f => f.Id == AdId).Select(x => new { AdName = x.AdName, AdPrice = x.AdPrice, AdDescription = x.AdDescription, AdLinks = x.AdLinks }).FirstOrDefault();
                    return new JsonResult(AdsTable, new JsonSerializerOptions { PropertyNamingPolicy = null });
                }
                else
                {
                    var AdsTable = AdDatabase.AdsTable.Where(f=> f.Id == AdId).Select(x => new { AdName = x.AdName, AdPrice = x.AdPrice, AdLinks = x.AdMainLink }).FirstOrDefault();
                    return new JsonResult(AdsTable, new JsonSerializerOptions { PropertyNamingPolicy = null });
                }


            }
            
        }

        [HttpGet]
        public IActionResult GetAds(int PageNumber, string DateSort,string PriceSort)
        {
            using (var AdDatabase = new AdsContext())
            {
                List<Ads> AdsTableToResponse = new List<Ads>(); //Create ad list
                int PageSize = 10; // Items per page 

                //HELL NO SORTING

                string sorting = DateSort + PriceSort; //Variants: 00 DESC0 ASC0 0DESC 0ASC DESCDESC ASCASC DESCASC ASCDESC 
                switch (sorting)
                {
                    case "00": //No Sorting
                        AdsTableToResponse = AdDatabase.AdsTable.ToList(); 
                        break;
                    case "DESC0": // CrTime DESC Price None
                        AdsTableToResponse = AdDatabase.AdsTable.OrderByDescending(f => f.CreationTime).ToList();
                        break;
                    case "ASC0": // CrTime ASC Price None
                        AdsTableToResponse = AdDatabase.AdsTable.OrderBy(f => f.CreationTime).ToList();
                        break;
                    case "0DESC": // CrTime None Price DESC
                        AdsTableToResponse = AdDatabase.AdsTable.OrderByDescending(f => f.AdPrice).ToList();
                        break;
                    case "0ASC": // CrTime None Price ASC
                        AdsTableToResponse = AdDatabase.AdsTable.OrderBy(f => f.AdPrice).ToList();
                        break;
                    case "DESCDESC": // CrTime DESC Price DESC
                        AdsTableToResponse = AdDatabase.AdsTable.OrderByDescending(f => f.CreationTime).ThenByDescending(f => f.AdPrice).ToList();
                        break;
                    case "ASCASC": // CrTime ASC Price ASC
                        AdsTableToResponse = AdDatabase.AdsTable.OrderBy(f => f.CreationTime).ThenBy(f => f.AdPrice).ToList();
                        break;
                    case "DESCASC": // CrTime DESC Price ASC
                        AdsTableToResponse = AdDatabase.AdsTable.OrderByDescending(f => f.CreationTime).ThenBy(f => f.AdPrice).ToList();
                        break;
                    case "ASCDESC": // CrTime ASC Price DESC
                        AdsTableToResponse = AdDatabase.AdsTable.OrderBy(f => f.CreationTime).ThenBy(f => f.AdPrice).ToList();
                        break;
                }

               
                //After sorting select only needed columns
                List<ShortAds> AdsTable = AdsTableToResponse.Select(x => new ShortAds { AdName = x.AdName, AdMainLink = x.AdMainLink, AdPrice = x.AdPrice }).ToList(); ///.Skip(PageNumber * PageSize).Take(PageSize)
                int totpages = (AdsTable.Count + PageSize) / PageSize; //COUNT TOTAL PAGES
                AdsTable = AdsTable.Skip(PageNumber * PageSize).Take(PageSize).ToList(); // Take neede page
                AdsResponse response = new AdsResponse(); //Create Response
                response.AdsList = AdsTable; //List of ads
                response.total_pages = totpages;
                response.per_page = PageSize; 
                response.page = PageNumber;
                return new JsonResult(response, new JsonSerializerOptions{PropertyNamingPolicy = null});
            }      
        }


    



    }
}
