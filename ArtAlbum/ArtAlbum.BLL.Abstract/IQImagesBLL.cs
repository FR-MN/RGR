using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.BLL.Abstract
{
    public interface IQImagesBLL
    {
        bool AddImage(QImageDTO image, Guid questionId);
        bool RemoveImage(Guid imageId);
        IEnumerable<QImageDTO> GetImagesByQuestionId(Guid questionId);
        QImageDTO GetImageById(Guid imageId);
    }
}
