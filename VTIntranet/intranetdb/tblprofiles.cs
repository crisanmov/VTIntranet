namespace VTIntranet.intranetdb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblprofiles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblprofiles()
        {
            tblprofilestags = new HashSet<tblprofilestags>();
            tblusersprofiles = new HashSet<tblusersprofiles>();
        }

        [Key]
        public int idProfile { get; set; }

        [StringLength(100)]
        public string profileName { get; set; }

        [StringLength(255)]
        public string profileDescription { get; set; }

        public bool profileActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblprofilestags> tblprofilestags { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblusersprofiles> tblusersprofiles { get; set; }
    }
}
