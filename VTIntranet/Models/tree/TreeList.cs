using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranet.Models.tree
{
    public class TreeList
    {
        public long IntranetId { get; set; }
        public long ParentId { get; set; }
        public String Description { get; set; }
        public String Name { get; set; }
        public List<TreeList> SubMenu { get; set; }

        public TreeList()
        {
        }
        public TreeList(long idIntranet, string description, string name, List<TreeList> submenu)
        {
            IntranetId = idIntranet;
            Description = description;
            Name = name;
            SubMenu = submenu;
        }


    }
}