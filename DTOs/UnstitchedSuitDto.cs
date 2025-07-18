using HaneensCollection.DTOs;

public class UnstitchedSuitDto : ProductDto
{
    public ShirtDetailsDto Shirt { get; set; }
    public DupattaDetailsDto Dupatta { get; set; }
    public TrouserDetailsDto Trouser { get; set; }
    public string Note { get; set; }
}

public class ShirtDetailsDto
{
    public string EmbroideredNeckline { get; set; }
    public string DigitalPrint { get; set; }
    public string EmbroideredBorder { get; set; }
    public string Fabric { get; set; }
    public string Color { get; set; }
}

public class DupattaDetailsDto
{
    public string DigitalPrint { get; set; }
    public string Fabric { get; set; }
    public string Color { get; set; }
}

public class TrouserDetailsDto
{
    public string Description { get; set; }
    public string Fabric { get; set; }
    public string Color { get; set; }
}
