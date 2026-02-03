namespace CyroServer;

public class User {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Rank { get; set; } = "Pilot"; // Marka evrenine uygun rütbe
}