using ArtAlbum.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;
using ArtAlbum.DAL.Abstract;
using ArtAlbum.BLL.DefaultLogic.Exceptions;

namespace ArtAlbum.BLL.DefaultLogic
{
    public class CommentsBLL : ICommentsBLL
    {
        private ICommentsDAL commentsDAL;
        private IUsersDAL usersDAL;
        private IImagesDAL imagesDAL;

        public CommentsBLL(ICommentsDAL commentsDAL, IUsersDAL usersDAL, IImagesDAL imagesDAL)
        {
            if (commentsDAL == null || usersDAL == null || imagesDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.commentsDAL = commentsDAL;
            this.usersDAL = usersDAL;
            this.imagesDAL = imagesDAL;
        }

        private bool IsCommentCorrect(CommentDTO comment)
        {
            try
            {
                usersDAL.GetUserById(comment.AuthorId);
            }
            catch
            {
                return false;
            }
            return !string.IsNullOrWhiteSpace(comment.Data) && comment.DateOfCreating <= DateTime.Now;
        }

        public bool AddComment(CommentDTO comment, Guid imageId)
        {
            if (comment == null || imageId == null)
            {
                throw new ArgumentNullException("comment data is null");
            }
            else if (!IsCommentCorrect(comment))
            {
                throw new IncorrectDataException();
            }

            try
            {
                imagesDAL.GetImageById(imageId);
            }
            catch
            {
                throw new ArgumentException("incorrect image id");
            }

            return commentsDAL.AddComment(comment) && commentsDAL.AddCommentToImage(comment.Id, imageId);
        }

        public CommentDTO GetCommentById(Guid commentId)
        {
            if (commentId == null)
            {
                throw new ArgumentNullException("comment id is null");
            }
            return commentsDAL.GetCommentById(commentId);
        }

        public IEnumerable<CommentDTO> GetCommentsByImageId(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("image id is null");
            }
            try
            {
                imagesDAL.GetImageById(imageId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("image id is incorrect, image doesn't exist", e);
            }
            return commentsDAL.GetAllComments().Join(commentsDAL.GetCommentsByImageId(imageId),
                comment => comment.Id, commentId => commentId, (comment, commentId) => new CommentDTO
                { Id = commentId, Data = comment.Data, DateOfCreating = comment.DateOfCreating, AuthorId = comment.AuthorId });
        }

        public bool RemoveCommment(Guid commentId)
        {
            if (commentId == null)
            {
                throw new ArgumentNullException("comment id is null");
            }
            commentsDAL.RemoveCommentFromImage(commentId, commentsDAL.GetImageByCommentId(commentId));
            return commentsDAL.RemoveCommment(commentId);
        }
    }
}
