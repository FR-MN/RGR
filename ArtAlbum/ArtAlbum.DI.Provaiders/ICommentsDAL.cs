using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.Abstract
{
    public interface ICommentsDAL
    {
        bool AddComment(CommentDTO comment);
        bool AddCommentToImage(Guid commentId, Guid imageId);
        bool RemoveCommment(Guid commentId);
        bool RemoveCommentFromImage(Guid commentId, Guid imageId);
        CommentDTO GetCommentById(Guid commentId);
        IEnumerable<Guid> GetCommentsByImageId(Guid imageId);
        Guid GetImageByCommentId(Guid commentId);
        IEnumerable<CommentDTO> GetAllComments();
    }
}
