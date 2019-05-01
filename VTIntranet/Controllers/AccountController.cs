using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VTIntranet.Models;
using VTIntranet.Services.Implements;
using VTIntranet.Models.Identity;
using VTIntranet.Utils;

namespace VTIntranet.Controllers
{
    public class AccountController : ControllerBase
    {

        private AccountServices accountServices;

        private IAuthenticationManager _auth
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        public AccountController()
        {
            accountServices = new AccountServices();
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                bool passed = this.accountServices.verifyUser(model);

                if (passed)
                {

                    //save session userData
                    /*UsersHelper uh = new UsersHelper();
                    string userName = model.UserName.ToString();

                    usersModelHelper data = uh.GetUser(userName);
                    this.Session["userData"] = data;*/

                    string userName = model.UserName.ToString();
                    this.Session["userName"] = userName;

                    var identityClaims = accountServices.AddIdentityBasic(model);

                    this._auth.SignIn(new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = model.RememberMe
                    },identityClaims);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "User or password incorrect.");
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _Log.Error("Unable to login, AccountController.Login, Error -> ", e);
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

        }

        public ActionResult Manage(int? idUser)
        {
            ViewBag.idUser = idUser;
            var idProfile = this.accountServices.getProfile();
            ViewBag.idProfile = idProfile;

            return View();
        }

        [AllowAnonymous]
        public ActionResult ManageChange(RegisterUserViewModel model)
        {
            try
            {
                var user = this.accountServices.UpdatePassword(model);
                if (user == true && model.userTypeRelationship != 1)
                {
                    this._auth.SignOut();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    if (user == true)
                    {
                        return Json(JsonSerialResponse.ResultSuccess(user, "Query done correctly"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(JsonSerialResponse.ResultRegisterNotFound("Verify your password"), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(JsonSerialResponse.ResultError("Error ->" + ex.Message + "[Stack-trace]" + ex.StackTrace), JsonRequestBehavior.AllowGet);
            }
        }


        // POST: /Account/LogOff
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this._auth.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                                    DefaultAuthenticationTypes.ExternalCookie);
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult NoAccess()
        {
            return View();
        }

    }
}