using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.BLL.Abstract
{
    public interface ITagsBLL
    {
        bool AddTagToImage(TagDTO tag, Guid imageId);
        bool RemoveTagFromImage(Guid tagId, Guid imageId);
        TagDTO GetTagById(Guid tagId);
        IEnumerable<TagDTO> GetTagsByImageId(Guid imageId);
        IEnumerable<ImageDTO> GetImagesByTagId(Guid tagId);
        IEnumerable<TagDTO> GetAllTags();
    }
}
