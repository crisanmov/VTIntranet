using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranet.Models
{
    public class Multimedia
    {
        //IdMultimedia == "Album"
        public int IdMultimedia { get; set; }
        public int IdEvent { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
    }
}