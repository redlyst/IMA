﻿using System;
//using CustomAuthenticationMVC.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using PowerAppsCMS.Models;

namespace PowerAppsCMS.CustomAuthentication
{
    /// <summary>
    /// Propertis membership yang digunakan pada kemananan User
    /// </summary>
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        /// <summary>
        /// Propertis yang digunakan dalam identitas login User
        /// </summary>
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Roles { get; set; }

        #endregion

        #region UnUsed

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public CustomMembershipUser(User user) : base("CustomMembership", user.Username, user.ID, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.ID;
            Name = user.Name;
            Roles = user.Role.Name;
        }

        #endregion

    }
}