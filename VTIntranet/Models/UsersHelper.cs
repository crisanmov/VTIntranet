using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace VTIntranet.Models
{
    public class UsersHelper
    {
        private SqlConnection con;

        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["intranetContext"].ToString();
            con = new SqlConnection(constr);
        }

        public usersModelHelper GetUser(string userName)
        {
            Conectar();
            string query = "SELECT idUser, userNameReal, userEmail FROM tblusers WHERE userName ='" + userName  + "' ";
            SqlCommand com = new SqlCommand(query, con);
            con.Open();

            SqlDataReader rows = com.ExecuteReader();
            usersModelHelper data = new usersModelHelper();

            while (rows.Read())
            {
                data.idUser = int.Parse(rows["idUser"].ToString());
                data.userName = rows["userNameReal"].ToString();
                data.userEmail = rows["userEmail"].ToString();

            }

            con.Close();
            return data;
        }

        public int GetProfile(String UserName)
        {
            Conectar();
            string query = @"SELECT tblprofiles.idProfile
                            FROM tblprofiles
                            INNER JOIN tblusersprofiles on tblusersprofiles.idProfile = tblprofiles.idProfile
                            INNER JOIN tblusers on tblusers.idUser = tblusersprofiles.idUser
                            WHERE tblusers.userName = @UserName";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@UserName", SqlDbType.VarChar);
            com.Parameters["@UserName"].Value = UserName;

            con.Open();
            int r = Convert.ToInt32(com.ExecuteScalar());

            con.Close();

            return r;
            
        }
    }
}