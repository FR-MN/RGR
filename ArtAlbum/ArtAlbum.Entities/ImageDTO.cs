using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.Entities
{
    public class ImageDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }       
        public string Author { get; set; }       
        public DateTime DateOfCreating { get; set; }
    }
}
