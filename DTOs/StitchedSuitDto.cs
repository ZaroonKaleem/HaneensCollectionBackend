using HaneensCollection.DTOs;

public class StitchedSuitDto : ProductDto
{
    public string CategoryType { get; set; } // 1-Piece, 2-Piece, or 3-Piece
    public ShirtDetailsDto Shirt { get; set; }
    public DupattaDetailsDto Dupatta { get; set; }
    public TrouserDetailsDto Trouser { get; set; }
    public string Note { get; set; }
}
