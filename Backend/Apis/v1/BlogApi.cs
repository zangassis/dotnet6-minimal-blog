namespace MinimalBlog.Apis.v1;

public class BlogApi : IBlogApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/v1/posts", Find);
        app.MapGet("/v1/posts/recent", FindRecentPosts);
        app.MapGet("/v1/posts/{id}", FindById);
        app.MapPost("/v1/posts", Post).Produces<BlogPost>(StatusCodes.Status201Created);
        app.MapPut("/v1/posts/{id}", Put).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status204NoContent);
        app.MapDelete("v1/posts/{id}", Delete).Produces(StatusCodes.Status204NoContent);
    }

    public IResult Find(AppDbContext context) =>
        Results.Ok(GetPosts(context));

    public IResult FindById(Guid id, AppDbContext context)
    {
        var post = GetPosts(context).Where(x => x.Id == id).FirstOrDefault();

        if (post is null)
            return Results.NotFound();

        return Results.Ok(post);
    }

    public IResult Post(BlogPostDto createBlogPost, AppDbContext context)
    {
        try
        {
            var post = new BlogPost()
            {
                Id = createBlogPost.Id,
                Title = createBlogPost.Title,
                Content = createBlogPost.Content,
                Tags = createBlogPost.Tags,
                PublishedDate = createBlogPost.PublishedDate,
                CoverImage = createBlogPost.CoverImage,
                Author = createBlogPost.Author
            };

            context.Add(post);
            context.SaveChanges();

            return Results.Created($"/contact/{createBlogPost.Id}", createBlogPost);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }

    public IResult Put(Guid id, BlogPost updateBlogPost, AppDbContext context)
    {
        try
        {
            var blogPost = context.Posts.Find(id);

            if (blogPost is null)
                return Results.NotFound();

            context.Entry(blogPost).CurrentValues.SetValues(updateBlogPost);
            context.SaveChanges();

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"Error ocurred while puting to Post: {ex.Message}");
        }
    }

    public IResult Delete(Guid id, AppDbContext context)
    {
        var contact = context.Posts.Find(id);

        if (contact is null)
            return Results.NotFound();

        context.Posts.Remove(contact);
        context.SaveChangesAsync();

        return Results.NoContent();
    }

    public IResult FindRecentPosts(AppDbContext context) =>
         Results.Ok(GetPosts(context).OrderByDescending(p => p.PublishedDate));

    private List<BlogPostDto> GetPosts(AppDbContext context)
    {
        var authors = context.Authors;

        var posts = context.Posts;

        var tags = context.Tags;

        var listPostDto = new List<BlogPostDto>();

        foreach (var post in posts)
        {
            var author = authors.Where(a => a.Id == post.AuthorId).FirstOrDefault();
            var postTags = tags.Where(t => t.BlogPostId == post.Id);

            var postDto = new BlogPostDto(post.Id, post.Title, post.Content, postTags.ToList(), post.PublishedDate, post.CoverImage, author);
            listPostDto.Add(postDto);
        }

        return listPostDto;
    }
}