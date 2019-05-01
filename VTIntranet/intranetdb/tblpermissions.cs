namespace VTIntranet.intranetdb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblpermissions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblpermissions()
        {
            tbluserspermissions = new HashSet<tbluserspermissions>();
        }

        [Key]
        public int idPermission { get; set; }

        [Required]
        [StringLength(50)]
        public string permissionController { get; set; }

        [Required]
        [StringLength(60)]
        public string permissionAction { get; set; }

        [StringLength(50)]
        public string permissionArea { get; set; }

        [Required]
        [StringLength(10)]
        public string permissionImageClass { get; set; }

        [StringLength(50)]
        public string permissionActiveli { get; set; }

        public bool permissionEstatus { get; set; }

        public int permissionParentId { get; set; }

        public bool permissionIsParent { get; set; }

        public bool? permissionHasChild { get; set; }

        public bool permissionMenu { get; set; }

        public bool permissionIsHtml { get; set; }

        public string permissionTitle { get; set; }

        public string permissionDescription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbluserspermissions> tbluserspermissions { get; set; }
    }
}
