using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Events.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Events.Data_Access;

namespace Events.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Support()
        {
            ViewData["Message"] = "Grants, support, financial aid, vocational training, etc.";

            return View(GetItems(this.ControllerContext.RouteData.Values["action"].ToString()));
        }

        public async Task<IActionResult> Events()
        {
            ViewData["Message"] = "Events for every day of the week.";
            Dictionary<string, Item> items = await GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View("Events", items);
        }

        public async Task<IActionResult> Orgs()
        {
            ViewData["Message"] = "Philly has a ton of amazing organizations to get involved with.";
            Dictionary<string, Item> items = await GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View("Orgs", items);
        }

        public IActionResult Lead()
        {
            ViewData["Message"] = "Take a bigger role in the community.";

            return View(GetItems(this.ControllerContext.RouteData.Values["action"].ToString()));
        }

        public IActionResult Volunteer()
        {
            ViewData["Message"] = "Get involved in the community.";

            return View(GetItems(this.ControllerContext.RouteData.Values["action"].ToString()));
        }
        public IActionResult Jobs()
        {
            ViewData["Message"] = "Move on up.";
            FirebaseDataAccess da = new FirebaseDataAccess();
            return View(da.GetItems(this.ControllerContext.RouteData.Values["action"].ToString()));
        }
        public async Task<IActionResult> Network()
        {
            ViewData["Message"] = "Check this page to meet other members in the city.";

            Dictionary<string, Item> items = await GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View("Network", items);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "This site is my repository for everything happening in the Philly Jewish community.";

            return View(GetItems(this.ControllerContext.RouteData.Values["action"].ToString()));
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Admin(string title, string details, string category)
        {
            Item i = new Item();
            i.Title = title;
            i.Details = details;
            i.Created = DateTime.Now;
            i.Category = category;
            string jsonObject = JsonConvert.SerializeObject(i);
            
            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri("https://aepi-a016a.firebaseio.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = await client.PostAsync("https://aepi-a016a.firebaseio.com/.json", content);
            }
            return View();
        }

        public List<Events.Models.Item> GetItemList(string category)
        {
            List<Item> items = new List<Item>() {
                new Item { Title="Super Sunday", Details="The Jewish Federation's annual pledge phone a thon.", Category="Events", Modified = new DateTime(2017, 2, 12) },
                new Item { Title="Shaw Levin", Details="Software developer and organizer of the Philly AEPi alumni club.", Category="Network", Link="https://www.linkedin.com/in/shawlevin", Tags="aepi" },
                new Item { Title="Jeff Ginsburg", Details="Senior JavaScript developer with extensive optimization experience.", Category="Network" },
                new Item { Title="Ethan Riback", Details="2016 Drexel grad doing mobile development for Wayfair.", Category="Network" },
                new Item { Title="Senior Software Developer", Details="Senior developer opening in Center City with global investment bank. Microsoft stack and financial industry experience a plus. ", Category="Jobs" },
                new Item { Title="JEVS", Details="Human services organization providing educational and vocational programs around Philadelphia.", Category="Orgs" },
                new Item { Title="Board of Governors - Drexel", Details="This alumni group has openings!", Category="Lead" },
                new Item { Title="Jewish Park Clean Up", Details="Repair the World visits the Gladwyne Memorial Cemetary to help clean up the grounds.", Category="Volunteer", Modified = new DateTime(2017, 2, 24) },
                new Item { Title="Couples Shabbat", Details="Repair the World visits the Gladwyne Memorial Cemetary to help clean up the grounds.", Category="Events", Modified = new DateTime(2017, 2, 24) },
                new Item { Title="Purim Party", Details="Repair the World visits the Gladwyne Memorial Cemetary to help clean up the grounds.", Category="Events", Modified = new DateTime(2017, 3, 8) },
                new Item { Title="Young Involved Philly", Details="Philly young professionals interested in politics, philanthropy and networking.", Category="Org", Modified = new DateTime(2017, 3, 8) },
                new Item { Title="Summer Camp Grant", Details="The Jewish Federation offers grants for first year Jewish summrt camp participants.", Category="Support" }
            };

            return items.Where(i => i.Category == category).OrderByDescending(i=> i.Modified).ToList();
        }
    }
}
