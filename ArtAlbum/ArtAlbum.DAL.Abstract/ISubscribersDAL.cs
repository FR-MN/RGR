using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.Abstract
{
    public interface ISubscribersDAL
    {
        bool AddSubscriberToUser(Guid subscriberId, Guid userId);
        bool RemoveSubscriberFromUser(Guid subscriberId, Guid userId);
        IEnumerable<Guid> GetSubscribersOfUser(Guid userId);
        IEnumerable<Guid> GetSubscriptionsOfUser(Guid subscriberId);
        
    }
}
