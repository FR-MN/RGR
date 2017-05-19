using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.Entities
{
    public class UserAvatarDTO
    {
        public Guid UserId { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
    }
}
