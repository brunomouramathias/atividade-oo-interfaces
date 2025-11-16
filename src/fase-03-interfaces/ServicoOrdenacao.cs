namespace Fase03Interfaces
{
    public class ServicoOrdenacao
    {
        private readonly IAlgoritmoOrdenacao _algoritmo;

        public ServicoOrdenacao(IAlgoritmoOrdenacao algoritmo)
        {
            _algoritmo = algoritmo;
        }

        public int[] OrdenarLista(int[] lista)
        {
            return _algoritmo.Ordenar(lista);
        }
    }
}

