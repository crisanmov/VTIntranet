﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VTIntranet.intranetdb;


using VTIntranet.Models;

namespace VTIntranet.Controllers
{
    public class HomeController : Controller
    {
        ManageModelNew noticeDB = new ManageModelNew();

        public ActionResult Index()
        {
            var userName = this.Session["userName"];
            string id = this.Session["idProfile"].ToString();
            int idProfile = int.Parse(id);

            ViewBag.UserName = userName;
            ViewBag.IdProfile = idProfile;

            TagHelper th = new TagHelper();
            NoticeHelper nh = new NoticeHelper();

            //serializer for notices "calendar"
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(nh.getAllNotice());
            ViewBag.news2 = serializedResult;

            //notices for grid view
            ViewBag.news = nh.getAllNotice();

            //serializer for brands
            //ViewBag.tags = th.getTagProfile(1);
            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idProfile));
            ViewBag.Navbar = serializedResultB;
            
            return View();
        }

        //Attachments
        [HttpGet]
        public ActionResult Attachment()
        {
            //serializer for brands
            string id = this.Session["idUser"].ToString();
            int idUser = int.Parse(id);
            TagHelper th = new TagHelper();

            var tag = Request["tag"];
            ViewBag.Tag = tag;
            //get deptos
            var serializerDepto = new JavaScriptSerializer();
            var serializedResultD = serializerDepto.Serialize(th.GetDepto(tag));
            ViewBag.D = serializedResultD;

            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idUser));
            ViewBag.Navbar = serializedResultB;

            return View();

        }

        [HttpPost]
        public JsonResult DeleteAttach(String idAttach, String tagClabe, String idDepto, String fileName)
        {
            int idAttachment = int.Parse(idAttach);
            int idDep = int.Parse(idDepto);

            AttachmentHelper ah = new AttachmentHelper();
            int idTag = ah.GetIdTag(tagClabe);

            //delete references tblAttachmentstags
            int delR = ah.DeleteAttachRelation(idAttachment, idTag, idDep);
            //delete from tblAttachments
            int delA = ah.DeleteAttach(idAttachment);
            //delete file from Path
            string _path = Path.Combine(Server.MapPath("~/UploadedFiles/attachments/"), fileName);
            System.IO.File.Delete(_path);

            if (delR == 1 && delA == 1)
            {
                return Json("successfully");
            }
            else
            {
                return Json("Error");
            }
            
        }

        [HttpGet]
        public JsonResult GetAttachment(String tagClabe, String idAttachment)
        {

            
            return Json("successfully");
        }

        [HttpGet]
        public JsonResult GetFilesAttach(String tagClabe)
        {

            AttachmentHelper ah = new AttachmentHelper();
            //get attachments
            var serializerAttach = new JavaScriptSerializer();
            var serializedResultA = serializerAttach.Serialize(ah.GetAttachments(tagClabe));
            //ViewBag.Attachments = serializedResultA;

            return Json(serializedResultA, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getNotice(String idNotice)
        {
            System.Diagnostics.Debug.WriteLine("###############################################################");
            System.Diagnostics.Debug.WriteLine(idNotice);
            int id = Int32.Parse(idNotice);

            //ManageModelNew mn = new ManageModelNew();
            //var Notice = mn.getNotice(id);
            NoticeHelper nh = new NoticeHelper();
            var Notice = nh.getNotice(id);

            return Json(Notice, JsonRequestBehavior.AllowGet);
            //return Json("");
        }

        [HttpGet]
        public JsonResult GetDeptos(String brand)
        {
            TagHelper th = new TagHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(th.GetDepto(brand));

            return Json(serializedResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTags()
        {
            TagHelper th = new TagHelper();
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(th.gettAll());

            return Json(serializedResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTagName(String[] Tags)
        {

            var objects = JsonConvert.DeserializeObject<List<object>>(Tags[0]);
            //var result = objects.Select(obj => JsonConvert.SerializeObject(obj)).ToArray();

            TagHelper th = new TagHelper();
            var myList = new List<string>();

            foreach(string field in objects)
            {
                myList.Add(th.GetBrand(field));
            }
           

            
            return Json("successfully");
        }

        [HttpPost]
        public JsonResult SaveNotice(Notice notice)
        {
            Activity a = new Activity
            {
                Title = notice.Title,
                Description = notice.Description,
                Date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
            };

            Notice n = new Notice()
            {
                StartDateNotice = notice.StartDateNotice,
                EndDateNotice = notice.EndDateNotice,
            };

            NoticeHelper nh = new NoticeHelper();
            nh.CreateActivity(a, n);

            return Json("successfully");

        }

        [HttpPost]
        public ActionResult SaveEvent(HttpPostedFileBase filePost)
        {
            try
            {
                if (filePost.ContentLength > 0)
                {
                    string title = Convert.ToString(Request["title"]);
                    string url = Convert.ToString(Request["url"]);
                    string description = Convert.ToString(Request["description"]);

                    System.Diagnostics.Debug.WriteLine(title);
                    System.Diagnostics.Debug.WriteLine(url);
                    System.Diagnostics.Debug.WriteLine(description);

                    string _FileName = Path.GetFileName(filePost.FileName);

                    StringBuilder builder = new StringBuilder();
                    Random random = new Random();
                    char ch;
                    for (int i = 0; i < _FileName.Length; i++)
                    {
                        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                        builder.Append(ch);
                    }
                    string fileName = builder.ToString().ToLower();
                    fileName = fileName + ".jpg";
                    System.Diagnostics.Debug.WriteLine(fileName);

                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    filePost.SaveAs(_path);

                    Event evt = new Event
                    {
                        Title = title,
                        Description = description,
                        FileName = fileName,
                        Path = _path,
                        Url = url
                    };

                    EventHelper eh = new EventHelper();
                    eh.CreateActivity(evt);

                }

                return Json(new { success = true, responseText = "succesfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }

        }

        [HttpPost]
        public ActionResult SaveAlbum(IEnumerable<HttpPostedFileBase> filesPost)
        {
            try
            {
                foreach (var file in filesPost)
                {
                    if (file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(_FileName);
                        string idAlb = Convert.ToString(Request["idAlbum"]);
                        int idMultimedia = 0;

                        StringBuilder builder = new StringBuilder();
                        Random random = new Random();
                        char ch;
                        for (int i = 0; i < _FileName.Length; i++)
                        {
                            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                            builder.Append(ch);
                        }
                        string fileName = builder.ToString().ToLower();
                        fileName = fileName + extension;
                        System.Diagnostics.Debug.WriteLine(fileName);

                        string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                        file.SaveAs(_path);

                        Multimedia mult = new Multimedia
                        {
                            FileName = fileName,
                            Path = _path
                        };

                        MultimediaHelper mh = new MultimediaHelper();
                        idMultimedia = mh.CreateMultimedia(mult);
                        var idAlbum = int.Parse(idAlb);
                        mh.saveEventMult(idAlbum, idMultimedia);

                    }
                }

                return Json(new { success = true, responseText = "succesfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }

        }

        [HttpPost]
        public ActionResult SaveFilePdf(IEnumerable<HttpPostedFileBase> attachmentPost)
        {
            try
            {
                foreach(var file in attachmentPost)
                {
                    if (file.ContentLength > 0)
                    {
                        String FileExt = Path.GetExtension(file.FileName).ToUpper();

                        if(FileExt == ".PDF")
                        {
                            string title_temp = Convert.ToString(Request["title"]);
                            string fileClabe = Convert.ToString(Request["fileClabe"]);
                            string title = title_temp + "-" + fileClabe + Path.GetExtension(file.FileName);
                            int idAttachment = 0;

                            string _path = Path.Combine(Server.MapPath("~/UploadedFiles/attachments/"), title);
                            file.SaveAs(_path);

                            /*Stream str = file.InputStream;
                            BinaryReader br = new BinaryReader(str);
                            Byte[] FileDet = br.ReadBytes((Int32)str.Length);*/

                            Attachment attachment = new Attachment()
                            {
                                AttachmentName = title,
                                AttachmentDirectory = _path,
                                
                            };

                            AttachmentHelper at = new AttachmentHelper();
                            //save references attachment
                            idAttachment = at.CreateAttachment(attachment);

                            string tag = Convert.ToString(Request["brandClabe"]);
                            string depto = Convert.ToString(Request["deptoClabe"]);

                            int idTag = at.GetIdTag(tag);
                            int idDepto = at.GetIdDepto(depto);


                            //create relationship
                            at.SaveAttachTagDepto(idAttachment, idTag, idDepto);


                        }

                    }
                }

                return Json("successfully");
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }

        

        //********************** Order functions *****************

        public ActionResult About()
        {
            //serializer for brands
            string id = this.Session["idUser"].ToString();
            int idUser = int.Parse(id);
            TagHelper th = new TagHelper();

            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idUser));
            ViewBag.Navbar = serializedResultB;
            return View();
        }

        public ActionResult Contact()
        {
            //serializer for brands
            string id = this.Session["idUser"].ToString();
            int idUser = int.Parse(id);
            TagHelper th = new TagHelper();

            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idUser));
            ViewBag.Navbar = serializedResultB;
            return View();
        }

        [HttpPost]
        public JsonResult Create(User user)
        {
            return Json("Response from Create");
        }

        public ActionResult Directory()
        {
            //serializer for brands
            string id = this.Session["idUser"].ToString();
            int idUser = int.Parse(id);
            TagHelper th = new TagHelper();

            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idUser));
            ViewBag.Navbar = serializedResultB;

            return View();
        }

        public ActionResult Events()
        {
            //serializer for brands
            string id = this.Session["idUser"].ToString();
            int idUser = int.Parse(id);
            TagHelper th = new TagHelper();

            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idUser));
            ViewBag.Navbar = serializedResultB;

            //events
            EventHelper eh = new EventHelper();
            ViewBag.events = eh.getAllEvent();
            var userName = this.Session["userName"];
            ViewBag.UserName = userName;

            return View();
        }

        public ActionResult Post()
        {
            //serializer for brands
            string id = this.Session["idUser"].ToString();
            int idUser = int.Parse(id);
            TagHelper th = new TagHelper();

            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idUser));
            ViewBag.Navbar = serializedResultB;

            return View();
        }

        public ActionResult Talend()
        {
            //serializer for brands
            string id = this.Session["idUser"].ToString();
            int idUser = int.Parse(id);
            TagHelper th = new TagHelper();

            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idUser));
            ViewBag.Navbar = serializedResultB;
            return View();
        }

        public ActionResult Volunteer()
        {
            //serializer for brands
            string id = this.Session["idUser"].ToString();
            int idUser = int.Parse(id);
            TagHelper th = new TagHelper();

            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idUser));
            ViewBag.Navbar = serializedResultB;
            return View();
        }

        
    }
}