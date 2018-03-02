using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdLookUp.Models
{
    public class Directory
    {
        public string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}