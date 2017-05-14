using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.DataBase.Exceptions
{
    public class NotFoundDataException : Exception
    {
        public NotFoundDataException()
        {

        }
        public NotFoundDataException(string message) : base(message)
        {

        }
        public NotFoundDataException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
