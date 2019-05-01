using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using VTIntranet.Utils;

namespace VTIntranet.Controllers
{
    public class ControllerBase:Controller
    {
        private string ActionKey;
        private string Controller;

        List<string> permissions = new List<string>();
        string message = "You do not have permission to perform this action";


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ActionKey = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "-" + filterContext.ActionDescriptor.ActionName;
            Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;


            switch (ActionKey)
            {
                case "Account-Login":
                    if (Request.IsAuthenticated) filterContext.Result = RedirectToAction("Index", "Home");
                    // filterContext.Result = RedirectToAction("Login", "Account");
                    break;
                case "Account-LogOff":
                    // filterContext.Result = RedirectToAction("Login", "LogOff");
                    break;
                default:

                    if (Request.IsAuthenticated)
                    {

                        var identity = (ClaimsIdentity)User.Identity;
                        IEnumerable<Claim> claims = identity.Claims;

                        var permissionsClaims = claims.Where(t => t.Type == "userpermission").ToList(); // get All permissions

                        if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                        { // Check for authorization

                            // Evaluando si es petición Ajax
                            if (filterContext.HttpContext.Request.IsAjaxRequest())
                            {
                                if (permissionsClaims.Where(t => t.Value == ActionKey).FirstOrDefault() == null)
                                {
                                    filterContext.Result = Json(JsonSerialResponse.ResultError(message), JsonRequestBehavior.AllowGet); return;
                                }
                            }
                            else // De lo contrario es petición de Vista
                            {
                                if (permissionsClaims.Where(t => t.Value == ActionKey).FirstOrDefault() == null)
                                {
                                    filterContext.Result = RedirectToAction("NoAccess", "Account"); return;
                                }
                            }
                        }

                    }
                    else
                    {

                        filterContext.Result = RedirectToAction("Login", "Account");
                    }
                    break;

            }





        }
    }
}