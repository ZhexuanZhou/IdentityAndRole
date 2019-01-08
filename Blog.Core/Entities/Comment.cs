using System;

namespace Blog.Core.Entities
{
    public class Comment : Entity
    {
        public string Body{get;set;}
        public DateTime Create{get;set;}

        public Guid AppUserId{get;set;}
        public AppUser AppUser{get;set;}
    }
}