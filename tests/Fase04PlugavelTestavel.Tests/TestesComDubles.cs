using Fase04PlugavelTestavel.Services;
using Fase04PlugavelTestavel.Tests.Fakes;

namespace Fase04PlugavelTestavel.Tests
{
    /// <summary>
    /// Testes unitários demonstrando uso de dublês (fakes).
    /// </summary>
    public static class TestesComDubles
    {
        public static void ExecutarTodos()
        {
            Console.WriteLine("=== TESTES COM DUBLÊS (FAKE) ===\n");

            Teste1_DeveUsarAlgoritmoInjetado();
            Teste2_ListaVazia_DeveRetornarListaVazia();
            Teste3_ListaNula_DeveRetornarListaVazia();
            Teste4_AlgoritmoNulo_DeveLancarExcecao();
            Teste5_DeveDelegarParaAlgoritmo();

            Console.WriteLine("=== TODOS OS TESTES PASSARAM! ===\n");
        }

        private static void Teste1_DeveUsarAlgoritmoInjetado()
        {
            Console.WriteLine("Teste 1: DeveUsarAlgoritmoInjetado");
            
            // Arrange
            var fake = new FakeAlgoritmoOrdenacao();
            var servico = new ServicoOrdenacao(fake);
            int[] listaEntrada = { 5, 2, 8, 1 };

            // Act
            var resultado = servico.ProcessarLista(listaEntrada);

            // Assert
            Verificar(fake.QuantidadeDeChamadas == 1, "Algoritmo deveria ser chamado 1 vez");
            Verificar(ArraysIguais(fake.UltimaListaRecebida, listaEntrada), "Lista recebida deve ser igual à entrada");
            Verificar(ArraysIguais(resultado, new int[] { 1, 2, 3 }), "Resultado deve ser [1, 2, 3]");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste2_ListaVazia_DeveRetornarListaVazia()
        {
            Console.WriteLine("Teste 2: ListaVazia_DeveRetornarListaVazia");
            
            var fake = new FakeAlgoritmoOrdenacao();
            var servico = new ServicoOrdenacao(fake);
            int[] listaVazia = Array.Empty<int>();

            var resultado = servico.ProcessarLista(listaVazia);

            Verificar(resultado.Length == 0, "Resultado deveria estar vazio");
            Verificar(fake.QuantidadeDeChamadas == 0, "Algoritmo não deveria ser chamado");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste3_ListaNula_DeveRetornarListaVazia()
        {
            Console.WriteLine("Teste 3: ListaNula_DeveRetornarListaVazia");
            
            var fake = new FakeAlgoritmoOrdenacao();
            var servico = new ServicoOrdenacao(fake);

            var resultado = servico.ProcessarLista(null!);

            Verificar(resultado != null, "Resultado não deveria ser nulo");
            Verificar(resultado!.Length == 0, "Resultado deveria estar vazio");
            Verificar(fake.QuantidadeDeChamadas == 0, "Algoritmo não deveria ser chamado");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste4_AlgoritmoNulo_DeveLancarExcecao()
        {
            Console.WriteLine("Teste 4: AlgoritmoNulo_DeveLancarExcecao");
            
            bool excecaoLancada = false;
            try
            {
                new ServicoOrdenacao(null!);
            }
            catch (ArgumentNullException)
            {
                excecaoLancada = true;
            }

            Verificar(excecaoLancada, "Deveria lançar ArgumentNullException");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste5_DeveDelegarParaAlgoritmo()
        {
            Console.WriteLine("Teste 5: DeveDelegarParaAlgoritmo");
            
            var fake = new FakeAlgoritmoOrdenacao();
            var servico = new ServicoOrdenacao(fake);
            int[] lista1 = { 9, 3, 7 };
            int[] lista2 = { 15, 2, 11 };

            servico.ProcessarLista(lista1);
            servico.ProcessarLista(lista2);

            Verificar(fake.QuantidadeDeChamadas == 2, "Algoritmo deveria ser chamado 2 vezes");
            Verificar(ArraysIguais(fake.UltimaListaRecebida, lista2), "Última lista deve ser a segunda");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Verificar(bool condicao, string mensagem)
        {
            if (!condicao)
                throw new Exception($"Falha na verificação: {mensagem}");
        }

        private static bool ArraysIguais(int[] a, int[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
    }
}
