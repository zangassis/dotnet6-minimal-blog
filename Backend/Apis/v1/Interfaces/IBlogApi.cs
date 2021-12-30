namespace MinimalBlog.Apis.v1.Interfaces;

public interface IBlogApi
{
    IResult Find(AppDbContext context);
    IResult FindRecentPosts(AppDbContext context);
    IResult FindById(Guid id, AppDbContext context);
    IResult Post(BlogPostDto createBlogPost, AppDbContext context);
    IResult Put(Guid id, BlogPost updateBlogPost, AppDbContext context);
    IResult Delete(Guid id, AppDbContext context);
    void Register(WebApplication app);
}