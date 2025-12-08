namespace Fase11MiniProjeto.Domain.Interfaces
{
    /// <summary>
    /// Contrato para algoritmos de ordenação.
    /// Demonstra o uso de interfaces para definir capacidades.
    /// </summary>
    public interface IAlgoritmoOrdenacao
    {
        string Nome { get; }
        int[] Ordenar(int[] lista);
    }
}
