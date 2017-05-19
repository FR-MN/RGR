using ArtAlbum.BLL.Abstract;
using ArtAlbum.BLL.DefaultLogic;
using ArtAlbum.DAL.Abstract;
using ArtAlbum.DAL.DataBase;
using ArtAlbum.DAL.DataBase.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DI.Providers
{
    public static class Provider
    {
        static Provider()
        {
            string typeDAL = ConfigurationManager.AppSettings["DAL"];
            string typeBLL = ConfigurationManager.AppSettings["BLL"];

            switch (typeDAL)
            {
                case "DataBase":
                    {
                        UsersDAL = new UsersDAL();
                        ImagesDAL = new ImagesDAL();
                        RelationsDAL = new UsersImagesDAL();
                        RolesDAL = new RolesDAL();
                        SubscribersDAL = new SubscribersDAL();
                        LikesDAL = new LikesDAL();
                        AvatarDAL = new UsersAvatarsDAL();
                        CommentsDAL = new CommentsDAL();
                    }
                    break;
                default: { throw new ConfigurationFileException("error in configuration file"); }
            }
            switch (typeBLL)
            {
                case "DefaultLogic":
                    {
                        UsersBLL = new UsersBLL(UsersDAL, RelationsDAL, RolesDAL, SubscribersDAL, LikesDAL, AvatarDAL, CommentsDAL);
                        ImagesBLL = new ImagesBLL(ImagesDAL, RelationsDAL, LikesDAL);
                        RelationsBLL = new UsersImagesBLL(UsersDAL, ImagesDAL, RelationsDAL);
                        RolesBLL = new RolesBLL(RolesDAL, UsersDAL);
                        SubscribersBLL = new SubscribersBLL(UsersDAL, SubscribersDAL);
                        LikesBLL = new LikesBLL(UsersDAL, LikesDAL, ImagesDAL);
                        CommentsBLL = new CommentsBLL(CommentsDAL, UsersDAL, ImagesDAL);
                    }
                    break;
                default: { throw new ConfigurationFileException("error in configuration file"); }
            }
        }

        public static IUsersDAL UsersDAL { get; private set; }
        public static IImagesDAL ImagesDAL { get; private set; }
        public static IUsersImagesDAL RelationsDAL { get; private set; }
        public static IRolesDAL RolesDAL { get; private set; }
        public static ISubscribersDAL SubscribersDAL { get; private set; }
        public static ILikesDAL LikesDAL { get; private set; }
        public static IUsersAvatarsDAL AvatarDAL { get; set; }
        public static ICommentsDAL CommentsDAL { get; set; }
        public static IUsersBLL UsersBLL { get; private set; }
        public static IImagesBLL ImagesBLL { get; private set; }
        public static IUsersImagesBLL RelationsBLL { get; private set; }
        public static IRolesBLL RolesBLL { get; private set; }
        public static ISubscribersBLL SubscribersBLL { get; private set; }
        public static ILikesBLL LikesBLL { get; private set; }
        public static ICommentsBLL CommentsBLL { get; set; }
    }
}
