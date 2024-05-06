using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryNote>().HasKey(cn => new { cn.noteid, cn.categoryid });
            modelBuilder.Entity<CategoryNote>().HasOne(n => n.note).WithMany(cn => cn.categoriesNotes).HasForeignKey(n => n.noteid);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Note> notes { get; set; }
        public DbSet<CategoryNote> categoriesNotes { get; set; }
        public DbSet<Category> categories { get; set; }
    }
}
