namespace MinimalBlog.Apis.v1;

public class AuthorApi : IAuthorApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/v1/authors", Find);
        app.MapPost("/v1/authors", Post).Produces<Author>(StatusCodes.Status201Created);
        app.MapDelete("v1/authors/{id}", Delete).Produces(StatusCodes.Status204NoContent);
    }

    public IResult Find(AppDbContext context) =>
        Results.Ok(context.Authors);

    public IResult Post(Author createAuthor, AppDbContext context)
    {
        try
        {
            context.Add(createAuthor);
            context.SaveChanges();

            return Results.Created($"/tags/{createAuthor.Id}", createAuthor);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }

    public IResult Delete(Guid id, AppDbContext context)
    {
        var author = context.Authors.Find(id);

        if (author is null)
            return Results.NotFound();

        context.Authors.Remove(author);
        context.SaveChangesAsync();

        return Results.NoContent();
    }
}
