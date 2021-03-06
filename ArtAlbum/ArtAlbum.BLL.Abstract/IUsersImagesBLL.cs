﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;

namespace ArtAlbum.BLL.Abstract
{
    public interface IUsersImagesBLL
    {
        bool AddImageToUser(Guid userId, Guid imageId);
        bool RemoveImageFromUser(Guid userId, Guid imageId);
        IEnumerable<ImageDTO> GetImagesByUser(Guid userId);
        IEnumerable<UserDTO> GetUsersByImage(Guid imageId);
    }
}
