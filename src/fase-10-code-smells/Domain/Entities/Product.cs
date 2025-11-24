namespace Fase10CodeSmells.Domain.Entities
{
    public sealed record Product(int Id, string Name, decimal Price, int Stock);
    
    public sealed record ProductDetails(string Name, string Description, decimal Price, int Quantity, string Category);
}

