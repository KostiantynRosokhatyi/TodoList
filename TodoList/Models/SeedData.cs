using Users.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoList.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UserListContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UserListContext>>()))
            {
                if (context.User.Any())
                {
                    return;
                }
                context.User.AddRange(
                    new User
                    {

                        Email = "test1@gmail.com",
                        Password = "123"
                    },
                    new User
                    {

                        Email = "test2@gmail.com",
                        Password = "123"
                    },
                     new User
                     {

                         Email = "test3@gmail.com",
                         Password = "123"
                     },
                     new User
                     {

                         Email = "test4@gmail.com",
                         Password = "123"
                     }
                );
                context.SaveChanges();
            }
        }
    }
}
