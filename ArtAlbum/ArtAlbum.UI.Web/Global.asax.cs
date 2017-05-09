using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ArtAlbum.UI.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // создание ролей и администратора по умлочанию, если они отсутствуют в бд
            if (RoleVM.GetAllRoles().FirstOrDefault(role => role == "User") == null)
            {
                RoleVM.Add("User");
            }
            if (RoleVM.GetAllRoles().FirstOrDefault(role => role == "Admin") == null)
            {
                RoleVM.Add("Admin");
            }
            if (UserVM.GetAllUsers().Count() == 0)
            {
                UserVM.Add(new RegisterVM() { Nickname = "admin", FirstName = "admin", LastName = "admin", Password = "Qwerty1234", ConfirmPassword = "Qwerty1234", DateOfBirth = new DateTime(2000, 1, 1), Email = "admin@admin.com" });
                RoleVM.AddRoleToUser("admin", "Admin");
            }
        }
    }
}
