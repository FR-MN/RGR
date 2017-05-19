using ArtAlbum.BLL.Abstract;
using ArtAlbum.DI.Providers;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtAlbum.UI.Web.Models
{
    public class UserAvatarDataVM
    {
        private static IUsersBLL usersLogic = Provider.UsersBLL;

        public Guid Id { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }

        public static explicit operator UserAvatarDataVM(UserAvatarDTO data)
        {
            return new UserAvatarDataVM() { Id = data.UserId, Data = data.Data, Type = data.Type };
        }

        public static UserAvatarDataVM GetUserAvatarById(Guid userId)
        {
            return (UserAvatarDataVM)usersLogic.GetUserAvatarById(userId);
        }

        public static bool AddUserAvatar(byte[] data, string type, Guid userId)
        {
            return usersLogic.AddUserAvatar(new UserAvatarDTO() { Data = data, Type = type, UserId = userId });
        }

        public static bool RemoveUserAvatar(Guid userId)
        {
            try
            {
                usersLogic.GetUserById(userId);
            }
            catch
            {
                return false;
            }
            return usersLogic.RemoveUserAvatarById(userId);
        }
    }
}