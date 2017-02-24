using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Support()
        {
            ViewData["Message"] = "Grants, support, financial aid, vocational training, etc.";
            Dictionary<string, Item> items = await DataFactory.DataAccessFactory().GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View(items);
        }

        public async Task<IActionResult> Events()
        {
            ViewData["Message"] = "Events for every day of the week.";
            Dictionary<string, Item> items = await DataFactory.DataAccessFactory().GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View(items);
        }

        public async Task<IActionResult> Orgs()
        {
            ViewData["Message"] = "Philly has a ton of amazing organizations to get involved with.";
            Dictionary<string, Item> items = await DataFactory.DataAccessFactory().GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View(items);
        }

        public async Task<IActionResult> Lead()
        {
            ViewData["Message"] = "Take a bigger role in the community.";

            Dictionary<string, Item> items = await DataFactory.DataAccessFactory().GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View(items);
        }

        public async Task<IActionResult> Volunteer()
        {
            ViewData["Message"] = "Get involved in the community.";
            Dictionary<string, Item> items = await DataFactory.DataAccessFactory().GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View(items);
        }

        public async Task<IActionResult> Jobs()
        {
            ViewData["Message"] = "Move on up.";
            Dictionary<string, Item> items = await DataFactory.DataAccessFactory().GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View(items);
        }
        public async Task<IActionResult> Network()
        {
            ViewData["Message"] = "Check this page to meet other members in the city.";

            Dictionary<string, Item> items = await DataFactory.DataAccessFactory().GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View("Network", items);
        }

        public async Task<IActionResult> Candidates()
        {
            ViewData["Message"] = "Find your next great hire from our list of community members.";

            Dictionary<string, Item> items = await DataFactory.DataAccessFactory().GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View("Candidates", items);
        }

        public async Task<IActionResult> Contact()
        {
            ViewData["Message"] = "This site is my repository for everything happening in the Philly Jewish community.";

            Dictionary<string, Item> items = await DataFactory.DataAccessFactory().GetItems(this.ControllerContext.RouteData.Values["action"].ToString());
            return View(items);
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Admin()
        {
            ViewBag.Categories = new List<SelectListItem>
            {
                new SelectListItem { Text="Network" },
                new SelectListItem { Text="Jobs" },
                new SelectListItem { Text="Events" },
                new SelectListItem { Text="Orgs" },
                new SelectListItem { Text="Candidates" },
                new SelectListItem { Text="Lead" },
                new SelectListItem { Text="Volunteer" },
                new SelectListItem { Text="Support" }
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Admin(string Title, string details, string category, string link, string image, string tags)
        {
            ViewBag.Categories = new List<SelectListItem>
            {
                new SelectListItem { Text="Network" },
                new SelectListItem { Text="Jobs" },
                new SelectListItem { Text="Events" },
                new SelectListItem { Text="Orgs" },
                new SelectListItem { Text="Candidates" },
                new SelectListItem { Text="Lead" },
                new SelectListItem { Text="Volunteer" },
                new SelectListItem { Text="Support" }
            };

            Item item = new Item();
            item.Title = Title;
            item.Details = details;
            item.Link = link;
            item.Image = image;
            item.Category = category;
            item.Tags = tags;
            string jsonObject = JsonConvert.SerializeObject(item);
            
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
