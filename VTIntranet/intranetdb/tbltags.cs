namespace VTIntranet.intranetdb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbltags
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbltags()
        {
            tblattachmentstags = new HashSet<tblattachmentstags>();
            tblprofilestags = new HashSet<tblprofilestags>();
            tbluserstags = new HashSet<tbluserstags>();
        }

        [Key]
        public int idTag { get; set; }

        [StringLength(100)]
        public string tagName { get; set; }

        [StringLength(255)]
        public string tagDescription { get; set; }

        public bool tagActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblattachmentstags> tblattachmentstags { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblprofilestags> tblprofilestags { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbluserstags> tbluserstags { get; set; }
    }
}
