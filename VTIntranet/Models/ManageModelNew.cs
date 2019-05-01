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
    public class ManageModelNew
    {
        private SqlConnection con;

        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["intranetContext"].ToString();
            con = new SqlConnection(constr);
        }

        //get all notices
        public List<NewModelHelper> getAllNotice()
        {
            Conectar();
            List<NewModelHelper> news = new List<NewModelHelper>();

            SqlCommand com = new SqlCommand("select * from tblnotice where isEvent = 0", con);
            con.Open();
            SqlDataReader registros = com.ExecuteReader();
            while (registros.Read())
            {
                
                NewModelHelper notice = new NewModelHelper
                {
                    IdNotice = int.Parse(registros["idNotice"].ToString()),
                    Title = registros["title"].ToString(),
                    Description = registros["description"].ToString(),
                    StartDateNotice = DateTime.Parse(registros["startDateNotice"].ToString()),
                    EndDateNotice = DateTime.Parse(registros["endDateNotice"].ToString()),
                    /*FileName = registros["fileName"].ToString(),
                    Path = registros["path"].ToString(),
                    Url = registros["url"].ToString(),
                    IsEvent = Boolean.Parse(registros["isEvent"].ToString())*/

                };
                news.Add(notice);
                
                
            }
            con.Close();
            return news;
        }

        //get all notices
        public List<NewModelHelper> getAllEvent()
        {
            Conectar();
            List<NewModelHelper> news = new List<NewModelHelper>();

            SqlCommand com = new SqlCommand("select * from tblnotice where isEvent = 1", con);
            con.Open();
            SqlDataReader registros = com.ExecuteReader();
            while (registros.Read())
            {
                NewModelHelper notice = new NewModelHelper
                {
                    IdNotice = int.Parse(registros["idNotice"].ToString()),
                    Title = registros["title"].ToString(),
                    Description = registros["description"].ToString(),
                    //StartDateNotice = DateTime.Parse(registros["startDateNotice"].ToString()),
                    //EndDateNotice = DateTime.Parse(registros["endDateNotice"].ToString()),
                    FileName = registros["fileName"].ToString(),
                    Path = registros["path"].ToString(),
                    Url = registros["url"].ToString(),
                    IsEvent = Boolean.Parse(registros["isEvent"].ToString())

                };
                news.Add(notice);   
            }
            con.Close();
            return news;
        }

        //get notice for idNew
        public NewModelHelper getNotice(int idNew)
        {
            Conectar();
            SqlCommand com = new SqlCommand("select * from tblnotice where idNotice = @idNotice", con);
            com.Parameters.Add("@idNotice", SqlDbType.Int);
            com.Parameters["@idNotice"].Value = idNew;
            con.Open();

            SqlDataReader rows = com.ExecuteReader();
            NewModelHelper notice = new NewModelHelper();
  
            while (rows.Read())
            {

                notice.IdNotice = int.Parse(rows["idNotice"].ToString());
                notice.Title = rows["title"].ToString();
                notice.Description = rows["description"].ToString();
                notice.StartDateNotice = DateTime.Parse(rows["startDateNotice"].ToString());
                notice.EndDateNotice = DateTime.Parse(rows["endDateNotice"].ToString());

            }
            con.Close();
            return notice;
        }

        //save notice
        public int Create(NewModelHelper notice)
        {
            Conectar();
            SqlCommand com = new SqlCommand("select max(idNotice) from tblnotice", con);
            con.Open();
            int idNotice = 0;
            try
            {
                idNotice = Convert.ToInt32(com.ExecuteScalar());
            }
            catch
            {
                idNotice = 0;
            }
            
            con.Close();

            if (notice.IsEvent)
            {
                SqlCommand command = new SqlCommand("insert into tblnotice(idNotice, title, description, fileName, path, url, isEvent) values (@idNotice, @title, @description, @fileName, @path, @url, @isEvent)", con);
                command.Parameters.Add("@idNotice", SqlDbType.Int);
                command.Parameters.Add("@title", SqlDbType.VarChar);
                command.Parameters.Add("@description", SqlDbType.VarChar);
                command.Parameters.Add("@fileName", SqlDbType.VarChar);
                command.Parameters.Add("@path", SqlDbType.VarChar);
                command.Parameters.Add("@url", SqlDbType.VarChar);
                command.Parameters.Add("@isEvent", SqlDbType.Bit);
                command.Parameters["@idNotice"].Value = idNotice + 1;
                command.Parameters["@title"].Value = notice.Title;
                command.Parameters["@description"].Value = notice.Description;
                command.Parameters["@fileName"].Value = notice.FileName;
                command.Parameters["@path"].Value = notice.Path;
                command.Parameters["@url"].Value = notice.Url;
                command.Parameters["@isEvent"].Value = notice.IsEvent;
                con.Open();
                int i = command.ExecuteNonQuery();
                con.Close();
                return i;
            }
            else
            {                
                SqlCommand command = new SqlCommand("insert into tblnotice(idNotice, title, description, startDateNotice, endDateNotice, isEvent) values (@idNotice, @title, @description, @startDateNotice, @endDateNotice, @isEvent)", con);
                command.Parameters.Add("@idNotice", SqlDbType.Int);
                command.Parameters.Add("@title", SqlDbType.VarChar);
                command.Parameters.Add("@description", SqlDbType.VarChar);
                command.Parameters.Add("@startDateNotice", SqlDbType.DateTime);
                command.Parameters.Add("@endDateNotice", SqlDbType.DateTime);
                command.Parameters.Add("@isEvent", SqlDbType.Bit);
                command.Parameters["@idNotice"].Value = idNotice + 1;
                command.Parameters["@title"].Value = notice.Title;
                command.Parameters["@description"].Value = notice.Description;
                command.Parameters["@startDateNotice"].Value = notice.StartDateNotice;
                command.Parameters["@endDateNotice"].Value = notice.EndDateNotice;
                command.Parameters["@isEvent"].Value = notice.IsEvent;
                con.Open();
                int i = command.ExecuteNonQuery();
                con.Close();
                return i;
            }

            

            
        }
    }
}