namespace VTIntranet.intranetdb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblusers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblusers()
        {
            tbluserspermissions = new HashSet<tbluserspermissions>();
            tblusersprofiles = new HashSet<tblusersprofiles>();
            tbluserstags = new HashSet<tbluserstags>();
        }

        [Key]
        public int idUser { get; set; }

        [StringLength(100)]
        public string userName { get; set; }

        [StringLength(200)]
        public string userPasswordHash { get; set; }

        [StringLength(100)]
        public string userNameReal { get; set; }

        [StringLength(100)]
        public string userFirstLastName { get; set; }

        [StringLength(100)]
        public string userSecondLastName { get; set; }

        [StringLength(100)]
        public string userEmail { get; set; }

        public int? idRelationship { get; set; }

        public bool? userActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbluserspermissions> tbluserspermissions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblusersprofiles> tblusersprofiles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbluserstags> tbluserstags { get; set; }
    }
}
