using System;

namespace ArtAlbum.Entities
{
    public class QImageDTO
    {
        public Guid Id { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
    }
}
