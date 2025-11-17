using System;
using Xunit;

namespace Fase04PlugavelTestavel.Tests
{
    public class ServicoOrdenacaoTests
    {
        [Fact]
        public void ProcessarLista_DeveUsarAlgoritmoInjetado()
        {
            // Arrange
            var fake = new FakeAlgoritmoOrdenacao();
            var servico = new ServicoOrdenacao(fake);
            int[] listaEntrada = { 5, 2, 8, 1 };

            // Act
            var resultado = servico.ProcessarLista(listaEntrada);

            // Assert
            Assert.Equal(1, fake.QuantidadeDeChamadas);
            Assert.Equal(listaEntrada, fake.UltimaListaRecebida);
            Assert.Equal(new int[] { 1, 2, 3 }, resultado);
        }

        [Fact]
        public void ProcessarLista_ListaVazia_DeveRetornarListaVazia()
        {
            // Arrange
            var fake = new FakeAlgoritmoOrdenacao();
            var servico = new ServicoOrdenacao(fake);
            int[] listaVazia = new int[0];

            // Act
            var resultado = servico.ProcessarLista(listaVazia);

            // Assert
            Assert.Empty(resultado);
            Assert.Equal(0, fake.QuantidadeDeChamadas);
        }

        [Fact]
        public void ProcessarLista_ListaNula_DeveRetornarListaVazia()
        {
            // Arrange
            var fake = new FakeAlgoritmoOrdenacao();
            var servico = new ServicoOrdenacao(fake);

            // Act
            var resultado = servico.ProcessarLista(null);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            Assert.Equal(0, fake.QuantidadeDeChamadas);
        }

        [Fact]
        public void Construtor_AlgoritmoNulo_DeveLancarExcecao()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ServicoOrdenacao(null));
        }

        [Fact]
        public void ProcessarLista_DeveDelegarParaAlgoritmo()
        {
            // Arrange
            var fake = new FakeAlgoritmoOrdenacao();
            var servico = new ServicoOrdenacao(fake);
            int[] lista1 = { 9, 3, 7 };
            int[] lista2 = { 15, 2, 11 };

            // Act
            servico.ProcessarLista(lista1);
            servico.ProcessarLista(lista2);

            // Assert
            Assert.Equal(2, fake.QuantidadeDeChamadas);
            Assert.Equal(lista2, fake.UltimaListaRecebida);
        }
    }
}

