namespace Models.Application;

public class VonageAppRequest
{
    public string Name { get; set; }

    public Keys Keys { get; set; }

    public Privacy Privacy { get; set; }

    public Capabilities Capabilities { get; set; }
}