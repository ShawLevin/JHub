using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Models
{
    public class AdminViewModel
    {
        public Item item;
        public String category;
        public IEnumerable<string> categories { get; set; } 
    }
}
