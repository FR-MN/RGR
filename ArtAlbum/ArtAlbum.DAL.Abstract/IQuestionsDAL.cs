using ArtAlbum.Entities;
using System;
using System.Collections.Generic;

namespace ArtAlbum.DAL.Abstract
{
    public interface IQuestionsDAL
    {
        bool AddQuestion(QuestionDTO question);
        bool AddQuestionToUser(Guid questionId, Guid userId);
        bool RemoveQuestionById(Guid questionId);
        bool RemoveQuestionFromUser(Guid questionId, Guid userId);
        QuestionDTO GetQuestionById(Guid questionId);
        IEnumerable<QuestionDTO> GetAllQuestions();
        IEnumerable<Guid> GetUsersIdsByQuestionId(Guid questionId);
    }
}
