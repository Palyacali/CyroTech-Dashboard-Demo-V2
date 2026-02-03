using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

string dbPath = "users.json";

// 1. KAYIT İŞLEVİ
app.MapPost("/api/register", async (User newUser) => {
    var users = new List<User>();
    if (File.Exists(dbPath)) {
        var json = await File.ReadAllTextAsync(dbPath);
        users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }
    if (users.Any(u => u.Username == newUser.Username)) return Results.BadRequest("ID in use.");
    
    users.Add(newUser);
    await File.WriteAllTextAsync(dbPath, JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
    return Results.Ok();
});

// 2. GİRİŞ İŞLEVİ
app.MapPost("/api/login", async (User loginData) => {
    if (!File.Exists(dbPath)) return Results.Unauthorized();
    var json = await File.ReadAllTextAsync(dbPath);
    var users = JsonSerializer.Deserialize<List<User>>(json);
    var user = users?.FirstOrDefault(u => u.Username == loginData.Username && u.Password == loginData.Password);
    
    return user is not null ? Results.Ok(user) : Results.Unauthorized();
});

app.Run("http://localhost:5050");

// --- ÖNEMLİ: Hataların çözümü için bu tanım en altta kalmalı ---
public record User(string Username, string Password, string Rank);