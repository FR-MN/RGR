using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.Entities
{
    public class LikeDTO
    {
        public Guid LikerId { get; set; }
        public DateTime DateOfLike { get; set; }
    }
}
