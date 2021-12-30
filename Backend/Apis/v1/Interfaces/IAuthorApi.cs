namespace MinimalBlog.Apis.v1.Interfaces;

public interface IAuthorApi
{
    IResult Find(AppDbContext context);
    IResult Post(Author createAuthor, AppDbContext context);
    IResult Delete(Guid id, AppDbContext context);
    void Register(WebApplication app);
}