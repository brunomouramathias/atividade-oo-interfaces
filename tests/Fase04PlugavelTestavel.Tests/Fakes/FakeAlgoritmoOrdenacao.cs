using Fase04PlugavelTestavel.Domain.Interfaces;

namespace Fase04PlugavelTestavel.Tests.Fakes
{
    /// <summary>
    /// Dublê (fake) para testes sem I/O.
    /// Simula um algoritmo de ordenação para verificar comportamento do serviço.
    /// </summary>
    public sealed class FakeAlgoritmoOrdenacao : IAlgoritmoOrdenacao
    {
        public int[] UltimaListaRecebida { get; private set; } = Array.Empty<int>();
        public int QuantidadeDeChamadas { get; private set; }

        public int[] Ordenar(int[] lista)
        {
            QuantidadeDeChamadas++;
            UltimaListaRecebida = (int[])lista.Clone();
            
            // Retorna uma ordenação previsível para testes
            return new int[] { 1, 2, 3 };
        }
    }
}
