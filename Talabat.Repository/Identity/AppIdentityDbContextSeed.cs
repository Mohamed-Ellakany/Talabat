using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {

        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Mohamed Ellakany",
                    Email = "MohamedEllakany@gmail.com",
                    UserName = "MohamedEllakany",
                    PhoneNumber = "01234567891"

                };

                await userManager.CreateAsync(User, "P@ssw0rd");

            }




        }
    }
}
