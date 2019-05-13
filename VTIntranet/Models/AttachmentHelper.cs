using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace VTIntranet.Models
{
    public class AttachmentHelper
    {
        private SqlConnection con;

        //connection db
        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["intranetContext"].ToString();
            con = new SqlConnection(constr);
        }

        //save attachment
        public int CreateAttachment(Attachment file)
        {
            Conectar();

            string query = @"insert into tblattachments(attachmentName, attachmentDirectory, attachmentActive) 
                            values (@attachmentName, @attachmentDirectory, @attachmentActive)";

            SqlCommand addAttach = new SqlCommand(query, con);
            addAttach.Parameters.Add("@attachmentName", SqlDbType.VarChar);
            addAttach.Parameters.Add("@attachmentDirectory", SqlDbType.VarChar);
            addAttach.Parameters.Add("@attachmentActive", SqlDbType.VarChar);
            addAttach.Parameters["@attachmentName"].Value = file.AttachmentName;
            addAttach.Parameters["@attachmentDirectory"].Value = file.AttachmentDirectory;
            addAttach.Parameters["@attachmentActive"].Value = "1";

            con.Open();
            int result = addAttach.ExecuteNonQuery();
            con.Close();

            if (result == 1)
            {
                SqlCommand com = new SqlCommand("select max(idAttachment) from tblattachments", con);
                con.Open();
                int idAttachment = Convert.ToInt32(com.ExecuteScalar());
                con.Close();

                return idAttachment;
            }
            else { return 0; }



        }

        //create relationship attachment, tag and department
        public int SaveAttachTagDepto(int attachment, int tag, int depto)
        {
            string a = "node";
            Conectar();

            string query = "INSERT INTO tblattachmentstags(idAttachment, idTag, idDepto, attachmentTagsActive) values (@idAttachment, @idTag, @idDepto, @attachmentTagsActive)";
            SqlCommand attachTagDepto = new SqlCommand(query, con);
            attachTagDepto.Parameters.Add("@idAttachment", SqlDbType.Int);
            attachTagDepto.Parameters.Add("@idTag", SqlDbType.Int);
            attachTagDepto.Parameters.Add("@idDepto", SqlDbType.Int);
            attachTagDepto.Parameters.Add("@attachmentTagsActive", SqlDbType.VarChar);
            attachTagDepto.Parameters["@idAttachment"].Value = attachment;
            attachTagDepto.Parameters["@idTag"].Value = tag;
            attachTagDepto.Parameters["@idDepto"].Value = depto;
            attachTagDepto.Parameters["@attachmentTagsActive"].Value = "1";

            con.Open();
            int r = attachTagDepto.ExecuteNonQuery();
            con.Close();

            return r;
        }

        //get idTag from tblrags
        public int GetIdTag(string clabe)
        {
            Conectar();

            string query = @"SELECT idTag FROM tbltags WHERE clabe = @clabe";
            SqlCommand idTag = new SqlCommand(query, con);
            idTag.Parameters.Add("@clabe", SqlDbType.VarChar);
            idTag.Parameters["@clabe"].Value = clabe;

            con.Open();
            int r = Convert.ToInt32(idTag.ExecuteScalar());
            con.Close();

            return r;
        }

        //get idDepto from tbldepto
        public int GetIdDepto(string clabe)
        {
            Conectar();

            string query = @"SELECT idDepto FROM tbldepto WHERE clabe = @clabe";
            SqlCommand idDepto = new SqlCommand(query, con);
            idDepto.Parameters.Add("@clabe", SqlDbType.VarChar);
            idDepto.Parameters["@clabe"].Value = clabe;

            con.Open();
            int r = Convert.ToInt32(idDepto.ExecuteScalar());
            con.Close();

            return r;
        }

        //get attachments for clabe of Tag
        public List<Attachment> GetAttachments(String tagClabe)
        {
            Conectar();
            List<Attachment> attachments = new List<Attachment>();

            string query = @"SELECT tblattachments.idAttachment, tblattachments.attachmentName, tbltags.tagName, tblattachments.attachmentDirectory
                            FROM tblattachments
	                            INNER JOIN tblattachmentstags ON tblattachmentstags.idAttachment = tblattachments.idAttachment
	                            INNER JOIN tbltags ON tbltags.idTag = tblattachmentstags.idTag
                            WHERE tbltags.clabe = @tagClabe";

            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.Add("@tagClabe", SqlDbType.VarChar);
            com.Parameters["@tagClabe"].Value = tagClabe;

            con.Open();
            SqlDataReader registros = com.ExecuteReader();
            while (registros.Read())
            {
                Attachment attach = new Attachment()
                {
                    IdAttachment = int.Parse(registros["idAttachment"].ToString()),
                    AttachmentName = registros["attachmentName"].ToString(),
                    AttachmentDirectory = registros["attachmentDirectory"].ToString(),
                    TagName = registros["tagName"].ToString(),
                    //AttachmentActive = registros["attachmentActive"].ToString()
                };

                attachments.Add(attach);
            }

            con.Close();
            return attachments;
        }
    }
}