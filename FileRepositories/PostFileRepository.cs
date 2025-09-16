using System;
using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "Posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post Post)
    {
        string PostsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> Posts = JsonSerializer.Deserialize<List<Post>>(PostsAsJson)!;
        int maxId = Posts.Count > 0 ? Posts.Max(c => c.Id) : 0;
        Post.Id = maxId + 1;
        Posts.Add(Post);
        PostsAsJson = JsonSerializer.Serialize(Posts);
        await File.WriteAllTextAsync(filePath, PostsAsJson);
        return Post;
    }

    public async Task DeleteAsync(int id)
    {
        string PostsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> Posts = JsonSerializer.Deserialize<List<Post>>(PostsAsJson)!;
        Post? PostToRemove = Posts.SingleOrDefault(p => p.Id == id);
        if (PostToRemove is null)
        {
            throw new InvalidOperationException(
                            $"Post with ID '{id}' not found");
        }
        Posts.Remove(PostToRemove);
        PostsAsJson = JsonSerializer.Serialize(Posts);
        await File.WriteAllTextAsync(filePath, PostsAsJson);
    }

    public IQueryable<Post> GetMany()
    {
        string PostsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> Posts = JsonSerializer.Deserialize<List<Post>>(PostsAsJson)!;
        return Posts.AsQueryable();
    }


    public async Task<Post> GetSingleAsync(int id)
    {
        string PostsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> Posts = JsonSerializer.Deserialize<List<Post>>(PostsAsJson)!;
        Post? Post = Posts.SingleOrDefault(p => p.Id == id);
         if (Post is null)
        {
            throw new InvalidOperationException(
                            $"Post with ID '{id}' not found");
        }
        return Post;
    }

    public async Task UpdateAsync(Post Post)
    {
        string PostsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> Posts = JsonSerializer.Deserialize<List<Post>>(PostsAsJson)!;
        Post? PostToUpdate = Posts.SingleOrDefault(Post);
         if (PostToUpdate is null)
        {
            throw new InvalidOperationException(
                            $"Post with ID '{Post.Id}' not found");
        }
        Posts.Remove(PostToUpdate);
        Posts.Add(Post);
        
        PostsAsJson = JsonSerializer.Serialize(Posts);
        await File.WriteAllTextAsync(filePath, PostsAsJson);
    }
}