namespace Fase11MiniProjeto.Domain.Entities
{
    /// <summary>
    /// Entidade que representa um resultado de ordenação.
    /// </summary>
    public sealed record OrdenacaoResultado(
        int Id,
        string AlgoritmoUsado,
        int[] ListaOriginal,
        int[] ListaOrdenada,
        long TempoMs);
}
