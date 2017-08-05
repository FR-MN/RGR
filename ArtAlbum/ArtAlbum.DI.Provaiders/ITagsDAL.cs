using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.Abstract
{
    public interface ITagsDAL
    {
        bool AddTag(TagDTO tag);
        bool AddTagToImage(Guid tagId, Guid imageId);
        bool RemoveTag(Guid tagId);
        bool RemoveTagFromImage(Guid tagId, Guid imageId);
        TagDTO GetTagById(Guid tagId);
        IEnumerable<Guid> GetTagsByImageId(Guid imageId);
        IEnumerable<Guid> GetImagesByTagId(Guid tagId);
        IEnumerable<TagDTO> GetAllTags();
    }
}
