public class MailSettings
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool IsSecurity { get; set; }
    public string From { get; set; }
    public string To { get; set; }
}

public class MailResponse
{
    public string Message { get; set; }
    public string Status { get; set; }

    public MailResponse(string message, string status)
    {
        Message = message;
        Status = status;
    }
}
