using ArtAlbum.Entities;
using System;
using System.Collections.Generic;

namespace ArtAlbum.DAL.Abstract
{
    public interface IAnswersDAL
    {
        bool AddAnswer(AnswerDTO answer);
        bool AddAnswerToUser(Guid answerId, Guid userId);
        bool AddAnswerToQuestion(Guid answerId, Guid questionId);
        bool RemoveAnswer(Guid answerId);
        bool RemoveAnswerFromUser(Guid answerId, Guid userId);
        bool RemoveAnswerFromQuestion(Guid answerId, Guid questionId);
        IEnumerable<AnswerDTO> GetAnswersByQuestionId(Guid questionId);
        IEnumerable<Guid> GetUsersIdsByAnswerId(Guid answerId);
    }
}
