using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloApp.Model
{
    public partial class Blog
    {
        public Blog()
        {
            Post = new HashSet<Post>();
        }

        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Blog")]
        public virtual Category Category { get; set; }
        [InverseProperty("Blog")]
        public virtual ICollection<Post> Post { get; set; }
    }
}
