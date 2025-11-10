using System;

namespace Fase02Procedural
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 2 - PROCEDURAL ===\n");

            // Exemplo 1: Codificação
            Console.WriteLine("--- Codificação de Mensagem ---");
            string mensagem = "Hello World";
            
            Console.WriteLine($"Original: {mensagem}");
            Console.WriteLine($"Base64: {Codificacao.CodificarMensagem(mensagem, "base64")}");
            Console.WriteLine($"Cesar: {Codificacao.CodificarMensagem(mensagem, "cesar")}");
            Console.WriteLine($"Invertido: {Codificacao.CodificarMensagem(mensagem, "invertido")}");
            Console.WriteLine($"Padrão: {Codificacao.CodificarMensagem(mensagem, "outro")}\n");

            // Exemplo 2: Ordenação
            Console.WriteLine("--- Ordenação de Lista ---");
            int[] lista = { 5, 2, 8, 1, 9, 3 };
            
            Console.WriteLine($"Original: [{string.Join(", ", lista)}]");
            Console.WriteLine($"Bubble Sort: [{string.Join(", ", Ordenacao.OrdenarLista(lista, "bubble"))}]");
            Console.WriteLine($"Quick Sort: [{string.Join(", ", Ordenacao.OrdenarLista(lista, "quick"))}]");
            Console.WriteLine($"Insertion Sort: [{string.Join(", ", Ordenacao.OrdenarLista(lista, "insertion"))}]");
            Console.WriteLine($"Padrão: [{string.Join(", ", Ordenacao.OrdenarLista(lista, "outro"))}]");
        }
    }
}

