using ArtAlbum.BLL.Abstract;
using ArtAlbum.DI.Providers;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtAlbum.UI.Web.Models
{
    public class CommentVM
    {
        private static ICommentsBLL commentsLogic = Provider.CommentsBLL;

        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Data { get; set; }
        public DateTime DateOfCreating { get; set; }

        internal static bool AddComment(CommentVM comment, Guid imageId)
        {
            if (comment != null && !string.IsNullOrWhiteSpace(comment.Data))
            {
                return commentsLogic.AddComment(comment, imageId);
            }
            return false;
        }

        internal static bool RemoveComment(Guid commentId)
        {
            return commentsLogic.RemoveCommment(commentId);
        }

        internal static IEnumerable<CommentVM> GetCommentsByImageId(Guid imageId)
        {
            List<CommentVM> list = new List<CommentVM>();
            foreach (var comment in commentsLogic.GetCommentsByImageId(imageId))
            {
                list.Add((CommentVM)comment);
            }
            return list;
        }

        internal static bool IsCommentExist(Guid commentId)
        {
            try
            {
                commentsLogic.GetCommentById(commentId);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static explicit operator CommentVM(CommentDTO data)
        {
            return new CommentVM { Id = data.Id, Data = data.Data, AuthorId = data.AuthorId, DateOfCreating = data.DateOfCreating };
        }
        public static implicit operator CommentDTO(CommentVM data)
        {
            return new CommentDTO() { Id = data.Id, Data = data.Data, AuthorId = data.AuthorId, DateOfCreating = data.DateOfCreating };
        }
    }
}