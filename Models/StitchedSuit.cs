using Microsoft.EntityFrameworkCore;

public class StitchedSuit : Product
{
    public string CategoryType { get; set; } // 1-Piece, 2-Piece, or 3-Piece
    public ShirtDetails Shirt { get; set; } = new();
    public DupattaDetails Dupatta { get; set; } = new();
    public TrouserDetails Trouser { get; set; } = new();
    public string Note { get; set; }
}
