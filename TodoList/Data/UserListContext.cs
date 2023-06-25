using Microsoft.EntityFrameworkCore;

namespace Users.Data
{
    public class UserListContext : DbContext
    {
        public UserListContext (DbContextOptions<UserListContext> options)
            : base(options)
        {
        }

        public DbSet<TodoList.Models.User> User { get; set; } = default!;
        public DbSet<TodoList.Models.TodoItem> TodoItem { get; set; } = default!;
    }
}
