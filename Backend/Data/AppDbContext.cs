namespace MinimalBlog.Data;

public class AppDbContext : DbContext
{
    public DbSet<BlogPost> Posts { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<Author> Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var conn = @"Data Source=blogdb.db";

        optionsBuilder.UseSqlite(conn);

        base.OnConfiguring(optionsBuilder);
    }
}