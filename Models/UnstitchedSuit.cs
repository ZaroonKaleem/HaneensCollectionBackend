using Microsoft.EntityFrameworkCore;

public class UnstitchedSuit : Product
{
    public ShirtDetails Shirt { get; set; } = new();
    public DupattaDetails Dupatta { get; set; } = new();
    public TrouserDetails Trouser { get; set; } = new();
    public string Note { get; set; }
}

[Owned]
public class ShirtDetails
{
    public string EmbroideredNeckline { get; set; }
    public string DigitalPrint { get; set; }
    public string EmbroideredBorder { get; set; }
    public string Fabric { get; set; }
    public string Color { get; set; }
}

[Owned]
public class DupattaDetails
{
    public string DigitalPrint { get; set; }
    public string Fabric { get; set; }
    public string Color { get; set; }
}

[Owned]
public class TrouserDetails
{
    public string Description { get; set; } // e.g., "Solid Cambric Trouser"
    public string Fabric { get; set; }
    public string Color { get; set; }
}
