using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTIntranet.intranetdb;

namespace VTIntranet.Models
{
    public class usersTagsModelHelper
    {
        public int idUser { get; set; }
        public string userName { get; set; }
        public int idProfile { get; set; }
        public string profileName { get; set; }
        public int idTag { get; set; }
        public string tagName { get; set; }
        public List<tbltags> tags { get; set; }
        public List<tblattachments> attachments { get; set; }
        public List<tagsModelHelper> tagsModel { get; set; }
        public List<attachmentsModelHelper> attachModel { get; set; }
        public usersTagsModelHelper()
        {
            tags = new List<tbltags>();
            attachments = new List<tblattachments>();
            tagsModel = new List<tagsModelHelper>();
            attachModel = new List<Models.attachmentsModelHelper>();
        }

    }
}