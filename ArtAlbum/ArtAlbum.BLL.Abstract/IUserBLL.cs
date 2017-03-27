using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;


namespace ArtAlbum.BLL.Abstract
{
    public interface IUserBLL
    {
        bool AddUser(UserDTO user);
        UserDTO GetUserById(Guid userId);
        bool RemoveUserById(Guid userId);
        bool UpdateUser(UserDTO user);
        IEnumerable<UserDTO> GetAllUsers();
    }
}
