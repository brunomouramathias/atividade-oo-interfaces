namespace Fase04PlugavelTestavel
{
    // Dublê (fake) para testes sem I/O
    public sealed class FakeAlgoritmoOrdenacao : IAlgoritmoOrdenacao
    {
        public int[] UltimaListaRecebida { get; private set; }
        public int QuantidadeDeChamadas { get; private set; }

        public FakeAlgoritmoOrdenacao()
        {
            UltimaListaRecebida = new int[0];
            QuantidadeDeChamadas = 0;
        }

        public int[] Ordenar(int[] lista)
        {
            QuantidadeDeChamadas++;
            UltimaListaRecebida = (int[])lista.Clone();
            
            // Retorna uma ordenação previsível para testes
            return new int[] { 1, 2, 3 };
        }
    }
}

