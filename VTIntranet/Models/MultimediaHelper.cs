using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace VTIntranet.Models
{
    public class MultimediaHelper
    {
        private SqlConnection con;

        //connection db
        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["intranetContext"].ToString();
            con = new SqlConnection(constr);
        }

        //getAll Multimedia
        public List<Multimedia> getAlbumPortriat(int idAlbum)
        {
            Conectar();
            List<Multimedia> portrait = new List<Multimedia>();

            string q = @"SELECT tblevent.idEvent, tblactivity.title, tblactivity.description, tblmultimedia.fileName, tblmultimedia.idMultimedia, tblmultimedia.path 
                        FROM tblevent 
                        INNER JOIN tbleventmultimedia ON tblevent.idEvent = tbleventmultimedia.idEvent 
                        INNER JOIN tblactivity ON tblactivity.idActivity = tblevent.idEvent 
                        INNER JOIN tblmultimedia ON tblmultimedia.idMultimedia = tbleventmultimedia.idMultimedia 
                        WHERE tblevent.idEvent = @idAlbum 
                        ORDER BY tblmultimedia.idMultimedia
                        OFFSET 0 ROWS FETCH FIRST 4 ROWS ONLY";

            SqlCommand com = new SqlCommand(q, con);
            com.Parameters.Add("@idAlbum", SqlDbType.Int);
            com.Parameters["@idAlbum"].Value = idAlbum;
            con.Open();
            SqlDataReader rows = com.ExecuteReader();
            while (rows.Read())
            {
                Multimedia img = new Multimedia()
                {
                    IdMultimedia = int.Parse(rows["idMultimedia"].ToString()),
                    IdEvent = int.Parse(rows["idEvent"].ToString()),
                    FileName = rows["fileName"].ToString(),
                    Path = rows["path"].ToString()

                };
                portrait.Add(img);
            }
            con.Close();
            return portrait;
        }

        //save Multimedia
        public int CreateMultimedia(Multimedia mult)
        {
            Conectar();

            SqlCommand addEvent = new SqlCommand("insert into tblmultimedia(fileName, path) values (@fileName, @path)", con);
            addEvent.Parameters.Add("@fileName", SqlDbType.VarChar);
            addEvent.Parameters.Add("@path", SqlDbType.VarChar);
            addEvent.Parameters["@fileName"].Value = mult.FileName;
            addEvent.Parameters["@path"].Value = mult.Path;

            con.Open();
            int r = addEvent.ExecuteNonQuery();
            con.Close();

            SqlCommand com = new SqlCommand("select max(idMultimedia) from tblmultimedia", con);
            con.Open();
            int idMultimedia = Convert.ToInt32(com.ExecuteScalar());
            con.Close();

            if (r == 1)
            {
                return idMultimedia ;
            }
            else
            {
                return 0;
            }
            
        }

        //save relationship event -> image
        public int saveEventMult(int idEvt, int idMult)
        {
            Conectar();

            SqlCommand evtMult = new SqlCommand("insert into tbleventmultimedia(idEvent, idMultimedia) values (@idEvent, @idMultimedia)", con);
            evtMult.Parameters.Add("@idEvent", SqlDbType.Int);
            evtMult.Parameters.Add("@idMultimedia", SqlDbType.Int);
            evtMult.Parameters["@idEvent"].Value = idEvt;
            evtMult.Parameters["@idMultimedia"].Value = idMult;

            con.Open();
            int r = evtMult.ExecuteNonQuery();
            con.Close();

            return r;

        }

        //get images for ID event
        public List<Multimedia> getImages(int idEvent)
        {
            Conectar();
            List<Multimedia> album = new List<Multimedia>();

            string query = @"SELECT tblmultimedia.idMultimedia as idMultimedia, tblevent.idEvent as idEvent, tblmultimedia.fileName as fileName, tblmultimedia.path as path
                            FROM tblevent
                            INNER JOIN tbleventmultimedia ON tblevent.idEvent = tbleventmultimedia.idEvent
                            INNER JOIN tblactivity ON tblactivity.idActivity = tblevent.idEvent
                            INNER JOIN tblmultimedia ON tblmultimedia.idMultimedia = tbleventmultimedia.idMultimedia
                            WHERE tblevent.idEvent = @idEvent";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@idEvent", SqlDbType.Int);
            com.Parameters["@idEvent"].Value = idEvent;
            con.Open();
            SqlDataReader rows = com.ExecuteReader();
            while (rows.Read())
            {
                Multimedia img = new Multimedia()
                {
                    IdMultimedia = int.Parse(rows["idMultimedia"].ToString()),
                    IdEvent = int.Parse(rows["idEvent"].ToString()),
                    FileName = rows["fileName"].ToString(),
                    Path = "/UploadedFiles/" + rows["fileName"].ToString()

                };
                album.Add(img);
            }
            return album;
        }

        //get images for 
    }
}
