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

namespace ArtAlbum.DI.Provaiders
{
    public static class Provaider
    {
        static Provaider()
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
                    }
                    break;
                default: { throw new ConfigurationFileException("error in configuration file"); }
            }
            switch (typeBLL)
            {
                case "DefaultLogic":
                    {
                        UsersBLL = new UsersBLL(UsersDAL, RelationsDAL);
                        ImagesBLL = new ImagesBLL(ImagesDAL, RelationsDAL);
                        RelationsBLL = new UsersImagesBLL(UsersDAL, ImagesDAL, RelationsDAL);
                    }
                    break;
                default: { throw new ConfigurationFileException("error in configuration file"); }
            }
        }

        public static IUsersDAL UsersDAL { get; private set; }
        public static IImagesDAL ImagesDAL { get; private set; }
        public static IUsersImagesDAL RelationsDAL { get; private set; }
        public static IUsersBLL UsersBLL { get; private set; }
        public static IImagesBLL ImagesBLL { get; private set; }
        public static IUsersImagesBLL RelationsBLL { get; private set; }
    }
}
