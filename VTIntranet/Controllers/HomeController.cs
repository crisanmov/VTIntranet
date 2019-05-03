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
            
            TagHelper th = new TagHelper();
            NoticeHelper nh = new NoticeHelper();

            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(nh.getAllNotice());

            ViewBag.tags = th.getTagProfile(1);
            ViewBag.news = nh.getAllNotice();
            ViewBag.news2 = serializedResult;

            var userName = this.Session["userName"];
            ViewBag.UserName = userName;

            return View();
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

                    EventHelper eh = new  EventHelper();
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
                foreach(var file in filesPost)
                {
                    if(file.ContentLength > 0)
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

        [HttpPost]
        public JsonResult Create(User user)
        {
            return Json("Response from Create");
        }

        public ActionResult Events()
        {
            ViewBag.Message = "Eventos";
            //tags
            TagHelper mt = new TagHelper();
            ViewBag.tags = mt.getTagProfile(1);
            //events
            EventHelper eh = new EventHelper();
            ViewBag.events = eh.getAllEvent();
            //ViewBag.test = "Hola";
            var userName = this.Session["userName"];
            ViewBag.UserName = userName;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Parent()
        {
            ViewBag.Message = "Parent Partial View";

            //tags
            TagHelper mt = new TagHelper();
            ViewBag.tags = mt.getTagProfile(1);
            
            return View();
        }

        [ChildActionOnly]
        public ActionResult Display(string name)
        {
            return Content("Child Action " + name);
        }

        /*
        [ChildActionOnly]
        public ActionResult Chat()
        {
            //tags
            TagHelper mt = new TagHelper();
            ViewBag.tags = mt.getTagProfile(1);

            var userName = this.Session["userName"];
            ViewBag.UserName = userName;

            return View();        }

        public ActionResult Chat()
        {
            //tags
            TagHelper mt = new TagHelper();
            ViewBag.tags = mt.getTagProfile(1);

            var userName = this.Session["userName"];
            ViewBag.UserName = userName;
            
            return View();
        }*/

        public ActionResult Volunteer()
        {
            //tags
            TagHelper mt = new TagHelper();
            ViewBag.tags = mt.getTagProfile(1);
            return View();
        }
    }
}