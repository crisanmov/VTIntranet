namespace VTIntranet.intranetdb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblattachmentstags
    {
        [Key]
        public int idAttachmentTag { get; set; }

        public int idAttachment { get; set; }

        public int idTag { get; set; }

        public bool attachmentTagsActive { get; set; }

        public virtual tblattachments tblattachments { get; set; }

        public virtual tbltags tbltags { get; set; }
    }
}
