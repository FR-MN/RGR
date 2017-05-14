using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.Abstract
{
    public interface IUsersDAL
    {
        bool AddUser(UserDTO user);
        UserDTO GetUserById(Guid userId);
        bool RemoveUserById(Guid userId);
        bool UpdateUser(UserDTO user);
        IEnumerable<UserDTO> GetAllUsers();
    }
}
