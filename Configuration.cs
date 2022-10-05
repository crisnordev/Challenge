namespace courseappchallenge;

public static class Configuration
{
    public static SendGridConfiguration SendGridKey = new();

    public class SendGridConfiguration
    {
        public string Token { get; set; }
    }
}