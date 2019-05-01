using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//libraries for sql
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VTIntranet.Models
{
    public class TagHelper
    {
        private SqlConnection con;

        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["intranetContext"].ToString();
            con = new SqlConnection(constr);
        }

        //get all tags
        public List<Tag> gettAll()
        {
            Conectar();
            List<Tag> tags = new List<Tag>();

            SqlCommand com = new SqlCommand("select * from tbltags", con);
            con.Open();
            SqlDataReader registros = com.ExecuteReader();
            while (registros.Read())
            {
                Tag tag = new Tag
                {
                    idTag = int.Parse(registros["idTag"].ToString()),
                    tagName = registros["tagName"].ToString(),
                    tagDescription = registros["tagDescription"].ToString(),
                    //tagActive = int.Parse(registros["tagActive"].ToString())
                };
                tags.Add(tag);
            }
            con.Close();
            return tags;
        }

        //get all tags for idProfile
        public List<Tag> getTagProfile(int idProfile)
        {
            Conectar();
            List<Tag> tags = new List<Tag>();

            SqlCommand com = new SqlCommand("select tagName from tbltags t inner join tblprofilestags pt on t.idTag = pt.idTag where pt.idProfile = 1", con);
            con.Open();
            SqlDataReader registros = com.ExecuteReader();
            while (registros.Read())
            {
                Tag tag = new Tag
                {
                    //idTag = int.Parse(registros["idTag"].ToString()),
                    tagName = registros["tagName"].ToString(),
                    //tagDescription = registros["tagDescription"].ToString(),
                    //tagActive = int.Parse(registros["tagActive"].ToString())
                };
                tags.Add(tag);
            }
            con.Close();
            return tags;
        }
    }
}