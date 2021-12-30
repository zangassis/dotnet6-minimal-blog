var builder = WebApplication.CreateBuilder(args);

// Register your services
RegisterServices(builder.Services);

var app = builder.Build();

// App configurations
ConfigureApp(app);

app.Run();

void ConfigureApp(WebApplication app)
{
    var ctx = app.Services.CreateScope().ServiceProvider.GetService<AppDbContext>();
    ctx.Database.EnsureCreated();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    // Configure the Middleware
    var blogApis = app.Services.GetServices<IBlogApi>();
    var tagApis = app.Services.GetServices<ITagApi>();
    var authorApis = app.Services.GetServices<IAuthorApi>();

    foreach (var tagApi in tagApis)
    {
        if (tagApi is null) ThrowException();

        tagApi.Register(app);
    }

    foreach (var api in blogApis)
    {
        if (api is null) ThrowException();

        api.Register(app);
    }

    foreach (var author in authorApis)
    {
        if (author is null) ThrowException();

        author.Register(app);
    }

    app.UseCors(builder => builder.AllowAnyOrigin());
}

void ThrowException() =>
    throw new InvalidProgramException("Apis not found");

void RegisterServices(IServiceCollection services)
{
    // Add services to the container.
    services.AddDbContext<AppDbContext>();

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Blogs API",
            Description = "Blog administration",
            Version = "v1"
        });
    });

    services.AddTransient<IBlogApi, BlogApi>();
    services.AddTransient<ITagApi, TagApi>();
    services.AddTransient<IAuthorApi, AuthorApi>();

    services.AddCors(options => options.AddDefaultPolicy(builder =>
    {
    builder.WithOrigins(
        "https://<your-app-name>.herokuapp.com/v1/posts/recent",
        "https://<your-app-name>.herokuapp.com/v1/posts/{id}");
    }));
}