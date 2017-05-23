using ArtAlbum.BLL.Abstract;
using ArtAlbum.DI.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtAlbum.UI.Web.Models
{
    public class RoleVM
    {
        private static IRolesBLL rolesLogic = Provider.RolesBLL;

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static bool Add(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                return rolesLogic.AddRole(new Entities.RoleDTO { Id = Guid.NewGuid(), Name = roleName });
            }
            return false;
        }

        public static string[] GetRolesForUser(string nickname)
        {
            Guid userId = UserVM.GetUserIdByNickname(nickname);
            return rolesLogic.GetRolesByUserId(userId).Select(role => role.Name).ToArray();
        }

        public static string[] GetAllRoles()
        {
            return rolesLogic.GetAllRoles().Select(role => role.Name).ToArray();
        }

        public static bool AddRoleToUser(string nickname, string roleName)
        {
            Guid userId = UserVM.GetUserIdByNickname(nickname);
            Guid roleId = rolesLogic.GetAllRoles().FirstOrDefault(x => x.Name == roleName).Id;
            if (userId != null && roleId != null)
            {
                return rolesLogic.AddRoleToUser(userId, roleId);
            }
            return false;
        }

        public static bool RemoveRoleFromUser(string nickname, string roleName)
        {
            Guid userId = UserVM.GetUserIdByNickname(nickname);
            Guid roleId = rolesLogic.GetAllRoles().FirstOrDefault(x => x.Name == roleName).Id;
            if (userId != null && roleId != null)
            {
                return rolesLogic.RemoveRoleFromUser(userId, roleId);
            }
            return false;
        }
    }
}