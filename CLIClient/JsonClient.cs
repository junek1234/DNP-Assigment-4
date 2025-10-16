using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ApiContracts;
using Entities;



public class JsonClient
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public JsonClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<T>> GetCollectionAsync<T>(string path)
    {
        var result = await _client.GetFromJsonAsync<ICollection<T>>(path, _options);
        return result ?? Array.Empty<T>();
    }

    public async Task<T?> GetAsync<T>(string path)
    {
        return await _client.GetFromJsonAsync<T>(path, _options);
    }
    public async Task<T> CreateAsync<T>(string path, T item)
    {
        var response = await _client.PostAsJsonAsync(path, item, _options);
        response.EnsureSuccessStatusCode();
        var returned = await response.Content.ReadFromJsonAsync<T>(_options);
        if (returned == null) throw new JsonException("Failed to deserialize response.");
        return returned;
    }

    public async Task<T> UpdateAsync<T>(string path, T item)
    {
        var response = await _client.PatchAsJsonAsync(path, item, _options);
        response.EnsureSuccessStatusCode();

        // If the server returned 204 No Content (or an empty body), avoid trying to deserialize.
        // Many APIs return NoContent() for PATCH/PUT; in that case, return the original item as a fallback.
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent ||
            response.Content == null ||
            response.Content.Headers.ContentLength == 0)
        {
            return item;
        }

        var returned = await response.Content.ReadFromJsonAsync<T>(_options);
        if (returned == null) throw new JsonException("Failed to deserialize response.");
        return returned;
    }

    public async Task DeleteAsync(string path)
    {
        var response = await _client.DeleteAsync(path);
        response.EnsureSuccessStatusCode();
    }

    // TODO: getposts, comments and users with filtering


    // Posts
    public Task<ICollection<Post>> GetPostsAsync() => GetCollectionAsync<Post>("posts");
    public Task<Post?> GetPostAsync(int id) => GetAsync<Post>($"posts/{id}");
    public Task<CreatePostDto> CreatePostAsync(CreatePostDto post) => CreateAsync("posts", post);
    public Task<UpdatePostDto> UpdatePostAsync(int id, UpdatePostDto post) => UpdateAsync($"posts/{id}", post);
    public Task DeletePostAsync(int id) => DeleteAsync($"posts/{id}");


    // Comments
    public Task<ICollection<Comment>> GetCommentsAsync() => GetCollectionAsync<Comment>("comments");
    public Task<CreateCommentDto> CreateCommentAsync(CreateCommentDto comment) => CreateAsync("comments", comment);
    public Task<Comment?> GetCommentAsync(int id) => GetAsync<Comment>($"comments/{id}");
    public Task<UpdateCommentDto> UpdateCommentAsync(int id, UpdateCommentDto comment) => UpdateAsync($"comments/{id}", comment);
    public Task DeleteCommentAsync(int id) => DeleteAsync($"comments/{id}");

    // Users
    public Task<ICollection<User>> GetUsersAsync() => GetCollectionAsync<User>("users");
    public Task<User?> GetUserAsync(int id) => GetAsync<User>($"users/{id}");
    public Task<CreateUserDto> CreateUserAsync(CreateUserDto users) => CreateAsync("users", users);
    public Task<UpdateUserDto> UpdateUserAsync(int id, UpdateUserDto user) => UpdateAsync($"users/{id}", user);
    public Task DeleteUserAsync(int id) => DeleteAsync($"users/{id}");
}

