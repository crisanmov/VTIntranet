using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace VTIntranet.Models
{
    public class NewModelHelper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdNotice { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateNotice { get; set; }
        public DateTime EndDateNotice { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public Boolean IsEvent { get; set; }

    }
}