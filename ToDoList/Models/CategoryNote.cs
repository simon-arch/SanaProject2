using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    [Table("categoryNote")]
    public class CategoryNote
    {
        public int noteid { get; set; }
        public Note note { get; set; }
        public int categoryid { get; set; }
        public Category category { get; set; }
    }
}
