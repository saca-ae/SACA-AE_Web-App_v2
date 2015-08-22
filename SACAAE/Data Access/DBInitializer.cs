using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Data_Access
{
    public class DBInitializer
    {
        public static void init(SACAAEContext context)
        {
            var userManager = new UserManager<User>(new UserStore<User>(context));

            const string usernameAdmin = "amasis";
            const string emailAdmin = "amasis1@itcr.ac.cr";
            const string nameAdmin = "Alejandro Masís";
            const string passAdmin = "sacapassword";
            //Admin
            var admin = userManager.FindByName(usernameAdmin);
            if (admin == null)
            {
                admin = new User { UserName = usernameAdmin, Email = emailAdmin, Name = nameAdmin };
                var result = userManager.Create(admin, passAdmin);
                result = userManager.SetLockoutEnabled(admin.Id, false);
            }

        }
    }
}