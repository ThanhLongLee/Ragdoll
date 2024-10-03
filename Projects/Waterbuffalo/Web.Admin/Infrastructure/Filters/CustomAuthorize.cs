using Unity.Common.Configuration;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Admin.Infrastructure.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly List<AccountRole> _acceptTypes;
        public CustomAuthorizeAttribute(AccountRole[] types = null)
        {
            _acceptTypes = new List<AccountRole>();
            if (types != null)
                _acceptTypes = types.ToList();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                if (_acceptTypes != null && _acceptTypes.Any())
                {
                    var user = (System.Security.Claims.ClaimsIdentity)httpContext.User.Identity;
                    var userRoles = user.FindFirstValue("UserRole");
                    Enum.TryParse(userRoles, out AccountRole accountRole);
                    if (accountRole == AccountRole.Admin)
                        return true;

                    if (!_acceptTypes.Contains(accountRole))
                        return false;
                }
            }

            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var result = new ViewResult()
                {
                    ViewName = "~/Views/HttpErrors/AccessDenied.cshtml"
                };
                filterContext.Result = result;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Account",
                            action = "SignIn",
                            area = "",
                            returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                        }));
            }
        }
    }

}