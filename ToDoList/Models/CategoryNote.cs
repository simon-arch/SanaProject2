using Dapper.Contrib.Extensions;

namespace ToDoList.Models
{
    [Table("categoryNote")]
    public class CategoryNote
    {
        public int noteid { get; set; }
        public int categoryid { get; set; }
        [Computed]
        public Note note { get; set; }
        [Computed]
        public Category category { get; set; }
    }
}
