namespace VTIntranet.intranetdb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblattachments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblattachments()
        {
            tblattachmentstags = new HashSet<tblattachmentstags>();
        }

        [Key]
        public int idAttachment { get; set; }

        [StringLength(255)]
        public string attachmentName { get; set; }

        [StringLength(255)]
        public string attachmentShortName { get; set; }

        public string attachmentDirectory { get; set; }

        public string attachmentUrl { get; set; }

        public int? attachmentUserLastChange { get; set; }

        public DateTime? attachmentDateLastChange { get; set; }

        [StringLength(500)]
        public string attachmentContentType { get; set; }

        public bool attachmentActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblattachmentstags> tblattachmentstags { get; set; }
    }
}
