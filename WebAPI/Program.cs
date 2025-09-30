using FileRepositories;
using LearnWebAPI.Middlewares;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
// builder.Services.AddSwaggerGen(c =>
//     {
//         c.SwaggerDoc("v1", new OpenApiInfo { Title = "LearnWebAPI", Version = "v1" });
//     });;

builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

// if (app.Environment.IsDevelopment())
// {
//     app.UserSwagger();
//     app.UseSwaggerUI();
// }
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();