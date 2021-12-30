namespace MinimalBlog.Apis.v1.Interfaces;

public interface ITagApi
{
    IResult Find(AppDbContext context);
    IResult Post(Tag createTag, AppDbContext context);
    IResult Delete(Guid id, AppDbContext context);
    void Register(WebApplication app);
}
