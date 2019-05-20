using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VTIntranet.Models;

namespace VTIntranet.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index()
        {
            //serializer for brands
            string id = this.Session["idUser"].ToString();
            int idUser = int.Parse(id);
            TagHelper th = new TagHelper();

            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idUser));
            ViewBag.Navbar = serializedResultB;

            //get all events
            EventHelper eh = new EventHelper();
            var serializerEvent = new JavaScriptSerializer();
            var serializedResultE = serializerEvent.Serialize(eh.getAllEvent());
            
            ViewBag.events = serializedResultE;

            return View();
        }

        // GET: Gallery for ID}
        [HttpGet]
        public ActionResult Album(int idEvent)
        {
            //serializer for brands
            string id = this.Session["idUser"].ToString();
            int idUser = int.Parse(id);

            //get all tags
            TagHelper th = new TagHelper();
            ViewBag.tags = th.getTagProfile(1);

            //get all events
            EventHelper eh = new EventHelper();
            ViewBag.events = eh.getAllEvent();

            //get images for idEvent
            MultimediaHelper mh = new MultimediaHelper();
            //ViewBag.images = mh.getImages(idEvent);


            //serializer for brands
            //ViewBag.tags = th.getTagProfile(1);
            var serializerBrand = new JavaScriptSerializer();
            var serializedResultB = serializerBrand.Serialize(th.GetBrand(idUser));
            ViewBag.Navbar = serializedResultB;

            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(mh.getImages(idEvent));
            ViewBag.images = serializedResult;

            return View();
        }

        [HttpGet]
        public JsonResult getPortrait(String idAlbum)
        {
            System.Diagnostics.Debug.WriteLine("###############################################################");
            System.Diagnostics.Debug.WriteLine(idAlbum);
            int id = Int32.Parse(idAlbum);

            MultimediaHelper mh = new MultimediaHelper();
            var Portrait = mh.getAlbumPortriat(id);

            return Json(Portrait, JsonRequestBehavior.AllowGet);
            //return Json("");
        }
    }
}