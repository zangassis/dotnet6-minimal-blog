namespace MinimalBlog.Models;

public record BlogPost
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<Tag> Tags { get; set; }
    public DateTime PublishedDate { get; set; } = DateTime.Now;
    public Guid AuthorId { get; set; }
    public string CoverImage { get; set; }
    public Author Author { get; set; }
}