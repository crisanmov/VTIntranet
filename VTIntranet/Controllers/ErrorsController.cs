using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VTIntranet.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult customexplain(int id)
        {
            string message = "";
            switch (id)
            {
                case 1:
                    message = "No existe el archivo";
                    break;

                default:
                    break;
            }

            ViewBag.message = message;

            return View();
        }
    }
}