using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public enum PostLevel
    {
        Beginner,
        Intermediate,
        Advanced
    }
    public class Post
    {
        [Key]
        public string Id { get; set; }

        public int Order { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.UtcNow;

        public DateTime? RevisionDate { get; set; }

        public bool IsPublished { get; set; } = false;

        public string CategoryId { get; set; }

        public Category Category { get; set; }

        //public List<Category> Categories { get; set; } = new List<Category>();

        public string Content { get; set; }

        public List<PostContent> PostContents { get; set; } = new List<PostContent>();

        public int PostLevelId { get; set; }

        public PostLevel PostLevel { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

    }
}
