using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    [Table("category")]
    public class Category
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
}
