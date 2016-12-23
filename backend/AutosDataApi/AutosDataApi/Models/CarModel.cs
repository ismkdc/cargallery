using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutosDataApi.Models
{
    public class CarModel
    {
        public Dictionary<string,string> Info { get; set; }
        public List<string> Images { get; set; }
    }
}