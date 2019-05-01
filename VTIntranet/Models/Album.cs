using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranet.Models
{
    public class Album
    {
        public int IdEvent { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }
        public int IdMultimedia { get; set; }
        public string Path { get; set; }
    }
}