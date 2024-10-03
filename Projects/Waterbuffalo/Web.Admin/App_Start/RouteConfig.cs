using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            const string regexGuid = "^[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}$";
            const string regexInt = "^[0-9]*$";

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            //===============================================
            //                  *HOME*
            //===============================================
            routes.MapRoute("Home", "", new
            {
                controller = "Home",
                action = "Index",
            }, namespaces: new[] { "Web.Admin.Controllers" });

            //===============================================
            //                  *ACCOUNT*
            //===============================================

            // Sign In
            routes.MapRoute("AccountSignIn", "sign-in", new
            {
                controller = "Account",
                action = "SignIn",
            }, namespaces: new[] { "Web.Admin.Controllers" });

            // Sign Out
            routes.MapRoute("AccountSignOut", "sign-out", new
            {
                controller = "Account",
                action = "SignOut",
            }, namespaces: new[] { "Web.Admin.Controllers" });



            //===============================================
            //                  *Permission*
            //===============================================

            routes.MapRoute("GroupRoleList", "group-role/list", new
            {
                controller = "GroupRole",
                action = "ListGroupRole",
            }, namespaces: new[] { "Web.Admin.Controllers" });


            routes.MapRoute("GroupRoleInsert", "group-role/add-new", new
            {
                controller = "GroupRole",
                action = "InsertGroupRole",
            }, namespaces: new[] { "Web.Admin.Controllers" });


            routes.MapRoute("GroupRoleUpdate", "group-role/update/{id}", new
            {
                controller = "GroupRole",
                action = "UpdateGroupRole",
            }, constraints: new { id = regexGuid }, namespaces: new[] { "Web.Admin.Controllers" });



            //===============================================
            //                  *EMPLOYEE*
            //===============================================
            routes.MapRoute("EmployeeList", "employee", new
            {
                controller = "Employee",
                action = "List",
            }, namespaces: new[] { "Web.Admin.Controllers" });

            routes.MapRoute("EmployeeAdd", "employee/new", new
            {
                controller = "Employee",
                action = "Add",
            }, namespaces: new[] { "Web.Admin.Controllers" });

            routes.MapRoute("EmployeeUpdate", "employee/update/{id}", new
            {
                controller = "Employee",
                action = "Update",
            }, constraints: new { id = regexGuid }, namespaces: new[] { "Web.Admin.Controllers" });


            routes.MapRoute("EmployeeProfileUser", "employee/profile", new
            {
                controller = "Employee",
                action = "ProfileUser",
            }, namespaces: new[] { "Web.Admin.Controllers" });

            routes.MapRoute("EmployeeProfileUserChangePassword", "employee/profile/change-password", new
            {
                controller = "Employee",
                action = "ChangePasswordByUserSelf",
            }, namespaces: new[] { "Web.Admin.Controllers" });

            //===============================================
            //                  *EDITION*
            //===============================================
            routes.MapRoute("EditionList", "edition-list", new
            {
                controller = "Edition",
                action = "List",
            }, namespaces: new[] { "Web.Admin.Controllers" });


            //=================================================
            //                  *ERROR PAGES*
            //=================================================
            routes.MapRoute(
                name: "ErrorCode",
                url: "error",
                defaults: new { controller = "HttpErrors", action = "ErrorCode" }
                , namespaces: new[] { "Web.Admin.Controllers" });


            routes.MapRoute(
                name: "NotFound",
                url: "not-found",
                defaults: new { controller = "HttpErrors", action = "NotFound" }
                , namespaces: new[] { "Web.Admin.Controllers" });

            routes.MapRoute(
                    name: "NotFoundUrl",
                    url: "{*url}",
                    defaults: new { controller = "HttpErrors", action = "NotFound" }
                    , namespaces: new[] { "Web.Admin.Controllers" });

        }
    }
}
