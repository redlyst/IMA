﻿using PowerAppsCMS.Models;
using Newtonsoft.Json;
using PowerAppsCMS.CustomAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Configuration;
using System.DirectoryServices;

namespace PowerAppsCMS.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return LogOut();
            //}
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                string domain = WebConfigurationManager.AppSettings["ActiveDirectoryUrl"];
                string ldapUser = loginView.UserName;// WebConfigurationManager.AppSettings["ADUsername"];
                string ldapPassword = loginView.Password;// WebConfigurationManager.AppSettings["ADPassword"];

                using (DirectoryEntry entry = new DirectoryEntry(domain, ldapUser, ldapPassword))
                {
                    try
                    {
                        if (entry.Guid == null)
                        {
                            ModelState.AddModelError("", "Username or Password invalid.");
                            return View();
                        }
                        else
                        {
                            if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                            {
                                var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                                if (user != null)
                                {
                                    CustomSerializeModel userModel = new Models.CustomSerializeModel()
                                    {
                                        UserId = user.UserId,
                                        Name = user.Name,
                                        //LastName = user.LastName,
                                        Roles = user.Roles
                                    };

                                    string userData = JsonConvert.SerializeObject(userModel);
                                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                                        (
                                        1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                                        );

                                    string enTicket = FormsAuthentication.Encrypt(authTicket);
                                    HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                                    Response.Cookies.Add(faCookie);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Sorry your account not register yet in our system, please contact the administrator to register your account.");
                                return View();
                            }
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Username or Password invalid");
                        return View();
                    }
                    //if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                    //{
                    //    var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                    //    if (user != null)
                    //    {
                    //        CustomSerializeModel userModel = new Models.CustomSerializeModel()
                    //        {
                    //            UserId = user.UserId,
                    //            Name = user.Name,
                    //            //LastName = user.LastName,
                    //            Roles = user.Roles
                    //        };

                    //        string userData = JsonConvert.SerializeObject(userModel);
                    //        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                    //            (
                    //            1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                    //            );

                    //        string enTicket = FormsAuthentication.Encrypt(authTicket);
                    //        HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                    //        Response.Cookies.Add(faCookie);
                    //    }

                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Something Wrong : Username or Password invalid ^_^ ");
            return View(loginView);
        }

        public ActionResult LogOut()
        {         
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }
    }
}