namespace VTIntranet.intranetdb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbluserspermissions
    {
        [Key]
        public int idUserPermission { get; set; }

        public int idUser { get; set; }

        public int idPermission { get; set; }

        public bool? userPermissionActive { get; set; }

        public virtual tblpermissions tblpermissions { get; set; }

        public virtual tblusers tblusers { get; set; }
    }
}
