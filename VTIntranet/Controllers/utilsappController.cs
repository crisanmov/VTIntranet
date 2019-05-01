using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTIntranet.Repository;
using VTIntranet.Services;
using VTIntranet.Services.Implements;
using VTIntranet.Utils;

namespace VTIntranet.Controllers
{
    public class utilsappController : ControllerBase
    {
        private IntranetRepository unit;
        private AccountServices accountServices;
        private IntranetService services;

        public utilsappController()
        {
            unit = new IntranetRepository();
            accountServices = new AccountServices();
            services = new IntranetService();
        }

        // GET: utilsapp
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Navbar()
        {
            var iduser = this.accountServices.getUserId();
            var menu = this.unit.UsersPermissionsRepository.Get(y => y.idUser == iduser && y.tblpermissions.permissionIsHtml == true && y.userPermissionActive == Constantes.activeRecord).Select(t => t.tblpermissions).ToList();

            // var items = data.navbarItems().Where(x => x.isHtml == true).ToList();

            return PartialView("_Navbar", menu);
        }

        public ActionResult downloadbinaryDocument(int id)
        {
            try
            {
                var data = this.services.getAttachmentById(id);
                var exist = System.IO.File.Exists(Server.MapPath(data.attachmentUrl));

                if (!exist)
                {
                    _Log.Info(string.Concat("File does not exists.", data.attachmentName));
                    return RedirectToAction("customexplain", "Errors",new { id= 1});
                }
                else
                {
                    _Log.Info(string.Concat("Generated file. {0}", data.attachmentName));
                    return File(data.attachmentUrl, string.Concat("{0};charset=UTF-8;base64", data.attachmentContentType), data.attachmentName);
                }
                
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get file, utilsappController.downloadbinaryDocument, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }


    }
}