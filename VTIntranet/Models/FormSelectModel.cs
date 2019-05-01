using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranet.Models
{
    public class formSelectModel
    {
        public string value { get; set; }
        public string text { get; set; }
        public int valueInt { get; set; }
        public string shortText { get; set; }
        public string selected { get; set; }
        public int userPermission { get; set; }
        public int idUser { get; set; }
        public int idPermission { get; set; }
        public bool userPermissionActive { get; set; }
        public int userTag { get; set; }
        public bool userTagActive { get; set; }
        public int attachmentTag { get; set; }
        public int idAttachment { get; set; }
        public bool attachmentTagActive { get; set; }
        public int profileTag { get; set; }
        public int idTag { get; set; }
        public int idProfile { get; set; }
        public bool profileTagActive { get; set; }
        public formSelectModel()
        { }

        public formSelectModel(string value, string text)
        {
            this.value = value;
            this.text = text;
        }

        public formSelectModel initialize()
        {
            //List<FormSelectModel> lst = new List<FormSelectModel>();
            formSelectModel frm = new formSelectModel();
            frm.value = "0"; frm.text = "Seleccione ..."; return frm;
        }
        public formSelectModel initialize_en()
        {
            //List<FormSelectModel> lst = new List<FormSelectModel>();
            formSelectModel frm = new formSelectModel();
            frm.value = "0"; frm.text = "Select ..."; return frm;
        }
    }
}