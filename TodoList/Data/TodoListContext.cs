using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data
{
    public class TodoListContext : DbContext
    {
        public TodoListContext(DbContextOptions<TodoListContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItem { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("User");

            modelBuilder.Entity<TodoItem>()
                .ToTable("TodoItem")
                .HasOne(ti => ti.User)
                .WithMany(u => u.TodoItems);

            base.OnModelCreating(modelBuilder);
        }
    }
}
