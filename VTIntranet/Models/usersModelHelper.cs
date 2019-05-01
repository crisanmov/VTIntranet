using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranet.Models
{
    public class usersModelHelper
    {
        public int idUser { get; set; }
        public string userName { get; set; }
        public string userCompledName { get; set; }
        public string userSecondLastName { get; set; }
        public string userEmail { get; set; }
        public string userProfileType { get; set; }
        public int idProfile { get; set; }
    }
}