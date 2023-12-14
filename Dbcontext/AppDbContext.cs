using Dudoan.Model;
using Microsoft.EntityFrameworkCore;

namespace Dudoan.Dbcontext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Video> Videos { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Watch> Watches { get; set; }
    public DbSet<Like> Likes { get; set; }
}