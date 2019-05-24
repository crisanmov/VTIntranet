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

            SqlCommand com = new SqlCommand("select * from tbltags order by tagName", con);
            con.Open();
            SqlDataReader registros = com.ExecuteReader();
            while (registros.Read())
            {
                Tag tag = new Tag
                {
                    idTag = int.Parse(registros["idTag"].ToString()),
                    tagName = registros["tagName"].ToString(),
                    tagDescription = registros["tagDescription"].ToString(),
                    clabe = registros["clabe"].ToString()
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

        //get tags for idProfile
        public List<Brand> GetBrand(int idProfile)
        {
            Conectar();
            List<Brand> brands = new List<Brand>();
            string query = @"SELECT tbltags.idTag, tbltags.tagName, tbltags.clabe, tbldepto.idDepto, tbldepto.name 
                             FROM tbltags
                             INNER JOIN tbltagdepto on tbltagdepto.idTag = tbltags.idTag
	                         INNER JOIN tbldepto on tbldepto.idDepto = tbltagdepto.idDepto
	                         INNER JOIN tblprofilestags on tblprofilestags.idTag = tbltags.idTag 
                             WHERE tblprofilestags.idProfile = @idProfile
                             ORDER BY tagName";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@idProfile", SqlDbType.Int);
            com.Parameters["@idProfile"].Value = idProfile;
            con.Open();
            SqlDataReader rows = com.ExecuteReader();

            while (rows.Read())
            {
                Brand brand = new Brand()
                {
                    IdTag = int.Parse(rows["idTag"].ToString()),
                    TagName = rows["tagName"].ToString(),
                    IdDepto = int.Parse(rows["idDepto"].ToString()),
                    DeptoName = rows["name"].ToString(),
                    Clabe = rows["clabe"].ToString()

                };
                brands.Add(brand);
            }
            return brands;

        }

        public List<Depto> GetDepto(string brand)
        {
            Conectar();
            List<Depto> deptos = new List<Depto>();

            string query = @"SELECT tbldepto.idDepto, tbldepto.name, tbldepto.clabe
                            FROM tbldepto
	                            INNER JOIN tbltagdepto ON tbltagdepto.idDepto = tbldepto.idDepto
	                            INNER JOIN tbltags ON tbltags.idTag = tbltagdepto.idTag
                            WHERE tbltags.clabe = @tagName
                            ORDER BY tbldepto.name";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@tagName", SqlDbType.VarChar);
            com.Parameters["@tagName"].Value = brand;
            con.Open();
            SqlDataReader rows = com.ExecuteReader();

            while (rows.Read())
            {
                Depto depto = new Depto()
                {
                    
                    IdDepto = int.Parse(rows["idDepto"].ToString()),
                    DeptoName = rows["name"].ToString(),
                    Clabe = rows["clabe"].ToString(),

                };
                deptos.Add(depto);
            }
            return deptos;

        }

        public String GetBrand(String brand)
        {
            Conectar();
            string[] result = { };



            return "a";

        }


    }
}