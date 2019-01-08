using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blog.Infrastracture.Database
{
    public class DataSeed
    {
        public static async Task SeedAsync(
            ApplicationDbContext applicationDbContext, 
            ILoggerFactory loggerFactory, 
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IConfiguration configuration,
            int retry = 0
        )
        {
            int retryForAvailability = retry;
            try
            {
                if(!applicationDbContext.Roles.Any())
                {
                    var roles = new List<AppRole>()
                    {
                        new AppRole {Id = Guid.NewGuid(), Name = ApplicationDbContext.RoleName.SuperUser, Description = "Full permission"},
                        new AppRole {Id = Guid.NewGuid(), Name = ApplicationDbContext.RoleName.User, Description = "Limited permission"}
                    };

                    foreach (var role in roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name).Result)
                        {
                            await roleManager.CreateAsync(role);
                        }
                    }
                }
                var adminInfo = configuration.GetSection("SuperUserInfo");
                var admin = new AppUser
                {
                    Id = Guid.NewGuid(), 
                    UserName = "admin@example.com",
                    FirstName = "Khang",
                    LastName = "Tran",
                    Email = "admin@example.com"
                };
                if (!applicationDbContext.Users.Any())
                {
                    var createResult = await userManager
                        .CreateAsync(admin, "Zhexuan363100.");

                    var user = await userManager.FindByNameAsync(admin.UserName);

                    var addRoleResult = userManager
                        .AddToRoleAsync(user, ApplicationDbContext.RoleName.SuperUser)
                        .Result;
                    var addClaimResult =
                        userManager.AddClaimAsync(user, new Claim(ApplicationDbContext.ClaimName.SuperUser, "true")).Result;

                    var logger = loggerFactory.CreateLogger<DataSeed>();
                    if (createResult.Succeeded && addRoleResult.Succeeded && addClaimResult.Succeeded)
                        logger.LogInformation("Super User is created successfully");
                    else
                        logger.LogError("createResult: " + createResult.ToString() + " | roleResult: " + addRoleResult +
                                    " | claimResult: " + addClaimResult);
                }

                if(!applicationDbContext.PostTags.Any())
                {
                    var tags = new[]
                    {
                        new Tag { Describtion = "Golden" },
                        new Tag { Describtion = "Pineapple" },
                        new Tag { Describtion = "Girlscout" },
                        new Tag { Describtion = "Cookies" }
                    };

                    var posts = new[]
                    {
                        new Post { Title = "Best Boutiques on the Eastside", Body="1", Create = DateTime.Now, LastModify=DateTime.Now, AppUser = admin},
                        new Post { Title = "Avoiding over-priced Hipster joints",Body="2", Create = DateTime.Now, LastModify=DateTime.Now, AppUser = admin },
                        new Post { Title = "Where to buy Mars Bars", Body="3", Create = DateTime.Now, LastModify=DateTime.Now, AppUser = admin }
                    };

                    applicationDbContext.AddRange(tags);
                    applicationDbContext.PostTags.AddRange(
                        new PostTag{
                            Post = posts[0],
                            TagId = tags[0].Id
                        }, 
                        new PostTag{
                            Post = posts[0],
                            TagId = tags[1].Id
                        }, 
                        new PostTag{
                            Post = posts[1],
                            TagId = tags[1].Id
                        }, 
                        new PostTag{
                            Post = posts[2],
                            TagId = tags[2].Id
                        }, 
                        new PostTag{
                            Post = posts[0],
                            TagId = tags[3].Id
                        }
                    );
                    // applicationDbContext.AddRange(posts);
                }
                await applicationDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<DataSeed>();
                    logger.LogError(ex.Message);
                    
                    await SeedAsync(applicationDbContext, loggerFactory,userManager,roleManager, configuration, retryForAvailability);
                }
            }
        }     
    }   
}