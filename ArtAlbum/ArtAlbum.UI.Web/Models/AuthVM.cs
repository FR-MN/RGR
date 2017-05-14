using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtAlbum.UI.Web.Models
{
    public class AuthVM
    {
        [MaxLength(50)]
        public string Nickname { get; set; }
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}