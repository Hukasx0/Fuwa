using Fuwa.Models;
using Microsoft.EntityFrameworkCore;

namespace Fuwa.Data
{
    public class FuwaDbContext: DbContext
    {
        public FuwaDbContext(DbContextOptions<FuwaDbContext> options): base(options) {}
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<CodeSnippet> CodeSnippets { get; set; } = null!;
        public DbSet<Post> Posts { get;  set; } = null!;
        public DbSet<PostComment> PostComments { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
    }
}
