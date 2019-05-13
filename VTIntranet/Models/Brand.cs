using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranet.Models
{
    public class Brand
    {
        public int IdTag { get; set; }
        public string TagName { get; set; }
        public int IdDepto { get; set; }
        public string DeptoName { get; set; }
        public string Clabe { get; set; }

    }
}