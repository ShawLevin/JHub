using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Models
{
    public class Item
    {
        public string Title;
        public string Details;
        public string Category;
        public string Link;
        public string Image;
        public DateTime Created;
        public DateTime Modified;
        public DateTime EventDate;
        public DateTime EventTime;
        public string Location;
        public string Tags;
        public IEnumerable<string> Tag { get { return String.IsNullOrEmpty(Tags) ? new string[] { } : Tags.Split(','); } }
    }
}
