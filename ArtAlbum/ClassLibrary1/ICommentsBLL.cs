using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.BLL.Abstract
{
    public interface ICommentsBLL
    {
        bool AddComment(CommentDTO comment, Guid imageId);
        bool RemoveCommment(Guid commentId);
        CommentDTO GetCommentById(Guid commentId);
        IEnumerable<CommentDTO> GetCommentsByImageId(Guid imageId);
    }
}
