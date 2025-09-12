using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Connecta.Models
{
    public class UserPlan
    {
        public int Id { get; set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public int Price { get; internal set; }
        public int ContactLimit { get; internal set; }
    }
}