using ArtAlbum.Entities;
using System;

namespace ArtAlbum.PL.Models
{
    public class ImageVM
    {
        private string description;

        public ImageVM(string description)
        {
            DateOfCreating = DateTime.Now;
            Description = description;
            Id = Guid.NewGuid();
        }
        public ImageVM(Guid id, string description, DateTime dateOfCreating)
        {
            DateOfCreating = DateTime.Now;
            Description = description;
            if (dateOfCreating > DateTime.Now || dateOfCreating.Year < 1960 || dateOfCreating == null)
            {
                throw new ArgumentNullException("date of creating is incorrect");
            }
            DateOfCreating = dateOfCreating;
            if (id == null)
            {
                throw new ArgumentNullException("id is null");
            }
            Id = id;
        }

        public Guid Id { get; private set; }
        public DateTime DateOfCreating { get; private set; }
        public string Description
        {
            get { return description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) && value.Length > 500)
                {
                    throw new ArgumentException("incorrect description");
                }
                description = value;
            }
        }

        public override string ToString()
        {
            return string.Format("Description: {0}, date of creating: {1}", Description, DateOfCreating);
        }

        public static explicit operator ImageVM(ImageDTO data)
        {
            return new ImageVM(data.Id, data.Description, data.DateOfCreating);
        }
        public static implicit operator ImageDTO(ImageVM data)
        {
            return new ImageDTO() { Id = data.Id, Description = data.Description, DateOfCreating = data.DateOfCreating };
        }
    }
}
