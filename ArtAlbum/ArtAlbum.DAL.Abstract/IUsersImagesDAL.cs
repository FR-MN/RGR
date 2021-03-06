﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.Abstract
{
    public interface IUsersImagesDAL
    {
        bool AddRelation(Guid userId, Guid imageId);
        bool RemoveRelation(Guid userId, Guid imageId);
        IEnumerable<Guid> GetImagesIdsByUserId(Guid userId);
        IEnumerable<Guid> GetUsersIdsByImageId(Guid imageId);
    }
}
