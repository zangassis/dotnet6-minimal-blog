namespace MinimalBlog.Models.DTOs;

public record BlogPostDto(Guid Id, string Title, string Content, List<Tag> Tags, DateTime PublishedDate, string CoverImage, Author Author);

