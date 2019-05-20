using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VTIntranet.Models;

namespace VTIntranet.Models
{
    public class EventHelper
    {
        private SqlConnection con;

        //connection db
        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["intranetContext"].ToString();
            con = new SqlConnection(constr);
        }

        //get all event
        public List<Event> getAllEvent()
        {
            Conectar();
            List<Event> events = new List<Event>();

            SqlCommand com = new SqlCommand("SELECT * FROM tblevent INNER JOIN tblactivity ON tblactivity.idActivity = tblevent.idEvent", con);
            con.Open();
            SqlDataReader registros = com.ExecuteReader();
            while (registros.Read())
            {
                Event evt = new Event
                {
                    IdEvent = int.Parse(registros["idEvent"].ToString()),
                    Title = registros["title"].ToString(),
                    Description = registros["description"].ToString(),
                    FileName = registros["fileName"].ToString(),
                    //Path = registros["path"].ToString(),
                    Url = registros["url"].ToString(),

                };
                events.Add(evt);
            }
            con.Close();
            return events;
        }

        //save activity
        public int CreateActivity(Event evt)
        {
            int n = 0;
            Conectar();

            SqlCommand addActivity = new SqlCommand("insert into tblactivity(title, description, date) values (@title, @description, @date)", con);
            addActivity.Parameters.Add("@title", SqlDbType.VarChar);
            addActivity.Parameters.Add("@description", SqlDbType.VarChar);
            addActivity.Parameters.Add("@date", SqlDbType.DateTime);

            addActivity.Parameters["@title"].Value = evt.Title;
            addActivity.Parameters["@description"].Value = evt.Description;
            addActivity.Parameters["@date"].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            con.Open();
            int i = addActivity.ExecuteNonQuery();
            con.Close();

            if (i == 1)
            {
                SqlCommand com = new SqlCommand("select max(idActivity) from tblactivity", con);
                con.Open();
                int idActivity = Convert.ToInt32(com.ExecuteScalar());
                con.Close();

                n = CreateEvent(evt, idActivity);

            }

            return n;
        }

        //save event
        public int CreateEvent(Event evt, int idActivity)
        {

            SqlCommand addEvent = new SqlCommand("insert into tblevent(idEvent, fileName, path, url) values (@idEvent, @fileName, @path, @url)", con);
            addEvent.Parameters.Add("@idEvent", SqlDbType.Int);
            addEvent.Parameters.Add("@fileName", SqlDbType.VarChar);
            addEvent.Parameters.Add("@path", SqlDbType.VarChar);
            addEvent.Parameters.Add("@url", SqlDbType.VarChar);

            addEvent.Parameters["@idEvent"].Value = idActivity;
            addEvent.Parameters["@fileName"].Value = evt.FileName;
            addEvent.Parameters["@path"].Value = evt.Path;
            addEvent.Parameters["@url"].Value = evt.Url;

            con.Open();
            int r = addEvent.ExecuteNonQuery();
            con.Close();

            return r;

        }
    }
}