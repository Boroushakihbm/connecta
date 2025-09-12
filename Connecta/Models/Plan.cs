using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Connecta.Models
{
    public class Plan
    {
        public int Id { get;  set; }
        public string Name { get;  set; }
        public string Description { get;  set; }
        public int Price { get;  set; }
        public int ContactLimit { get;  set; }
        public int DurationDays { get; set; }
    }
}