using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;



namespace VTIntranet.Models
{
    public class NoticeHelper
    {
        private SqlConnection con;

        //connection db
        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["intranetContext"].ToString();
            con = new SqlConnection(constr);
        }

        //getAll Notices
        public List<Notice> getAllNotice()
        {
            Conectar();
            List<Notice> notices = new List<Notice>();

            SqlCommand com = new SqlCommand("SELECT * FROM tblnotice INNER JOIN tblactivity ON tblactivity.idActivity = tblnotice.idNotice", con);
            con.Open();
            SqlDataReader rows = com.ExecuteReader();
            while (rows.Read())
            {
                Notice notice = new Notice()
                {
                    IdNotice = int.Parse(rows["idNotice"].ToString()),
                    Title = rows["title"].ToString(),
                    Description = rows["description"].ToString(),
                    StartDateNotice = rows["startDateNotice"].ToString(),
                    EndDateNotice = rows["endDateNotice"].ToString(),
                    //StartDateNotice = DateTime.Parse(rows["startDateNotice"].ToString()),
                };
                notices.Add(notice);
            }
            con.Close();
            return notices;
        }

        //save activity
        public int CreateActivity(Activity activity, Notice notice)
        {
            int n =0;
            Conectar();

            SqlCommand addActivity = new SqlCommand("insert into tblactivity(title, description, date) values (@title, @description, @date)", con);
            addActivity.Parameters.Add("@title", SqlDbType.VarChar);
            addActivity.Parameters.Add("@description", SqlDbType.VarChar);
            addActivity.Parameters.Add("@date", SqlDbType.DateTime);

            addActivity.Parameters["@title"].Value = activity.Title;
            addActivity.Parameters["@description"].Value = activity.Description;
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

                n = CreateNotice(notice, idActivity);
            
            }

            return n;
        }

        //save notice
        public int CreateNotice(Notice notice, int idActivity)
        {

            SqlCommand addNotice = new SqlCommand("insert into tblnotice(idNotice, startDateNotice, endDateNotice) values (@idNotice, @startDateNotice, @endDateNotice)", con);
            addNotice.Parameters.Add("@idNotice", SqlDbType.Int);
            addNotice.Parameters.Add("@startDateNotice", SqlDbType.DateTime);
            addNotice.Parameters.Add("@endDateNotice", SqlDbType.DateTime);

            addNotice.Parameters["@idNotice"].Value = idActivity;
            addNotice.Parameters["@startDateNotice"].Value = notice.StartDateNotice;
            addNotice.Parameters["@endDateNotice"].Value = notice.EndDateNotice;

            con.Open();
            int r = addNotice.ExecuteNonQuery();
            con.Close();

            return r;
           
        }

        //get notice for id
        public Notice getNotice(int idNotice)
        {
            Conectar();
            //SqlCommand com = new SqlCommand("select * from tblnotice where idNotice = @idNotice", con);
            SqlCommand com = new SqlCommand("SELECT * FROM tblnotice FULL OUTER JOIN tblactivity ON tblactivity.idActivity = tblnotice.idNotice WHERE idNotice = @idNotice", con);
            com.Parameters.Add("@idNotice", SqlDbType.Int);
            com.Parameters["@idNotice"].Value = idNotice;
            con.Open();

            SqlDataReader rows = com.ExecuteReader();
            Notice notice = new Notice();

            while (rows.Read())
            {

                notice.IdNotice = int.Parse(rows["idNotice"].ToString());
                notice.Title = rows["title"].ToString();
                notice.Description = rows["description"].ToString();
                notice.StartDateNotice = rows["startDateNotice"].ToString();
                notice.EndDateNotice = rows["endDateNotice"].ToString();

            }
            con.Close();
            return notice;
        }

    }
}