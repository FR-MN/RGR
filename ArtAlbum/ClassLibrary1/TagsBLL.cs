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
    public class TagsBLL : ITagsBLL
    {
        private ITagsDAL tagsDAL;
        private IImagesDAL imagesDAL;

        public TagsBLL(ITagsDAL tagsDAL, IImagesDAL imagesDAL)
        {
            if (tagsDAL == null || imagesDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.tagsDAL = tagsDAL;
            this.imagesDAL = imagesDAL;
        }

        private bool IsTagCorrect(TagDTO tag)
        {
            return !string.IsNullOrWhiteSpace(tag.Name) && tag.Name.Length <= 50;
        }

        public bool AddTagToImage(TagDTO tag, Guid imageId)
        {
            if (IsTagCorrect(tag))
            {
                bool isAdded = false;
                foreach (var tagData in tagsDAL.GetAllTags())
                {
                    if (tagData.Name == tag.Name)
                    {
                        tag.Id = tagData.Id;
                        isAdded = true;
                    }
                }
                if (!isAdded)
                {
                    tagsDAL.AddTag(tag);
                }

                try
                {
                    imagesDAL.GetImageById(imageId);
                }
                catch
                {
                    throw new ArgumentException("incorrect image id");
                }
                return tagsDAL.AddTagToImage(tag.Id, imageId);
            }
            return false;
        }

        public IEnumerable<TagDTO> GetAllTags()
        {
            return tagsDAL.GetAllTags();
        }

        public IEnumerable<ImageDTO> GetImagesByTagId(Guid tagId)
        {
            if (tagId == null)
            {
                throw new ArgumentNullException("tag id is null");
            }
            try
            {
                tagsDAL.GetTagById(tagId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("tag id is incorrect, tag doesn't exist", e);
            }
            return imagesDAL.GetAllImages().Join(tagsDAL.GetImagesByTagId(tagId),
                image => image.Id, imageId => imageId, (image, imageId) => new ImageDTO
                { Id = imageId, Description = image.Description, DateOfCreating = image.DateOfCreating, Data = image.Data, Type = image.Type });
        }

        public TagDTO GetTagById(Guid tagId)
        {
            if (tagId == null)
            {
                throw new ArgumentNullException("tag id is null");
            }
            return tagsDAL.GetTagById(tagId);
        }

        public IEnumerable<TagDTO> GetTagsByImageId(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("image id is null");
            }
            try
            {
                imagesDAL.GetImageById(imageId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("image id is incorrect, image doesn't exist", e);
            }
            return tagsDAL.GetAllTags().Join(tagsDAL.GetTagsByImageId(imageId),
                tag => tag.Id, tagId => tagId, (tag, tagId) => new TagDTO
                { Id = tagId, Name = tag.Name });
        }

        public bool RemoveTagFromImage(Guid tagId, Guid imageId)
        {
            if (tagId == null)
            {
                throw new ArgumentNullException("tag id is null");
            }
            tagsDAL.RemoveTagFromImage(tagId, imageId);

            if (tagsDAL.GetImagesByTagId(tagId) == null && tagsDAL.GetImagesByTagId(tagId).Count() == 0)
            {
                return tagsDAL.RemoveTag(tagId);
            }
            return true;
        }
    }
}
