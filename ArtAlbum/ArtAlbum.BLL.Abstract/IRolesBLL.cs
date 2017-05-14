using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.BLL.Abstract
{
    public interface IRolesBLL
    {
        bool AddRole(RoleDTO role);
        bool RemoveRole(Guid roleId);
        IEnumerable<RoleDTO> GetAllRoles();
        IEnumerable<RoleDTO> GetRolesByUserId(Guid userId);
        bool AddRoleToUser(Guid userId, Guid roleId);
        bool RemoveRoleFromUser(Guid userId, Guid roleId);
    }
}
