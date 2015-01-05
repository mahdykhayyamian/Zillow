using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Zillow.Controllers
{
    public class HomeController : Controller
    {
        private const string baseSearchURL = "http://www.zillow.com/webservice/GetSearchResults.htm?zws-id=X1-ZWz1dyb53fdhjf_6jziz";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ZillowSearch()
        {
            ViewBag.Message = "Welcome To Zillow Search";

            return View();
        }

        public async Task<ActionResult> ZillowSearchResult(string citystatezip, string address)
        {
            ViewBag.SyncOrAsync = "Asynchronous";
            ViewBag.citystatezip = citystatezip;
            ViewBag.address = address;

            string url = CreateSearchURL(citystatezip, address);
            string xmlResponse = await CallZillowAPI(url);
            ViewBag.response = xmlResponse;
            var result = readSearchResutlsFromXml(xmlResponse);

            if (result != null && result.message.code.Equals("0"))
            {
                ViewBag.SearchResults = result;
            }
            else
            {
                ViewBag.errorMessage = result.message.text;
            }

            return View("ZillowSearch");
        }

        private string CreateSearchURL(string citystatezip, string address)
        {
            string url = baseSearchURL + "&citystatezip=" + citystatezip + "&address=" + address;
            return url;
        }

        private async Task<string> CallZillowAPI(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    return (await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                return e.Message + "\n" + e.StackTrace;
            }
        }

        public searchresults readSearchResutlsFromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(searchresults));
            StringReader rdr = new StringReader(xml);
            searchresults sr = (searchresults)serializer.Deserialize(rdr);
            return sr;
        }
    }
}