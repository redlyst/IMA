﻿using PowerAppsCMS.CustomAuthentication;
using PowerAppsCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PowerAppsCMS.CustomAuthentication
{
    /// <summary>
    /// Atribut otorisasi user untuk keamanan User yang diakses pada Controller
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Mengembalikan data User yang sedang menggnakan aplikasi/login
        /// </summary>
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        /// <summary>
        /// Method AuthorizeCore, method inti dari otorisasi terhadap User untuk pengecekan apakah sebuah Username termasuk ke dalam salah satu Role
        /// </summary>
        /// <param name="httpContext">Parameter httpContext yang didapat dari browser</param>
        /// <returns>Return 'True' jika sebuah Username termasuk ke dalam sebuah Role, 'False' jika tidak</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //bool val = false;

            //if (HttpContext.Current.User.Identity.Name != null)
            //{
            //    using (PowerAppsCMSEntities dbContext = new PowerAppsCMSEntities())
            //    {
            //        string userNoDomain = HttpContext.Current.User.Identity.Name.Split('\\')[1];
            //        var user = dbContext.Users.Where(a => a.Username.Equals(userNoDomain)).FirstOrDefault();//.Role.Name;

            //        string[] roles = Roles.Split(',');

            //        if(roles.Count() > 1)
            //        {
            //            foreach(var item in roles)
            //            {
            //                if (user != null && item.Trim().Equals(user.Role.Name) && !val)
            //                    val = true;
            //            }
            //        }

            //        //if (user != null && Roles.Contains(user.Role.Name))
            //        //    val = true;
            //    }
            //}

            //return val;

            return ((CurrentUser != null && !CurrentUser.IsInRole(Roles)) || CurrentUser == null) ? false : true;
        }

        /// <summary>
        /// Method HandleUnauthorizedRequest, method handling untuk mengembalikan Login Error
        /// </summary>
        /// <param name="filterContext">Parameter filterContext yang didapat dari browser</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;

            if (CurrentUser == null)
            //if (HttpContext.Current.User.Identity.Name == null)
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
                        //controller = "Error",
                        //action = "Unauthorized",
                        controller = "Account",
                        action = "Login",
                        errorMessage = "Error"
                    }
                    ));
            }
            else
            {
                routeData = new RedirectToRouteResult
                (new System.Web.Routing.RouteValueDictionary
                 (new
                 {
                     controller = "Error",
                     action = "AccessDenied"
                 }
                 ));
            }

            filterContext.Result = routeData;
        }

    }
}