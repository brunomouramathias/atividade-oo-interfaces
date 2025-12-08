namespace Fase04PlugavelTestavel.Domain.Interfaces
{
    /// <summary>
    /// Contrato para algoritmos de ordenação.
    /// Define o comportamento esperado sem especificar a implementação.
    /// </summary>
    public interface IAlgoritmoOrdenacao
    {
        /// <summary>
        /// Ordena um array de inteiros em ordem crescente.
        /// </summary>
        /// <param name="lista">Array a ser ordenado.</param>
        /// <returns>Novo array ordenado (não modifica o original).</returns>
        int[] Ordenar(int[] lista);
    }
}
