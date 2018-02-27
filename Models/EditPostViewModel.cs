using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class EditPostViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public SelectList Categories { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public List<string> Tags { get; set; }

        public string NewTags { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public DateTime? RevisionDate { get; set; }
    }
}
