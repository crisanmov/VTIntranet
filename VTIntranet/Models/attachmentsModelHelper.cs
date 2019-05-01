using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranet.Models
{
    public class attachmentsModelHelper
    {
        public int idAttachment { get; set; }
        public string attachmentName { get; set; }
        public string attachmentTagsName { get; set; }
        public string attachmentUrl { get; set; }
        public string attachmentDirectory { get; set; }
        public string attachmentShortName { get; set; }
        public string attachmentContentType { get; set; }
        public string attachmentUrlComplete { get; set;}
        public string Url { get; set; }
        public DateTime attachmentDate { get; set; }
        public string attachmentDateLastChange { get; set; }
        public string attachmentFileType { get; set; }
    }
}