namespace VTIntranet.intranetdb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblusersprofiles
    {
        [Key]
        public int idUserProfile { get; set; }

        public int idUser { get; set; }

        public int idProfile { get; set; }

        public bool? userProfileActive { get; set; }

        public virtual tblprofiles tblprofiles { get; set; }

        public virtual tblusers tblusers { get; set; }
    }
}
