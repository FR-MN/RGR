using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.Abstract
{
    public interface IUsersAvatarsDAL
    {
        bool AddUserAvatar(UserAvatarDTO userAvatar);
        UserAvatarDTO GetUserAvatarById(Guid userId);
        bool RemoveUserAvatarById(Guid userId);
    }
}
