namespace CourseAppChallenge;

public static class Configuration
{
    public static SendGridConfig SendGridKey { get; set; }
    
    public class SendGridConfig
    {
        public string SendGridApiKey { get; set; }
    }
}