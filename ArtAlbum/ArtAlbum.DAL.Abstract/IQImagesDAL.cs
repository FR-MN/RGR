using ArtAlbum.Entities;
using System;
using System.Collections.Generic;

namespace ArtAlbum.DAL.Abstract
{
    public interface IQImagesDAL
    {
        bool AddImage(QImageDTO image);
        bool AddImageToQuestion(Guid imageId, Guid questionId);
        bool RemoveImage(Guid imageId);
        bool RemoveImageFromQuestion(Guid imageId, Guid questionId);
        IEnumerable<Guid> GetImagesIdsByQuestionId(Guid questionId);
        QImageDTO GetImageById(Guid imageId);
        IEnumerable<Guid> GetQuestionsIdsByImageId(Guid imageId);
    }
}
