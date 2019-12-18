using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloApp.Model
{
    public partial class Post
    {
        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(1000)]
        public string Content { get; set; }
        public long BlogId { get; set; }

        [ForeignKey("BlogId")]
        [InverseProperty("Post")]
        public virtual Blog Blog { get; set; }
    }
}
