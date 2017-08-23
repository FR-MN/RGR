using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.BLL.Abstract
{
    public interface IAnswersBLL
    {
        bool AddAnswer(AnswerDTO answer, Guid userId, Guid questionId);
        bool RemoveAnswer(Guid answerId);
        IEnumerable<AnswerDTO> GetAnswersByQuestionId(Guid questionId);
        UserDTO GetUsersByAnswerId(Guid answerId);
    }
}
