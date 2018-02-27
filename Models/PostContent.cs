using System;

namespace BlogCore.Models
{
    public class PostContent
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }
        public Post Parent { get; set; }

        public int Version { get; set; } = 1;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }
}