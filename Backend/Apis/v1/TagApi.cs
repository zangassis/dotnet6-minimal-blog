namespace MinimalBlog.Apis.v1;

public class TagApi : ITagApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/v1/tags", Find);
        app.MapPost("/v1/tags", Post).Produces<Tag>(StatusCodes.Status201Created);
        app.MapDelete("v1/tags/{id}", Delete).Produces(StatusCodes.Status204NoContent);
    }

    public IResult Find(AppDbContext context) =>
        Results.Ok(context.Tags);

    public IResult Post(Tag createTag, AppDbContext context)
    {
        try
        {
            context.Add(createTag);
            context.SaveChanges();

            return Results.Created($"/tags/{createTag.Id}", createTag);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }

    public IResult Delete(Guid id, AppDbContext context)
    {
        var tag = context.Tags.Find(id);

        if (tag is null)
            return Results.NotFound();

        context.Tags.Remove(tag);
        context.SaveChangesAsync();

        return Results.NoContent();
    }
}
