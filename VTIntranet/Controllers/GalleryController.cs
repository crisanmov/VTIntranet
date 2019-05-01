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
            //get all tags
            TagHelper th = new TagHelper();
            ViewBag.tags = th.getTagProfile(1);

            //get all events
            EventHelper eh = new EventHelper();
            ViewBag.events = eh.getAllEvent();

            return View();
        }

        // GET: Gallery for ID}
        [HttpGet]
        public ActionResult Album(int idEvent)
        {
            //get all tags
            TagHelper th = new TagHelper();
            ViewBag.tags = th.getTagProfile(1);

            //get all events
            EventHelper eh = new EventHelper();
            ViewBag.events = eh.getAllEvent();

            //get images for idEvent
            MultimediaHelper mh = new MultimediaHelper();
            //ViewBag.images = mh.getImages(idEvent);

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