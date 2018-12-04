
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using HEGIC.TPClaims.Models;
using HEGIC.TPClaims.Controllers;

namespace HEGIC.TPClaims.Filter
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class TPClaimsAuthorize : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext != null)
            {
                var baseController = (BaseController)filterContext.Controller;

                if (!filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any() && !baseController.SessionStore.ItemExists(SessionKeys.USERDETAILS))
                {
                    if (baseController.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult
                        {
                            Data = new
                            {
                                Status = AjaxResponseStatusCodes.Error,
                                RedirectTo = "/Error/SessionExpired"
                            },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                                new RouteValueDictionary {
                                                            { "Controller", "Login" },
                                                            { "Action", "Index" },
                                                            {"returnUrl",""}
                                                     });
                    }
                }
            }
        }
    }
}
