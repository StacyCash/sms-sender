namespace Models.Application;

public class VonageApp
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public Keys Keys { get; set; }
    
    public Privacy Privacy { get; set; }
    
    public Capabilities Capabilities { get; set; }
}