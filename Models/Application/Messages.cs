namespace Models.Application;

public class Messages
{
    public Webhooks webhooks { get; set; }
    
    public string Version { get; set; }
    
    public bool authenticate_inbound_media { get; set; }
}