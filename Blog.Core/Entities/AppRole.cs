using System;
using Microsoft.AspNetCore.Identity;

namespace Blog.Core.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public string Description{get;set;}
    }
}