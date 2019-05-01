namespace VTIntranet.intranetdb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbluserstags
    {
        [Key]
        public int idUserTag { get; set; }

        public int? idUser { get; set; }

        public int? idTag { get; set; }

        public bool? userTagActive { get; set; }

        public virtual tbltags tbltags { get; set; }

        public virtual tblusers tblusers { get; set; }
    }
}
