using ArtAlbum.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;
using ArtAlbum.DAL.Abstract;

namespace ArtAlbum.BLL.DefaultLogic
{
    public class QImagesBLL : IQImagesBLL
    {
        private IQuestionsDAL questionsDAL;
        private IQImagesDAL qimagesDAL; 

        public QImagesBLL(IQImagesDAL qimagesDAL, IQuestionsDAL questionsDAL)
        {
            if (qimagesDAL == null || questionsDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.qimagesDAL = qimagesDAL;
            this.questionsDAL = questionsDAL;
        }

        private bool IsImageCorrect(QImageDTO image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image data is null");
            }
            if (image.Id == null)
            {
                throw new ArgumentException("incorrect Id");
            }
            return true;
        }

        public bool AddImage(QImageDTO image, Guid questionId)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image data is null");
            }
            else if (!IsImageCorrect(image))
            {
                throw new Exception("IncorrectDataException");
            }
            try
            {
                questionsDAL.GetQuestionById(questionId);
            }
            catch
            {
                throw new ArgumentNullException("question doesn't exist");
            }
            return qimagesDAL.AddImage(image);
        }

        public QImageDTO GetImageById(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("image id is null");
            }
            return qimagesDAL.GetImageById(imageId);
        }

        public IEnumerable<QImageDTO> GetImagesByQuestionId(Guid questionId)
        {
            if (questionId == null)
            {
                throw new ArgumentNullException("question id is null");
            }
            foreach (var qimageId in qimagesDAL.GetImagesIdsByQuestionId(questionId))
            {
                yield return qimagesDAL.GetImageById(qimageId);
            }
        }

        public bool RemoveImage(Guid qimageId)
        {
            if (qimageId == null)
            {
                throw new ArgumentNullException("answer id is null");
            }
            try
            {
                qimagesDAL.GetImageById(qimageId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("answer id is incorrect, answer doesn't exist", e);
            }
            return qimagesDAL.RemoveImageFromQuestion(qimageId, qimagesDAL.GetQuestionsIdsByImageId(qimageId).First()) && qimagesDAL.RemoveImage(qimageId);
        }
    }
}
