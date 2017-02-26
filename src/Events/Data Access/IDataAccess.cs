using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;

namespace Events.Data_Access
{
    public interface IDataAccess
    {
        Task<Dictionary<string, Item>> GetItems(string category);
    }

    public class FirebaseDataAccess : IDataAccess
    {
        public async Task<Dictionary<string, Item>> GetItems(string category)
        {
            Dictionary<string, Item> items = new Dictionary<string, Item>();

            var retValue = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://aepi-a016a.firebaseio.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(".json");
                if (response.IsSuccessStatusCode)
                {
                    retValue = await response.Content.ReadAsStringAsync();
                    items = JsonConvert.DeserializeObject<Dictionary<string, Item>>(retValue);
                    items = items.Where(i => i.Value.Category == category).OrderByDescending(i=>i.Value.Created).ToDictionary(k => k.Key, v => v.Value);

                }
            }
            return items;
        }
    }

    public class OfflineDataAccess : IDataAccess
    {
        public Task<Dictionary<string, Item>> GetItems(string category)
        {
            string[] categories = { "Networking","Orgs","Events","Jobs" };
            Dictionary<string, Item> items = new Dictionary<string, Item>();
            for (int i = 0; i < 100; i++)
            {
                items.Add(i.ToString(), new Item { Title = $"Item {i}", Category = categories[i % categories.Length], Created = DateTime.Now, Details = "Details", Link = "http://www.google.com" });
            }
            return Task.FromResult<Dictionary<string, Item>>(items);
        }
    }
}
