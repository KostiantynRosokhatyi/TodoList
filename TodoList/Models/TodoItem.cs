namespace TodoList.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public DateTime ItemDate { get; set; }
        public bool IsItemCompleted { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
