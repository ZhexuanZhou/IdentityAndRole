using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Core.Entities
{
    public class Post : Entity
    {
        public string Title{get;set;}
        public string Body{get;set;}
        public DateTime Create{get;set;}
        public DateTime LastModify{get;set;}

        public ICollection<PostTag> PostTags{get; set;} = new List<PostTag>();

        public ICollection<Comment> Comments{get;set;} = new List<Comment>();
        public Guid AppUserId{get;set;}
        public AppUser AppUser{get;set;}
    }
}