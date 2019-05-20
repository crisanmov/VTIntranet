using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranet.Models
{
    public class Attachment
    {
        public int IdAttachment { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentDirectory { get; set; }
        public string TagName { get; set; }
        public string AttachmentActive { get; set; }

        public int IdDepto { get; set; }
    }
}