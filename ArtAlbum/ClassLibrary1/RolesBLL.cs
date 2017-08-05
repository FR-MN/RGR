using ArtAlbum.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;
using ArtAlbum.DAL.Abstract;

namespace ArtAlbum.BLL.DefaultLogic
{
    public class RolesBLL : IRolesBLL
    {
        private IRolesDAL rolesDAL;
        private IUsersDAL usersDAL;

        public RolesBLL(IRolesDAL rolesDAL, IUsersDAL usersDAL)
        {
            if (rolesDAL == null || usersDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.rolesDAL = rolesDAL;
            this.usersDAL = usersDAL;
        }

        private bool IsRoleCorrect(RoleDTO role)
        {
            return role != null && !string.IsNullOrWhiteSpace(role.Name);
        }

        private bool IsRelationExist(Guid userId, Guid roleId)
        {
            try
            {
                usersDAL.GetUserById(userId);
                rolesDAL.GetRoleById(roleId);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool AddRole(RoleDTO role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role data is null");
            }
            else if (!IsRoleCorrect(role))
            {
                throw new Exception("IncorrectDataException");
            }

            return rolesDAL.AddRole(role);
        }

        public bool AddRoleToUser(Guid userId, Guid roleId)
        {
            if (userId == null || roleId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            if (!IsRelationExist(userId, roleId))
            {
                throw new ArgumentException("one of the ids are incorrect, user or image doesn't exist");
            }
            foreach (var role in GetRolesByUserId(userId))
            {
                if (role.Id == roleId)
                {
                    return false;
                }
            }
            return rolesDAL.AddRoleToUser(userId, roleId);
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            return rolesDAL.GetAllRoles();
        }

        public IEnumerable<RoleDTO> GetRolesByUserId(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
            try
            {
                usersDAL.GetUserById(userId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("user id is incorrect, user doesn't exist", e);
            }
            return rolesDAL.GetAllRoles().Join(rolesDAL.GetRolesIdsByUserId(userId),
                   role => role.Id, roleId => roleId, (role, roleId) => new RoleDTO
                   { Id = roleId, Name = role.Name });
        }

        public bool RemoveRole(Guid roleId)
        {
            if (roleId == null)
            {
                throw new ArgumentNullException("role id is null");
            }
            foreach (var userId in rolesDAL.GetUsersIdsByRoleId(roleId).ToArray())
            {
                rolesDAL.RemoveRoleFromUser(userId, roleId);
            }
            return rolesDAL.RemoveRole(roleId);
        }

        public bool RemoveRoleFromUser(Guid userId, Guid roleId)
        {
            if (userId == null || roleId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            if (!IsRelationExist(userId, roleId))
            {
                throw new ArgumentException("one of the ids are incorrect, user or award doesn't exist");
            }
            foreach (var role in GetRolesByUserId(userId))
            {
                if (role.Id == roleId)
                {
                    return rolesDAL.RemoveRoleFromUser(userId, roleId);

                }
            }
            return false;
        }
    }
}
