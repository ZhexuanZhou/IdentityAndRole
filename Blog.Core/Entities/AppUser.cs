using System;
using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName{get;set;}
        public string LastName{get;set;}
        public string Gender{get;set;}
    }
}