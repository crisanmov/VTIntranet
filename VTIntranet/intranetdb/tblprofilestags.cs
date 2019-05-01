namespace VTIntranet.intranetdb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblprofilestags
    {
        [Key]
        public int idProfileTag { get; set; }

        public int idProfile { get; set; }

        public int idTag { get; set; }

        public bool profileTagActive { get; set; }

        public virtual tblprofiles tblprofiles { get; set; }

        public virtual tbltags tbltags { get; set; }
    }
}
