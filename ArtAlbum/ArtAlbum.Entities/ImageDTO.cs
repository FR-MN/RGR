﻿using System;
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
        public DateTime DateOfCreating { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
    }
}
