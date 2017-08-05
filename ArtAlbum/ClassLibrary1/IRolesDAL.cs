using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.Abstract
{
    public interface IRolesDAL
    {
        bool AddRole(RoleDTO role);
        bool RemoveRole(Guid roleId);
        RoleDTO GetRoleById(Guid roleId);
        IEnumerable<RoleDTO> GetAllRoles();
        IEnumerable<Guid> GetRolesIdsByUserId(Guid userId);
        IEnumerable<Guid> GetUsersIdsByRoleId(Guid roleId);
        bool AddRoleToUser(Guid userId, Guid roleId);
        bool RemoveRoleFromUser(Guid userId, Guid roleId);
    }
}
