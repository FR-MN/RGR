using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.Entities
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Data { get; set; }
        public DateTime DateOfCreating { get; set; }
        public Guid AuthorId { get; set; }
    }
}
