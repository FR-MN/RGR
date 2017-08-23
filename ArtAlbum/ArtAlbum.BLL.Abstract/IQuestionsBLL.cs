using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.BLL.Abstract
{
    public interface IQuestionsBLL
    {
        bool AddQuestion(QuestionDTO question, Guid authorId);
        bool RemoveQuestionById(Guid questionId);
        QuestionDTO GetQuestionById(Guid questionId);
        IEnumerable<QuestionDTO> GetAllQuestions();
        UserDTO GetUserByQuestionId(Guid questionId);
    }
}
