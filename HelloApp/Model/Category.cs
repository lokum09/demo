using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloApp.Model
{
    public partial class Category
    {
        public Category()
        {
            Blog = new HashSet<Blog>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Blog> Blog { get; set; }
    }
}
