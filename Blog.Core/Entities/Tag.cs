using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Core.Entities
{
    public class Tag : Entity
    {
        public string Describtion{get;set;}

        public ICollection<PostTag> PostTags { get; set;} = new List<PostTag>();

    }
}