using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class LinkCategoryToPost
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int CategoryId { get; set; }
    }
}
