using dudoan.model;
using Microsoft.EntityFrameworkCore;

namespace dudoan.dbcontext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Video> Videos { get; set; }
    public DbSet<Tag> Tags { get; set; }
}