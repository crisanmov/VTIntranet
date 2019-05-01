using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTIntranet.intranetdb;
using VTIntranet.Models;
using VTIntranet.Models.Identity;
using VTIntranet.Services.Implements;
using VTIntranet.Utils;

namespace VTIntranet.Controllers
{
    public class usersController : ControllerBase
    {
        private AccountServices accountServices;
        private FormControlServices formservice;

        public usersController()
        {
            accountServices = new AccountServices();
            formservice = new FormControlServices();
        }

        // GET: users
        public ActionResult Index(int? page, int? pageSize)
        {
            List<usersModelHelper> lst = accountServices.getAllUser();
            IQueryable<usersModelHelper> query = lst.AsQueryable();

            int pSize = (pageSize ?? 15);
            int pageNumber = (page ?? 1);
            ViewBag.psize = pSize;

            IPagedList<usersModelHelper> user = null;

            user = query.ToPagedList(pageNumber, pSize);

            return View(user);
        }

        [AllowAnonymous]
        public ActionResult Register(int? idUser,int? idProfile)
        {
            ViewBag.idUser = idUser;
            ViewBag.idProfile = idProfile;
            var _idProfile = this.accountServices.getProfile();
            ViewBag._ProfileID = _idProfile;
            return View();
        }
        
        // GET: /Account/Register
        [HttpPost]
        public JsonResult RegisterUser(RegisterUserViewModel model,List<tbluserspermissions> userPermissionView, List<tblprofilestags> profileTagView)
        {
            try
            {
                var user = this.accountServices.AddUser(model, userPermissionView, profileTagView);
                if(user != 0)
                {
                    return Json(JsonSerialResponse.ResultSuccess(user, "Query done correctly"),JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonSerialResponse.ResultRegisterNotFound("No data"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _Log.Error("Unable to save user , usersController.RegisterUser, Error ->", ex);
                return Json(JsonSerialResponse.ResultError("Error ->" + ex.Message + "[Stack-trace]" + ex.StackTrace), JsonRequestBehavior.AllowGet);
            }

        }

        [AllowAnonymous]
        public JsonResult getProfiles()
        {
            try
            {
                return Json(JsonSerialResponse.ResultSuccess(formservice.SelectProfile(), "Query done correctly"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get profile list , usersController.getProfiles, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }

        }

        [AllowAnonymous]
        public JsonResult getInfoUser(int idUser)
        {
            try
            {
                return Json(JsonSerialResponse.ResultSuccess(this.accountServices.getInfoUser(idUser), "Query done correctly"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get info the user info, usersController.getProfiles, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public JsonResult getPermission()
        {
            try
            {
                return Json(JsonSerialResponse.ResultSuccess(this.accountServices.getPermissions(), "Query done correctly"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get permissions list, usersController.getPermission, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public JsonResult getTags()
        {
            try
            {
                return Json(JsonSerialResponse.ResultSuccess(this.accountServices.getTags(), "Query done correctly"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get tags list, usersController.getTags, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult getPermissionsByidUser(int idUser)
        {
            try
            {
                return Json(JsonSerialResponse.ResultSuccess(this.accountServices.getPermissionsByidUser(idUser), "Query done correctly"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get permissions list by user, usersController.getPermissionsByidUser, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult getTagsByidProfile(int idProfile)
        {
            try
            {
                return Json(JsonSerialResponse.ResultSuccess(this.accountServices.getTagsByidProfile(idProfile), "Query done correctly"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get tag list by user, usersController.getTagsByidProfile, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult getTagsByidUser(int idUser)
        {
            try
            {
                return Json(JsonSerialResponse.ResultSuccess(this.accountServices.getTagsByidUser(idUser), "Query done correctly"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                _Log.Error("Unable to get tag list by user, usersController.getTagsByidUser, Error ->", e);
                return Json(JsonSerialResponse.ResultError("Error ->" + e.Message + "[Stack-trace]" + e.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }

    }
}