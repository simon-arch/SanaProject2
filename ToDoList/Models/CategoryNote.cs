namespace ToDoList.Models
{
    public class CategoryNote
    {
        public int noteid { get; set; }
        public int categoryid { get; set; }
        public Category category { get; set; }
    }
}
