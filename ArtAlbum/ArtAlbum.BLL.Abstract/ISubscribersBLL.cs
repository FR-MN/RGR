using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.BLL.Abstract
{
    public interface ISubscribersBLL
    {
        bool AddSubscriberToUser(Guid subscriberId, Guid userId);
        bool RemoveSubscriberFromUser(Guid subscriberId, Guid userId);
        IEnumerable<UserDTO> GetSubscribersOfUser(Guid userId);
        IEnumerable<UserDTO> GetSubscriptionsOfUser(Guid subscriberId);
    }
}
