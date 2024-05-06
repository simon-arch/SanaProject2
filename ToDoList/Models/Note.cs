using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    [Table("note")]
    public class Note
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public DateTime? deadline { get; set; }
        public int statuscode { get; set; }
        public List<CategoryNote> categoriesNotes { get; set; } = new List<CategoryNote>();
    }
}
